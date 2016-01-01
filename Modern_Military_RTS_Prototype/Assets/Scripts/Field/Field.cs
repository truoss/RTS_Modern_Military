using UnityEngine;
using System.Collections.Generic;

public class Field : MonoBehaviour {
    Renderer mesh;

    public List<BaseUnit> Units;

    public bool isSelected {
        get {
            if (GameLogic.I) {
                return GameLogic.I.SelectedField == this;
            } else {
                return false;
            }
        }
    }

	void Start () {
        mesh = GetComponentInChildren<MeshRenderer>();
        if (mesh == null)
            Debug.LogError("No MeshRenderer found!");
	}

	void Update () {
	    if (isSelected == true) {
            mesh.material.color = Color.red;
        } else {
            mesh.material.color = Color.white;
        }
	}

    void OnMouseOver () {
        if (Input.GetMouseButtonDown(0)) {
            GameLogic.I.SelectField(this);
        }
    }
}
