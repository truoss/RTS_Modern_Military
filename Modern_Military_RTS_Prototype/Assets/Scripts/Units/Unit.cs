using UnityEngine;
using UnityEngine.Networking;

public class Unit : MonoBehaviour {
    public int UnitID;
    Renderer mesh;
    UnitData data;
    public UnitData UnitStats { get { return data; } }
    public Player.Side Side;

    public float curSoftpoints;
    public float curHardpoints;
    public Vector3 curCargo;

    NavMeshAgent agent;
    public bool isSelected {
        get {
            if (GameLogic.I)
                return GameLogic.I.SelectedUnit == this;
            else
                return false;
        }
    }

    public bool isMoving {
        get {
            if (agent)
                return agent.velocity != Vector3.zero;
            else if (CurrentField != TargetField && TargetField != null)
                return true;
            else
                return false;
        }
    }

    GameObject _targetField;
    public GameObject TargetField {
        get {
            return _targetField;
        }

        set {
            if (value != _targetField) {
                _targetField = value;
                if (agent && TargetField != null)
                    agent.destination = TargetField.transform.position;
            }
        }
    }
    public GameObject CurrentField;
    RaycastHit hit;

    Color baseColor;
    void Start () {
        mesh = GetComponentInChildren<Renderer>();
        baseColor = mesh.material.color;

        agent = GetComponent<NavMeshAgent>();

        //gameObject.AddComponent<NetworkTransform>();
    }

    public void InitData (UnitData input) {
        this.data = input;

        curSoftpoints = data.Attributes.maxSoftpoint;
        curHardpoints = data.Attributes.maxHardpoint;
        curCargo = data.Cargo.defaultCargo;

        var meshObj = Instantiate(data.Mesh);
        meshObj.transform.SetParent(transform);
        mesh = GetComponentInChildren<Renderer>();

        mesh.material = new Material(data.Material);
        gameObject.name = data.Name;
    }

    void Update () {
        if (isSelected == true) {
            mesh.material.color = Color.blue;
        } else {
            mesh.material.color = baseColor;
        }
    }

    void FixedUpdate () {
        if (isMoving) {
            transform.position = Vector3.Lerp(transform.position, TargetField.transform.position, data.Attributes.Mobility * Time.deltaTime);
        } else if (CurrentField != null) {
            UpdateStackPosition();
        }
    }

    public void DoUpdateMovement () {
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100.0F, LayerMask.NameToLayer("Field"))) {
            if (hit.transform.gameObject) {
                Debug.LogWarning(hit.transform.gameObject.name, hit.transform.gameObject);
                Field field = null;
                if (CurrentField == hit.transform.gameObject) {
                    field = CurrentField.GetComponent<Field>();
                    if (field.Units.Count > 0) {
                        if (!field.Units.Contains(this)) {
                            field.Units.Add(this);
                        }
                    }
                } else if (CurrentField != hit.transform.gameObject) {
                    if (CurrentField != null) {
                        field = CurrentField.GetComponent<Field>();
                        if (field.Units.Count > 0) 
                            field.Units.Remove(this);
                    }
                    CurrentField = hit.transform.gameObject;
                    field = CurrentField.GetComponent<Field>();                    
                        field.Units.Add(this);
                }
            }
        }
    }

    void OnMouseOver () {
        if (!isSelected)
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

    void OnMouseExit () {
        mesh.material.color = baseColor;
    }

    public void UpdateStackPosition()
    {
        var pos = CurrentField.transform.position;
        var field = CurrentField.GetComponent<Field>();
        int idx = -1;
        for (int i = 0; i < field.Units.Count; i++)
        {
            if (field.Units[i] == this)
            {
                idx = i;
                break;
            }
        }
        if (idx != -1)
        {
            transform.position = new Vector3(pos.x, pos.y + mesh.bounds.size.y * idx, pos.z);
        }
    }
}
