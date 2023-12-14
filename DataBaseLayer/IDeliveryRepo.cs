using AcrhiveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public interface IDeliveryRepo
    {
        Task<List<Delivery>> GetDeliveriesByCopy(int copyId);
        Task<List<Delivery>> GetDeliveriesByPerson(int personId);
        Task<List<Delivery>> GetDeliveriesByDocument(int documentId);
        Task<Delivery> GetDeliveryAsync(int id);

        Task<int> UpsertDelivery(Delivery delivery);
        Task UpsertDeliveries(List<Delivery> deliveries);
        Task DeleteDelivery(int id);
        Task DeleteDeliveries(List<int> deliveryIds);
    }
}
