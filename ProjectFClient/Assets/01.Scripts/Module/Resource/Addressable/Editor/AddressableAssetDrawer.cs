using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace H00N.Resources
{
    [CustomPropertyDrawer(typeof(AddressableAsset<>))]
    public class AddressableAssetDrawer : PropertyDrawer
    {
        private Object virtualObject;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // var referenceProperty = property.FindPropertyRelative("reference");
            var keyProperty = property.FindPropertyRelative("key");
            string keyValue = keyProperty.stringValue;

            Object prevValue = AddressableEditorUtils.GetAssetFromKey(keyValue);
            Type genericType = GetGenericType(property);
            Object currentValue = EditorGUI.ObjectField(position, label, prevValue, genericType, false);
            
            // 값이 변경되었는지 확인
            if (prevValue != currentValue)
            {
                if (currentValue == null)
                {
                    keyProperty.stringValue = "";
                }
                else
                {
                    if (currentValue is Component component)
                        currentValue = component.gameObject;

                    string key = AddressableEditorUtils.GetKeyFromAsset(currentValue);
                    if (key == null)
                    {
                        Debug.LogError($"[AddressableAsset] Asset key not found. Asset name : {currentValue.name}");
                        keyProperty.stringValue = keyValue;
                    }
                    else
                    {
                        keyProperty.stringValue = key;
                    }
                }

                property.serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        private Type GetGenericType(SerializedProperty property)
        {
            // property.serializedObject.targetObject는 인스턴스를 제공
            // property.propertyPath를 통해 제네릭 타입을 추출
            Type targetType = property.serializedObject.targetObject.GetType();
            string[] pathParts = property.propertyPath.Split('.');
            Type currentType = targetType;

            foreach (var part in pathParts)
            {
                if (part == "Array" || part.StartsWith("data["))
                    continue; // 배열 처리 생략 (필요 시 추가)

                var field = currentType.GetField(part, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                {
                    currentType = field.FieldType;
                    if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(AddressableAsset<>))
                    {
                        return currentType.GetGenericArguments()[0]; // T 타입 반환
                    }
                }
            }
            return null;
        }
    }
}