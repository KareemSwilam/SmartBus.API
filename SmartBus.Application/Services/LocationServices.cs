using MapsterMapper;
using SmartBus.Application.Dtos.LocationDtos;
using SmartBus.Application.IServices;
using SmartBus.Application.Result;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Services
{
    public class LocationServices : ILocationServices
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public LocationServices(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        public async Task<CustomResult<List<LocationDto>>> GetLocations(string? name)
        {
            var result = new List<LocationDto> ();
            if (name == null)
            {
                var locations = await _unit.LocationRepository.GetAll();
                result = _mapper.Map<List<LocationDto>>(locations);
            }
            else
            {
                var Searchlocations = await _unit.LocationRepository.GetAll(l => l.Name.ToLower().Contains(name!.ToLower()));
                result = _mapper.Map<List<LocationDto>>(Searchlocations);
            }
            return CustomResult<List<LocationDto>>.Success(result);
        }
    }
}
