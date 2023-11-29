﻿using AcrhiveModels;
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
    public class CorrectionRepo(ArchiveDbContext context): ICorrectionRepo
    {
        private readonly ArchiveDbContext _context = context;

        public async Task<Correction> GetCorrectionAsync(int id)
        {
            var correction = await _context.Corrections.FirstOrDefaultAsync(x => x.Id == id);
            return correction ?? throw new Exception("Correction not found");
        }
        public async Task<List<Correction>> GetCorrectionList(Original original)
        {
            return await _context.Corrections.Where(x => x.OriginalId == original.Id).ToListAsync();
        }

        public async Task<int> UpsertCorrection(Correction correction)
        {
            if (correction.Id > 0)
            {
                return await UpdateCorrection(correction);
            }
            return await CreateCorrection(correction);
        }
        private async Task<int> CreateCorrection(Correction correction)
        {
            await _context.Corrections.AddAsync(correction);
            await _context.SaveChangesAsync();
            if (correction.Id <= 0) { throw new Exception("Could not Create the correction as expected"); }
            return correction.Id;
        }
        private async Task<int> UpdateCorrection(Correction correction)
        {
            var dbCorrection = await _context.Corrections
                .Include(x => x.Original)
                .Include(x => x.Document)
                .FirstOrDefaultAsync(x =>x.Id == correction.Id) ?? throw new Exception("Correction not found");

            dbCorrection.OriginalId = correction.OriginalId;
            dbCorrection.DocumentId = correction.DocumentId;
            dbCorrection.CorrectionNumber = correction.CorrectionNumber;
            dbCorrection.Description = correction.Description;

            await _context.SaveChangesAsync();
            return correction.Id;
        }
        public async Task UpsertCorrections(List<Correction> corrections)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var correction in corrections)
                    {
                        var success = await UpsertCorrection(correction) > 0;
                        if (!success) { throw new Exception($"Error saving the original {correction.CorrectionNumber}"); }
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
        public async Task DeleteCorrection(int id)
        {
            var correction = await _context.Corrections.FirstOrDefaultAsync(x => x.Id == id);
            if (correction == null) { return; }
            correction.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCorrections(List<int> correctionIds)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (var correctionId in correctionIds)
                    {
                        await DeleteCorrection(correctionId);
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

        public async Task<int> GetLastCorectionNumberAsync(int id)
        {
            return await _context.Corrections.Where(x => x.OriginalId == id).MaxAsync(y => y.CorrectionNumber);
        }
    }
}
