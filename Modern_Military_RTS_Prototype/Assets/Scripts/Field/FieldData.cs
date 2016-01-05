using UnityEngine;

[System.Serializable]
public class FieldData {

    public enum FieldType {
        Plain,
        Urban,
        NotSet
    }

    public FieldType fieldType;
    public Material Material;
    public GameObject Mesh;

    public FieldMobilityType[] FieldMobilityType;

}

[System.Serializable]
public class FieldAttribute {
    public float cover;
    public float mobility;
    public float visibility;
}

[System.Serializable]
public class FieldMobilityType {
    public UnitData.MobilityType MobilityType;
    public FieldAttribute FieldAttribute;
}