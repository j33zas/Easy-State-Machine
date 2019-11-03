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

    [MenuItem("EasyStateMachine/lmao")]
    public static void OpenWindow()
    {
        var mySelf = GetWindow<StateNodesBase>();

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

        EditorGUILayout.LabelField("STATES EDITOR", myStyle);

        Space(2);

        EditorGUILayout.LabelField("Right click to add a note", otherStyle);

        EditorGUILayout.EndVertical();

        MouseInputs(Event.current);

        //BACKLINES

        EditorGUI.DrawRect(new Rect(0, toolBarHeight, position.width, position.height), Color.gray);

        graphRect.x = panRect.x;
        graphRect.y = panRect.y;
        GUI.BeginGroup(graphRect);

        for (int i = 0; i < position.width; i += 10)
        {
            Vector2 p1 = new Vector2(i, 0);
            Vector2 p2 = new Vector2(i, position.height - toolBarHeight);
            Handles.DrawLine(p1, p2);
        }

        BeginWindows();

        var oldColor = GUI.backgroundColor;
        for (int i = 0; i < _allNodes.Count; i++)
        {
            if (_allNodes[i] == _selectedNode)
                GUI.backgroundColor = Color.green;

            _allNodes[i].myRect = GUI.Window(i, _allNodes[i].myRect, DrawNode, _allNodes[i].myName);
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
            MainFuncion();
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
                    break;
                }
            }
            if (_selectedNode == oldSelected)
                _selectedNode = null;
            else
                Repaint();
        }
    }

    private void CreateNode(Vector2 pos)
    {
        pos = new Vector2(0, 0);
        var newNode = new Node(new Rect(pos.x, pos.y, 100, 100));
        _allNodes.Add(newNode);
    }

    private void DrawNode(int id)
    {
        EditorGUILayout.LabelField("STATE");

        var node = _allNodes[id];

        EditorGUILayout.TextArea(node.title, wrappedText, GUILayout.Height(20));

        EditorGUILayout.LabelField("DESCRIPTION");

        EditorGUILayout.TextArea(node.description, wrappedText, GUILayout.Height(10));

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
        CreateNode(new Vector2(250, 250));
    }

    private void Space(int cant)
    {
        for (int i = 0; i < cant; i++)
        {
            EditorGUILayout.Space();
        }
    }
}
