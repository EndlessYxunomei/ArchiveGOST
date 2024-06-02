using AcrhiveModels;
using System.Globalization;

namespace ArchiveGHOST.Client.Converters
{
    public class DocumentTypeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DocumentType type)
            {
                return type switch
                {
                    DocumentType.AddOriginal => "Получение оригинала",
                    DocumentType.CreateCopy => "Выпуск копий",
                    DocumentType.DeleteCopy => "Аннулирование копий",
                    DocumentType.DeliverCopy => "Выдача копий",
                    DocumentType.AddCorrection => "Внесение изменений",
                    _ => "неизвестный тип",
                };
            }
            else if (value is IList<DocumentType> types)
            {
                List<string> list = [];
                foreach (var doctype in types)
                {
                    switch (doctype)
                    {
                        case DocumentType.AddOriginal:
                            list.Add("Получение оригинала");
                            break;
                        case DocumentType.CreateCopy:
                            list.Add("Выпуск копий");
                            break;
                        case DocumentType.DeleteCopy:
                            list.Add("Аннулирование копий");
                            break;
                        case DocumentType.DeliverCopy:
                            list.Add("Выдача копий");
                            break;
                        case DocumentType.AddCorrection:
                            list.Add("Внесение изменений");
                            break;
                        default:
                            list.Add("неизвестный тип");
                            break;
                    }
                }
                return list;
            }
            else
            {
                return string.Empty;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }
}
