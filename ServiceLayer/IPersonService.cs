using AcrhiveModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IPersonService
    {
        Task<List<PersonListDto>> GetPersonList();
        Task<int> UpsertPerson(PersonDetailDto personDetailDto);
        Task<PersonDetailDto> GetPersonDetail(int id);
		Task DeletePerson(int id);
	}
}
