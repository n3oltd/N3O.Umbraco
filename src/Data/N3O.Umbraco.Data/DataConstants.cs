namespace N3O.Umbraco.Data {
    public static class DataConstants {
        public static class ApiNames {
            public const string Content = "Content";
            public const string Export = "Export";
            public const string Import = "Import";
        }

        public static class ContentTypes {
            public const string Csv = "text/csv";
            public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }
        
        public static class DatePatterns {
            public const string DayMonthYear = "dmy";
            public const string MonthDayYear = "mdy";
            public const string YearMonthDay = "ymd";
        }
        
        public static class DecimalSeparators {
            public const string Comma = ",";
            public const string Point = ".";
        }

        public static class Limits {
            public static class Columns {
                public const int MaxValues = 10;
            }
        }

        public static class MetadataKeys {
            public static class Cells {
                public const string Title = nameof(Title);
            }
        }

        public static class PropertyEditorAliases {
            public const string ImportErrorsViewer = "N3O.Umbraco.Data.ImportErrorsViewer";
            public const string ImportFieldsEditor = "N3O.Umbraco.Data.ImportFieldsEditor";
        }
        
        public static class Tables {
            public static class Imports {
                public const string Name = "N3O_Imports";
                public const string PrimaryKey = "PK_N3O_Imports";
            }
        }
    }
}