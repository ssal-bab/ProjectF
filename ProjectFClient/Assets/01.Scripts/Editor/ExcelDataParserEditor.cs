using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ClosedXML.Excel;
using System.IO;
using System.Linq;
using System.Text;

#nullable enable

public class ExcelDataParserEditor : EditorWindow
{
    // private string inputForderPath = "";
    // private List<string> outputFolders = new();

    // [MenuItem("Tools/Excel Data Parser")]
    // public static void ShowWindow()
    // {
    //     GetWindow<ExcelDataParserEditor>("Data Table Exporter").minSize = new Vector2(600, 300);
    // }

    // private void OnEnable()
    // {
    //     inputForderPath = EditorPrefs.GetString("ExcelDataParser/InputForderPath", "");
    //     outputFolders = EditorPrefs.GetString("ExcelDataParser/OutputFolders", "")
    //                     .Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList();
    // }

    // private void OnGUI()
    // {
    //     DrawFileSelection("Selected Excel File:", ref inputForderPath, "ExcelDataParser/inputForderPath");

    //     GUILayout.Label("Output Folders:", EditorStyles.boldLabel);
    //     for (int i = outputFolders.Count - 1; i >= 0; i--)
    //     {
    //         GUILayout.BeginHorizontal();
    //         GUILayout.Label(outputFolders[i]);
    //         if (GUILayout.Button("Remove", GUILayout.Width(100)))
    //         {
    //             outputFolders.RemoveAt(i);
    //             SaveOutputFolders();
    //         }
    //         GUILayout.EndHorizontal();
    //     }

    //     if (GUILayout.Button("Add Output Folder"))
    //     {
    //         string path = EditorUtility.OpenFolderPanel("Select Output Folder", "", "");
    //         if (!string.IsNullOrEmpty(path) && !outputFolders.Contains(path))
    //         {
    //             outputFolders.Add(path);
    //             SaveOutputFolders();
    //         }
    //     }

    //     if (GUILayout.Button("Parse To CS"))
    //     {
    //         if (!string.IsNullOrEmpty(inputForderPath) && outputFolders.Count > 0)
    //             ParseToCS();
    //         else
    //             Debug.LogWarning("Please select an Excel file and output folder.");
    //     }
    // }

    // private void DrawFileSelection(string label, ref string path, string prefsKey, bool allowClear = false)
    // {
    //     GUILayout.Label(label, EditorStyles.boldLabel);
    //     GUILayout.BeginHorizontal();
    //     GUILayout.Label(path);
    //     if (allowClear && GUILayout.Button("Clear", GUILayout.Width(70)))
    //     {
    //         path = "";
    //         EditorPrefs.SetString(prefsKey, path);
    //     }
    //     if (GUILayout.Button("Select", GUILayout.Width(150)))
    //     {
    //         path = EditorUtility.OpenFilePanel("Select File", "", "xlsx");
    //         EditorPrefs.SetString(prefsKey, path);
    //     }
    //     GUILayout.EndHorizontal();
    // }

    // private void SaveOutputFolders() => EditorPrefs.SetString("ExcelDataParser/OutputFolders", string.Join(";", outputFolders));

    // private void ParseToCS()
    // {
    //     if(File.Exists(inputForderPath))
    //     {
    //         Debug.LogError("A file with the same name already exists in the directory path you entered.");
    //         return;
    //     }

    //     Dictionary<string, string>? parentFieldNames = !string.IsNullOrEmpty(parentFilePath)
    //         ? ReadExcelColumns(parentFilePath).ToDictionary(col => col[0], col => col[1])
    //         : null;

    //     var columns = ReadExcelColumns(inputForderPath);
    //     string className = Path.GetFileNameWithoutExtension(inputForderPath);
    //     string code = GenerateCSharpCode(className, columns, parentFieldNames);

    //     foreach (string path in outputFolders)
    //     {
    //         SaveToFile(path, className, code);
    //     }
    // }

    // private void SaveToFile(string outputPath, string className, string content)
    // {
    //     Directory.CreateDirectory(outputPath);
    //     string filePath = Path.Combine(outputPath, $"{className}.cs");
    //     File.WriteAllText(filePath, content);
    //     Debug.Log($"C# file created: {filePath}");
    // }

    // private string GenerateCSharpCode(string className, List<List<string>> columns, Dictionary<string, string>? parentFieldNames)
    // {
    //     if (columns.Count == 0) return "// No Excel data found";

    //     var sb = new StringBuilder();
    //     sb.AppendLine("using H00N.DataTables;");
    //     sb.AppendLine("using ProjectF.Datas;");
    //     sb.AppendLine("\nnamespace ProjectF.DataTables");
    //     sb.AppendLine("{");
        
    //     if (parentFieldNames == null)
    //         WriteFields(sb, className, columns);
    //     else
    //         WriteFieldsWithParent(sb, className, columns, parentFieldNames);

    //     sb.AppendLine("}");
    //     return sb.ToString();
    // }

    // private void WriteFields(StringBuilder sb, string className, List<List<string>> columns)
    // {
    //     sb.AppendLine($"    public partial class {className}Row : DataTableRow");
    //     sb.AppendLine("    {");
    //     foreach (var column in columns)
    //     {
    //         if (!string.IsNullOrEmpty(column[0]) && !column[0].StartsWith("//") && column[0] != "id")
    //             sb.AppendLine($"        public {column[1]} {column[0]};");
    //     }
    //     sb.AppendLine("    }");
    //     sb.AppendLine($"    public partial class {className} : DataTable<{className}Row> {{ }}");
    // }

    // private void WriteFieldsWithParent(StringBuilder sb, string className, List<List<string>> columns, Dictionary<string, string> parentFieldNames)
    // {
    //     string parentClassName = Path.GetFileNameWithoutExtension(parentFilePath);
    //     sb.AppendLine($"    public partial class {className}Row : {parentClassName}Row");
    //     sb.AppendLine("    {");
    //     foreach (var column in columns)
    //     {
    //         if (!string.IsNullOrEmpty(column[0]) && !column[0].StartsWith("//") && column[0] != "id" && !parentFieldNames.ContainsKey(column[0]))
    //             sb.AppendLine($"        public {column[1]} {column[0]};");
    //     }
    //     sb.AppendLine("    }");
    //     sb.AppendLine($"    public partial class {className} : {parentClassName} {{ }}");
    // }

    // private List<List<string>> ReadExcelColumns(string path)
    // {
    //     if (!File.Exists(path))
    //     {
    //         Debug.LogError($"File not found: {path}");
    //         return new();
    //     }

    //     using var workbook = new XLWorkbook(path);
    //     var worksheet = workbook.Worksheet(1);
    //     return Enumerable.Range(1, worksheet.LastColumnUsed()?.ColumnNumber() ?? 0)
    //            .Select(col => Enumerable.Range(2, 2).Select(row => worksheet.Cell(row, col).GetString().Trim()).ToList())
    //            .Where(colData => colData.Count == 2 && !string.IsNullOrEmpty(colData[0]))
    //            .ToList();
    // }
}