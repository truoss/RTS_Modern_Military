using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakePrefabLibrary
{
    [MenuItem("Assets/Create/FieldDataAsset")]
    public static void CreateFieldDataAsset()
    {
        FieldDataAsset asset = ScriptableObject.CreateInstance<FieldDataAsset>();

        AssetDatabase.CreateAsset(asset, "Assets/FieldDataAsset.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/UnitDataAsset")]
    public static void CreateUnitDataAsset()
    {
        UnitDataAsset asset = ScriptableObject.CreateInstance<UnitDataAsset>();

        AssetDatabase.CreateAsset(asset, "Assets/UnitDataAsset.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
