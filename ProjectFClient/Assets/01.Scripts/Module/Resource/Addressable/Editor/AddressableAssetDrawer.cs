using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace H00N.Resources
{
    [CustomPropertyDrawer(typeof(AddressableAsset<>))]
    public class AddressableAssetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // var referenceProperty = property.FindPropertyRelative("reference");
            var keyProperty = property.FindPropertyRelative("key");
            string keyValue = keyProperty.stringValue;

            Type genericType = GetGenericType(property);
            Object prevValue = AddressableEditorUtils.GetAssetFromKey(keyValue, genericType);
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
            if(pathParts.Length <= 0)
                return null;

            string fieldName = pathParts[^1];
            while (targetType != null)
            {               
                var field = targetType.GetField(fieldName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                {
                    Type currentType = field.FieldType;
                    if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(AddressableAsset<>))
                        return currentType.GetGenericArguments()[0]; // T 타입 반환
                }

                targetType = targetType.BaseType;
            }

            return null;
        }
    }
}