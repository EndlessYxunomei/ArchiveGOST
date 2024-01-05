using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class CompanyListDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public static explicit operator CompanyListDto(Company company)
        {
            return new CompanyListDto()
            { 
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
            };
        }
    }
}
