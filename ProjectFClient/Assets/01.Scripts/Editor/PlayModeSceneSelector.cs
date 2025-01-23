using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayModeSceneSelector
{
    private const string START_SCENE_NAME_KEY = "StartSceneName"; // 저장 키
    private const string LAST_SCENE_PATH_KEY = "LastScenePath"; // 저장 키
    private const string MENU_ROOT_NAME = "Tools/Start Scene/";

    private const string BOOTSTRAP = "Bootstrap";
    private static readonly string[] SCENES = { 
        BOOTSTRAP
    };

    private static bool changingSceneFlag = false;

    static PlayModeSceneSelector()
    {
        // 플레이 모드 상태 변경 이벤트 등록
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        
        string loadedOrDefualtSceneName = EditorPrefs.GetString(START_SCENE_NAME_KEY, BOOTSTRAP);
        SetPlayModeScene(loadedOrDefualtSceneName);
    }

    private static void SetPlayModeScene(string sceneName)
    {
        string scenePath = GetScenePathByName(sceneName);
        if(string.IsNullOrEmpty(scenePath))
        {
            Debug.LogWarning($"There are no scenes registered in the build settings with name. : {sceneName}");
            return;
        }

        foreach(string i in SCENES)
            Menu.SetChecked(MENU_ROOT_NAME + i, false);
        Menu.SetChecked(MENU_ROOT_NAME + sceneName, true);

        EditorPrefs.SetString(START_SCENE_NAME_KEY, sceneName);
    }

    [MenuItem(MENU_ROOT_NAME + BOOTSTRAP)]
    public static void SelectScene_BOOTSTRAP() => SetPlayModeScene(BOOTSTRAP);

    private static string GetScenePathByName(string sceneName)
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach(EditorBuildSettingsScene scene in scenes)
        {
            string scenePath = scene.path;
            if(scenePath.Contains(sceneName))
                return scenePath;
        }

        return "";
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch(state)
        {
            case PlayModeStateChange.ExitingEditMode:
                LoadPlayModeStartScene();
                break;
            case PlayModeStateChange.EnteredEditMode:
                LoadLastScene();
                break;
        }
    }

    private static void LoadPlayModeStartScene()
    {
        if(changingSceneFlag)
            return;

        string targetSceneName = EditorPrefs.GetString(START_SCENE_NAME_KEY, BOOTSTRAP);
        string targetScenePath = GetScenePathByName(targetSceneName);
        Debug.Log(targetScenePath);

        if (string.IsNullOrEmpty(targetScenePath))
            return;

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            string currentScenePath = SceneManager.GetActiveScene().path;
            EditorPrefs.SetString(LAST_SCENE_PATH_KEY, currentScenePath);

            changingSceneFlag = true;
            EditorSceneManager.OpenScene(targetScenePath);
            changingSceneFlag = false;
        }
        else
        {
            EditorApplication.isPlaying = false;
        }
    }

    private static void LoadLastScene()
    {
        if (changingSceneFlag)
            return;

        string lastScenePath = EditorPrefs.GetString(LAST_SCENE_PATH_KEY, string.Empty);
        if(string.IsNullOrEmpty(lastScenePath))
            return;

        if(lastScenePath == SceneManager.GetActiveScene().path)
            return;

        changingSceneFlag = true;
        EditorSceneManager.OpenScene(lastScenePath);
        changingSceneFlag = false;
    }
}
