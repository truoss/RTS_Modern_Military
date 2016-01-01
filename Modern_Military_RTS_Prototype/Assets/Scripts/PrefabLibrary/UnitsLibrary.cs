using UnityEngine;
using System.Collections;

//[CreateAssetMenu(fileName = "PrefabLibrary", menuName = "PrefabLibrary", order = 0)]
//[System.Serializable]
public class UnitsLibrary : ScriptableObject
{
    static UnitsLibrary instance;
    public static UnitsLibrary I
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<UnitsLibrary>("UnitsLibrary");

                if (instance == null)
                    Debug.LogError("Could not load prefab library, make sure it is exported with the built executeable!");

                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }

    public GameObject BaseUnit;    
}
