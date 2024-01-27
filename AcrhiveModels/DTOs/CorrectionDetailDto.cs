using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class CorrectionDetailDto
    {
        public int Id { get; set; }
        public int OriginalId { get; set; }
        public string OriginalName { get; set; } = string.Empty;
        public string OriginalCaption { get; set; } = string.Empty;
        public int CorrectionNumber { get; set; }
        public string Description {  get; set; } = string.Empty;
        public DocumentListDto? Document { get; set; }

        public static explicit operator CorrectionDetailDto(Correction correction)
        {
            return new CorrectionDetailDto()
            {
                Id = correction.Id,
                OriginalId = correction.OriginalId,
                Description = correction.Description,
                CorrectionNumber = correction.CorrectionNumber,
                Document = (DocumentListDto)correction.Document!,
                OriginalName = correction.Original!.Name,
                OriginalCaption = correction.Original!.Caption
            };
        }
    }
}
