using UnityEditor;
using ProjectF;
using UnityEngine;

[InitializeOnLoad]
public class DataControllerEditor
{
    [MenuItem("Tools/DataControl/Reset Data")]
    public static void ResetData()
    {
        GameSetting.ResetGameSetting();
    }

    [MenuItem("Tools/DataControl/Copy User ID")]
    public static void CopyUserID()
    {
        GUIUtility.systemCopyBuffer = GameSetting.LastLoginUserID;
    }
}
