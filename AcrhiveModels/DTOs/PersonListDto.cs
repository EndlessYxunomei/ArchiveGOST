using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class PersonListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public static explicit operator PersonListDto(Person person)
        {
            return new PersonListDto
            {
                Id = person.Id,
                FullName = person.LastName + " " + (person.FirstName ?? "")
            };
        }
    }
}
