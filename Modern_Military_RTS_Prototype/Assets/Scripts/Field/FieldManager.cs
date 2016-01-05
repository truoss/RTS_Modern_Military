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

    public Field[,] Map;
    public Field[] SpawnableFields;

    //Generate grid on gamestart
    public bool Init () {
        setSizes();
        BuildMap();
        SpawnableFields = new Field[2];
        SpawnableFields[0] = Map[0, Mathf.RoundToInt(gridHeightInHexes * 0.5f)];
        Map[0, Mathf.RoundToInt(gridHeightInHexes * 0.5f)].PlayerSide = Player.Side.Blue;
        Map[0, Mathf.RoundToInt(gridHeightInHexes * 0.5f)].isSpawnable = true;
        SpawnableFields[1] = Map[Mathf.RoundToInt(gridWidthInHexes - 1), Mathf.RoundToInt(gridHeightInHexes * 0.5f)];
        Map[Mathf.RoundToInt(gridWidthInHexes - 1), Mathf.RoundToInt(gridHeightInHexes * 0.5f)].PlayerSide = Player.Side.Red;
        Map[Mathf.RoundToInt(gridWidthInHexes - 1), Mathf.RoundToInt(gridHeightInHexes * 0.5f)].isSpawnable = true;
        return true;
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
    public void BuildMap () {
        GameObject hexGridGO = new GameObject("FieldGrid");
        Map = new Field[gridHeightInHexes, gridWidthInHexes];
        for (int y = 0; y < gridHeightInHexes; y++) {
            for (int x = 0; x < gridWidthInHexes; x++) {
                var gobj = new GameObject("Field" + calcWorldCoords(new Vector2(x, y)));
                gobj.transform.SetParent(hexGridGO.transform);
                var field = gobj.AddComponent<Field>();
                if (field) {
                    field.fieldType = GetRandomFieldType();
                    gobj.AddComponent<Rigidbody>();
                    gobj.GetComponent<Rigidbody>().isKinematic = true;
                }
                field.Init();
                gobj.transform.position = calcWorldCoords(new Vector2(x, y));
                Map[x, y] = field;
            }
        }
        Debug.Log("Done creating Grid");
    }

    public FieldData.FieldType GetRandomFieldType () {
        return FieldDataAsset.I.FieldTypes[Random.Range(0, FieldDataAsset.I.FieldTypes.Length)].fieldType;
    }
}
