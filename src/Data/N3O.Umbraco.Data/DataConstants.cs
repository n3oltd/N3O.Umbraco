namespace N3O.Umbraco.Data;

public static class DataConstants {
    public const string Separator = "//";
    
    public static class ApiNames {
        public const string Content = "Content";
        public const string ContentTypes = "ContentTypes";
        public const string DataTypes = "DataTypes";
        public const string Exports = "Exports";
        public const string Imports = "Imports";
    }

    public static class ContentTypes {
        public const string Csv = "text/csv";
        public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string Zip = "application/zip";
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

    public static class Export {
        public static class Stages {
            public const string Collating = "Collating";
            public const string Complete = "Complete";
            public const string Formatting = "Formatting";
        }
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
        public const string ImportNoticesViewer = "N3O.Umbraco.Data.ImportNoticesViewer";
        public const string ImportDataEditor = "N3O.Umbraco.Data.ImportDataEditor";
    }
    
    public static class SecurityGroups {
        public const string ExportUsers = "Export Users";
        public const string ImportUsers = "Import Users";
    }
    
    public static class Tables {
        public static class Imports {
            public const string Name = "N3O_Imports";
            public const string PrimaryKey = "PK_N3O_Imports";
        }
    }

    public static class Webhooks {
        public static class HookIds {
            public const string Import = nameof(Import);
        }
        
        public static class Headers {
            public const string ContentId = "N3O-Import-Content-Id";
            public const string Name = "N3O-Import-Name";
            public const string Replaces = "N3O-Import-Replaces";
        }
    }
}
