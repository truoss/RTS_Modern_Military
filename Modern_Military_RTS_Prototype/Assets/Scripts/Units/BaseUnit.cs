using UnityEngine;
using System.Collections;

public class BaseUnit : MonoBehaviour
{
    Renderer mesh;
    NavMeshAgent agent;
    public bool isSelected {
        get
        {
            if (GameLogic.I)
                return GameLogic.I.SelectedUnit == this;
            else
                return false;
        }
    }    

    public bool isMoving = false;

    public float MovementSpeed;

    public GameObject TargetField;
    public GameObject CurrentField;

    Color baseColor;
    void Start()
    {
        mesh = GetComponentInChildren<Renderer>();
        baseColor = mesh.material.color;

        agent = GetComponent<NavMeshAgent>();
    }

    //TODO:
    //move from a to b
    public void DoFixedUpdate()
    {
        if (TargetField && agent)
            agent.destination = TargetField.transform.position;        
    }

    void OnMouseOver()
    {
        mesh.material.color -= new Color(0.1F, 0, 0) * Time.deltaTime;
        if (Input.GetMouseButtonDown(0))
        {
            GameLogic.I.SelectUnit(this);
        }
    }

    void OnMouseExit()
    {
        mesh.material.color = baseColor;
    }
}
