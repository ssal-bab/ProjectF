using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using ShibaInspector.EidtorWindow;
using ShibaInspector.Attributes;
using ProjectF;

public class AccountSelector : ShibaMenuEditorWindow
{
    // 드롭다운에 표시할 문자열 배열
    private List<string> stringOptions;

    [Serializable]
    private class OptionListWrapper { public List<string> options; }

    private const string OPTIONS_KEY   = "DropdownEditorWindow_OptionsList";
    private const string SELECT_KEY    = "DropdownEditorWindow_SelectedOption";

    // 메뉴 항목을 통해 에디터 창 열기
    [MenuItem("Tools/Account Selector")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(AccountSelector), false, "Account Selector");
        window.minSize = new Vector2(500.0f, 300.0f);
        window.Show();
    }

    // 에디터 창이 활성화될 때 이전에 저장된 옵션 불러오기
    protected override void OnEnable()
    {
        base.OnEnable();

        stringOptions = new();

        OptionListWrapper wrapper = JsonUtility.FromJson<OptionListWrapper>(EditorPrefs.GetString(OPTIONS_KEY, ""));
        stringOptions = wrapper.options;

        AddMenu("Select", new SelectContent(stringOptions));
        AddMenu("Regist", new RegistContent(stringOptions));
        AddMenu("Unregist", new UnregistContent(stringOptions));
    }

    [Serializable]
    public class SelectContent
    {
        private int selectedIndex = 0;
    
        // 검색어를 입력받기 위한 변수
        private string searchText = "";

        private List<string> stringOptions;

        public SelectContent(List<string> stringOptions)
        {
            this.stringOptions = stringOptions;
        }

        [OnGUI]
        public void OnGUI()
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
                localSelectedIndex = EditorGUILayout.Popup("Accounts", localSelectedIndex, filteredOptions.ToArray());
                // 원본 배열의 인덱스 값으로 업데이트
                selectedIndex = filteredIndices[localSelectedIndex];
            }
        }

        [Button]
        public void Select()
        {
            GameSetting.LastLoginUserID = stringOptions[selectedIndex];
            Debug.Log("Selected Account: " + stringOptions[selectedIndex]);
        }
    }

    public class RegistContent
    {
        private string newOptionText = "";
        private List<string> stringOptions;

        public RegistContent(List<string> stringOptions)
        {
            this.stringOptions = stringOptions;
        }

        [OnGUI]
        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                newOptionText = EditorGUILayout.TextField(newOptionText);
                GUI.enabled = !string.IsNullOrEmpty(newOptionText);
                if (GUILayout.Button("Add Account", GUILayout.Width(120)))
                {
                    // 중복 방지
                    if (!stringOptions.Contains(newOptionText))
                    {
                        SaveOptions(newOptionText);
                        newOptionText = "";
                    }
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();
        }

        [Button]
        public void RegistCurrentAccount()
        {
            SaveOptions(GameSetting.LastLoginUserID);
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
    }

    public class UnregistContent
    {
        private string deleteOptionText = "";

        private List<string> stringOptions;

        public UnregistContent(List<string> stringOptions)
        {
            this.stringOptions = stringOptions;
        }

        [OnGUI]
        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                deleteOptionText = EditorGUILayout.TextField(deleteOptionText);
                GUI.enabled = !string.IsNullOrEmpty(deleteOptionText);
                if (GUILayout.Button("Remove Account", GUILayout.Width(120)))
                {
                    // 중복 방지
                    if (stringOptions.Contains(deleteOptionText))
                    {
                        DeleteOptions(deleteOptionText);
                        deleteOptionText = "";
                    }
                }
                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();
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
}