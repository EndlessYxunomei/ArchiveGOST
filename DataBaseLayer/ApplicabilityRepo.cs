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
    public class ApplicabilityRepo(ArchiveDbContext context): IApplicabilityRepo
    {
        private readonly ArchiveDbContext _context = context;

        public async Task<List<Applicability>> GetApplicabilityList()
        {
            return await _context.Applicabilities.ToListAsync();
        }
        public async Task<List<Applicability>> GetApplicabilityListByOriginal(int originalId)
        {
            //var origina = await _context.Copies.FirstOrDefaultAsync(x => x.Id == copy.Id) ?? throw new Exception("Copy to deliver not found");
            //return await _context.Deliveries.Where(y => y.Copies.Contains(copyToDeliver)).ToListAsync();
            return await _context.Applicabilities.Where(x => x.Originals.Contains(_context.Originals.First(x => x.Id == originalId))).ToListAsync();
        }
        public async Task<List<Applicability>> GetFreeApplicabilityList(int originalId)
        {
            return await _context.Applicabilities.Except(_context.Applicabilities.Where(x => x.Originals.Contains(_context.Originals.First(y => y.Id == originalId)))).ToListAsync();
        }
        public async Task<Applicability?> GetApplicabilityAsync(int id)
        {
            var appl = await _context.Applicabilities.Include(x => x.Originals).FirstOrDefaultAsync(x => x.Id == id);
            return appl;// ?? throw new Exception("Original not found");
        }

        public async Task UpsertApplicabilities(List<Applicability> applicabilities)
        {
            using var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                foreach (var applicability in applicabilities)
                {
                    var success = await UpsertApplicability(applicability) > 0;
                    if (!success) { throw new Exception($"Error saving the applicability {applicability.Id}"); }
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                await transaction.RollbackAsync();
                throw;
            }
            //не работает в SQLite
            /*using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var applicability in applicabilities)
                    {
                        var success = await UpsertApplicability(applicability) > 0;
                        if (!success) { throw new Exception($"Error saving the applicability {applicability.Id}"); }
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
        public async Task<int> UpsertApplicability(Applicability applicability)
        {
            if (applicability.Id > 0)
            {
                return await UpdateApplicability(applicability);
            }
            return await CreateApplicability(applicability);
        }
        private async Task<int> CreateApplicability(Applicability applicability)
        {
            applicability.CreatedDate = DateTime.Now;
            await _context.Applicabilities.AddAsync(applicability);
            await _context.SaveChangesAsync();
            if (applicability.Id <= 0) { throw new Exception("Could not Create the applicability as expected"); }
            return applicability.Id;
        }
        private async Task<int> UpdateApplicability(Applicability applicability)
        {
            var dbApplicability = await _context.Applicabilities
                .Include(x => x.Originals)
                .FirstOrDefaultAsync(x => x.Id == applicability.Id) ?? throw new Exception("Applicability not found");

            dbApplicability.Description = applicability.Description;
            if (applicability.Originals != null)
            {
                dbApplicability.Originals = applicability.Originals;
            }
            dbApplicability.LastModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return applicability.Id;
        }
        public async Task DeleteApplicabilities(List<int> applicabilityIds)
        {
            using var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                foreach (var applicabilityId in applicabilityIds)
                {
                    await DeleteApplicability(applicabilityId);
                }
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                await transaction.RollbackAsync();
                throw;
            }
            //не работает в SQLite
            /*using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var applicabilityId in applicabilityIds)
                    {
                        await DeleteApplicability(applicabilityId);
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
        public async Task DeleteApplicability(int id)
        {
            var applicability = await _context.Applicabilities.FirstOrDefaultAsync(x => x.Id == id);
            if (applicability == null) { return; }
            applicability.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
