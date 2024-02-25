using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class DocumentDetailDto
    {
        public required string Name { get; set; }
        public required int Id { get; set; }
        public string? Description { get; set; }
        public required DocumentType DocumentType { get; set; }
        public DateTime Date { get; set; }
        public CompanyDto? Company { get; set; }

        public static explicit operator DocumentDetailDto(Document document)
        {
            return new DocumentDetailDto()
            {
                DocumentType = document.DocumentType,
                Name = document.Name,
                Id = document.Id,
                Date = new DateTime(document.Date.Year, document.Date.Month, document.Date.Day),
                Description = document.Description,
                Company = document.Company == null ? null : (CompanyDto)document.Company
            };
        }
    }
}
