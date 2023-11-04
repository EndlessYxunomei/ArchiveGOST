using System;
using System.Collections.Generic;
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
        public required string Name { get; set; }
        //Наименование документа
        public required string Caption { get; set; }
        //Форматы листов
        public string? PageFormat { get; set; }
        //Количестов листов
        public int PageCount { get; set; }
        //Организация, хранящая подлинник
        public int CompanyId { get; set; }
        //Сопроводительный документ
        public int DocumentId { get; set; }
        //Примечания
        public string? Notes { get; set; }
        //Лицо, принявшее документ
        public int PersonId { get; set; }

    }
}
