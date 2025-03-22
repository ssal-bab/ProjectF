using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using ClosedXML.Excel;
using System.Text.RegularExpressions;

#nullable enable

public class ExcelDataParserEditor : EditorWindow
{
    private string inputFolderPath = "";
    private List<string> outputFolders = new();
    private Dictionary<string, Dictionary<string,string>> parentClassFieldDic = new();

    [MenuItem("Tools/Excel Data Parser")]
    public static void ShowWindow()
    {
        GetWindow<ExcelDataParserEditor>("Excel Data Parser").minSize = new Vector2(600, 300);
    }

    private void OnEnable()
    {
        inputFolderPath = EditorPrefs.GetString("ExcelDataParser/InputFolderPath", "");
        outputFolders = EditorPrefs.GetString("ExcelDataParser/OutputFolders", "")
                        .Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    private void OnGUI()
    {
        DrawFileSelection("Selected Excel Folder:", ref inputFolderPath, "ExcelDataParser/InputFolderPath");

        GUILayout.Label("Output Folders:", EditorStyles.boldLabel);
        for (int i = outputFolders.Count - 1; i >= 0; i--)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(outputFolders[i]);
            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                outputFolders.RemoveAt(i);
                SaveOutputFolders();
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Output Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Select Output Folder", "", "");
            if (!string.IsNullOrEmpty(path) && !outputFolders.Contains(path))
            {
                outputFolders.Add(path);
                SaveOutputFolders();
            }
        }

        if (GUILayout.Button("Parse To CS"))
        {
            if (!string.IsNullOrEmpty(inputFolderPath) && outputFolders.Count > 0)
                ParseToCS();
            else
                Debug.LogWarning("Please select an Excel folder and output folder.");
        }
    }

