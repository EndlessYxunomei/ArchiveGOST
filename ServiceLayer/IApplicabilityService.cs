using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IApplicabilityService
    {
        Task<List<ApplicabilityListDto>> GetApplicabilityListAsync();
        Task<List<ApplicabilityListDto>> GetApplicabilityListByOriginalAsync(int id);
        Task<List<ApplicabilityListDto>> GetFreeApplicabilities(int originalId);
        Task<int> UpsertApplicability(ApplicabilityListDto applicability);
        Task DeleteApplicability(int id);
        Task DeleteOriginalFromApplicability(int id, int originalId);
    }
}
