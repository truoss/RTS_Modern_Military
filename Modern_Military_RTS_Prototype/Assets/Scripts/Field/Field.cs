using UnityEngine;
using System.Collections.Generic;

public class Field : MonoBehaviour {
    public int FieldID;
    Renderer mesh;

    public List<Unit> Units = new List<Unit>();
    private FieldData Data;
    public int x;
    public int y;

    public bool isSpawnable = false;
    public Player.Side PlayerSide = Player.Side.Neutral;

    public FieldData.FieldType fieldType = FieldData.FieldType.NotSet;

    public void Init (int x, int y) {
        if (fieldType == FieldData.FieldType.NotSet) {
            Debug.LogError("fieldType not set", this);
            return;
        }
        for (int i = 0; i < FieldDataAsset.I.FieldTypes.Length; i++) {
            if (FieldDataAsset.I.FieldTypes[i].fieldType == fieldType) {
                Data = FieldDataAsset.I.FieldTypes[i];
                break;
            }
        }
        if (Data == null) {
            Debug.LogError("couldn't find " + fieldType.ToString() + "in FieldAssetData", this);
            return;
        }
        GameObject hex = (GameObject) Instantiate(Data.Mesh);
        hex.transform.SetParent(transform);
        mesh = GetComponentInChildren<MeshRenderer>();
        if (mesh == null) {
            Debug.LogError("No MeshRenderer found!");
        } else {
            mesh.material = new Material(Data.Material);
        }
        this.x = x;
        this.y = y;
    }

    void Update () {
        if (isSelected == true) {
            mesh.material.color = Color.red;
        } else if (isSpawnable == true) {
            mesh.material.color = Color.yellow;
        } else {
            mesh.material.color = Data.Material.color;
        }
    }

    void OnMouseOver () {
        /*
        * Done in GameState
        * Do only hover?
        if (Input.GetMouseButtonDown(0))
        {
            GameLogic.I.SelectUnit(this);
        }
        */
    }

    public bool isSelected {
        get {
            if (GameLogic.I) {
                return GameLogic.I.SelectedField == this;
            } else {
                return false;
            }
        }
    }
}
