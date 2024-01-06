using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class DocumentListDto
    {
        public required string Name { get; set; }
        public required int Id { get; set; }
        public required DocumentType DocumentType { get; set; }
        public DateTime Date { get; set; }
        public override string ToString()
        {
            return $"{Name} от {Date:d}";
        }

        public static explicit operator DocumentListDto(Document document)
        {
            return new DocumentListDto()
            {
                DocumentType = document.DocumentType,
                Name = document.Name,
                Id = document.Id,
                Date = new DateTime(document.Date.Year, document.Date.Month, document.Date.Day)
            };
        }
    }
}
