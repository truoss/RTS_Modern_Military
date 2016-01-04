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

    public enum MobilityType
    {
        Footed,
        Wheeled,
        Tracked
    }

    public enum SpecialType
    {
        Infantry,
        Cargo,
        Logistic,
        Artillery
    }

    public enum ArmorType
    {
        Softpoint,
        Hardpoint
    }

    public Unit[] UnitLibrary;

    [System.Serializable]
    public class Unit
    {
        public string Name;
        public MobilityType MobilityType;
        public SpecialType SpecialType;
        public ArmorType BestAgainst;
        public Attributes Attributes;
    }

    [System.Serializable]
    public class Attributes
    {
        public float Mobility;
        public float FirepowerSoft;
        public float FirepowerHard;
        public float Firerange;
        public float Cover;
        public float Visibility;
        public float Fuel;
        public float FuelConsumption;
        public float Ammunition;
        public float AmmunitionConsumption;
        public float Softpoint;
        public float Hardpoint;        
    }    
}


