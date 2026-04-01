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
    public class TripStopRepository:Repository<TripStop>, ITripStopRepository
    {
        public TripStopRepository(ApplicationContext context):base(context)
        {

        }
    }
}
