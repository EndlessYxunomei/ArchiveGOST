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
        Task<List<ApplicabilityDto>> GetApplicabilityListAsync();
        Task<List<ApplicabilityDto>> GetApplicabilityListByOriginalAsync(int id);
        Task<List<ApplicabilityDto>> GetFreeApplicabilities(int originalId);
        Task<ApplicabilityDto?> GetApplicabilityAsync(int id);
        Task<bool> CheckApplicability(string description);
        Task<int> UpsertApplicability(ApplicabilityDto applicability);
        Task DeleteApplicability(int id);
        Task DeleteOriginalFromApplicability(int id, int originalId);
        Task AddOriginalToApplicability(int id, int originalId);
    }
}
