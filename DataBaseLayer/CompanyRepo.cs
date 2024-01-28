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
    public class CompanyRepo(ArchiveDbContext context) : ICompanyRepo
    {
        private readonly ArchiveDbContext _context = context;

        public async Task<Company> GetCompanyAsync(int id)
        {
            var company = await _context.Companies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return company ?? throw new Exception("Company not found");
        }
        public async Task<List<Company>> GetCompanyList()
        {
            return await _context.Companies.AsNoTracking().ToListAsync();
        }

        public async Task UpsertCompanies(List<Company> companies)
        {
            using var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                foreach (var company in companies)
                {
                    var success = await UpsertCompany(company) > 0;
                    if (!success) { throw new Exception($"Error saving the company {company.Name}"); }
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
                    foreach (var company in companies)
                    {
                        var success = await UpsertCompany(company) > 0;
                        if (!success) { throw new Exception($"Error saving the company {company.Name}"); }
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
        public async Task<int> UpsertCompany(Company company)
        {
            if (company.Id > 0)
            {
                return await UpdateCompany(company);
            }
            return await CreateCompany(company);
        }
        private async Task<int>CreateCompany(Company company)
        {
            company.CreatedDate = DateTime.Now;
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            if (company.Id <= 0) { throw new Exception("Could not Create the company as expected"); }
            return company.Id;
        }
        private async Task<int>UpdateCompany(Company company)
        {
            var dbCompany = await _context.Companies.FirstOrDefaultAsync(x => x.Id == company.Id) ?? throw new Exception("Company not found");

            dbCompany.LastModifiedDate = DateTime.Now;
            dbCompany.Name = company.Name;
            dbCompany.Description = company.Description;
            
            await _context.SaveChangesAsync();
            return company.Id;
        }
        public async Task DeleteCompanies(List<int> companyIds)
        {
            using var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                foreach (var companyId in companyIds)
                {
                    await DeleteCompany(companyId);
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
                    foreach (var companyId in companyIds)
                    {
                        await DeleteCompany(companyId);
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
        public async Task DeleteCompany(int id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);
            if (company == null) { return; }
            company.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}
