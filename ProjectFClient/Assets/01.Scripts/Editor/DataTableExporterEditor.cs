using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using ClosedXML.Excel;
using System;

namespace H00N.DataTables.Editors
{
    public class DataTableExporterEditor : EditorWindow
    {
        private static readonly Dictionary<string, string> TypeMap = new Dictionary<string, string>() {
            ["int"] = typeof(int).FullName,
            ["long"] = typeof(long).FullName,
            ["float"] = typeof(float).FullName,
            ["double"] = typeof(int).FullName,
            ["int[]"] = typeof(int[]).FullName,
            ["long[]"] = typeof(long[]).FullName,
            ["float[]"] = typeof(float[]).FullName,
            ["double[]"] = typeof(double[]).FullName,
            ["string[]"] = typeof(string[]).FullName,
        };

        private string inputFolderPath = "";  
        private List<string> outputFolderPathList = new List<string>();

        [MenuItem("Tools/Data Table Exporter")]
        public static void ShowWindow()
        {
            DataTableExporterEditor window = GetWindow<DataTableExporterEditor>("Data Table Exporter");
            window.minSize = new Vector2(600, 300);
            window.maxSize = new Vector2(1000, 500);
        }

        private void OnEnable()
        {
            inputFolderPath = EditorPrefs.GetString("DataTableExporter/InputFolderPathCache", "");
            outputFolderPathList = EditorPrefs.GetString("DataTableExporter/OutputFolderPathCache", "").Split(";").ToList();
        }

        private void OnGUI()
        {
            GUILayout.Label("Selected Excel Folder:", EditorStyles.boldLabel);
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(inputFolderPath);
            if (GUILayout.Button("Select Excel Folder", GUILayout.Width(150)))
            {
                inputFolderPath = EditorUtility.OpenFolderPanel("Select Excel Folder", "", "");
                EditorPrefs.SetString("DataTableExporter/InputFolderPathCache", inputFolderPath);
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
                    EditorPrefs.SetString("DataTableExporter/OutputFolderPathCache", string.Join(";", outputFolderPathList));
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
                    EditorPrefs.SetString("DataTableExporter/OutputFolderPathCache", string.Join(";", outputFolderPathList));
                }
            }

            GUILayout.Space(10);

            // 변환 버튼
            if (GUILayout.Button("Export Table"))
            {
                if (!string.IsNullOrEmpty(inputFolderPath) && outputFolderPathList.Count > 0)
                {
                    ExportTable(inputFolderPath, outputFolderPathList);
                }
                else
                {
                    Debug.LogWarning("Please select a folder first.");
                }
            }
        }

        private void ExportTable(string folderPath, List<string> outputPathList)
        {
            string[] excelFilePaths = Directory.GetFiles(folderPath, "*.xlsx");
            var allJsonData = new Dictionary<string, string>();  // 모든 Excel 파일의 데이터를 저장할 리스트

            foreach (var excelFilePath in excelFilePaths)
            {
                using var workbook = new XLWorkbook(excelFilePath);
                foreach(var worksheet in workbook.Worksheets)
                {
                    // 두번째, 세번째, 네번째 이상의 행을 분리합니다.
                    var headerRow = worksheet.Row(2); // 두번째 행 (변수 이름들)
                    var typeRow = worksheet.Row(3);   // 세번째 행 (변수 타입)
                    int columnCount = worksheet.LastColumnUsed().ColumnNumber(); // 사용된 마지막 열
                    var table = new Dictionary<string, Dictionary<string, object>>();

                    for (int i = 4; i <= worksheet.LastRowUsed().RowNumber(); i++)  // 4번째 행부터 실제 값들이 시작됨
                    {
                        var dataRow = worksheet.Row(i);
                        var record = new Dictionary<string, object>();
                        string key = dataRow.Cell(1).GetString(); // 첫 번째 열을 키로 사용

                        if (string.IsNullOrEmpty(key)) continue; // 키가 없으면 건너뜀

                        for (int j = 1; j <= columnCount; j++)
                        {
                            string columnName = headerRow.Cell(j).GetString();
                            string typeString = typeRow.Cell(j).GetString();
                            string cellValue = dataRow.Cell(j).GetString();

                            if (string.IsNullOrEmpty(columnName) || columnName.StartsWith("//"))
                                continue;

                            // 타입 변환
                            if(TypeMap.TryGetValue(typeString, out string typeName))
                            {
                                try {
                                    Type type = Type.GetType(typeName);
                                    if (type == null)
                                        throw new Exception($"Type mismath.");

                                    record[columnName] = JsonConvert.DeserializeObject(cellValue, type);
                                }
                                catch (Exception err) {
                                    Debug.LogError($"TableName: {worksheet.Name}, ColumnName: {columnName}, TypeString: {typeString}, CellValue: {cellValue}\n{err}");
                                    record[columnName] = cellValue;
                                }
                            }
                            else
                                record[columnName] = cellValue;
                        }

                        table[key] = record;  // 하나의 레코드를 리스트에 추가
                    }
                    
                    string fileName = worksheet.Name;
                    string jsonData = JsonConvert.SerializeObject(table, Formatting.None);
                    allJsonData.Add(fileName, jsonData);
                }
            }

            // JSON 형식으로 출력
            string jsonOutput = JsonConvert.SerializeObject(allJsonData, Formatting.None);
            Debug.Log(jsonOutput);

            foreach(var row in allJsonData)
            {
                foreach(string outputPath in outputPathList)
                {
                    if(string.IsNullOrEmpty(outputPath))
                        continue;

                    string path = Path.Combine(outputPath, $"{row.Key}.json");
                    File.WriteAllText(path, row.Value);
                }
            }

            foreach (string outputPath in outputPathList)
            {
                if (string.IsNullOrEmpty(outputPath))
                    continue;
                string path = Path.Combine(outputPath, $"DataTableJson.json");
                File.WriteAllText(path, jsonOutput);
                Debug.Log("JSON file saved to: " + path);
            }

            AssetDatabase.Refresh();
        }
    }
}
