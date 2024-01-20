using AcrhiveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public interface IApplicabilityRepo
    {
        Task<List<Applicability>> GetApplicabilityList();
        Task<List<Applicability>> GetApplicabilityListByOriginal(int originalId);
        Task<List<Applicability>> GetFreeApplicabilityList(int originalId);
        Task<Applicability?> GetApplicabilityAsync(int id);

        Task<int> UpsertApplicability(Applicability applicability);
        Task UpsertApplicabilities(List<Applicability> applicabilities);
        Task DeleteApplicability(int id);
        Task DeleteApplicabilities(List<int> applicabilityIds);
    }
}
