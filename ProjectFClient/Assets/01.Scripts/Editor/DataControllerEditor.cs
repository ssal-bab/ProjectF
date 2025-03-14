using UnityEditor;
using ProjectF;

[InitializeOnLoad]
public class DataControllerEditor
{
    [MenuItem("Tools/Reset Data")]
    public static void ResetData()
    {
        GameSetting.ResetGameSetting();
    }
}
