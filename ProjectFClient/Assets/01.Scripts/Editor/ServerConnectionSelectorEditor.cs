using System;
using ProjectF;
using ProjectF.Networks;
using UnityEditor;

[InitializeOnLoad]
public class ServerConnectionSelectorEditor
{
    private const string MENU_ROOT_NAME = "Tools/Server Connection/";

    static ServerConnectionSelectorEditor()
    {
        SetServerConnectionType(GameSetting.LastServerConnection);
    }

    private static void SetServerConnectionType(EServerConnectionType connectionType)
    {
        foreach(EServerConnectionType i in Enum.GetValues(typeof(EServerConnectionType)))
            Menu.SetChecked(MENU_ROOT_NAME + i.ToString(), false);

        Menu.SetChecked(MENU_ROOT_NAME + connectionType.ToString(), true);
        GameSetting.LastServerConnection = connectionType;
    }

    [MenuItem(MENU_ROOT_NAME + "Development")]
    public static void SelectScene_DEVELOPMENT() => SetServerConnectionType(EServerConnectionType.Development);

    [MenuItem(MENU_ROOT_NAME + "Local")]
    public static void SelectScene_LOCAL() => SetServerConnectionType(EServerConnectionType.Local);
}
