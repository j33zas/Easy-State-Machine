using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class StateNodesBase : EditorWindow
{
    private GUIStyle myStyle;
    private GUIStyle wrappedText;

    private GUIStyle otherStyle;

    List<Node> _allNodes = new List<Node>();

    private Rect graphRect;
    private Vector2 panRect;

    private const float toolBarHeight = 100;

    private bool _panning = false;

    private Node _selectedNode;

    private Node _nodeOver;

    private Node _targetNode;

    Node _currentNode;

    Vector2 screenPosPos;

    StateMachineScriptable _myMachine;

    [MenuItem("Unity+/EasyStateMachine/NodeEditor")]
    public static void OpenWindow(StateMachineScriptable newMachine)
    {
        var mySelf = GetWindow<StateNodesBase>();

        mySelf._myMachine = newMachine;

        mySelf.graphRect = new Rect(0, toolBarHeight, 1000, 1000);
        mySelf.panRect = new Vector2(0, toolBarHeight);

        mySelf.myStyle = new GUIStyle();
        mySelf.myStyle.fontSize = 20;
        mySelf.myStyle.alignment = TextAnchor.MiddleCenter;

        mySelf.otherStyle = new GUIStyle();
        mySelf.otherStyle.fontSize = 15;
        mySelf.otherStyle.alignment = TextAnchor.LowerCenter;

        mySelf.wrappedText = new GUIStyle(EditorStyles.textField);
        mySelf.wrappedText.wordWrap = true;
        mySelf.Show();


        if (mySelf._myMachine._states.Count != 0 && mySelf._myMachine._states != null)
        {
            //mySelf._allNodes = mySelf._myMachine._states;
            for (int i = 0; i < mySelf._myMachine._states.Count; i++)
            {
                mySelf.CreateNode(Event.current);
            }
        }
    }

    private void OnGUI()
    {
        Space(2);

        EditorGUILayout.BeginVertical(GUILayout.Height(500));

        EditorGUILayout.LabelField("STATEMACHINE EDITOR: " + _myMachine.name, myStyle);

        Space(2);

        EditorGUILayout.LabelField("Right click to add a note", otherStyle);

        EditorGUILayout.EndVertical();

        MouseInputs(Event.current);

        //BACKLINES

        EditorGUI.DrawRect(new Rect(0, toolBarHeight, position.width, position.height), Color.gray);

        graphRect.x = panRect.x;
        graphRect.y = panRect.y;
        GUI.BeginGroup(graphRect);

        for (int i = 0; i < _allNodes.Count; i++)
        {
            foreach (var c in _allNodes[i].connected)
                Handles.DrawLine(new Vector2(_allNodes[i].myRect.position.x + _allNodes[i].myRect.width / 2f, _allNodes[i].myRect.position.y + _allNodes[i].myRect.height / 2f), new Vector2(c.myRect.position.x + c.myRect.width / 2f, c.myRect.position.y + c.myRect.height / 2f));
        }

        BeginWindows();

        var oldColor = GUI.backgroundColor;
        for (int i = 0; i < _allNodes.Count; i++)
        {
            if (_allNodes[i] == _selectedNode)         
                GUI.backgroundColor = Color.white;
            if (_allNodes[i] == _targetNode)
                GUI.backgroundColor = Color.red;
                
            _allNodes[i].myRect = GUI.Window(i, _allNodes[i].myRect, DrawNode, _allNodes[i].title);
            GUI.backgroundColor = oldColor;
        }

        EndWindows();

        GUI.EndGroup();
    }

    private void MouseInputs(Event current)
    {
        if ((!graphRect.Contains(current.mousePosition)) && !(focusedWindow == this || mouseOverWindow == this))
            return;

        _nodeOver = null;
        foreach (var node in _allNodes)
        {
            node.SetMouseOver(current, panRect);
            if (node.isOver)
            {
                _nodeOver = node;
                break;
            }
        }

        if (current.button == 1 && current.type == EventType.MouseDown)
        {
            MenuOptions(_nodeOver);
        }

        var oldSelected = _selectedNode;

        if (current.button == 0 && current.type == EventType.MouseDown)
        {
            _selectedNode = _nodeOver;
            if (_selectedNode == oldSelected)
            {
                _selectedNode = null;
            }
            else
                Repaint();
        }

        screenPosPos = current.mousePosition;
    }

    private void CreateNode(Event current)
    {
        var newNode = new Node(new Rect(screenPosPos.x -20, screenPosPos.y -200, 140, 130));
        //object[] tempStates = _currentNode.GetAllDerivedTypes(typeof(State));
        //_myMachine._states.Add((State)tempStates[_currentNode.indexTest]);
        _allNodes.Add(newNode);
    }

    private void DrawNode(int id)
    {
        EditorGUILayout.LabelField("STATE");

        _currentNode = _allNodes[id];

        _currentNode.indexTest = EditorGUILayout.Popup(_currentNode.indexTest, _currentNode.StateNames());

        EditorGUILayout.LabelField("DESCRIPTION");

        _currentNode.description = EditorGUILayout.TextArea(_currentNode.description, wrappedText, GUILayout.Height(15));

        Space(2);

        if (!_panning)
        {
            GUI.DragWindow();

            if (!_currentNode.isOver) return;

            if (_currentNode.myRect.x < 0)
                _currentNode.myRect.x = 0;

            if (_currentNode.myRect.y < toolBarHeight - panRect.y)
                _currentNode.myRect.y = toolBarHeight - panRect.y;
        }
    }

    public void MainFuncion()
    {
        CreateNode(Event.current);
    }

    public void SetTarget()
    {
        _targetNode = null;
        _targetNode = _nodeOver;
    }

    public void DeleteNode()
    {
        _allNodes.Remove(_nodeOver);
        Repaint();
    }

    public void Connect()
    {
        _nodeOver.connected.Add(_targetNode);
    }

    public void Disconnect()
    {
        _nodeOver.connected.Remove(_targetNode);
    }

    public void MenuOptions(Node nodeOver)
    {
        var menuVar = new GenericMenu();
        menuVar.AddItem(new GUIContent("Add node"), false, MainFuncion);       

        if (nodeOver == null)
        {
            menuVar.AddDisabledItem(new GUIContent("Delete node"));
            menuVar.AddDisabledItem(new GUIContent("Connect"));
            menuVar.AddDisabledItem(new GUIContent("Disconnect"));
            menuVar.AddDisabledItem(new GUIContent("Set as target"));
        }

        else if (nodeOver != null && _targetNode == null)
        {
            menuVar.AddItem(new GUIContent("Delete node"), false, DeleteNode);
            menuVar.AddDisabledItem(new GUIContent("Connect"));
            menuVar.AddDisabledItem(new GUIContent("Disconnect"));
            menuVar.AddItem(new GUIContent("Set as target"), false, SetTarget);
        }

        else 
        {
            menuVar.AddItem(new GUIContent("Delete node"), false, DeleteNode);
            menuVar.AddItem(new GUIContent("Connect"), false, Connect);
            menuVar.AddItem(new GUIContent("Disconnect"), false, Disconnect);
            menuVar.AddItem(new GUIContent("Set new target"), false, SetTarget);
        }
            
        menuVar.ShowAsContext();
    }

    private void Space(int cant)
    {
        for (int i = 0; i < cant; i++)
        {
            EditorGUILayout.Space();
        }
    }
}
