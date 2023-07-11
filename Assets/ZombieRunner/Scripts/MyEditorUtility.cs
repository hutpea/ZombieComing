#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MyEditorUtility : MonoBehaviour
{
    [MenuItem("Open Scene/Boot #1")]
    public static void OpenSceneBoot()
    {
        string localPath = "Assets/Shared/Scenes/Boot.unity";
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene(localPath);
    }
}
#endif