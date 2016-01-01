using UnityEngine;
using System.Collections.Generic;

public class Field : MonoBehaviour {
    public Renderer rend;
    MeshRenderer mesh;
    public List<BaseUnit> Units;
    //TODO: field types, values

	// Use this for initialization
	void Start () {
        rend = GetComponentInChildren<MeshRenderer>();
        mesh = GetComponentInChildren<MeshRenderer>();
        if (mesh == null)
            Debug.LogError("No MeshRenderer found!");
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnMouseOver () {
        if (Input.GetMouseButtonDown(0)) {
            rend.material.color -= Color.red;
        }
    }

    void OnMouseExit () {
        rend.material.color = Color.white;
    }
}
