using UnityEngine;
using System.Collections;

[System.Serializable]
public class UnitData
{
    public enum MobilityType
    {
        Footed,
        Wheeled,
        Tracked
    }

    public enum SpecialType
    {
        Infantry,
        Logistic,
        Artillery
    }

    public enum ArmorType
    {
        Softpoint,
        Hardpoint
    }

    public enum CargoType
    {
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
public class Attributes
{
    public float Mobility;
    public float FirepowerSoft;
    public float FirepowerHard;
    public float Firerange;
    public float Cover;
    public float Visibility;
    //public float Fuel;
    public float FuelConsumption;
    //public float Ammunition;
    public float AmmunitionConsumption;
    public float maxSoftpoint;
    public float maxHardpoint;
}

[System.Serializable]
public class Cargo
{
    public int maxCargo;
    public Vector3 allowedCargoTypes = new Vector3(1, 1, 1); // (fuel, ammo, infantry)
    public Vector3 defaultCargo = new Vector3(0, 0, 0);
}

public class Unit : MonoBehaviour
{    
    Renderer mesh;
    UnitData data;
    public UnitData UnitStats { get { return data; } }
    public Player.Side Side;

    public float curSoftpoints;
    public float curHardpoints;
    public Vector3 curCargo;

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

    public void InitData(UnitData input)
    {
        this.data = input;

        curSoftpoints = data.Attributes.maxSoftpoint;
        curHardpoints = data.Attributes.maxHardpoint;
        curCargo = data.Cargo.defaultCargo;

        var meshObj = Instantiate(data.Mesh);
        meshObj.transform.SetParent(transform);
        mesh = GetComponentInChildren<Renderer>();

        mesh.material = new Material(data.Material);
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
