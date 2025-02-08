using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace H00N.DataTables.Editors
{
    public class DataTableExporterEditor : EditorWindow
    {
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
            GUILayout.Label("Selected TSV Folder:", EditorStyles.boldLabel);
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(inputFolderPath);
            if (GUILayout.Button("Select TSV Folder", GUILayout.Width(150)))
            {
                inputFolderPath = EditorUtility.OpenFolderPanel("Select TSV Folder", "", "");
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
