using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface ICorrectionService
    {
        Task<int> GetFreeCorrectionNumber(int originalId);
        Task<bool> CheckCorrectionNumber(int originalId, int number);
        Task<int> UpsertCorrection(CorrectionDetailDto correction);
        Task DeleteCorrection(int id);
        Task<CorrectionDetailDto> GetCorrectionDetailAsync(int id);
        Task<CorrectionListDto> GetCorrectionAsync(int id);
        Task<List<CorrectionListDto>> GetCorrectionList(int originalId);
    }
}
