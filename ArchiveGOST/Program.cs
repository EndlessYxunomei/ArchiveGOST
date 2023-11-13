using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ArchiveGOST_DbLibrary;
using Archive_Helpers;
using AcrhiveModels;

namespace ArchiveGOST
{
    internal class Program
    {
        //Конфигурация базы данных
        private static IConfigurationRoot _configuration;
        private static DbContextOptionsBuilder<ArchiveDbContext> _optionsBuilder;
        static void BuildOptions()
        {
            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<ArchiveDbContext>();
            //_optionsBuilder.UseSqlite(_configuration.GetConnectionString("ArchiveLibrary"));
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ArchiveLibrarySQLServer"));
        }
        
        
        static void Main(string[] args)
        {
            //настраиваем подключение к базе данных
            BuildOptions();
            //тестирование
            CreateNewDatabase();
            CreateNewItems();
            Console.WriteLine("Всё прошло удачно");
        }
        private static void CreateNewDatabase()
        {
            using (var context = new ArchiveDbContext(_optionsBuilder.Options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        private static void CreateNewItems()
        {
            Console.WriteLine("Создаем пробыне элементы");
            using (var context = new ArchiveDbContext(_optionsBuilder.Options))
            {
                Company testCompany = new() { Name = "TestCompany" };
                context.Companies.Add(testCompany);

                Document testDocumentOriginal = new() { Name = "testOriginalAdd", Company = testCompany, DocumentType = DocumentType.AddOriginal };
                context.Documents.Add(testDocumentOriginal);

                Original original1 = new() { Name = "111", Caption = "original 1", InventoryNumber = 1, Company = testCompany, Document = testDocumentOriginal };
                Original original2 = new() { Name = "222", Caption = "original 2", InventoryNumber = 2, Company = testCompany, Document = testDocumentOriginal };
                context.Originals.Add(original1);
                context.Originals.Add(original2);

                context.SaveChanges();
            }
        }
    }
}