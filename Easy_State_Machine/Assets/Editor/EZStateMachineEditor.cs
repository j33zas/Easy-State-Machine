using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(EZStateMachine))]
public class EZStateMachineEditor : Editor
{
    EZStateMachine _ezSM;

    SaveStateMachines _Machines;

    string _name;

    int _machineIndex;
    int _actualIndex;
    bool _listOFF = false;

    bool _closedButton = true;

    private void OnEnable()
    {
        _ezSM = (EZStateMachine)target;
        _Machines = Resources.Load<SaveStateMachines>("Machines/Availables Machines");
    }

    public override void OnInspectorGUI()
    {
        if(_Machines.newAvailablesMachines.Count > 0) //TIENE PROBLEMAS AL LIMPIAR LA LISTA CON MISSINGS
        {
            for (int i = 0; i < _Machines.newAvailablesMachines.Count - 1; i++)
            {
                if (_Machines.newAvailablesMachines[i] == null)
                    _Machines.newAvailablesMachines.RemoveAt(i);
            }
          
        }


        if (_Machines.newAvailablesMachines.Count > 0)
            _listOFF = false;
        else
            _listOFF = true;

        EditorGUI.BeginDisabledGroup(_listOFF);
        if (_Machines.newAvailablesMachines.Count > 0)
        {
            string[] _allMachines = new string[_Machines.newAvailablesMachines.Count];
            int _index = 0;

            for (int i = 0; i < _Machines.newAvailablesMachines.Count; i++)
            {
                if (_ezSM.mySM == _Machines.newAvailablesMachines[i])
                    _machineIndex = i;
            }

            foreach (var item in _Machines.newAvailablesMachines)
            {
                Debug.Log(_Machines.newAvailablesMachines.Count);
                _allMachines[_index] = item.name;
                _index++;
            }
            _index = 0;
            _machineIndex = EditorGUILayout.Popup("State Machines Available", _machineIndex, _allMachines);

            if (_actualIndex != _machineIndex)
            {
                _actualIndex = _machineIndex;
                _ezSM.mySM = _Machines.newAvailablesMachines[_actualIndex];
            }
        }
        else
            EditorGUILayout.Popup("State Machines Available", 0, new String[0]);

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        _name = EditorGUILayout.TextField("Name: StateMachine_", _name);
        EditorGUILayout.Space();

        if (GUILayout.Button("Create and asign StateMachine"))
        {
            if (!AssetDatabase.IsValidFolder("Assets/StateMachines"))
                AssetDatabase.CreateFolder("Assets", "StateMachines");
            StateMachineScriptable stateMachine = ScriptableObjectUtility.CreateAsset<StateMachineScriptable>("Assets/StateMachines/", "StateMachine_" + _name);

            if (!_Machines.newAvailablesMachines.Contains(stateMachine))
                _Machines.newAvailablesMachines.Add(stateMachine);
            _ezSM.mySM = stateMachine;
            StateNodesBase.OpenWindow(stateMachine);
            EditorUtility.SetDirty(stateMachine);
            EditorUtility.SetDirty(_ezSM.mySM);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        if (_ezSM != null)
            _closedButton = false;
        else
            _closedButton = true;

        EditorGUI.BeginDisabledGroup(_closedButton);
        if (GUILayout.Button("Open Machine"))
        {
            StateNodesBase.OpenWindow(_ezSM.mySM);
        }
        EditorGUI.EndDisabledGroup();
    }    
}
