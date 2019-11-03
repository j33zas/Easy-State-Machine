using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string myName;
    public Rect myRect;
    public int rendomInt;
    public string title;
    public string description;

    public bool isOver;

    public Node(Rect myRect)
    {
        this.myRect = myRect;
    }

    public void SetMouseOver(Event current, Vector2 pan)
    {
        isOver = myRect.Contains(current.mousePosition - pan);
    }
}
