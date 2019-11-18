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

    private bool _selectedSomething = false;

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

        if (current.button == 1 && current.type == EventType.MouseDown)
        {
            MenuOptions();
        }

        foreach (var node in _allNodes)
        {
            node.SetMouseOver(current, panRect);
        }

        var oldSelected = _selectedNode;

        if (current.button == 0 && current.type == EventType.MouseDown)
        {
            foreach (var node in _allNodes)
            {
                if (node.isOver)
                {
                    _selectedNode = node;
                    _selectedSomething = true;
                    break;
                }
            }
            if (_selectedNode == oldSelected)
            {
                _selectedNode = null;
                _selectedSomething = false;
            }        
            else
                Repaint();
        }

        screenPosPos = current.mousePosition;
    }

    private void CreateNode(Event current)
    {
        var newNode = new Node(new Rect(screenPosPos.x -20, screenPosPos.y -200, 250, 150));

        _allNodes.Add(newNode);
    }

    private void DrawNode(int id)
    {
        EditorGUILayout.LabelField("STATE");

        var node = _allNodes[id];

        node.title = EditorGUILayout.TextArea(node.title, wrappedText, GUILayout.Height(20));

        EditorGUILayout.LabelField("DESCRIPTION");

        node.description = EditorGUILayout.TextArea(node.description, wrappedText, GUILayout.Height(15));

        Space(2);

        var n = EditorGUILayout.TextField("Connect to:", "");
        if (n != "" && n != " ")
        {
            for (int i = 0; i < _allNodes.Count; i++)
            {
                if (_allNodes[i].title == n)
                    _allNodes[id].connected.Add(_allNodes[i]);
                else if
                    (_allNodes[i].title == n)
                    _allNodes[id].connected.Remove(_allNodes[i]);
            }
            Repaint();
        }


        if (!_panning)
        {
            GUI.DragWindow();

            if (!node.isOver) return;

            if (node.myRect.x < 0)
                node.myRect.x = 0;

            if (node.myRect.y < toolBarHeight - panRect.y)
                node.myRect.y = toolBarHeight - panRect.y;
        }
    }

    public void MainFuncion()
    {
        CreateNode(Event.current);
    }

    public void DeleteNode()
    {
        Debug.Log("dou");
        _allNodes.Remove(_selectedNode);
        DestroyImmediate(_selectedNode);
        Repaint();
    }

    public void MenuOptions()
    {
        var menuVar = new GenericMenu();
        menuVar.AddItem(new GUIContent("Add node"), false, MainFuncion);

        if (_selectedSomething == false)
            menuVar.AddDisabledItem(new GUIContent("Delete node"));        
        else
            menuVar.AddItem(new GUIContent("Delete node"), false, DeleteNode);


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
