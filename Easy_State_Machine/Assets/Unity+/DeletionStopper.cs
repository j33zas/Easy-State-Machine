using UnityEngine;
using UnityEditor;

[ExecuteAlways()]
public class DeletionStopper : UnityEditor.AssetModificationProcessor
{
    static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions opt)
    {
        FavList list = (FavList)AssetDatabase.LoadAssetAtPath("Assets/Unity+/Favourites/List/Your_favourites.Asset", typeof(FavList));
        if (list.favs.Contains(AssetDatabase.LoadAssetAtPath(path, typeof(object))))
        {
            EditorUtility.DisplayDialog("Error", "You can't delete objects that are favourited,  remove it from favourites to delete it", "Thanks", null);
            return AssetDeleteResult.FailedDelete;
        }
        else
            return AssetDeleteResult.DidDelete;
    }
}