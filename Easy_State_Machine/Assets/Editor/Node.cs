using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Rect myRect;
    public int rendomInt;
    public string title;
    public string description;

    public List<Node> connected;

    public bool isOver;

    public Node(Rect myRect)
    {
        this.myRect = myRect;
        connected = new List<Node>();
    }

    public void SetMouseOver(Event current, Vector2 pan)
    {
        isOver = myRect.Contains(current.mousePosition - pan);
    }
}
