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
        Task<List<CompanyListDto>> GetCompanyList();
        Task CreateCompany();
    }
}
