using AcrhiveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public interface ICopyRepo
    {
        Task<Copy> GetCopyAsync(int id);
        Task<List<Copy>> GetCopyListByOriginal(int originalId);
        Task<List<Copy>> GetCopyListByDocument(int documentId);
        Task<List<Copy>> GetCopyListByDelivery(int deliveryId);
        Task<int> GetLastCopyNumberAsync(int originalId);

        Task<int> UpsertCopy(Copy copy);
        Task UpsertCopies(List<Copy> copies);
        Task DeleteCopy(int id);
        Task DeleteCopies(List<int> copyIds);
    }
}
