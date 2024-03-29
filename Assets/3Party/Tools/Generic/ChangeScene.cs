#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public class ChangeScene : Editor
{
    [MenuItem("Open Scene/Loading #1")]
    public static void OpenLoading()
    {
        OpenScene(Const.SCENE_LOADING);
    }

    [MenuItem("Open Scene/Login #2")]
    public static void OpenLogin()
    {
        OpenScene(Const.SCENE_MAIN);
    }

    [MenuItem("Open Scene/Main #3")]
    public static void OpenMain()
    {
        OpenScene(Const.SCENE_GAME);
    }

    [MenuItem("Open Scene/Test #4")]
    public static void OpenTest()
    {
        OpenScene("Test");
    }
    private static void OpenScene(string sceneName)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Game/Scenes/" + sceneName + ".unity");
        }
    }
}
#endif