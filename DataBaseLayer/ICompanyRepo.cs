using AcrhiveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public interface ICompanyRepo
    {
        Task<List<Company>> GetCompanyList();
        Task<Company> GetCompanyAsync(int id);
        Task<int> UpsertCompany(Company company);
        Task UpsertCompanies(List<Company> companies);
        Task DeleteCompany(int id);
        Task DeleteCompanies(List<int> companyIds);
    }
}
