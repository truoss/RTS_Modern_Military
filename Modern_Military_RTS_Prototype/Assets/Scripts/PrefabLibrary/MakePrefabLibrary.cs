using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakePrefabLibrary
{
    [MenuItem("Assets/Create/UnitsLibrary")]
    public static void CreatePrefabLibrary()
    {
        UnitsLibrary asset = ScriptableObject.CreateInstance<UnitsLibrary>();

        AssetDatabase.CreateAsset(asset, "Assets/UnitsLibrary.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
