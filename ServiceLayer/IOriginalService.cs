using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IOriginalService
    {
        Task<List<OriginalListDto>> GetOriginalListAsync();
        Task<OriginalDetailDto> GetOriginalDetailAsync(int id);
        Task<OriginalListDto> GetOriginalAsync(int id);
        Task<int> GetLustInventoryNumber();
        Task<bool> CheckInventoryNumber(int inventorynumber);
        Task<int> UpsertOriginal(OriginalDetailDto originalDetailDto);
        Task DeleteOriginal(int id);
    }
}
