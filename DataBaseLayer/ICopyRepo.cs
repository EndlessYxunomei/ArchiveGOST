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
        Task<List<Copy>> GetCopyListByOriginal(Original original);
        Task<List<Copy>> GetCopyListByDocument(Document document);
        Task<List<Copy>> GetCopyListByDelivery(Delivery delivery);
        Task<int> GetLastCopyNumberAsync(int id);

        Task<int> UpsertCopy(Copy copy);
        Task UpsertCopies(List<Copy> copies);
        Task DeleteCopy(int id);
        Task DeleteCopies(List<int> copyIds);
    }
}
