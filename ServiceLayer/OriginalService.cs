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

        public async Task<OriginalDetailDto> GetOriginalDetailAsync(int id)
        {
            Original original = await originalRepo.GetOriginalAsync(id);
            return (OriginalDetailDto)original;
        }
        public async Task<List<OriginalListDto>> GetOriginalListAsync()
        {
            var originalList = await originalRepo.GetOriginalList();
            List<OriginalListDto> list = [];
            foreach (var original in originalList)
            {
                list.Add((OriginalListDto)original);
                //OriginalListDto dto = (OriginalListDto)original;
                //list.Add(dto);
            }
            return list;
        }
        public async Task<int> UpsertOriginal(OriginalDetailDto originalDetailDto)
        {
            Original newOriginal = new() 
            {
                Id = originalDetailDto.Id,
                InventoryNumber = originalDetailDto.InventoryNumber,
                Name = originalDetailDto.Name,
                Caption = originalDetailDto.Caption,
                PageFormat = originalDetailDto.PageFormat,
                PageCount = originalDetailDto.PageCount,
                Notes = originalDetailDto.Notes,
                CompanyId = originalDetailDto.Company?.Id,
                DocumentId = originalDetailDto?.Document?.Id,
                PersonId = originalDetailDto?.Person?.Id,

                CreatedDate = DateTime.Now
                //ПРИДУМАТЬ ЧТО ДЕЛАТЬ СО СПИСКАМИ КОПИЙ, КОРРЕКЦИ И ПРИМЕНИМОСТИ
            };
            return await originalRepo.UpsertOriginal(newOriginal);
        }
        public async Task DeleteOriginal(int id) => await originalRepo.DeleteOriginal(id);
        public async Task<int> GetLustInventoryNumber() => await originalRepo.GetLastInventoryNumberAsync();
        public async Task<bool> CheckInventoryNumber(int inventorynumber) => await originalRepo.CheckInventoryNumberAsync(inventorynumber);
        public async Task<OriginalListDto> GetOriginalAsync(int id)
        {
            var result = await originalRepo.GetOriginalAsync(id);
            return (OriginalListDto)result;
        }
    }
}
