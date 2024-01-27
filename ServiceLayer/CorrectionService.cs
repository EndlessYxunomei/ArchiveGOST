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
    public class CorrectionService: ICorrectionService
    {
        private readonly ICorrectionRepo correctionRepo;

        public CorrectionService(ICorrectionRepo correctionRepo)
        {
            this.correctionRepo = correctionRepo;
        }
        public CorrectionService(ArchiveDbContext context)
        {
            correctionRepo = new CorrectionRepo(context);
        }

        public async Task<CorrectionDetailDto> GetCorrectionDetailAsync(int id)
        {
            var cor = await correctionRepo.GetCorrectionAsync(id);
            return (CorrectionDetailDto) cor;
        }
        public async Task<List<CorrectionListDto>> GetCorrectionList(int originalId)
        {
            var corlist = await correctionRepo.GetCorrectionList(originalId);
            List<CorrectionListDto> list = [];
            foreach (var cor in corlist)
            {
                CorrectionListDto dto = (CorrectionListDto)cor;
                list.Add(dto);
            }
            return list;
        }
        public async Task<CorrectionListDto> GetCorrectionAsync(int id)
        {
            var cor = await correctionRepo.GetCorrectionAsync(id);
            return (CorrectionListDto)cor;
        }
        public async Task<int> GetFreeCorrectionNumber(int originalId)
        {
            var lustNumber = await correctionRepo.GetLastCorectionNumberAsync(originalId);
            return lustNumber +1;
        }
        public async Task<bool> CheckCorrectionNumber(int originalId, int number) => await correctionRepo.CheckCorrectionNumber(originalId, number);
        public async Task<int> UpsertCorrection(CorrectionDetailDto correction)
        {
            Correction newCorrection = new()
            {
                Description = correction.Description,
                CorrectionNumber = correction.CorrectionNumber,
                Id = correction.Id,
                DocumentId = correction.Document!.Id,
                OriginalId = correction.OriginalId,
            };
            return await correctionRepo.UpsertCorrection(newCorrection);
        }
        public async Task DeleteCorrection(int id) => await correctionRepo.DeleteCorrection(id);
    }
}
