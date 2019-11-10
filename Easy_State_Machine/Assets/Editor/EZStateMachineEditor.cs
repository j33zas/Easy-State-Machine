using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(EZStateMachine))]
public class EZStateMachineEditor : Editor
{
    EZStateMachine _ezSM;
    string _name;

    private void OnEnable()
    {
        _ezSM = (EZStateMachine)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (_ezSM.mySM == null)
        {
            EditorGUILayout.Space();
            _name = EditorGUILayout.TextField("Name: StateMachine_", _name);
            EditorGUILayout.Space();

            if (GUILayout.Button("Create and asign StateMachine"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/StateMachines"))
                    AssetDatabase.CreateFolder("Assets", "StateMachines");
                StateMachineScriptable stateMachine = ScriptableObjectUtility.CreateAsset<StateMachineScriptable>("Assets/StateMachines/", "StateMachine_" + _name);
                _ezSM.mySM = stateMachine;
                StateNodesBase.OpenWindow();
                EditorUtility.SetDirty(stateMachine);
                EditorUtility.SetDirty(_ezSM.mySM);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }    
}
