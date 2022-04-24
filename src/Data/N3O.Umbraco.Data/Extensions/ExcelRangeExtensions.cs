using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Exceptions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using ExcelNumberFormat = N3O.Umbraco.Data.Models.ExcelNumberFormat;

namespace N3O.Umbraco.Data.Extensions {
    public static class ExcelRangeExtensions {
        public static void ApplyFormatting(this ExcelRange cell, ExcelFormatting formatting) {
            ApplyFontWeight(cell, formatting.FontWeight);
            ApplyHorizontalAlignment(cell, formatting.HorizontalAlignment);
            ApplyNumberFormat(cell, formatting.NumberFormat);
        }

        private static void ApplyFontWeight(ExcelRange cell, FontWeight fontWeight) {
            switch (fontWeight) {
                case FontWeight.Bold:
                    cell.Style.Font.Bold = true;
                    break;

                case FontWeight.Normal:
                    break;

                default:
                    throw new InvalidOperationException($"Unrecognised value {fontWeight}");
            }
        }

        private static void ApplyHorizontalAlignment(ExcelRange cell, HorizontalAlignment alignment) {
            switch (alignment) {
                case HorizontalAlignment.Right:
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    break;

                case HorizontalAlignment.Centre:
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    break;

                case HorizontalAlignment.Left:
                    break;

                default:
                    throw UnrecognisedValueException.For(alignment);
            }
        }

        private static void ApplyNumberFormat(ExcelRange cell, ExcelNumberFormat numberFormat) {
            if (numberFormat != null) {
                cell.Style.Numberformat.Format = numberFormat.Pattern;
            }
        }
    }
}