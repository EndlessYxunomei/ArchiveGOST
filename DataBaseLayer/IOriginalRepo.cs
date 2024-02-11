using AcrhiveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public interface IOriginalRepo
    {
        Task<List<Original>> GetOriginalList();
        Task<Original> GetOriginalAsync(int id);
        Task<List<Original>> GetOriginalsByDocument(int docunentId);
        Task<List<Original>> GetOriginalsByCompany(int companyId);
        Task<int> GetLastInventoryNumberAsync();
        Task<bool> CheckInventoryNumberAsync(int inventoryNumber);
        Task<int> UpsertOriginal(Original original);
        Task UpsertOriginals(List<Original> originals);
        Task DeleteOriginal(int id);
        Task DeleteOriginals(List<int> originalIds);
        Task UpdateOriginalApplicabilities(int id, List<int> applicabilityId);
    }
}
