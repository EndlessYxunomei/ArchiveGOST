using AcrhiveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public interface ICorrectionRepo
    {
        Task<Correction> GetCorrectionAsync(int id);
        Task<List<Correction>> GetCorrectionList(int originalId);
        Task<List<Correction>> GetCorrectionListByDocument(int documentId);
        Task<int> GetLastCorectionNumberAsync(int originalId);
        Task<bool> CheckCorrectionNumber(int id, int number);
        Task<int> UpsertCorrection(Correction correction);
        Task UpsertCorrections(List<Correction> corrections);
        Task DeleteCorrection(int id);
        Task DeleteCorrections(List<int> correctionIds);
    }
}
