using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ClosedXML.Excel;
using System.IO;
using System.Linq;
using System.Text;

public class ExcelDataParserEditor : EditorWindow
{
    private string inputFolderPath = "";  
    private List<string> outputFolderPathList = new List<string>();

    [MenuItem("Tools/Excel Data Parser")]
     public static void ShowWindow()
     {
        ExcelDataParserEditor window = GetWindow<ExcelDataParserEditor>("Data Table Exporter");
        window.minSize = new Vector2(600, 300);
        window.maxSize = new Vector2(1000, 500);
        
    }

    private void OnEnable()
    {
        inputFolderPath = EditorPrefs.GetString("ExcelDataParser/InputFolderPathCache", "");
        outputFolderPathList = EditorPrefs.GetString("ExcelDataParser/OutputFolderPathCache", "").Split(";").ToList();
    }

    private void OnGUI()
    {
        GUILayout.Label("Selected Excel File:", EditorStyles.boldLabel);
            
        GUILayout.BeginHorizontal();
        GUILayout.Label(inputFolderPath);
        if (GUILayout.Button("Select Excel File", GUILayout.Width(150)))
        {
            inputFolderPath = EditorUtility.OpenFilePanel("Select Excel File", "", "xlsx");
            EditorPrefs.SetString("ExcelDataParser/InputFolderPathCache", inputFolderPath);
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("Output Folders:", EditorStyles.boldLabel);
        
        // 출력 폴더 리스트
        for (int i = 0; i < outputFolderPathList.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(outputFolderPathList[i]);
            if (GUILayout.Button("Remove", GUILayout.Width(150)))
            {
                outputFolderPathList.RemoveAt(i);
                EditorPrefs.SetString("ExcelDataParser/OutputFolderPathCache", string.Join(";", outputFolderPathList));
                GUILayout.EndHorizontal();
                break;
            }
            GUILayout.EndHorizontal();
        }

        // 출력 폴더 추가 버튼
        if (GUILayout.Button("Add Output Folder"))
        {
            string newPath = EditorUtility.OpenFolderPanel("Select Output Folder", "", "");
            if (!string.IsNullOrEmpty(newPath) && !outputFolderPathList.Contains(newPath))
            {
                outputFolderPathList.Add(newPath);
                EditorPrefs.SetString("ExcelDataParser/OutputFolderPathCache", string.Join(";", outputFolderPathList));
            }
        }

        GUILayout.Space(10);

        // 변환 버튼
        if (GUILayout.Button("Parsing To CS"))
        {
            if (!string.IsNullOrEmpty(inputFolderPath) && outputFolderPathList.Count > 0)
            {
                ParsingToCS(inputFolderPath, outputFolderPathList);
            }
            else
            {
                Debug.LogWarning("Please select a folder first.");
            }
        }
    }

    private void ParsingToCS(string folderPath, List<string> outputPathList)
    {
        List<string[]> data = ReadExcelColumns(folderPath);

        foreach(var d in data)
        {
            Debug.Log(string.Join(",", d));
        }

        string className = Path.GetFileName(folderPath).Split('.')[0];
        string code = GenerateCSharpCode(className, data);

        foreach(var path in outputPathList)
        {
            Debug.Log(path);
            SaveToFile(path, className, code);
        }
    }

    private void SaveToFile(string outputPath, string className, string content)
    {
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        string filePath = Path.Combine(outputPath, $"{className}.cs");
        File.WriteAllText(filePath, content);
        Debug.Log($"C# 파일 생성 완료: {filePath}");
    }

    private string GenerateCSharpCode(string className, List<string[]> columns)
    {
        if (columns.Count == 0) return "// 엑셀 데이터 없음";

        string baseClass = "DataTableRow";

        var sb = new StringBuilder();
        sb.AppendLine("using H00N.DataTables;");
        sb.AppendLine("using ProjectF.Datas;");
        sb.AppendLine("namespace ProjectF.DataTables");
        sb.AppendLine("{");
        sb.AppendLine($"    public class T{className} : {baseClass}");
        sb.AppendLine("    {");

        foreach (var column in columns)
        {
            string fieldName = column[0];

            // 주석(`//`)으로 시작하거나 빈 문자열이면 무시
            if (string.IsNullOrEmpty(fieldName) || fieldName.StartsWith("//"))
                continue;

            string fieldType = column[1]; // 데이터 타입 판별

            if(fieldName =="id")
            {
                if(fieldType == "int")
                {
                    continue;
                }
                else
                {
                    Debug.LogError("int가 아닌 id 발견견");
                    continue;
                }
            }

            sb.AppendLine($"        public {fieldType} {fieldName};");
        }

        sb.AppendLine("    }\n");
        sb.AppendLine($"    public class {className.Replace("Row", "")} : DataTable<{className}> {{ }}");
        sb.AppendLine("}");

        return sb.ToString();
    }

    private List<string[]> ReadExcelColumns(string path)
    {
        List<string[]> columns = new List<string[]>();

        if (!File.Exists(path))
        {
            Debug.LogError("파일을 찾을 수 없습니다: " + path);
            return columns;
        }

        using (var workbook = new XLWorkbook(path))
        {
            var worksheet = workbook.Worksheet(1); // 첫 번째 시트
            int rowCount = 3;   // 헤더가 마무리 되는 행
            int colCount = worksheet.LastColumnUsed().ColumnNumber(); // 마지막 사용된 열 번호

            // 각 열을 리스트에 저장
            for (int col = 1; col <= colCount; col++)
            {
                List<string> columnData = new List<string>();

                for (int row = 2; row <= rowCount; row++)
                {
                    Debug.Log($"{row}. {rowCount}");
                    columnData.Add(worksheet.Cell(row, col).GetString()); // 해당 열의 모든 행 데이터 추가
                }

                columns.Add(columnData.ToArray());
            }
        }

        return columns;
    }
}
