using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectsCreator : EditorWindow
{
    string _name;

    [MenuItem("EasyStateMachine/ScriptableObjects/Create/StateMachine")]
    public static void CreateStateMachine()
    {
        var window = GetWindow<ScriptableObjectsCreator>();
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        _name = EditorGUILayout.TextField("Name: StateMachine_", _name);
        EditorGUILayout.Space();

        //if (AssetDatabase.FindAssets("StateMachine_" + _name) == null)
        {
            if (GUILayout.Button("Create StateMachine"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/StateMachines"))
                    AssetDatabase.CreateFolder("Assets", "StateMachines");
                StateMachine stateMachine = ScriptableObjectUtility.CreateAsset<StateMachine>("Assets/StateMachines/", "StateMachine_" + _name);
                EditorUtility.SetDirty(stateMachine);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
