using AcrhiveModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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
                //var cnstr = _configuration.GetConnectionString("ArchiveLibrary");
                var cnstr = _configuration.GetConnectionString("ArchiveLibrarySQLServer");

                //optionsBuilder.UseSqlite(cnstr);
                optionsBuilder.UseSqlServer(cnstr);

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
            //связь много-много для орининалов и применимости
            modelBuilder.Entity<Original>()
            .HasMany(x => x.Applicabilities)
            .WithMany(p => p.Originals)
            .UsingEntity<Dictionary<string, object>>(
            "OriginalApplicabilities",
            ip => ip.HasOne<Applicability>()
            .WithMany()
            .HasForeignKey("ApplicabilityId")
            .HasConstraintName("FK_OriginalApplicability_Applicabilities_ApplicabilityId")
            .OnDelete(DeleteBehavior.ClientCascade),
            ip => ip.HasOne<Original>()
            .WithMany()
            .HasForeignKey("OriginalId")
            .HasConstraintName("FK_ApplicabilityOriginal_Originals_OriginalId")
            .OnDelete(DeleteBehavior.Cascade));

            //связь много-много для орининалов и изменений
            modelBuilder.Entity<Original>()
            .HasMany(x => x.Corrections)
            .WithMany(p => p.Originals)
            .UsingEntity<Dictionary<string, object>>(
            "OriginalCorrections",
            ip => ip.HasOne<Correction>()
            .WithMany()
            .HasForeignKey("ApplicabilityId")
            .HasConstraintName("FK_OriginalCorrection_Corrections_CorrectionId")
            .OnDelete(DeleteBehavior.ClientCascade),
            ip => ip.HasOne<Original>()
            .WithMany()
            .HasForeignKey("OriginalId")
            .HasConstraintName("FK_CorrectionOriginal_Originals_OriginalId")
            .OnDelete(DeleteBehavior.Cascade));

            /*modelBuilder.Entity<Copy>()
                .HasOne(x => x.Original).WithMany(y => y.Copies).OnDelete(DeleteBehavior.ClientCascade);//хз какой тут нужен режим удаления

            modelBuilder.Entity<Original>()
                .HasOne(x => x.Document).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Copy>()
                .HasOne(x => x.CreationDocument).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Copy>()
                .HasOne(x => x.DeletionDocument).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Delivery>()
                .HasOne(x => x.DeliveryDocument).WithOne().OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Delivery>()
                .HasOne(x => x.ReturnDocument).WithOne().OnDelete(DeleteBehavior.ClientCascade);*/
        }
    }
}