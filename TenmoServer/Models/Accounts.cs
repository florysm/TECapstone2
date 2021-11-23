using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Accounts
    {
        public int AccountID { get; set; }
        public int UserID { get; set; }
        public decimal Balance { get; set; }
    }
}
