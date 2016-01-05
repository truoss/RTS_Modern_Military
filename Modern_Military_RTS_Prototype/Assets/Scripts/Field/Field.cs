using UnityEngine;
using System.Collections.Generic;

public class Field : MonoBehaviour {
    Renderer mesh;

    public List<Unit> Units;
    public string Name;
    
    public FieldData.FieldType fieldType;

    public void Init () {
        mesh = GetComponentInChildren<MeshRenderer>();
        if (mesh == null) {
            Debug.LogError("No MeshRenderer found!");
        }
	}

	void Update () {
	    if (isSelected == true) {
            mesh.material.color = Color.red;
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
