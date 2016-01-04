using UnityEngine;
using System.Collections;

public class UnitDataAsset : ScriptableObject
{
    static UnitDataAsset instance;
    public static UnitDataAsset I
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<UnitDataAsset>("UnitDataAsset");

                if (instance == null)
                    Debug.LogError("Could not load UnitDataAsset, make sure it is exported with the built executeable!");

                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }
}
