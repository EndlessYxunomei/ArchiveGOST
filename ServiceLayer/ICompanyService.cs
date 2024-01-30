using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface ICompanyService
    {
        Task<List<CompanyDto>> GetCompanyList();
        Task<CompanyDto> GetCompanyAsync(int id);
        Task<int> UpsertCompany(CompanyDto company);
        Task DeleteCompany(int id);
        Task<bool> CheckCompany(string name);
    }
}
