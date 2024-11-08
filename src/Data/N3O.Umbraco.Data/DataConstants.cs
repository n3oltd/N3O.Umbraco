namespace N3O.Umbraco.Data;

public static class DataConstants {
    public static readonly string Separator = "//";
    
    public static class ApiNames {
        public const string Content = "Content";
        public const string ContentTypes = "ContentTypes";
        public const string DataTypes = "DataTypes";
        public const string Exports = "Exports";
        public const string Imports = "Imports";
    }

    public static class ContentTypes {
        public static readonly string Csv = "text/csv";
        public static readonly string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static readonly string Zip = "application/zip";
    }
    
    public static class DatePatterns {
        public static readonly string DayMonthYear = "dmy";
        public static readonly string MonthDayYear = "mdy";
        public static readonly string YearMonthDay = "ymd";
    }
    
    public static class DecimalSeparators {
        public static readonly string Comma = ",";
        public static readonly string Point = ".";
    }
    
    public static class Delimiters {
        public const string Comma = ",";
        public const string SemiColon = ";";
    }

    public static class Export {
        public static class Stages {
            public static readonly string Collating = "Collating";
            public static readonly string Complete = "Complete";
            public static readonly string Formatting = "Formatting";
        }
    }

    public static class Limits {
        public static class Columns {
            public static readonly int MaxValues = 10;
        }
    }

    public static class MetadataKeys {
        public static class Cells {
            public static readonly string Title = nameof(Title);
        }
    }

    public static class PropertyEditorAliases {
        public const string ImportNoticesViewer = "N3O.Umbraco.Data.ImportNoticesViewer";
        public const string ImportDataEditor = "N3O.Umbraco.Data.ImportDataEditor";
    }

    public static class SecurityGroups {
        public static class ExportUsers {
            public static readonly string Alias = "exportUsers";
            public static readonly string Name = "Export Users";
        }

        public static class ImportUsers { 
            public static readonly string Alias = "importUsers";
            public static readonly string Name = "Import Users";
    }
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
            public static readonly string ContentId = "N3O-Import-Content-Id";
            public static readonly string Name = "N3O-Import-Name";
            public static readonly string Replaces = "N3O-Import-Replaces";
        }
    }
}
