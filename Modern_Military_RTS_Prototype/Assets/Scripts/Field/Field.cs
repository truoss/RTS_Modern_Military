using UnityEngine;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
    public MeshRenderer mesh;
    public List<BaseUnit> Units;

    //TODO: field types, values

	// Use this for initialization
	void Start () {
        mesh = GetComponentInChildren<MeshRenderer>();
        if (mesh == null)
            Debug.LogError("No MeshRenderer found!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
