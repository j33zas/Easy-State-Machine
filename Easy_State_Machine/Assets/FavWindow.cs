using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FavWindow : EditorWindow
{
    FavList currentList;

    #region Searchbar
    string _currentsearch;

    string _searchQuery;

    List<Object> _searchResults = new List<Object>();
    #endregion

    [MenuItem("Unity+/Favourites")]
    public static void OpenWindow()
    {
        var _me = GetWindow<FavWindow>();
        _me.Show();
    }

    private void OnEnable()
    {
        /*
        if (currentList == null)
            if (AssetDatabase.FindAssets("Your_favourites") != null)
                currentList = (FavList)AssetDatabase.LoadAssetAtPath("Assets/Unity+/Favourites/List/", typeof(FavList));
        */
    }
    private void OnGUI()
    {
        if (AssetDatabase.FindAssets("Your_favourites").Length == 0)
        {
            if (GUILayout.Button("Create Favourites"))
            {
                currentList = ScriptableObjManager.CreateScriptable<FavList>("Assets/Unity+/Favourites/List/", "Your_favourites");
                currentList.favs = new List<Object>();
            }
            return;
        }

        SearchBar();
        foreach (var item in currentList.favs)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open"))
                AssetDatabase.OpenAsset(item);
            GUILayout.Space(5);

            if (GUILayout.Button("Remove"))
                currentList.favs.Remove(item);
            GUILayout.Space(5);

            GUILayout.TextField(item.name);
            GUILayout.Space(5);



            EditorGUILayout.EndHorizontal();
        }

    }
    void SearchBar()
    {
        EditorGUILayout.LabelField("search");
        EditorGUILayout.BeginHorizontal();
        _searchQuery = EditorGUILayout.TextField(_searchQuery);
        if (GUILayout.Button("Search", GUILayout.Width(50), GUILayout.Height(18)) || Input.GetKeyDown(KeyCode.Return))
        {
            _searchResults.Clear();
            string[] paths = AssetDatabase.FindAssets(_searchQuery);
            for (int i = 0; i < paths.Length - 1; i++)
            {

                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);

                Object _current = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));

                if (_current != null && !currentList.favs.Contains(_current))
                    _searchResults.Add(_current);
            }
        }
        EditorGUILayout.EndHorizontal();
        if (_searchResults.Count > 0)
        {
            for (int i = 0; i < _searchResults.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(_searchResults[i].name);

                GUI.DrawTexture(GUILayoutUtility.GetRect(30, 30), AssetPreview.GetAssetPreview(_searchResults[i]), ScaleMode.ScaleToFit);

                if (GUILayout.Button("add"))
                {
                    _searchResults.Remove(_searchResults[i]);

                    currentList.favs.Add(_searchResults[i]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
