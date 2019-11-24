using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeObject", menuName = "Easy State Machine/NodeSaveData")]
public class NodeObject : ScriptableObject
{
    public List<Node> nodes = new List<Node>();
    public List<System.Type> stateOfNode = new List<System.Type>();
    public List<Rect> rectOfNode = new List<Rect>();
    public List<List<Node>> connectionsOfNode = new List<List<Node>>();
}
