using FundAnalysis.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundAnalysis.API.DbContexts
{
    public class FundContext : DbContext
    {
        public FundContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Fund> Funds { get; set; }
        public DbSet<FundPrice> FundPrices { get; set; }

    }
}
