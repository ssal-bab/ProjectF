#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using Unity.VisualScripting;

namespace ShibaInspector.Collections
{
    [CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    private bool foldout = true;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var keys = property.FindPropertyRelative("keys");
        var values = property.FindPropertyRelative("values");

        if (!foldout) return EditorGUIUtility.singleLineHeight;

        float height = EditorGUIUtility.singleLineHeight * 2; // Header + Buttons
        for (int i = 0; i < keys.arraySize; i++)
        {
            var keyProp = keys.GetArrayElementAtIndex(i);
            var valueProp = values.GetArrayElementAtIndex(i);

            height += Mathf.Max(
                EditorGUI.GetPropertyHeight(keyProp, GUIContent.none, true),
                EditorGUI.GetPropertyHeight(valueProp, GUIContent.none, true)
            ) + 4f;
        }
        
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var keys = property.FindPropertyRelative("keys");
        var values = property.FindPropertyRelative("values");
        var shouldApply = property.FindPropertyRelative("shouldApply");
        var notApplied = property.FindPropertyRelative("notApplied");

        if(notApplied.boolValue == true)
            label.text += " *";

        foldout = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), foldout, label, true);
        position.y += EditorGUIUtility.singleLineHeight;

        if (!foldout) return;

        EditorGUI.indentLevel++;

        float half = position.width / 2f;

        for (int i = 0; i < keys.arraySize; i++)
        {
            var keyProp = keys.GetArrayElementAtIndex(i);
            var valueProp = values.GetArrayElementAtIndex(i);

            float height = Mathf.Max(
                EditorGUI.GetPropertyHeight(keyProp, GUIContent.none, true),
                EditorGUI.GetPropertyHeight(valueProp, GUIContent.none, true)
            );

            Rect keyRect = new Rect(position.x, position.y, half - 5, height);
            Rect valRect = new Rect(position.x + half - 10, position.y, half - 20, height);
            Rect delRect = new Rect(position.x + position.width - 20, position.y, 20, height);

            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none, true);
            EditorGUI.PropertyField(valRect, valueProp, GUIContent.none, true);

            if (GUI.Button(delRect, "X"))
            {
                keys.DeleteArrayElementAtIndex(i);
                values.DeleteArrayElementAtIndex(i);

                notApplied.boolValue = true;

                break;
            }

            position.y += height + 4f;
        }

        // Add Button
        Rect addBtnRect = new Rect(position.x + 15, position.y, half - 20, EditorGUIUtility.singleLineHeight);
        if (GUI.Button(addBtnRect, "Add"))
        {
            property.serializedObject.ApplyModifiedProperties();

            int index = keys.arraySize;
            keys.InsertArrayElementAtIndex(index);
            values.InsertArrayElementAtIndex(index);

            notApplied.boolValue = true;

            property.serializedObject.ApplyModifiedProperties();
        }

        // Apply Button
        Rect applyBtnRect = new Rect(position.x + half + 5, position.y, half - 35, EditorGUIUtility.singleLineHeight);
        if (GUI.Button(applyBtnRect, "Apply"))
        {
            shouldApply.boolValue = true;
            notApplied.boolValue = false;
        }

        EditorGUI.indentLevel--;
    }
}
}

#endif