using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcrhiveModels
{
    public class Document: FullAuditableModel
    {
        //Обозначение документа
        public required string Name { get; set; }
        //Описание документа
        [StringLength(ArchiveConstants.MAX_DESCRIPTION_LENGTH)]
        public string? Description { get; set; }
        //Дата документа (отличается от даты создания записи)
        public DateTime Date {  get; set; }
        //Компания, выпустившая документ
        public int CompanyId { get; set; }
        public virtual Company? Company { get; set; }
        //Тип документа
        public DocumentType DocumentType { get; set; }
    }
    public enum DocumentType
    {
        AddOriginal,
        CreateCopy,
        DeleteCopy,
        DeliverCopy,
        AddCorrection
    }
}
