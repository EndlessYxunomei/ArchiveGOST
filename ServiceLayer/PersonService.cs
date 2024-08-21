using AcrhiveModels.DTOs;
using ArchiveGOST_DbLibrary;
using DataBaseLayer;

namespace ServiceLayer
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepo personRepo;
        public PersonService(IPersonRepo personRepo)
        {
            this.personRepo = personRepo;
        }
        public PersonService(ArchiveDbContext dbContext)
        {
            personRepo = new PersonRepo(dbContext);
        }

		public async Task DeletePerson(int id)
		{
			await personRepo.DeletePerson(id);
		}

		public async Task<List<PersonListDto>> GetPersonList()
        {
            var personlist = await personRepo.GetPersonList();
            List<PersonListDto> list = [];
            foreach (var person in personlist)
            {
                PersonListDto dto = (PersonListDto)person;
                list.Add(dto);
            }
            return list;
        }

		public Task<int> UpsertPerson(PersonDetailDto personDetailDto)
		{
			throw new NotImplementedException();
		}

		public async Task<PersonDetailDto> GetPersonDetail(int id)
		{
			var person = await personRepo.GetPersonAsync(id);
			return (PersonDetailDto)person;
		}

		public async Task<bool> CheckPersonFullName(string lastName, string? firstName)
		{
			return await personRepo.CheckPersonFullName(lastName, firstName);
		}

		public async Task<PersonListDto> GetPerson(int id)
		{
			var person = await personRepo.GetPersonAsync(id);
			return (PersonListDto)person;
		}
	}
}
