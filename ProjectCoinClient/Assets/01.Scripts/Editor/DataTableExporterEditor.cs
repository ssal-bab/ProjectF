using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace H00N.DataTables.Editors
{
    public class DataTableExporterEditor : EditorWindow
    {
        private string inputFolderPath = "";  
        private string outputFolderPath = "";  

        [MenuItem("Tools/Data Table Exporter")]
        public static void ShowWindow()
        {
            GetWindow<DataTableExporterEditor>("Data Table Exporter");
        }

        private void OnEnable()
        {
            inputFolderPath = EditorPrefs.GetString("DataTableExporter/InputFolderPathCache", "");
            outputFolderPath = EditorPrefs.GetString("DataTableExporter/OutputFolderPathCache", "");
        }

        private void OnGUI()
        {
            GUILayout.Label("Data Table Exporter", EditorStyles.boldLabel);

            // 입력 폴더 선택 버튼
            if (GUILayout.Button("Select TSV Folder"))
            {
                inputFolderPath = EditorUtility.OpenFolderPanel("Select TSV Folder", "", "");
                EditorPrefs.SetString("DataTableExporter/InputFolderPathCache", inputFolderPath);
            }

            GUILayout.Label("Selected TSV Folder: " + inputFolderPath);

            // 출력 폴더 선택 버튼
            if (GUILayout.Button("Select Output Folder"))
            {
                outputFolderPath = EditorUtility.OpenFolderPanel("Select Output Folder", "", "");
                EditorPrefs.SetString("DataTableExporter/OutputFolderPathCache", outputFolderPath);
            }

            GUILayout.Label("Selected Output Folder: " + outputFolderPath);

            // 변환 버튼
            if (GUILayout.Button("Export Table"))
            {
                if (!string.IsNullOrEmpty(inputFolderPath) && !string.IsNullOrEmpty(outputFolderPath))
                {
                    ExportTable(inputFolderPath, outputFolderPath);
                }
                else
                {
                    Debug.LogWarning("Please select a folder first.");
                }
            }
        }

        private void ExportTable(string folderPath, string outputPath)
        {
            string[] tsvFilePaths = Directory.GetFiles(folderPath, "*.tsv");
            var allJsonData = new Dictionary<string, string>();  // 모든 TSV 파일의 데이터를 저장할 리스트

            foreach (var tsvFilePath in tsvFilePaths)
            {
                var fileLines = File.ReadAllLines(tsvFilePath);

                // 첫 번째, 두 번째, 세 번째 이상의 행을 분리합니다.
                var headers = fileLines[0].Split('\t');  // 첫 번째 행 (변수 이름들)
                var types = fileLines[1].Split('\t');    // 두 번째 행 (변수 타입)
                var table = new Dictionary<string, Dictionary<string, object>>();

                for (int i = 2; i < fileLines.Length; i++)  // 3번째 행부터 실제 값들이 시작됨
                {
                    var data = fileLines[i].Split('\t');
                    var record = new Dictionary<string, object>();

                    // 나머지 열을 변수명과 값으로 매칭하여 JSON 형식으로 구성
                    for (int j = 0; j < headers.Length; j++)
                    {
                        string key = headers[j];
                        string value = data[j];
                        if(key.StartsWith("//"))
                            continue;

                        // 필요한 경우 타입에 따라 값 처리 (예: 숫자, 문자열 등)
                        if (types[j] == "int")
                        {
                            record[key] = int.Parse(value);
                        }
                        else if (types[j] == "float")
                        {
                            record[key] = float.Parse(value);
                        }
                        else if (types[j] == "bool")
                        {
                            record[key] = bool.Parse(value);
                        }
                        else
                        {
                            record[key] = value;  // 기본적으로 문자열로 처리
                        }
                    }

                    table.Add(data[0], record);  // 하나의 레코드를 리스트에 추가
                }
                
                string fileName = Path.GetFileNameWithoutExtension(tsvFilePath);
                string jsonData = JsonConvert.SerializeObject(table, Formatting.None);
                allJsonData.Add(fileName, jsonData);
            }

            // JSON 형식으로 출력
            string jsonOutput = JsonConvert.SerializeObject(allJsonData, Formatting.None);
            Debug.Log(jsonOutput);

            foreach(var row in allJsonData)
            {
                string path = Path.Combine(outputPath, $"{row.Key}.json");
                File.WriteAllText(path, row.Value);
            }

            if (!string.IsNullOrEmpty(outputPath))
            {
                string path = Path.Combine(outputPath, $"DataTableJson.json");
                File.WriteAllText(path, jsonOutput);
                Debug.Log("JSON file saved to: " + path);
            }

            AssetDatabase.Refresh();
        }
    }
}
