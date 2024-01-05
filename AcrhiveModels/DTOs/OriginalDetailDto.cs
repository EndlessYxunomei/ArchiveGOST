using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class OriginalDetailDto
    {
        public int Id { get; set; }
        public int InventoryNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string? PageFormat { get; set; }
        public int PageCount { get; set; }
        public CompanyListDto? Company { get; set; }
        public DocumentListDto? Document { get; set; }
        public string? Notes { get; set; }
        public PersonListDto? Person { get; set; }
        //список копий
        public List<CopyListDto> Copies { get; set; } = [];
        //список корекций
        public List<CorrectionListDto> Corrections { get; set; } = [];
        //список применяемости
        public List<ApplicabilityListDto> Applicabilities { get; set; } = [];
    }
}
