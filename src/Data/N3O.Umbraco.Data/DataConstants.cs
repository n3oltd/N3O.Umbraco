namespace N3O.Umbraco.Data {
    public static class DataConstants {
        public static class ApiNames {
            public const string Content = "Content";
            public const string Export = "Export";
            public const string Import = "Import";
        }

        public static class Columns {
            public const string Replaces = "Replaces";
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

        public static class PropertyEditorAliases {
            public const string ImportErrorsViewer = "N3O.Umbraco.Data.ImportErrorsViewer";
            public const string ImportFieldEditor = "N3O.Umbraco.Data.ImportFieldEditor";
        }
        
        public static class Tables {
            public static class Imports {
                public const string Name = "N3O_Imports";
                public const string PrimaryKey = "PK_N3O_Imports";
            }
        }
    }
}