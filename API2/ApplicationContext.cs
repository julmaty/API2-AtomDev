namespace API2
{
    using Microsoft.EntityFrameworkCore;

    public class ApplicationContext : DbContext
    {
        public DbSet<PeriodTableModel> Periods { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<ReportTextContent> ReportTextContents { get; set; } = null!;
        public DbSet<ReportDescription> ReportDescription { get; set; } = default!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportDescription>().HasData(
            new ReportDescription { Id = 1, Description = "Здоровье" },
            new ReportDescription { Id = 2, Description = "Климат, параметры атмосферы" },
            new ReportDescription { Id = 3, Description = "Исследования, научная база" },
            new ReportDescription { Id = 4, Description = "Ресурсы" }
    );
        }
    }
}
