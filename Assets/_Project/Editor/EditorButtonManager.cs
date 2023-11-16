using HangOn.ToolDebug;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(ButtonManager))]
public class EditorButtonManager : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ButtonManager buttonManager = (ButtonManager)target;

        EditorGUILayout.LabelField("______________________________________________________________________________");
        EditorGUILayout.HelpBox("Button Manager Utilities", MessageType.None);

        buttonManager.ButtonId = EditorGUILayout.IntField("Button id", buttonManager.ButtonId);
        EditorGUILayout.HelpBox("Button id to show/hide", MessageType.Info);

        if (GUILayout.Button("Activate"))
        {
            buttonManager.Activate(buttonManager.ButtonId);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        if(GUILayout.Button("Disable"))
        {
            buttonManager.Disable(buttonManager.ButtonId);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}
