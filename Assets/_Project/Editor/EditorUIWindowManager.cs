using UnityEngine;
using UnityEditor;
using HangOn.Navigation;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

[CustomEditor(typeof(UIWindowManager))]
public class EditorUIWindowManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UIWindowManager uIWindowManager = (UIWindowManager)target;
        EditorGUILayout.LabelField("______________________________________________________________________________");
        EditorGUILayout.HelpBox("UI Window Manager Utilities", MessageType.None);
        uIWindowManager.Id = EditorGUILayout.IntField("Id", uIWindowManager.Id);
        EditorGUILayout.HelpBox("ID of window to open/close", MessageType.Info);
        uIWindowManager.ShouldUIWindowsOverlap = EditorGUILayout.Toggle("Should UI Windows Overlap", uIWindowManager.ShouldUIWindowsOverlap);

        if(GUILayout.Button("Open"))
        {
            uIWindowManager.Open(uIWindowManager.Id);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        if(GUILayout.Button("Close"))
        {
            uIWindowManager.Close(uIWindowManager.Id);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        if(GUILayout.Button("Open All Windows"))
        {
            uIWindowManager.OpenAll();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        if (GUILayout.Button("Close All Windows"))
        {
            uIWindowManager.CloseAll();
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        GUILayout.BeginVertical();
        GUILayout.Space(30);
        GUILayout.Label("||||||||--------------------------QUICK NAVIGATION----------------------|||||||||||");
        GUILayout.Label("                                                  (Next window, Previous Window)");
            if (GUILayout.Button("--->"))
            {
                if (!uIWindowManager.CanFindOpenWindow())
                {
                    Debug.Log("Can't find open window, oepning first ui window..");
                    uIWindowManager.Open(0);
                }
                else
                {
                    bool nextWindowExists = uIWindowManager.CurrWindowIndex + 1 <= uIWindowManager.WindowCount - 1;
                    if (!nextWindowExists)
                    {
                        uIWindowManager.CurrWindowIndex = uIWindowManager.WindowCount - 1;
                        Debug.Log("Can't go any further while trying to go to next scene");
                        return;
                    }

                    int nextIndex = uIWindowManager.CurrWindowIndex + 1;
                    uIWindowManager.CurrWindowIndex++;
                    uIWindowManager.OpenByIndex(uIWindowManager.CurrWindowIndex);
                    Debug.Log("opening ui window [NEXT UI WINDOW NAME] after ui window [CURRENT UI WINDOW NAME]");
                }
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            if (GUILayout.Button("<---"))
            {
               
                if (!uIWindowManager.CanFindOpenWindow())
                {
                    Debug.Log("Can't find open window, oepning first ui window..");
                    uIWindowManager.Open(0);
                }
                else
                {
                    bool previousWindowExists = uIWindowManager.CurrWindowIndex - 1 >= 0;
                    if (!previousWindowExists)
                    {
                        uIWindowManager.CurrWindowIndex = 0;
                        Debug.Log("Can't go any further while trying to go to next scene");
                        return;
                    }

                    int nextIndex = uIWindowManager.CurrWindowIndex - 1;
                    uIWindowManager.CurrWindowIndex--;
                    uIWindowManager.OpenByIndex(uIWindowManager.CurrWindowIndex);
                    Debug.Log("opening ui window [NEXT UI WINDOW NAME] before ui window [CURRENT UI WINDOW NAME]");
                }
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        GUILayout.Space(30);
        GUILayout.Label("||||||||----------------------BETA TEST SCENE NAVIGATION----------------|||||||||||");
            if (GUILayout.Button("Next Scene"))
            {
                EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
                string[] scenePaths = new string[scenes.Length];

                for (int i = 0; i < scenes.Length; i++)
                {
                    scenePaths[i] = scenes[i].path;
                }

                var activeScenePath = scenePaths.Where(x => x == SceneManager.GetActiveScene().path).FirstOrDefault();
                string nextScenePath = "";
                int nextSceneIndex = -1;

                for (int i = 0; i < scenes.Length; i++)
                {
                    bool isActiveScene = scenes[i].path == activeScenePath;
                    bool isLastScene = i >= scenes.Length - 1;
                    if (isActiveScene && !isLastScene)
                    {
                        nextSceneIndex = i + 1;
                    }
                    if (i == nextSceneIndex)
                    {
                        nextScenePath = scenes[i].path;
                        break;
                    }
                }

                Debug.Log($"active scene path : {activeScenePath}");
                Debug.Log($"next scene path : {nextScenePath}");

                bool hasFoundNextScene = nextScenePath != "";
                if (!hasFoundNextScene)
                {
                    Debug.Log("Can't go any further while trying to go to next scene");
                }
                else
                {
                    Scene nextScene = EditorSceneManager.OpenScene(nextScenePath);
                }
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            if (GUILayout.Button("Previous Scene"))
            {
                EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
                string[] scenePaths = new string[scenes.Length];

                for (int i = 0; i < scenes.Length; i++)
                {
                    scenePaths[i] = scenes[i].path;
                }

                var activeScenePath = scenePaths.Where(x => x == SceneManager.GetActiveScene().path).FirstOrDefault();
                int activeSceneIndex = EditorSceneManager.GetActiveScene().buildIndex;
                string previousScenePath = "";
                int previousSceneIndex = -1;

                for (int i = scenes.Length - 1; i >= 0; i--)
                {
                    bool isActiveScene = scenes[i].path == activeScenePath;
                    if (isActiveScene)
                    {
                        previousSceneIndex = i - 1;
                    }
                    if (i == previousSceneIndex)
                    {
                        previousScenePath = scenes[i].path;
                        break;
                    }
                }

                Debug.Log($"active scene path : {activeScenePath}");
                Debug.Log($"previous scene path : {previousScenePath}");

                bool hasFoundPreviousScene = previousSceneIndex != -1; ;
                if (!hasFoundPreviousScene)
                {
                    Debug.Log("Can't go any further while trying to go to previous scene");
                }
                else
                {
                    Scene previousScene = EditorSceneManager.OpenScene(previousScenePath);
                }
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        GUILayout.EndVertical();
    }
}
