using AcrhiveModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class OriginalDetailDto: IIdentityModel
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

        public static explicit operator OriginalDetailDto(Original original)
        {
            List<Copy> copyList = original.Copies;
            List<CopyListDto> copyDtos = [];
            copyDtos.AddRange(copyList.Select(copy => (CopyListDto)copy));

            List<Correction> correctionList = original.Corrections;
            List<CorrectionListDto> corDtos = [];
            corDtos.AddRange(correctionList.Select(cor => (CorrectionListDto)cor));

            List<Applicability> applicList = original.Applicabilities;
            List<ApplicabilityListDto> appDtos = [];
            appDtos.AddRange(applicList.Select(apps => (ApplicabilityListDto)apps));

            return new OriginalDetailDto()
            {
                Id = original.Id,
                InventoryNumber = original.InventoryNumber,
                Name = original.Name,
                Caption = original.Caption,
                PageFormat = original.PageFormat,
                PageCount = original.PageCount,
                Notes = original.Notes,
                Company = original.Company != null ? (CompanyListDto)original.Company : null,
                Document = original.Document != null ? (DocumentListDto)original.Document : null,
                Person = original.Person != null ? (PersonListDto)original.Person : null,
                Copies = copyDtos,
                Corrections = corDtos,
                Applicabilities = appDtos
            };
        }
    }
}
