using AcrhiveModels;
using ArchiveGOST_DbLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;

namespace DataBaseLayer
{
    public class PersonRepo(ArchiveDbContext context): IPersonRepo
    {
        private readonly ArchiveDbContext _context = context;

        public async Task<Person> GetPersonAsync(int id)
        {
            var person = await _context.People.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return person ?? throw new Exception("Person not found");
        }
        public async Task<List<Person>> GetPersonList()
        {
            return await _context.People.AsNoTracking().ToListAsync();
        }

        public async Task UpsertPeople(List<Person> people)
        {
			using var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
			try
			{
				foreach (var person in people)
				{
					var success = await UpsertPerson(person) > 0;
					if (!success) { throw new Exception($"Error saving the person {person.LastName}"); }
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
                    foreach (var person in people)
                    {
                        var success = await UpsertPerson(person) > 0;
                        if (!success) { throw new Exception($"Error saving the person {person.LastName}"); }
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
        public async Task<int> UpsertPerson(Person person)
        {
            if (person.Id > 0)
            {
                return await UpdatePerson(person);
            }
            return await CreatePerson(person);
        }
        private async Task<int> CreatePerson(Person person)
        {
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            if (person.Id <= 0) { throw new Exception("Could not Create the person as expected"); }
            return person.Id;
        }
        private async Task<int> UpdatePerson(Person person)
        {
            var dbPerson = await _context.People.FirstOrDefaultAsync(x => x.Id == person.Id) ?? throw new Exception("Document not found");

            dbPerson.FirstName = person.FirstName;
            dbPerson.LastName = person.LastName;
            dbPerson.Department = person.Department;
            dbPerson.IsDeleted = person.IsDeleted;

            await _context.SaveChangesAsync();
            return person.Id;
        }
        public async Task DeletePeople(List<int> personIds)
        {
			using var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
			try
			{
				foreach (var personId in personIds)
				{
					await DeletePerson(personId);
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
					foreach (var personId in personIds)
					{
						await DeletePerson(personId);
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
        public async Task DeletePerson(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) { return; }
            person.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

		public async Task<bool> CheckPersonFullName(string lastName, string? firstName)
		{
			bool exist = await _context.People.AnyAsync(x => x.LastName == lastName && x.FirstName == firstName);
            return !exist;
		}
	}
}
