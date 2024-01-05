using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels.DTOs
{
    public class OriginalListDto
    {
        public int OriginalId { get; set; }
        public int OriginalInventoryNumber { get; set; }
        public string OriginalName { get; set; } = string.Empty;
        public string OriginalCaption {  get; set; } = string.Empty;
        public DateTime OriginalDate {  get; set; }
        public string? DocumentName { get; set; }

        public static explicit operator OriginalListDto(Original original)
        {
            return new OriginalListDto()
            {
                OriginalId = original.Id,
                OriginalInventoryNumber = original.InventoryNumber,
                OriginalName = original.Name,
                OriginalCaption = original.Caption,
                OriginalDate = original.CreatedDate,
                DocumentName = original.Document?.Name
            };
        }
    }
}
