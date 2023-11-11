using AcrhiveModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ArchiveGOST_DbLibrary
{
    public class ArchiveDbContext: DbContext
    {
        //Конструкторы: для Scuffold и для dependensy injection
        public ArchiveDbContext() { }
        public ArchiveDbContext(DbContextOptions options): base(options) { }

        //Конфигурация по умолчанию
        private static IConfigurationRoot _configuration;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //На случай, если мы не предаём настроек, то прописываем значение по умолчанию
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                _configuration = builder.Build();
                var cnstr = _configuration.GetConnectionString("ArchiveLibrary");

                optionsBuilder.UseSqlite(cnstr);
            }
        }

        //Таблицы
        public DbSet<Person> People { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Applicability> Applicabilities { get; set; }
        public DbSet<Correction> Corrections { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Original> Originals { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        //создание связей и пр
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}