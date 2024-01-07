using AcrhiveModels;
using ArchiveGOST_DbLibrary;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Transactions;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace DataBaseLayer
{
    public class OriginalRepo(ArchiveDbContext context): IOriginalRepo
    {
        public readonly ArchiveDbContext _context = context;

        public async Task<Original> GetOriginalAsync(int id)
        {
            var original = await _context.Originals.FirstOrDefaultAsync(x => x.Id == id);
            return original ?? throw new Exception("Original not found");
        }
        public async Task<List<Original>> GetOriginalList()
        {
            return await _context.Originals.Include(x => x.Document).OrderBy(x => x.InventoryNumber).ToListAsync();
        }
        public async Task<List<Original>> GetOriginalsByDocument(int docunentId)
        {
            return await _context.Originals.Where(x => x.DocumentId == docunentId).ToListAsync();
        }

        public async Task<int> UpsertOriginal(Original original)
        {
            if (original.Id > 0)
            {
                return await UpdateOriginal(original);
            }
            return await CreateOriginal(original);
        }
        private async Task<int> CreateOriginal(Original original)
        {
            //original.CreatedDate = DateTime.Now;
            await _context.Originals.AddAsync(original);
            await _context.SaveChangesAsync();
            if (original.Id <= 0) { throw new Exception("Could not Create the original as expected"); }
            return original.Id;
        }
        private async Task<int> UpdateOriginal(Original original)
        {
            var dbOriginal = await _context.Originals
                .Include(x => x.Applicabilities)
                .Include(x => x.Corrections)
                .Include(x => x.Copies)
                .Include(x => x.Company)
                .Include(x => x.Person)
                .Include(x => x.Document)
                .FirstOrDefaultAsync(x => x.Id == original.Id) ?? throw new Exception("Original not found");

            dbOriginal.Caption = original.Caption;
            dbOriginal.Name = original.Name;
            dbOriginal.InventoryNumber = original.InventoryNumber;
            dbOriginal.IsDeleted = original.IsDeleted;
            dbOriginal.Notes = original.Notes;
            dbOriginal.PageCount = original.PageCount;
            dbOriginal.PageFormat = original.PageFormat;
            dbOriginal.DocumentId = original.DocumentId;
            dbOriginal.CompanyId = original.CompanyId;
            dbOriginal.PersonId = original.PersonId;
            /*if (original.Applicabilities != null)
            {
                dbOriginal.Applicabilities = original.Applicabilities;
            }
            if (original.Corrections != null)
            {
                dbOriginal.Corrections = original.Corrections;
            }
            if (original.Copies != null)
            {
                dbOriginal.Copies = original.Copies;
            }*/

            await _context.SaveChangesAsync();
            return original.Id;
        }
        public async Task UpsertOriginals(List<Original> originals)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var original in originals)
                    {
                        var success = await UpsertOriginal(original) > 0;
                        if (!success) { throw new Exception($"Error saving the original {original.Name}"); }
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
                    foreach (var original in originals)
                    {
                        var success = await UpsertOriginal(original) > 0;
                        if (!success) { throw new Exception($"Error saving the original {original.Name}"); }
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
        public async Task DeleteOriginal(int id)
        {
            var original = await _context.Originals.FirstOrDefaultAsync(x => x.Id == id);
            if (original == null) { return; }
            original.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteOriginals(List<int> originalIds)
        {
            using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    foreach (var originalId in originalIds)
                    {
                        await DeleteOriginal(originalId);
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
                    foreach (var originalId in originalIds)
                    {
                        await DeleteOriginal(originalId);
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

        public async Task<int> GetLastInventoryNumberAsync()
        {
            return await _context.Originals.MaxAsync(y => y.InventoryNumber);
        }

        public async Task<bool> CheckInventoryNumberAsync(int inventoryNumber)
        {
            bool result = await _context.Originals.AnyAsync(x => x.InventoryNumber == inventoryNumber);
            return !result;
        }
    }
}
