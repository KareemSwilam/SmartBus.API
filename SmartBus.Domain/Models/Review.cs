using SmartBus.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public string PassangerId { get; set; }
        public ReviewType Type{ get; set; }
        public Guid TargetId { get; set; }
    }
}
