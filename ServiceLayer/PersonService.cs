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

        public Task CreatePerson()
        {
            throw new NotImplementedException();
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
    }
}
