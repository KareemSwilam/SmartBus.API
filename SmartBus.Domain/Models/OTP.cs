using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Domain.Models
{
    public class OTP
    {
        public int Id { get; set; }
        public string Value { get; set; }   
        public string UserId { get; set; }
        public DateTime ExpaireDate {  get; set; }
        public bool IsUsed { get; set; }

    }
}
