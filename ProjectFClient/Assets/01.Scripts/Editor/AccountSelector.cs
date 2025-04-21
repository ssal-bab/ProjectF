using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using ProjectF.Networks;
using ProjectF;
using System;
using Newtonsoft.Json;

public class AccountSelector : EditorWindow
{
    // 드롭다운에 표시할 문자열 배열
    private List<string> stringOptions;
    
    // 드롭다운에서 선택된 인덱스 (원본 배열 기준)
    private int selectedIndex = 0;
    
    // 검색어를 입력받기 위한 변수
    private string searchText = "";

    private string newOptionText = "";

    private string deleteOptionText = "";

    [Serializable]
    private class OptionListWrapper { public List<string> options; }

    private const string OPTIONS_KEY   = "DropdownEditorWindow_OptionsList";
    private const string SELECT_KEY    = "DropdownEditorWindow_SelectedOption";

    // 메뉴 항목을 통해 에디터 창 열기
    [MenuItem("Tools/Account Selector")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(AccountSelector), false, "Account Selector");
        window.Show();
    }

    // 에디터 창이 활성화될 때 이전에 저장된 옵션 불러오기
    private void OnEnable()
    {
        stringOptions = new();

        OptionListWrapper wrapper = JsonUtility.FromJson<OptionListWrapper>(EditorPrefs.GetString(OPTIONS_KEY, ""));
        stringOptions = wrapper.options;
    }

    // 에디터 창 GUI 그리기
    private void OnGUI()
    {
        // 검색어 입력 필드 추가
        searchText = EditorGUILayout.TextField("Search", searchText);

        // 검색어에 따라 옵션 필터링
        List<int> filteredIndices = new List<int>();    // 원본 배열의 인덱스와 매핑
        List<string> filteredOptions = new List<string>(); // 실제 드롭다운에 표시할 옵션

        for (int i = 0; i < stringOptions.Count; i++)
        {
            // 검색어가 비어있거나, 옵션에 검색어가 포함되어 있으면 추가
            if (string.IsNullOrEmpty(searchText) || stringOptions[i].ToLower().Contains(searchText.ToLower()))
            {
                filteredIndices.Add(i);
                filteredOptions.Add(stringOptions[i]);
            }
        }

        // 필터링된 결과가 있을 경우에만 드롭다운 표시
        if (filteredOptions.Count > 0)
        {
            int localSelectedIndex = 0;
            // 현재 선택된 옵션(원본 배열 기준)이 필터링 결과에 포함되어 있는지 확인
            int indexInFiltered = filteredIndices.IndexOf(selectedIndex);
            if (indexInFiltered != -1)
            {
                localSelectedIndex = indexInFiltered;
            }
            else
            {
                // 현재 선택된 옵션이 필터링 결과에 없으면 첫 번째 옵션을 기본 선택
                localSelectedIndex = 0;
                selectedIndex = filteredIndices[0];
            }
            
            // 드롭다운 UI를 통해 선택한 인덱스(필터링된 배열 기준)를 받아옴
            localSelectedIndex = EditorGUILayout.Popup("Options", localSelectedIndex, filteredOptions.ToArray());
            // 원본 배열의 인덱스 값으로 업데이트
            selectedIndex = filteredIndices[localSelectedIndex];
        }
        else
        {
            EditorGUILayout.LabelField("검색된 결과가 없습니다.");
        }

        // 선택 저장 버튼
        if (GUILayout.Button("Select"))
        {
            // EditorPrefs에 선택한 옵션 저장 (에디터 재실행 후에도 데이터 유지)
            GameSetting.LastLoginUserID = stringOptions[selectedIndex];
            Debug.Log("Selected Account: " + stringOptions[selectedIndex]);
        }
        EditorGUILayout.Space();

        // 선택 저장 버튼
        if (GUILayout.Button("Add Current Account"))
        {
            SaveOptions(GameSetting.LastLoginUserID);
        }
        EditorGUILayout.Space();

        // --- 새 옵션 추가 UI ---
        EditorGUILayout.BeginHorizontal();
        {
            newOptionText = EditorGUILayout.TextField("새 옵션", newOptionText);
            GUI.enabled = !string.IsNullOrEmpty(newOptionText);
            if (GUILayout.Button("옵션 추가", GUILayout.Width(80)))
            {
                // 중복 방지
                if (!stringOptions.Contains(newOptionText))
                {
                    SaveOptions(newOptionText);
                    newOptionText = "";
                }
                else
                {
                    Debug.LogWarning($"이미 같은 옵션이 존재합니다: {newOptionText}");
                }
            }
            GUI.enabled = true;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        // --- 새 옵션 추가 UI ---
        EditorGUILayout.BeginHorizontal();
        {
            deleteOptionText = EditorGUILayout.TextField("삭제 옵션", deleteOptionText);
            GUI.enabled = !string.IsNullOrEmpty(deleteOptionText);
            if (GUILayout.Button("옵션 삭제", GUILayout.Width(80)))
            {
                // 중복 방지
                if (stringOptions.Contains(deleteOptionText))
                {
                    DeleteOptions(deleteOptionText);
                    deleteOptionText = "";
                }
                else
                {
                    Debug.LogWarning($"없는 옵션입니다 : {deleteOptionText}");
                }
            }
            GUI.enabled = true;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SaveOptions(string newOption)
    {
        if(stringOptions.Contains(newOption))
        {
            Debug.LogError($"Already Regist Account : {newOption}");
            return;
        }

        stringOptions.Add(newOption);

        var wrapper = new OptionListWrapper { options = stringOptions };
        var json = JsonUtility.ToJson(wrapper);
        EditorPrefs.SetString(OPTIONS_KEY, json);   
    }

    private void DeleteOptions(string targetOption)
    {
        if(!stringOptions.Contains(targetOption))
        {
            Debug.LogError($"Not Regist Account : {targetOption}");
            return;
        }

        stringOptions.Remove(targetOption);

        var wrapper = new OptionListWrapper { options = stringOptions };
        var json = JsonUtility.ToJson(wrapper);
        EditorPrefs.SetString(OPTIONS_KEY, json);   
    }
}