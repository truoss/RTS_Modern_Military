using UnityEngine;

public class FieldManager : MonoBehaviour {

    public GameObject Hex;
    public int gridWidthInHexes = 10;
    public int gridHeightInHexes = 10;

    //Hex tile size ingame
    private float hexWidth;
    private float hexHeight;

    //Generate grid on gamestart
	void Start () {
        setSizes();
        createGrid();
	}

	void Update () {
	}

    //Initialise Hexagon size
    void setSizes() {
        hexWidth = Hex.GetComponentInChildren<MeshRenderer>().bounds.size.x;
        hexHeight = Hex.GetComponentInChildren<MeshRenderer>().bounds.size.z;
    }

    //Calculate first hexagon position
    Vector3 calcInitPos() {
        Vector3 initPos;
        //Inital position left bottom corner
        initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0, -gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
        return initPos;
    }

    //Convert Hex grids coordinates to world coordinates
    public Vector3 calcWorldCoords(Vector2 gridPos) {
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
    void createGrid () {
        GameObject hexGridGO = new GameObject("HexGrid");

        for (float y = 0; y < gridHeightInHexes; y++) {
            for (float x = 0; x < gridWidthInHexes; x++) {
                GameObject hex = (GameObject) Instantiate(Hex);
                //Current position in grid
                Vector2 gridPos = new Vector2(x, y);
                hex.transform.position = calcWorldCoords(gridPos);
                hex.transform.parent = hexGridGO.transform;
            }
        }
    }
}
