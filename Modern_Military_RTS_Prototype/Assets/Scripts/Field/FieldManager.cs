using UnityEngine;

public class FieldManager : MonoBehaviour {

    //Tile IDs
    public const int PLAIN_HEX = 0, URBAN_HEX = 1;

    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;

    public GameObject HexMesh;

    //Hex tile size ingame
    private float hexWidth;
    private float hexHeight;

    //Generate grid on gamestart
    void Start () {
        setSizes();
        BuildGrid();
    }

    void Update () {
    }

    //Initialise Hexagon size
    void setSizes () {
        hexWidth = HexMesh.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        hexHeight = HexMesh.GetComponentInChildren<MeshRenderer>().bounds.size.z;
    }

    //Calculate first hexagon position
    Vector3 calcInitPos () {
        Vector3 initPos;
        //Inital position left bottom corner
        initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0, -gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
        return initPos;
    }

    //Convert Hex grids coordinates to world coordinates
    public Vector3 calcWorldCoords (Vector2 gridPos) {
        //Position of the first hex tile
        Vector3 initPos = calcInitPos();
        //Every second row is offset by half of the tile width
        float offset = 0;
        if (gridPos.y % 2 != 0) {
            offset = hexWidth / 2;
        }

        float x = initPos.x + offset + gridPos.x * hexWidth;
        //Every new line is offset in z direction by 3/4 of the hexagon height
        float z = initPos.z + gridPos.y * hexHeight * 0.75f;
        return new Vector3(x, 0, z);
    }

    //Initilises and positions all tiles
    public void BuildGrid () {
        GameObject hexGridGO = new GameObject("FieldGrid");

        for (float y = 0; y < gridHeightInHexes; y++) {
            for (float x = 0; x < gridWidthInHexes; x++) { 
                var gobj = new GameObject("Field" + calcWorldCoords(new Vector2(x, y)));
                gobj.transform.SetParent(hexGridGO.transform);
                var field = gobj.AddComponent<Field>();
                if (field) {
                    field.fieldType = GetRandomFieldType();
                    gobj.AddComponent<Rigidbody>();
                    gobj.GetComponent<Rigidbody>().isKinematic = true;
                }
                GameObject hex = (GameObject) Instantiate(HexMesh);
                hex.transform.SetParent(gobj.transform);
                gobj.transform.position = calcWorldCoords(new Vector2(x, y));
                field.Init();
            }
        }
        Debug.Log("Done creating Grid");
    }

    public FieldData.FieldType GetRandomFieldType () {
        return FieldData.FieldType.Plain;
    }
}
