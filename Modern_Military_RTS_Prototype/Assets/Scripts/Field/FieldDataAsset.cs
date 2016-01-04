using UnityEngine;
using System.Collections;

public class FieldDataAsset : ScriptableObject {
    static FieldDataAsset instance;
    public static FieldDataAsset I {
        get {
            if (instance == null) {
                instance = Resources.Load<FieldDataAsset>("FieldDataAsset");

                if (instance == null)
                    Debug.LogError("Could not load FieldDataAsset, make sure it is exported with the built executeable!");

                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }


    public struct FieldData {
        public float cover;
        public float mobility;
        public float firerange;
        public float visibility;
    }

    void Start () {
        FieldData PlainInf;
        PlainInf.cover = 10;
        PlainInf.mobility = -10;
        PlainInf.firerange = 0;
        PlainInf.visibility = -20;

        FieldData PlainWheel;
        PlainWheel.cover = 0;
        PlainWheel.mobility = -10;
        PlainWheel.firerange = 0;
        PlainWheel.visibility = 0;

        FieldData UrbanInf;
        UrbanInf.cover = 80;
        UrbanInf.mobility = 10;
        UrbanInf.firerange = -50;
        UrbanInf.visibility = -75;

        FieldData UrbanWheel;
        UrbanWheel.cover = 25;
        UrbanWheel.mobility = 20;
        UrbanWheel.firerange = -50;
        UrbanWheel.visibility = -50;
    }
}