using AcrhiveModels;
using Archive_Helpers;
using ArchiveGOST_DbLibrary;
using DataBaseLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shouldly;

namespace ArchiveTests
{
    [TestClass]
    public class DBLayerTests
    {
        private OriginalRepo _originalRepo;
        private DocumentRepo _documentRepo;
        private ApplicabilityRepo _applicabilityRepo;
        private CompanyRepo _companyRepo;
        private CopyRepo _copyRepo;
        private CorrectionRepo _correctionRepo;
        private DeliveryRepo _deliveryRepo;
        private PersonRepo _personRepo;

        private static IConfigurationRoot? _configuration;
        private static DbContextOptionsBuilder<ArchiveDbContext>? _optionsBuilder;
        private static ArchiveDbContext? _dbContext;

        [TestInitialize]
        public void InitilizeTests()
        {
            //Подключаем базу данных (можно сделалть фальшивую базу, но это не нужно пока можно делать тестоую настоящую базу)
            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            _optionsBuilder = new DbContextOptionsBuilder<ArchiveDbContext>();
            var cstr = _configuration.GetConnectionString("ArchiveLibrary");
            _optionsBuilder.UseSqlite(cstr);
            _dbContext = new ArchiveDbContext(_optionsBuilder.Options);

            //Подключаем репы
            _originalRepo = new OriginalRepo(_dbContext);
            _documentRepo = new DocumentRepo(_dbContext);
            _applicabilityRepo = new ApplicabilityRepo(_dbContext);
            _companyRepo = new CompanyRepo(_dbContext);
            _copyRepo = new CopyRepo(_dbContext);
            _correctionRepo = new CorrectionRepo(_dbContext);
            _deliveryRepo = new DeliveryRepo(_dbContext);
            _personRepo = new PersonRepo(_dbContext);
        }
        
        [TestMethod]
        public async Task OriginalTest()
        {
            var result = await _originalRepo.GetOriginalList();
            result.Count.ShouldBe(15);
            result[5].Name.ShouldBe("47Б-128-202");
            result[5].Notes.ShouldBeNull();
            result[14].InventoryNumber.ShouldBeGreaterThan(200);

            var searchresult = await _originalRepo.GetOriginalsByDocument(1);
            searchresult.Count.ShouldBe(7);
            searchresult[0].Caption.ShouldBe("Элемент протектора");

            var lastnumber = await _originalRepo.GetLastInventoryNumberAsync();
            lastnumber.ShouldBeGreaterThan(1235);

            var testoriginal = await _originalRepo.GetOriginalAsync(2);
            testoriginal.PageCount.ShouldBeGreaterThan(400);
        }
        [TestMethod]
        public async Task DocumentTest()
        {
            var result = await _documentRepo.GetDocumentAsync(1);
            result.DocumentType.ShouldBe(AcrhiveModels.DocumentType.AddOriginal);
            result.Date.ShouldBe(new DateOnly(2000, 12, 7));

            var testlist = await _documentRepo.GetDocumentList();
            testlist.Count.ShouldBe(12);

            var testtypelist = await _documentRepo.GetDocumentList(AcrhiveModels.DocumentType.DeleteCopy);
            testtypelist.Count.ShouldBe(1);
            testtypelist[0].Description?.ShouldContain("состояние");
        }
        [TestMethod]
        public async Task ApplicabilityTest()
        {
            var listtest = await _applicabilityRepo.GetApplicabilityList();
            listtest.Count.ShouldBe(3);

            var listbyoriginal = await _applicabilityRepo.GetApplicabilityListByOriginal(1);
            listbyoriginal.Count.ShouldBe(1);
            listbyoriginal[0].Description.ShouldBe("Д42");
        }
        [TestMethod]
        public async Task CompanyTest()
        {
            var listtest = await _companyRepo.GetCompanyList();
            listtest.Count.ShouldBe(14);

            var testcompany = await _companyRepo.GetCompanyAsync(14);
            testcompany.Name.ShouldContain("ремонт");
        }
        [TestMethod]
        public async Task CorrectionTest()
        {
            var listtest = await _correctionRepo.GetCorrectionList(1);
            listtest.Count.ShouldBe(2);

            var testCorrection = await _correctionRepo.GetCorrectionAsync(2);
            testCorrection.CorrectionNumber.ShouldBe(1);

            var testDocumentlist = await _correctionRepo.GetCorrectionListByDocument(11);
            testDocumentlist.Count.ShouldBe(2);

            var testNumber = await _correctionRepo.GetLastCorectionNumberAsync(1);
            testNumber.ShouldBe(2);
        }
        [TestMethod]
        public async Task CopyTest()
        {
            var listtest_orig = await _copyRepo.GetCopyListByOriginal(1);
            listtest_orig.Count.ShouldBe(3);

            var listtest_doc = await _copyRepo.GetCopyListByDocument(5);
            listtest_doc.Count.ShouldBe(2);

            var listtest_deliver = await _copyRepo.GetCopyListByDelivery(1);
            listtest_deliver.Count.ShouldBe(2);

            var findtest = await _copyRepo.GetCopyAsync(3);
            findtest.CopyNumber.ShouldBe(3);

            var testnumber = await _copyRepo.GetLastCopyNumberAsync(1);
            testnumber.ShouldBe(3);
        }
        [TestMethod]
        public async Task DeliveryTest()
        {
            var findtest = await _deliveryRepo.GetDeliveryAsync(3);
            findtest.PersonId.ShouldBe(4);

            var listtest_doc = await _deliveryRepo.GetDeliveriesByDocument(9);
            listtest_doc.Count.ShouldBe(1);

            var listtest_per = await _deliveryRepo.GetDeliveriesByPerson(3);
            listtest_per.Count.ShouldBe(2);

            var listtest_copy = await _deliveryRepo.GetDeliveriesByCopy(4);
            listtest_copy.Count.ShouldBe(1);
            listtest_copy[0].PersonId.ShouldBe(4);
        }
        [TestMethod]
        public async Task PersonTest()
        {
            var findtest = await _personRepo.GetPersonAsync(2);
            findtest.Department?.ShouldContain("ОМТО");

            Person testP1 = new() { LastName = "Камоликова" };
            Person testP2 = new() { LastName = "Киселёв" };
            await _personRepo.UpsertPeople([testP1, testP2]);

            var listtest_create = await _personRepo.GetPersonList();
            listtest_create[4].LastName.ShouldContain("молик");
            listtest_create[5].Department.ShouldBeNull();

            listtest_create[5].Department = "ПО";
            await _personRepo.UpsertPerson(listtest_create[5]);
            var listtest_update = await _personRepo.GetPersonList();
            listtest_update[5].Department.ShouldNotBeNull();

            await _personRepo.DeletePeople([listtest_update[4].Id, listtest_update[5].Id]);
            var listtest_del = await _personRepo.GetPersonList();
            listtest_del.Count.ShouldBe(4);
        }
    }
}