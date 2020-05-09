using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectUtility
{
    /// <summary>
    /// Create, name, and place unique new ScriptableObject asset file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
            path = "Assets";
        else if (Path.GetExtension(path) != "")
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

        string assetFilePath = AssetDatabase.GenerateUniqueAssetPath($"{path}/New{typeof(T).ToString()}.asset");
        AssetDatabase.CreateAsset(asset, assetFilePath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }   
}
