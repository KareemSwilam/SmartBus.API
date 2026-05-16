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
    public class BusRepository:Repository<Bus>, IBusRepository  
    {
        public BusRepository(ApplicationContext contetxt):base(contetxt)
        {
            
        }

        public async Task<Bus> GetBusWithSeat(int BusId)
        {
            IQueryable<Bus> query = _dbSet.Where(b => b.Id == BusId).Include(b => b.Seats)
                                          .AsNoTracking().AsSplitQuery(); 
            return await query.FirstOrDefaultAsync();
        }
    }
}
