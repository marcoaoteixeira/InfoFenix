﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InfoFenix.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SQLs {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SQLs() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("InfoFenix.Resources.SQLs", typeof(SQLs).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS [migrations] (
        ///    [version]   TEXT        PRIMARY KEY NOT NULL,
        ///    [date]      DATETIME                NOT NULL
        ///);
        ///
        ///-- Seed first migration
        ///INSERT INTO [migrations] VALUES (&apos;00000000000000&apos;, &apos;0000-01-01 00:00:00&apos;);
        ///
        ///CREATE TABLE IF NOT EXISTS [document_directories] (
        ///    [document_directory_id] INTEGER PRIMARY KEY AUTOINCREMENT,
        ///    [code]                  TEXT                NOT NULL,
        ///    [label]                 TEXT                NOT NULL,
        ///    [path]               [rest of string was truncated]&quot;;.
        /// </summary>
        public static string CreateDatabaseSchema {
            get {
                return ResourceManager.GetString("CreateDatabaseSchema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS [migrations] (
        ///    [version]   TEXT        PRIMARY KEY NOT NULL,
        ///    [date]      DATETIME                NOT NULL
        ///);.
        /// </summary>
        public static string CreateMigrationTableSchema {
            get {
                return ResourceManager.GetString("CreateMigrationTableSchema", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    [version],
        ///    [date]
        ///FROM [migrations]
        ///WHERE
        ///    [version] = @version;.
        /// </summary>
        public static string GetAppliedMigration {
            get {
                return ResourceManager.GetString("GetAppliedMigration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    [document_id],
        ///    [document_directory_id],
        ///    [code],
        ///    [content],
        ///    [payload],
        ///    [path],
        ///    [last_write_time],
        ///    [index]
        ///FROM [documents]
        ///WHERE
        ///    (@document_id IS NULL OR ([document_id] = @document_id))
        ///AND (@code IS NULL OR ([code] = @code)).
        /// </summary>
        public static string GetDocument {
            get {
                return ResourceManager.GetString("GetDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    COUNT([document_id])
        ///FROM [documents]
        ///WHERE
        ///    [document_directory_id] = @document_directory_id
        ///AND (@index IS NULL OR ([index] = @index)).
        /// </summary>
        public static string GetDocumentCountByDocumentDirectory {
            get {
                return ResourceManager.GetString("GetDocumentCountByDocumentDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    [document_directories].[document_directory_id],
        ///    [document_directories].[code],
        ///    [document_directories].[label],
        ///    [document_directories].[path],
        ///    [document_directories].[position],
        ///    (SELECT
        ///        COUNT([documents].[document_id])
        ///     FROM [documents]
        ///     WHERE
        ///        [documents].[document_directory_id] = [document_directories].[document_directory_id]
        ///    ) AS [total_documents]
        ///FROM [document_directories]
        ///WHERE
        ///    [document_directories].[document_directory_id] = @ [rest of string was truncated]&quot;;.
        /// </summary>
        public static string GetDocumentDirectory {
            get {
                return ResourceManager.GetString("GetDocumentDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    [document_id],
        ///    [document_directory_id],
        ///    [code],
        ///    [content],
        ///    [path],
        ///    [last_write_time],
        ///    [index]
        ///FROM [documents]
        ///WHERE
        ///    (@document_id IS NULL OR ([document_id] = @document_id))
        ///AND (@code IS NULL OR ([code] = @code)).
        /// </summary>
        public static string GetDocumentSQLWithoutPayload {
            get {
                return ResourceManager.GetString("GetDocumentSQLWithoutPayload", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    [version],
        ///    [date]
        ///FROM [migrations]
        ///WHERE
        ///    (@version IS NULL OR ([version] = @version));.
        /// </summary>
        public static string ListAppliedMigrations {
            get {
                return ResourceManager.GetString("ListAppliedMigrations", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    [document_directories].[document_directory_id],
        ///    [document_directories].[code],
        ///    [document_directories].[label],
        ///    [document_directories].[path],
        ///    [document_directories].[position],
        ///    (SELECT
        ///        COUNT([documents].[document_id])
        ///     FROM [documents]
        ///     WHERE
        ///        [documents].[document_directory_id] = [document_directories].[document_directory_id]
        ///    ) AS [total_documents]
        ///FROM [document_directories]
        ///WHERE
        ///    (@code IS NULL OR ([document_directories].[code] L [rest of string was truncated]&quot;;.
        /// </summary>
        public static string ListDocumentDirectories {
            get {
                return ResourceManager.GetString("ListDocumentDirectories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    [documents].[document_id],
        ///    [documents].[document_directory_id],
        ///    [documents].[code],
        ///    [documents].[path],
        ///    [documents].[last_write_time],
        ///    [documents].[index]
        ///FROM [documents]
        ///    INNER JOIN [document_directories] ON [document_directories].[document_directory_id] = [documents].[document_directory_id]
        ///WHERE
        ///    [document_directories].[document_directory_id] = @document_directory_id;.
        /// </summary>
        public static string ListDocumentsByDocumentDirectoryNoContent {
            get {
                return ResourceManager.GetString("ListDocumentsByDocumentDirectoryNoContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT CASE
        ///    WHEN EXISTS(SELECT [name] FROM [sqlite_master] WHERE [type] = @type AND [name] = @name) THEN 1
        ///    ELSE 0
        ///END AS [exists];.
        /// </summary>
        public static string ObjectExists {
            get {
                return ResourceManager.GetString("ObjectExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT
        ///    [documents].[document_id],
        ///    [documents].[document_directory_id],
        ///    [documents].[code],
        ///    [documents].[content],
        ///    [documents].[payload],
        ///    [documents].[path],
        ///    [documents].[last_write_time],
        ///    [documents].[index]
        ///FROM [documents]
        ///    INNER JOIN [document_directories] ON [document_directories].[document_directory_id] = [documents].[document_directory_id]
        ///WHERE
        ///    [document_directories].[document_directory_id] = @document_directory_id
        ///AND (@index IS NULL OR ([documents [rest of string was truncated]&quot;;.
        /// </summary>
        public static string PaginateDocumentsByDocumentDirectory {
            get {
                return ResourceManager.GetString("PaginateDocumentsByDocumentDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE
        ///FROM [documents]
        ///WHERE
        ///    [document_id] = @document_id;.
        /// </summary>
        public static string RemoveDocument {
            get {
                return ResourceManager.GetString("RemoveDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM [documents]
        ///WHERE
        ///    [document_directory_id] = @document_directory_id;
        ///
        ///DELETE FROM [document_directories]
        ///WHERE
        ///    [document_directory_id] = @document_directory_id;.
        /// </summary>
        public static string RemoveDocumentDirectory {
            get {
                return ResourceManager.GetString("RemoveDocumentDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR REPLACE INTO [documents] (
        ///    [document_id],
        ///    [document_directory_id],
        ///    [code],
        ///    [content],
        ///    [payload],
        ///    [path],
        ///    [last_write_time],
        ///    [index]
        ///) VALUES (
        ///    @document_id,
        ///    @document_directory_id,
        ///    @code,
        ///    @content,
        ///    @payload,
        ///    @path,
        ///    @last_write_time,
        ///    @index
        ///);
        ///SELECT MAX([document_id]) FROM [documents];.
        /// </summary>
        public static string SaveDocument {
            get {
                return ResourceManager.GetString("SaveDocument", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR REPLACE INTO [document_directories] (
        ///    [document_directory_id],
        ///    [code],
        ///    [label],
        ///    [path],
        ///    [position]
        ///) VALUES (
        ///    @document_directory_id,
        ///    @code,
        ///    @label,
        ///    @path,
        ///    @position
        ///);
        ///SELECT MAX([document_directory_id]) FROM [document_directories].
        /// </summary>
        public static string SaveDocumentDirectory {
            get {
                return ResourceManager.GetString("SaveDocumentDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT OR REPLACE INTO [migrations] (
        ///    [version],
        ///    [date]
        ///) VALUES (
        ///    @version,
        ///    @date
        ///);.
        /// </summary>
        public static string SaveMigration {
            get {
                return ResourceManager.GetString("SaveMigration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE [documents] SET
        ///    [index] = 1
        ///WHERE
        ///    [document_id] = @document_id;.
        /// </summary>
        public static string SetDocumentIndex {
            get {
                return ResourceManager.GetString("SetDocumentIndex", resourceCulture);
            }
        }
    }
}