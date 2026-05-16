using Microsoft.EntityFrameworkCore;
using SmartBus.Domain.IRepository;
using SmartBus.Domain.Models;
using SmartBus.Infrasturcture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Infrasturcture.Repository
{
    public class StopSegmentRepository: Repository<StopSegment>, IStopSegmentRepository
    {
        public StopSegmentRepository(ApplicationContext context) : base(context )
        {
            
        }

        public async Task<double> GetPriceForSegment(Guid tripId, int fromStopOrder, int toStopOrder)
        {
            
            var result = await  _dbSet.Where(s => s.TripId == tripId && s.FromStopOrder >= fromStopOrder && s.FromStopOrder < toStopOrder).SumAsync(s => s.Price);
            
            return result;
        }
    }
}
