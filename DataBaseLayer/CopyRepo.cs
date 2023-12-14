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
using static System.Formats.Asn1.AsnWriter;

namespace DataBaseLayer
{
    public class CopyRepo(ArchiveDbContext context): ICopyRepo
    {
        private readonly ArchiveDbContext _context = context;

        public async Task<Copy> GetCopyAsync(int id)
        {
            var copy = await _context.Copies.FirstOrDefaultAsync(x => x.Id == id);
            return copy ?? throw new Exception("Copy not found");
        }
        public async Task<List<Copy>> GetCopyListByDelivery(int deliveryId)
        {
            var dbDelivery = await _context.Deliveries.FirstOrDefaultAsync(x => x.Id == deliveryId) ?? throw new Exception("Delivery not found");
            return await _context.Copies.Where(y => y.Deliveries.Contains(dbDelivery)).ToListAsync();
        }
        public async Task<List<Copy>> GetCopyListByDocument(int documentId)
        {
            return await _context.Copies.Where(x => x.CreationDocumentId == documentId || x.DeletionDocumentId == documentId).ToListAsync();
        }
        public async Task<List<Copy>> GetCopyListByOriginal(int originalId)
        {
            return await _context.Copies.Where(x => x.OriginalId == originalId).ToListAsync();
        }

        public async Task UpsertCopies(List<Copy> copies)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var copy in copies)
                    {
                        var success = await UpsertCopy(copy) > 0;
                        if (!success) { throw new Exception($"Error saving the copy {copy.CopyNumber}"); }
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
                    foreach (var copy in copies)
                    {
                        var success = await UpsertCopy(copy) > 0;
                        if (!success) { throw new Exception($"Error saving the copy {copy.CopyNumber}"); }
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
        private async Task<int> CreateCopy (Copy copy)
        {
            await _context.Copies.AddAsync(copy);
            await _context.SaveChangesAsync();
            if (copy.Id <= 0) { throw new Exception("Could not Create the Copy as expected"); }
            return copy.Id;
        }
        private async Task<int> UpdateCopy(Copy copy)
        {
            var dbCopy = await _context.Copies
                .Include(x => x.Original)
                .Include(x => x.Deliveries)
                .Include(x => x.CreationDocument)
                .Include(x => x.DeletionDocument)
                .FirstOrDefaultAsync(x => x.Id == copy.Id) ?? throw new Exception("Copy not found");

            dbCopy.CopyNumber = copy.CopyNumber;
            dbCopy.CreationDocumentId = copy.CreationDocumentId;
            dbCopy.DeletionDocumentId = dbCopy.DeletionDocumentId;
            dbCopy.DelitionDate = copy.DelitionDate;
            dbCopy.OriginalId = copy.OriginalId;
            if (copy.Deliveries != null)
            {
                dbCopy.Deliveries = copy.Deliveries;
            }

            await _context.SaveChangesAsync();
            return copy.Id;
        }
        public async Task<int> UpsertCopy(Copy copy)
        {
            if (copy.Id > 0)
            {
                return await UpdateCopy(copy);
            }
            return await CreateCopy(copy);
        }
        public async Task DeleteCopies(List<int> copyIds)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var copyId in copyIds)
                    {
                        await DeleteCopy(copyId);
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
                    foreach (var copyId in copyIds)
                    {
                        await DeleteCopy(copyId);
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
        public async Task DeleteCopy(int id)
        {
            var copy = await _context.Copies.FirstOrDefaultAsync(x => x.Id == id);
            if (copy == null) { return; }
            copy.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetLastCopyNumberAsync(int originalId)
        {
            return await _context.Copies.Where(x => x.OriginalId == originalId).MaxAsync(y => y.CopyNumber);
        }
    }
}
