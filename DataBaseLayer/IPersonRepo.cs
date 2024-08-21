using AcrhiveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseLayer
{
    public interface IPersonRepo
    {
        Task<Person> GetPersonAsync(int id);
        Task<List<Person>> GetPersonList();
        Task<bool> CheckPersonFullName(string lastName, string? firstName);
        Task<int> UpsertPerson(Person person);
        Task UpsertPeople(List<Person> people);
        Task DeletePerson(int id);
        Task DeletePeople(List<int> personIds);
    }
}
