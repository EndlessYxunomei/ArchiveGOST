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
        Task<List<Delivery>> GetDeliveriesByCopy(Copy copy);
        Task<List<Delivery>> GetDeliveriesByPerson(Person person);
        Task<List<Delivery>> GetDeliveriesByDocument(Document document);
        Task<Delivery> GetDeliveryAsync(int id);

        Task<int> UpsertDelivery(Delivery delivery);
        Task UpsertDeliveries(List<Delivery> deliveries);
        Task DeleteDelivery(int id);
        Task DeleteDeliveries(List<int> deliveryIds);
    }
}
