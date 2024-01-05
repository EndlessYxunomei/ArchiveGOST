using AcrhiveModels.DTOs;
using ArchiveGOST_DbLibrary;
using DataBaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepo companyRepo;
        public CompanyService(ICompanyRepo companyRepo)
        {
            this.companyRepo = companyRepo;
        }
        public CompanyService(ArchiveDbContext dbContext)
        {
            companyRepo = new CompanyRepo(dbContext);
        }

        public Task CreateCompany()
        {
            throw new NotImplementedException();
        }

        public async Task<List<CompanyListDto>> GetCompanyList()
        {
            var necopmany = await companyRepo.GetCompanyList();
            List<CompanyListDto> list = [];
            foreach (var company in necopmany)
            {
                CompanyListDto dto = (CompanyListDto)company;
                list.Add(dto);
            }
            return list;
        }
    }
}
