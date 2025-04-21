// Editor/ButtonAttributeEditor.cs
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace JeusInspector.Attributes
{
    // 모든 MonoBehaviour에 적용
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class ButtonAttributeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // 1) 일반 인스펙터 그리기
            DrawDefaultInspector();

            // 2) 타겟 오브젝트의 타입과 인스턴스
            var mb = (MonoBehaviour)target;
            var type = mb.GetType();

            // 3) [Button] 태그가 붙은 public/ private 파라미터 없는 메서드 리스트
            var methods = type
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0)
                .Where(m => m.GetParameters().Length == 0);

            // 4) 각 메서드별로 버튼 생성
            foreach (var method in methods)
            {
                var attr = (ButtonAttribute)method.GetCustomAttributes(typeof(ButtonAttribute), true).First();
                string label = string.IsNullOrEmpty(attr.Label) ? method.Name : attr.Label;

                if (GUILayout.Button(label))
                {
                    // 실행 직전에 에디터 상태 기록 (Undo 지원)
                    Undo.RecordObject(mb, $"Invoke {method.Name}");
                    method.Invoke(mb, null);
                
                    // 변경사항이 남을 수 있는 메서드라면 오브젝트/씬 더티 표시
                    if (!Application.isPlaying)
                    {
                        EditorUtility.SetDirty(mb);
                        if (mb.gameObject.scene.isLoaded)
                            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(mb.gameObject.scene);
                    }   
                }
            }
        }
    }   
}