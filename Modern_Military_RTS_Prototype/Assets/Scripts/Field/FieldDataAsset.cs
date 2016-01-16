using UnityEngine;

public class FieldDataAsset : ScriptableObject {
    static FieldDataAsset instance;
    public static FieldDataAsset I {
        get {
            if (instance == null) {
                instance = Resources.Load<FieldDataAsset>("FieldDataAsset");

                if (instance == null)
                    Debug.LogError("Could not load FieldDataAsset, make sure it is exported with the built executeable!");

                //DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public FieldData[] FieldTypes;

    public FieldData GetFieldDataFromFieldType (FieldData.FieldType fieldType) {
        for (int i = 0; i < FieldTypes.Length; i++) {
            if (FieldTypes[i].fieldType == fieldType)
                return FieldTypes[i];
        }
        return null;
    }
}