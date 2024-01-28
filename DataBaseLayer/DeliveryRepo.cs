using AcrhiveModels;
using ArchiveGOST_DbLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace DataBaseLayer
{
    public class DeliveryRepo(ArchiveDbContext context): IDeliveryRepo
    {
        private readonly ArchiveDbContext _context = context;

        public async Task<List<Delivery>> GetDeliveriesByCopy(int copyId)
        {
            var copyToDeliver = await _context.Copies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == copyId) ?? throw new Exception("Copy to deliver not found");
            return await _context.Deliveries.AsNoTracking().Where(y => y.Copies.Contains(copyToDeliver)).ToListAsync();
        }
        public async Task<List<Delivery>> GetDeliveriesByDocument(int documentId)
        {
            //return await _context.Deliveries.Where(x => x.DeliveryDocumentId == document.Id || x.ReturnDocumentId == document.Id).ToListAsync();
            return await _context.Deliveries.Where(x => x.DeliveryDocumentId == documentId)
                .Union(_context.Deliveries.Where(x => x.ReturnDocumentId == documentId))
                .AsNoTracking().ToListAsync();
        }
        public async Task<List<Delivery>> GetDeliveriesByPerson(int personId)
        {
            return await _context.Deliveries.AsNoTracking().Where(x => x.PersonId == personId).ToListAsync();
        }
        public async Task<Delivery> GetDeliveryAsync(int id)
        {
            var delivery = await _context.Deliveries
                .Include(x => x.Copies)
                .Include(x => x.Person)
                .Include(x => x.DeliveryDocument)
                .Include(x => x.ReturnDocument)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return delivery ?? throw new Exception("Delivery not found");
        }

        public async Task UpsertDeliveries(List<Delivery> deliveries)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var delivery in deliveries)
                    {
                        var success = await UpsertDelivery(delivery) > 0;
                        if (!success) { throw new Exception($"Error saving the delivery  {delivery.Id}"); }
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            //не работает в SQLite
            /*using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var delivery in deliveries)
                    {
                        var success = await UpsertDelivery(delivery) > 0;
                        if (!success) { throw new Exception($"Error saving the delivery  {delivery.Id}"); }
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }*/
        }
        public async Task<int> UpsertDelivery(Delivery delivery)
        {
            if (delivery.Id > 0)
            {
                return await UpdateDelivery(delivery);
            }
            return await CreateDelivery(delivery);
        }
        private async Task<int> CreateDelivery(Delivery delivery)
        {
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();
            if (delivery.Id <= 0) { throw new Exception("Could not Create the delivery as expected"); }
            return delivery.Id;
        }
        private async Task<int> UpdateDelivery(Delivery delivery)
        {
            var dbDelivery = await _context.Deliveries
                .Include(x => x.DeliveryDocument)
                .Include(x => x.ReturnDocument)
                .Include(x => x.Copies)
                .Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == delivery.Id) ?? throw new Exception("Delivery not found");

            dbDelivery.IsDeleted = delivery.IsDeleted;
            dbDelivery.DeliveryDate = delivery.DeliveryDate;
            dbDelivery.DeliveryDocumentId = delivery.DeliveryDocumentId;
            dbDelivery.ReturnDate = delivery.ReturnDate;
            dbDelivery.ReturnDocumentId = delivery.ReturnDocumentId;
            dbDelivery.PersonId = delivery.PersonId;
            if (delivery.Copies != null) 
            {
                dbDelivery.Copies = delivery.Copies;
            }

            await _context.SaveChangesAsync();
            return delivery.Id;
        }
        public async Task DeleteDeliveries(List<int> deliveryIds)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var deliveryId in deliveryIds)
                    {
                        await DeleteDelivery(deliveryId);
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            //не работает в SQLite
            /*using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var deliveryId in deliveryIds)
                    {
                        await DeleteDelivery(deliveryId);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }*/
        }
        public async Task DeleteDelivery(int id)
        {
            var delivery = await _context.Deliveries.FirstOrDefaultAsync(x => x.Id == id);
            if (delivery == null) { return; }
            delivery.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
