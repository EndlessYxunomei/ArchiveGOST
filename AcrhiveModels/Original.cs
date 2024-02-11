using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Original: FullAuditableModel
    {
        //Инвентарный номер (не является id но является уникальным)
        public required int InventoryNumber { get; set; }
        //Обозначение документа
        [StringLength(ArchiveConstants.MAX_ORIGINAL_NAME_LENGTH)]
        public required string Name { get; set; }
        //Наименование документа
        [StringLength(ArchiveConstants.MAX_ORIGINAL_CAPTION_LENGTH)]
        public required string Caption { get; set; }
        //Форматы листов
        [StringLength(ArchiveConstants.MAX_ORIGINAL_PAGES_FORMAT_LENGTH)]
        public string? PageFormat { get; set; }
        //Количестов листов
        public int PageCount { get; set; }
        //Организация, хранящая подлинник
        public int? CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        //Сопроводительный документ
        public int? DocumentId { get; set; }
        public virtual Document? Document { get; set; }
        //Примечания
        [StringLength(ArchiveConstants.MAX_ORIGINAL_NOTES_LENGTH)]
        public string? Notes { get; set; }
        //Лицо, принявшее документ
        public int? PersonId { get; set; }
        public virtual Person? Person { get; set;}
        //список копий
        public virtual List<Copy> Copies { get; set; } = [];
        //список корекций
        public virtual List<Correction> Corrections { get; set; } = [];
        //список применяемости
        public virtual List<Applicability> Applicabilities { get; set; } = [];

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Original original = (Original)obj;
            return original.Id == Id;
        }
        public override int GetHashCode()
        {
            return Id;
        }
    }
}
