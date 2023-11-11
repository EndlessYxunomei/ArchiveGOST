using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ArchiveGOST_DbLibrary;
using Archive_Helpers;

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
            _optionsBuilder.UseSqlite(_configuration.GetConnectionString("ArchiveLibrary"));
        }
        
        
        static void Main(string[] args)
        {
            //настраиваем подключение к базе данных
            BuildOptions();
            //тестирование
            Test1();
            Console.WriteLine("Всё прошло удачно");
        }
        private static void Test1()
        {
            using (var context = new ArchiveDbContext(_optionsBuilder.Options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
    }
}