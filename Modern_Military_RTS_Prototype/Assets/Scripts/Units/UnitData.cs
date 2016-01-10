using UnityEngine;

[System.Serializable]
public class UnitData {
    public enum MobilityType {
        Footed,
        Wheeled,
        Tracked
    }

    public enum SpecialType {
        Infantry,
        Logistic,
        Artillery
    }

    public enum ArmorType {
        Softpoint,
        Hardpoint
    }

    public enum CargoType {
        Infantry,
        Ammunition,
        Fuel,
        Empty
    }

    public string Name;
    public Material Material;
    public GameObject Mesh;
    public MobilityType mobilityType;
    public SpecialType specialType;
    public ArmorType BestAgainst;
    public Cargo Cargo;
    public Attributes Attributes;

    //TODO: Spezial
}

[System.Serializable]
public class Attributes {
    public float Mobility;
    public float FirepowerSoft;
    public float FirepowerHard;
    public float Firerange;
    public float Cover;
    public float Visibility;
    public float ViewDistance;
    //public float Fuel;
    public float FuelConsumption;
    //public float Ammunition;
    public float AmmunitionConsumption;
    public float maxSoftpoint;
    public float maxHardpoint;
}

[System.Serializable]
public class Cargo {
    public int maxCargo;
    public Vector3 allowedCargoTypes = new Vector3(1, 1, 1); // (fuel, ammo, infantry)
    public Vector3 defaultCargo = new Vector3(0, 0, 0);
}