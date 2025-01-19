using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace H00N.Resources
{
    [CustomPropertyDrawer(typeof(AddressableAsset<>))]
    public class AddressableAssetDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var referenceProperty = property.FindPropertyRelative("reference");

            if (referenceProperty == null)
            {
                EditorGUI.EndProperty();
                return;
            }

            // 이전 값 저장
            Object previousValue = referenceProperty.objectReferenceValue;

            // reference 필드와 변수 이름을 함께 표시
            EditorGUI.PropertyField(position, referenceProperty, label);

            // 값이 변경되었는지 확인
            Object currentValue = referenceProperty.objectReferenceValue;
            if (previousValue != currentValue)
            {
                var keyProperty = property.FindPropertyRelative("key");
                if (currentValue == null)
                {
                    keyProperty.stringValue = "";
                }
                else
                {
                    if (currentValue is Component component)
                        currentValue = component.gameObject;

                    string key = GetAddressableKey(currentValue);
                    if (key == null)
                    {
                        referenceProperty.objectReferenceValue = null;
                        keyProperty.stringValue = null;
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
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("reference"), label);
        }

        private string GetAddressableKey(Object targetObject)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings not found.");
                return null;
            }

            foreach (var group in settings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.MainAsset == targetObject)
                    {
                        return entry.address;
                    }
                }
            }

            Debug.LogError($"It is not an addressable asset : {targetObject.name}");
            return null;
        }
    }
}