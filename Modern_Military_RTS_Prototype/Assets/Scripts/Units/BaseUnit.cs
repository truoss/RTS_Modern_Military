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

    public bool isMoving
    {
        get
        {
            if (agent)
                return agent.velocity != Vector3.zero;
            else
                return false;
        }
    }

    public float MovementSpeed;

    GameObject _targetField;
    public GameObject TargetField
    {
        get
        {
            return _targetField;
        }

        set
        {
            if (value != _targetField)
            {
                _targetField = value;
                if (agent && TargetField != null)
                    agent.destination = TargetField.transform.position;
            }
        }
    }
    public GameObject CurrentField;
    RaycastHit hit;

    Color baseColor;
    void Start()
    {
        mesh = GetComponentInChildren<Renderer>();
        baseColor = mesh.material.color;

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isSelected == true)
        {
            mesh.material.color = Color.blue;
        }
        else
        {
            mesh.material.color = baseColor;
        }
    }

    public void DoFixedUpdate()
    {
        if (isMoving)
        {
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100.0F))
            {
                if (hit.transform.gameObject)
                    CurrentField = hit.transform.gameObject;
            }
        }
    }

    void OnMouseOver()
    {
        if(!isSelected)
            mesh.material.color = baseColor * 1.5f;

        /*
        * Done in GameState
        * Do only hover?
        if (Input.GetMouseButtonDown(0))
        {
            GameLogic.I.SelectUnit(this);
        }
        */
    }

    void OnMouseExit()
    {
        mesh.material.color = baseColor;
    }
}
