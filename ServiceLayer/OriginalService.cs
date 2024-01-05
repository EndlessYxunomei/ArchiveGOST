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
    public class OriginalService : IOriginalService
    {
        private readonly IOriginalRepo originalRepo;
        public OriginalService(ArchiveDbContext context) 
        {
            originalRepo = new OriginalRepo(context);
        }
        public OriginalService(IOriginalRepo originalRepo)
        {
            this.originalRepo = originalRepo;
        }

        public Task<OriginalDetailDto> GetOriginalDetailAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<OriginalListDto>> GetOriginalListAsync()
        {
            var originalList = await originalRepo.GetOriginalList();
            List<OriginalListDto> list = [];
            foreach (var original in originalList)
            {
                OriginalListDto dto = (OriginalListDto)original;
                list.Add(dto);
            }
            return list;
    }

        public Task<int> UpsertOriginal(OriginalDetailDto originalDetailDto)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteOriginal(int id)
        {
            await originalRepo.DeleteOriginal(id);
        }

        public async Task<int> GetLustInventoryNumber()
        {
            return await originalRepo.GetLastInventoryNumberAsync();
        }

        public Task<bool> CheckInventoryNumber(int inventorynumber)
        {
            throw new NotImplementedException();
        }
    }
}
