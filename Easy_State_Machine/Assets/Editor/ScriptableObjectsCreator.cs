using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectsCreator : EditorWindow
{
    string _name;

    bool _SMexists = false;

    string test;

    [MenuItem("Unity+/EasyStateMachine/Create/New StateMachine")]
    public static void CreateStateMachine()
    {
        var window = GetWindow<ScriptableObjectsCreator>();
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        _name = EditorGUILayout.TextField("Name: StateMachine_", _name);
        if(_name != test)
        {
            test = _name;
            Repaint();
        }
        EditorGUILayout.Space();

        var _allSM = AssetDatabase.LoadAllAssetsAtPath("Assets/StateMachineS/");
        Debug.Log(_allSM.Length);


        for (int i = 0; i < _allSM.Length - 1; i++)
        {
            Debug.Log(_allSM[i].name);
            if ("StateMachine_" + _name == _allSM[i].name)
                _SMexists = true;
            else
                _SMexists = false;
        }


        if (!_SMexists)
        {
            if (GUILayout.Button("Create StateMachine"))
            {
                if (!AssetDatabase.IsValidFolder("Assets/StateMachines"))
                    AssetDatabase.CreateFolder("Assets", "StateMachines");
                StateMachineScriptable stateMachine = ScriptableObjectUtility.CreateAsset<StateMachineScriptable>("Assets/StateMachines/", "StateMachine_" + _name);
                EditorUtility.SetDirty(stateMachine);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }
}
