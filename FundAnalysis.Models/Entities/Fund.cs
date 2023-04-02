using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundAnalysis.Models.Entities
{
    [Table("Funds")]
    public class Fund : EntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<FundPrice> FundPrices { get; set; }
    }
}
