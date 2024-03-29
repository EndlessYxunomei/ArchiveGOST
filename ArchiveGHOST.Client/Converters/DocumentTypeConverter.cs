﻿using AcrhiveModels;
using System.Globalization;

namespace ArchiveGHOST.Client.Converters
{
    public class DocumentTypeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not DocumentType type)
            {
                return string.Empty;
            }
            else
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
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
    }
}
