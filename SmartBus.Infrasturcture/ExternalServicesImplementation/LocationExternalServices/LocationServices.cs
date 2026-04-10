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
            var latLonList = apiResults.Select(r => new { r.lat, r.lon }).ToList();

            
            var existingById = await _unit.LocationRepository
                .GetAll(l => osmIds.Contains(l.OpenStreetMapId));

            
            var minLat = latLonList.Min(v => double.Parse(v.lat)) - 0.001;
            var maxLat = latLonList.Max(v => double.Parse(v.lat)) + 0.001;

            var minLon = latLonList.Min(v => double.Parse(v.lon)) - 0.001;
            var maxLon = latLonList.Max(v => double.Parse(v.lon)) + 0.001;
            var existingByCoords = await _unit.LocationRepository.GetAll(l =>
                l.Latitude >= minLat &&l.Latitude <= maxLat &&
                l.Longitude >= minLon && l.Longitude <= maxLon);

            
            

            var newLocations = new List<Location>();

            foreach (var item in apiResults)
            {
                bool existsById = existingById
                    .Any(l => l.OpenStreetMapId == item.place_id);

                bool existsByCoordinates = existingByCoords
                    .Any(l => IsSameLocation(l.Latitude, l.Longitude, double.Parse(item.lat), double.Parse(item.lon)));

                if (!existsById && !existsByCoordinates)
                {
                    newLocations.Add(new Location
                    {
                        Name = item.name.ToLower(),
                        Latitude = Math.Round(double.Parse(item.lat), 6),
                        Longitude = Math.Round(double.Parse(item.lon), 6),
                        City = item.Address?.city
                                ?? item.Address?.town
                                ?? item.Address?.village,                                
                        OpenStreetMapId = item.place_id
                    });
                }
            }

            // 7. Save new locations
            if (newLocations.Any())
            {
                _unit.LocationRepository.AddRange(newLocations);
                await _unit.SaveAsync();
            }

            // 8. Merge results
            var finalLocations = existingById
                .Concat(existingByCoords)
                .Concat(newLocations)
                .DistinctBy(l => l.OpenStreetMapId)
                .ToList();

            var result = _mapper.Map<List<LocationDto>>(finalLocations);

            return CustomResult<List<LocationDto>>.Success(result);
        }
        bool IsSameLocation(double lat1, double lon1, double lat2, double lon2)
        {
            const double tolerance = 0.0001; // ~10 meters
            return Math.Abs(lat1 - lat2) < tolerance &&
                   Math.Abs(lon1 - lon2) < tolerance;
        }
    }  
        
    
}
