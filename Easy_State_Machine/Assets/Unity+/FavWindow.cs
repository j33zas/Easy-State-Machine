using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FavWindow : EditorWindow
{
    FavList currentList;

    Color defaultColor;

    bool showSearches = false;

    #region Searchbar
    string _currentsearch;

    string _searchQuery;

    List<Object> _searchResults = new List<Object>();
    #endregion

    [MenuItem("Unity+/Favourites &f")]
    public static void OpenWindow()
    {
        var _me = GetWindow<FavWindow>();
        _me.Show();
    }

    [MenuItem("Assets/Add to favourites")]
    private static void AddToFavs()
    {
        var _me = GetWindow<FavWindow>();
        if (!_me.currentList.favs.Contains(Selection.activeObject))
            _me.currentList.favs.Add(Selection.activeObject);
        else
            EditorUtility.DisplayDialog("This Object is already favourited", "You can't have 2 of the same object in your favourites", "Ok", null);
    }

    private void OnEnable()
    {
        string[] test = new string[] { "Assets/Unity+/Favourites/List" };
        if (currentList == null && AssetDatabase.FindAssets("Your_favourites", test).Length > 0)
            currentList = (FavList)AssetDatabase.LoadAssetAtPath("Assets/Unity+/Favourites/List/Your_favourites.Asset", typeof(FavList));
        defaultColor = GUI.backgroundColor;
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
        if (!currentList)
            currentList = (FavList)EditorGUILayout.ObjectField(currentList, typeof(FavList), false);

        EditorGUILayout.LabelField("search");
        EditorGUILayout.BeginHorizontal();
        _searchQuery = EditorGUILayout.TextField(_searchQuery);
        if (GUILayout.Button("Search", GUILayout.Width(50), GUILayout.Height(18)) || Input.GetKeyDown(KeyCode.Return))
            showSearches = true;
        EditorGUILayout.EndHorizontal();
        if (showSearches)
            SearchBar();

        DrawFavourites();
    }

    void SearchBar()
    {
        _searchResults.Clear();
        string[] paths = AssetDatabase.FindAssets(_searchQuery);
        if(paths.Length>0)
        {
            for (int i = 0; i < paths.Length - 1; i++)
            {

                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);

                Object _current = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));

                if (_current != null && !currentList.favs.Contains(_current))
                    _searchResults.Add(_current);
            }
        }

        if (_searchResults.Count > 0)
        {
            EditorGUI.DrawRect(new Rect() , Color.black);
            for (int i = 0; i < _searchResults.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(_searchResults[i].name);

                GUI.DrawTexture(GUILayoutUtility.GetRect(30, 30), AssetPreview.GetMiniTypeThumbnail(_searchResults[i].GetType()), ScaleMode.ScaleToFit);

                if (GUILayout.Button("add"))
                {
                    if (!currentList.favs.Contains(_searchResults[i]))
                        currentList.favs.Add(_searchResults[i]);
                    else
                        EditorUtility.DisplayDialog("This Object is already favourited", "You can't have 2 of the same object in your favourites", "Ok", null);

                    _searchResults.Remove(_searchResults[i]);

                    Repaint();

                    showSearches = false;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        GUI.backgroundColor = defaultColor;
    }

    void DrawFavourites()
    {
        if (currentList.favs.Count >= 0)
        {
            for (int i = 0; i < currentList.favs.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("O", GUILayout.Width(25), GUILayout.Height(25)))
                    AssetDatabase.OpenAsset(currentList.favs[i]);
                GUILayout.Space(5);

                if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    currentList.favs.Remove(currentList.favs[i]);
                    Repaint();
                    break;
                }
                GUILayout.Space(5);

                GUI.DrawTexture(GUILayoutUtility.GetRect(25, 25), AssetPreview.GetMiniTypeThumbnail(currentList.favs[i].GetType()), ScaleMode.ScaleToFit);

                GUILayout.Label(currentList.favs[i].name);
                GUILayout.Space(5);

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}