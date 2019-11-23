using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int indexTest;

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

    //public State[] GetAllStates(System.Type aType)
    //{
    //    int index = 0;
    //    var allStateNames = StateNames();
    //    State[] resultStates = new State[allStateNames.Length - 1];

    //    var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
    //    foreach (var assembly in assemblies)
    //    {
    //        var types = assembly.GetTypes();
    //        foreach (var type in types)
    //        {
    //            if (type.IsSubclassOf(aType))
    //                resultStates[index] = type;
    //            index++;
    //        }
    //    }
    //}

    public System.Type[] GetAllDerivedTypes(System.Type aType)
    {
        var result = new List<System.Type>();
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(aType))
                    result.Add(type);
            }
        }
        return result.ToArray();
    }

    //Y en algún otro lado del código(al abrir la ventana podrían guardar esto así no lo buscan cada vez, consume pero no taaaanto) :

    public string[] StateNames()
    {
        var types = GetAllDerivedTypes(typeof(State));
        var typeNames = new string[types.Length];
        for (int i = 0; i < types.Length; i++)
        {
            typeNames[i] = types[i].ToString();
        }
        return typeNames;
    }
}
