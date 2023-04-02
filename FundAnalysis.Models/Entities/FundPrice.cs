using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundAnalysis.Models.Entities
{
    [Table("FundPrices")]
    public class FundPrice : EntityBase
    {
        [ForeignKey("Fund")]
        public Guid FundId { get; set; }
        public virtual Fund Fund { get; set; }
        public DateTime Date { get; set; }
        [Precision(18, 6)]
        public decimal Close { get; set; }
    }
}
