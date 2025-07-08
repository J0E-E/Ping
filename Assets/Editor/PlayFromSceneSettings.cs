using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayFromSceneSettings", menuName = "Tools/Play From Scene Settings")]
public class PlayFromSceneSettings : ScriptableObject
{
    public SceneAsset startScene;
}