    private void DrawFileSelection(string label, ref string path, string prefsKey)
    {
        GUILayout.Label(label, EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label(path);
        if (GUILayout.Button("Select", GUILayout.Width(150)))
        {
            path = EditorUtility.OpenFolderPanel("Select Folder", "", "");
            EditorPrefs.SetString(prefsKey, path);
        }
        GUILayout.EndHorizontal();
    }

    private void SaveOutputFolders() => EditorPrefs.SetString("ExcelDataParser/OutputFolders", string.Join(";", outputFolders));

    private void ParseToCS()
    {
        if (!Directory.Exists(inputFolderPath))
        {
            Debug.LogError($"Folder not found: {inputFolderPath}");
            return;
        }

        string[] excelFiles = Directory.GetFiles(inputFolderPath, "*.xlsx");
        Dictionary<string, Dictionary<string, string>> visited = new();

        foreach (string filePath in excelFiles)
        {
            ProcessExcelFile(filePath, visited);
        }
    }

    // 해결책 : 뽑은 컬럼즈를 딕셔너리에 저장
    private void ProcessExcelFile(string filePath, Dictionary<string, Dictionary<string, string>> visited)
    {
        string fileName = Path.GetFileNameWithoutExtension(filePath);

        if (visited.ContainsKey(fileName)) return; // Cycle prevention

        string parentTableClassName = "";
        string parentClassName = "";
        List<string> attributes = new();

        var columns = ReadExcelColumns(filePath);

        if (columns.Count > 0)
        {
            string header = ReadExcelHeader(filePath);

            if (header != string.Empty)
            {
                string[] metaInfo = header.Split(';');
                foreach (string info in metaInfo)
                {
                    if (info.StartsWith("TableRowBase="))
                    {
                        parentTableClassName = info.Split('=')[1];
                    }
                    if(info.StartsWith("Base="))
                    {
                        parentClassName = info.Split('=')[1];
                    }
                    else if (info.StartsWith("Attribute="))
                    {
                        attributes.Add(info.Split('=')[1]);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No data found in Excel file: " + filePath);
        }

        if(parentTableClassName != string.Empty && !visited.ContainsKey(parentTableClassName))
        {
            string parentFilePath = Directory.GetFiles(inputFolderPath, parentTableClassName + ".xlsx").FirstOrDefault();
            ProcessExcelFile(parentFilePath, visited);
        }
        
        visited.Add(fileName, new());

        foreach(var col in columns)
        {
            if(!IsSuitableField(col[0])) continue;

            visited[fileName].Add(col[0], col[1]);
        }

        Dictionary <string, string>? parentFieldNames = visited.ContainsKey(parentTableClassName) ? visited[parentTableClassName] : null;
        string code = GenerateCSharpCode(fileName, parentTableClassName, parentClassName, columns, parentFieldNames, attributes);

        foreach (string path in outputFolders)
        {
            SaveToFile(path, fileName, code);
        }
    }

    private string ReadExcelHeader(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return string.Empty;
        }

        using var workbook = new XLWorkbook(path);
        var worksheet = workbook.Worksheet(1);

        return worksheet.Cell(1, 1).GetString().Trim();
    }

    private List<List<string>> ReadExcelColumns(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return new();
        }

        using var workbook = new XLWorkbook(path);
        var worksheet = workbook.Worksheet(1);
        return Enumerable.Range(1, worksheet.LastColumnUsed()?.ColumnNumber() ?? 0)
               .Select(col => Enumerable.Range(2, 2).Select(row => worksheet.Cell(row, col).GetString().Trim()).ToList())
               .Where(colData => colData.Count == 2 && !string.IsNullOrEmpty(colData[0]))
               .ToList();
    }


    private void SaveToFile(string outputPath, string className, string content)
    {
        Directory.CreateDirectory(outputPath);
        string filePath = Path.Combine(outputPath, $"{className}.cs");
        File.WriteAllText(filePath, content);
        //Debug.Log($"C# file created: {filePath}");
    }

    private string GenerateCSharpCode(string className, string parentTableClassName, string parentClassName, List<List<string>> columns, Dictionary<string, string>? parentFieldNames, List<string> attributes)
    {
        if (columns.Count == 0) return "// No Excel data found";

        var sb = new StringBuilder();
        sb.AppendLine("using H00N.DataTables;");
        sb.AppendLine("using ProjectF.Datas;");
        sb.AppendLine("");
        sb.AppendLine($"namespace ProjectF.DataTables");
        sb.AppendLine("{");

        foreach (string attribute in attributes)
        {
            sb.AppendLine("    [" + attribute + "]");
        }

        if (parentTableClassName == string.Empty)
        {
            if(parentClassName != string.Empty)
            {
                string path = $"Assets/01.Scripts/SharedCode/DataTable/BaseTable/{parentClassName}.cs";
                Dictionary<string, string> field = !parentClassFieldDic.ContainsKey(parentClassName) ? GetClassFields(path) : parentClassFieldDic[parentClassName];
                WriteFieldWithParent(sb, className, parentClassName, columns, field);
            }
            else
            {
                WriteFields(sb, className, columns);
            }
        }
        else if(parentTableClassName != null)
        {
            if(parentFieldNames == null)
            {
                Debug.LogError("Crazy Error");
                return "// Parent class not found";
            }

            WriteFieldWithTableParent(sb, className, parentTableClassName, columns, parentFieldNames);
        }
        
        return sb.ToString();
    }

    private void WriteFields(StringBuilder sb, string className, List<List<string>> columns)
    {
        sb.AppendLine($"    public partial class {className}Row : DataTableRow");
        sb.AppendLine("    {");
        foreach (var column in columns)
        {
            if (IsSuitableField(column[0]))
                sb.AppendLine($"        public {column[1]} {column[0]};");
        }
        sb.AppendLine("    }");
        sb.AppendLine("");
        sb.AppendLine($"    public partial class {className} : DataTable<{className}Row> {{ }}");
        sb.AppendLine("}");
        
    }

    private void WriteFieldWithTableParent(StringBuilder sb, string className, string parentTableClassName, List<List<string>> columns, Dictionary<string, string> parentFieldNames)
    {
        sb.AppendLine($"    public partial class {className}Row : {parentTableClassName}Row");
        sb.AppendLine("    {");
        foreach (var column in columns)
        {
            if(IsSuitableField(column[0]) && !parentFieldNames.ContainsKey(column[0]))
                sb.AppendLine($"        public {column[1]} {column[0]};");
        }
        sb.AppendLine("    }");
        sb.AppendLine("");
        sb.AppendLine($"    public partial class {className} : {parentTableClassName} {{ }}");
        sb.AppendLine("}");
    }

    private void WriteFieldWithParent(StringBuilder sb, string className, string parentClassName, List<List<string>> columns, Dictionary<string, string> parentFieldNames)
    {
        sb.AppendLine($"    public partial class {className}Row : {parentClassName}Row");
        sb.AppendLine("    {");
        foreach (var column in columns)
        {
            if(IsSuitableField(column[0]) && !parentFieldNames.ContainsKey(column[0]))
                sb.AppendLine($"        public {column[1]} {column[0]};");
        }
        sb.AppendLine("    }");
        sb.AppendLine("");
        sb.AppendLine($"    public partial class {className} : {parentClassName}<{className}Row> {{ }}");
        sb.AppendLine("}");
    }

    private Dictionary<string, string> GetClassFields(string path)
    {
        string code = File.ReadAllText(path);

        // 필드 타입과 필드명을 추출하는 정규식
        string pattern = @"\b([\w<>]+)\s+(\w+)\s*;";
        MatchCollection matches = Regex.Matches(code, pattern);

        Dictionary<string, string> fieldDic = new Dictionary<string, string>();

        foreach (Match match in matches)
        {
            string fieldType = match.Groups[1].Value;
            string fieldName = match.Groups[2].Value;
            fieldDic.Add(fieldName, fieldType);
        }

        return fieldDic;
    }

    private bool IsSuitableField(string field) => 
    !string.IsNullOrEmpty(field) && !field.StartsWith("//") && field != "id";
}
