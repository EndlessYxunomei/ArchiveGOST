using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
	public class PersonDetailDto
	{
		public int Id { get; set; }
		public string? FirstName { get; set; }
		public required string LastName { get; set; }
		public string? Department { get; set; }
		public static explicit operator PersonDetailDto(Person person)
		{
			return new PersonDetailDto()
			{ 
				LastName = person.LastName,
				FirstName = person.FirstName,
				Department = person.Department,
				Id = person.Id
			};
		}
	}
}
