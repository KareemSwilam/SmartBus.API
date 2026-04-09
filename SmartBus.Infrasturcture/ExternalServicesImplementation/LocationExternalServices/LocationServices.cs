using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmartBus.Application.Dtos.LocationDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using SmartBus.Infrasturcture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.ExternalServicesImplementation.LocationExternalServices
{
    public class LocationServices:ILocationServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly LocationAPISetting _apiSetting;
        private readonly IUnitOfWork _unit;
        List<APIResponse> responses = new List<APIResponse>();
        private readonly IMapper _mapper;
        public LocationServices(IHttpClientFactory httpClientFactory, IOptions<LocationAPISetting> options, IUnitOfWork unit, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _apiSetting = options.Value;
            _unit = unit;
            _mapper = mapper;
        }

        public async Task<CustomResult<List<LocationDto>>> GetLocationsByName(string name)
        {
            var client = _httpClientFactory.CreateClient();

            var dbLocations = await _unit.LocationRepository
                .GetAll(l => l.Name.ToLower() == name.ToLower());

            if (dbLocations.Any())
            {
                var dto = _mapper.Map<List<LocationDto>>(dbLocations);
                return CustomResult<List<LocationDto>>.Success(dto);
            }

            client.DefaultRequestHeaders.UserAgent.ParseAdd("SmartBusApp");

            var response = await client.GetAsync(
                $"https://nominatim.openstreetmap.org/search?q={name}&format=json&addressdetails=1&countrycodes=eg&accept-language=en");

            if (!response.IsSuccessStatusCode)
                return CustomResult<List<LocationDto>>.Failure(
                    CustomError.ServerError("Failed to get location."));

            var apiResults = await response.Content.ReadFromJsonAsync<List<APIResponse>>();

            if (apiResults == null || !apiResults.Any())
                return CustomResult<List<LocationDto>>.Failure(
                    CustomError.NotFound("No locations found."));

            var osmIds = apiResults.Select(r => r.place_id).ToList();

            var existingLocations = await _unit.LocationRepository
                .GetAll(l => osmIds.Contains(l.OpenStreetMapId));

            var existingDict = existingLocations.ToDictionary(l => l.OpenStreetMapId);

            var newLocations = new List<Location>();

            foreach (var item in apiResults)
            {
                if (!existingDict.ContainsKey(item.place_id))
                {
                    newLocations.Add(new Location
                    {
                        Name = item.name.ToLower(),
                        Latitude = item.lat,
                        Longitude = item.lon,
                        City = item.Address.city,
                        OpenStreetMapId = item.place_id
                    });
                }
            }

            if (newLocations.Any())
            {
                _unit.LocationRepository.AddRange(newLocations);
                await _unit.SaveAsync();
            }

            var finalLocations = existingLocations.Concat(newLocations).ToList();

            var result = _mapper.Map<List<LocationDto>>(finalLocations);

            return CustomResult<List<LocationDto>>.Success(result);

        }     
    }
}
