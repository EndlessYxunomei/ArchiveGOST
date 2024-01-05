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

        public static explicit operator DocumentListDto(Document document)
        {
            return new DocumentListDto()
            {
                DocumentType = document.DocumentType,
                Name = document.Name,
                Id = document.Id
            };
        }
    }
}
