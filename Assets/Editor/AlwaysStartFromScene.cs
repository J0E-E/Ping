using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AlwaysStartFromScene
{
    const string settingsPath = "Assets/Scenes/PlayFromSceneSettings.asset";

    static AlwaysStartFromScene()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        var settings = AssetDatabase.LoadAssetAtPath<PlayFromSceneSettings>(settingsPath);
        if (settings == null || settings.startScene == null) return;
        string startScenePath = AssetDatabase.GetAssetPath(settings.startScene);
        
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // Save current scene to return to it after play mode, if desired
            EditorPrefs.SetString("LastActiveScene", EditorSceneManager.GetActiveScene().path);

            // If not already in the start scene, open it
            if (EditorSceneManager.GetActiveScene().path != startScenePath)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(startScenePath);
                }
                else
                {
                    EditorApplication.isPlaying = false; // cancel play mode if user cancels save
                }
            }
        }
        // Optionally restore previous scene after playmode
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            string lastScene = EditorPrefs.GetString("LastActiveScene", "");
            if (!string.IsNullOrEmpty(lastScene) && lastScene != startScenePath)
            {
                EditorSceneManager.OpenScene(lastScene);
            }
        }
    }
}