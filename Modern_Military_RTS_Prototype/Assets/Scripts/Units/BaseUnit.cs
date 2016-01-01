using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour
{
    Renderer mesh;
    public bool isSelected = false;
    public bool isMoving = false;

    public float MovementSpeed;

    public GameObject TargetField;
    public GameObject CurrentField;

    Color baseColor;
    void Start()
    {
        mesh = GetComponent<Renderer>();
        baseColor = mesh.material.color;
    }

    //TODO:
    //move from a to b
    //select Unit

    
    
    void OnMouseEnter()
    {
        mesh.material.color = Color.red;
    }

    void OnMouseOver()
    {
        mesh.material.color -= new Color(0.1F, 0, 0) * Time.deltaTime;
    }

    void OnMouseExit()
    {
        mesh.material.color = baseColor;
    }
}
