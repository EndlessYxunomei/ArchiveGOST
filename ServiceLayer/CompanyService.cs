using AcrhiveModels;
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

        public async Task DeleteCompany(int id)
        {
            await companyRepo.DeleteCompany(id);
        }
        public async Task<CompanyDto> GetCompanyAsync(int id)
        {
            var company = await companyRepo.GetCompanyAsync(id);
            return (CompanyDto)company;
        }
        public async Task<List<CompanyDto>> GetCompanyList()
        {
            var necopmany = await companyRepo.GetCompanyList();
            List<CompanyDto> list = [];
            foreach (var company in necopmany)
            {
                CompanyDto dto = (CompanyDto)company;
                list.Add(dto);
            }
            return list;
        }
        public async Task<int> UpsertCompany(CompanyDto company)
        {
            Company newComp = new()
            {
                Name = company.Name,
                Id = company.Id,
                Description = company.Description,
            };
            return await companyRepo.UpsertCompany(newComp);
        }
    }
}
