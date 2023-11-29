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

namespace DataBaseLayer
{
    public class ApplicabilityRepo(ArchiveDbContext context): IApplicabilityRepo
    {
        private readonly ArchiveDbContext _context = context;

        public async Task<List<Applicability>> GetApplicabilityList()
        {
            return await _context.Applicabilities.ToListAsync();
        }
        public async Task<List<Applicability>> GetApplicabilityListByOriginal(Original original)
        {
            //var origina = await _context.Copies.FirstOrDefaultAsync(x => x.Id == copy.Id) ?? throw new Exception("Copy to deliver not found");
            //return await _context.Deliveries.Where(y => y.Copies.Contains(copyToDeliver)).ToListAsync();
            var dbOriginal = await _context.Originals.FirstOrDefaultAsync(x => x.Id == original.Id) ?? throw new Exception("original not found");
            return await _context.Applicabilities.Where(x => x.Originals.Contains(dbOriginal)).ToListAsync();
        }

        public async Task UpsertApplicabilities(List<Applicability> applicabilities)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
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
            }
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

            await _context.SaveChangesAsync();
            return applicability.Id;
        }
        public async Task DeleteApplicabilities(List<int> applicabilityIds)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
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
            }
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
