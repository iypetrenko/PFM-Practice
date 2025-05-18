using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public decimal Limit { get; set; }
        public DateTime Period { get; set; }
    }
}
