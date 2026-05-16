using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class SceneCreator
{
    [MenuItem("Tools/Scenes/Create MainMenu Scene")]
    public static void CreateMainMenuScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject helperGO = new GameObject("SceneSetupHelper");
        SceneSetupHelper helper = helperGO.AddComponent<SceneSetupHelper>();
        helper.SetupMainMenuUI();
        helper.CreateManagers();

        string scenePath = "Assets/Scenes/MainMenu.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log($"Created MainMenu scene at {scenePath}");
    }

    [MenuItem("Tools/Scenes/Create LevelSelect Scene")]
    public static void CreateLevelSelectScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject helperGO = new GameObject("SceneSetupHelper");
        SceneSetupHelper helper = helperGO.AddComponent<SceneSetupHelper>();
        helper.SetupLevelSelectUI();
        helper.CreateManagers();

        string scenePath = "Assets/Scenes/LevelSelect.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log($"Created LevelSelect scene at {scenePath}");
    }
}
