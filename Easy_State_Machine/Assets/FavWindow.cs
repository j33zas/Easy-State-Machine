using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FavWindow : EditorWindow
{
    List<Object> favs = new List<Object>();

    Object currentOBJ;

    Object _lastSearch;

    List<Object> searchedItems = new List<Object>();

    [MenuItem("Unity+/Favourites")]
    public static void OpenWindow()
    {
        FavWindow _me = (FavWindow)GetWindow(typeof(FavWindow));
        _me.Show();
    }

    [MenuItem("Assets/Add to favourites")]
    public static void OpenWindow(object toadd = null)
    {
        //tome el item sobre el que se hizo click
        FavWindow _me = (FavWindow)GetWindow(typeof(FavWindow));
        _me.Show();
    }
    private void OnGUI()
    {
        currentOBJ = EditorGUILayout.ObjectField(currentOBJ, typeof(Object), true);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
            if (currentOBJ != null)
                if(!favs.Contains(currentOBJ))
                    favs.Add(currentOBJ);
                else
                    EditorGUILayout.HelpBox("Selected object is already a fovourite!!! D: *dies*", MessageType.Error);

        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < favs.Count - 1; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.TextArea(GUILayoutUtility.GetLastRect(), favs[i].name);
            EditorGUILayout.Space();
            EditorGUI.DrawPreviewTexture(GUILayoutUtility.GetRect(30,30), AssetPreview.GetAssetPreview(searchedItems[i]));

            EditorGUILayout.EndHorizontal();
        }
    }
}
