using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestFieldManager : MonoBehaviour
{

    public int x;
    public int y;
    public int range = 5;
    public Field result;

    public HexUtils.HexDirection dir;
    public HexUtils.HexDiagonal dig;
    public Field neighbour;
    public Field diagonal;

    public float waitTime = 5;
    bool isRunning = false;

    [ContextMenu("GetField")]
    public void GetField ()
    {
        result = FieldManager.I.GetField(x, y);
    }

    public void GetNeighbour ()
    {
        GetField();
        neighbour = FieldManager.I.GetOffsetNeighbour(result, dir);
    }

    public void GetDiagonal ()
    {
        GetField();
        diagonal = FieldManager.I.GetDiagonalNeighbour(result, dig);
    }

    public void TestLinedrawing ()
    {
        if (isRunning == false)
            StartCoroutine(AsyncMultiLinedrawing());
    }

    public void TestCoordinateRange ()
    {
        Field curField;
        Field midField;

        Vector2[] rangeCoords = HexUtils.OffsetCoordinateRange(new Vector2(x, y), range);
        for (int i = 0; i < rangeCoords.Length; i++) {
            //hexcoord = HexUtils.CubeToOffset(coords[i]);
            curField = FieldManager.I.GetField(rangeCoords[i]);
            if (curField != null) {
                curField.SetColor(Color.blue);
            }
        }
        midField = FieldManager.I.GetField(x, y);
        if (midField != null)
            midField.SetColor(Color.red);
    }

    public void TestReachable ()
    {
        Field curField;
        Field midField;
        
        Vector2[] reachableCoords = OffsetReachable(new Vector2(x, y), range);
        for (int i = 0; i < reachableCoords.Length; i++) {
            //hexcoord = HexUtils.CubeToOffset(coords[i]);
            curField = FieldManager.I.GetField(reachableCoords[i]);
            if (curField != null) {
                curField.SetColor(Color.blue);
            }
        }
        midField = FieldManager.I.GetField(x, y);
        if (midField != null)
            midField.SetColor(Color.yellow);            
    }

    public void ResetAllColor ()
    {
        foreach (Vector2 key in FieldManager.I.Map.Keys) {
            FieldManager.I.GetField(key).SetColor(Color.white);
        }
    }

    private IEnumerator AsyncMultiLinedrawing ()
    {
        isRunning = true;
        Field startField = null;
        Field endField = null;

        List<Field> midPoints = new List<Field>();
        List<Vector2> allOffsetCoords = new List<Vector2>();

        RaycastHit hit;
        Field field;
        // Waits for firstselection
        while (startField == null) {
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Camera.main.farClipPlane)) {
                    if (hit.transform.GetComponent<Field>()) {
                        field = hit.transform.GetComponent<Field>();
                        startField = field;
                    }
                }
            }
            yield return null;
        }
        startField.SetColor(Color.red);

        //Wenn shifttaste gedrückt

        // Waits for selection after startfield was selected
        while (endField == null) {
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Camera.main.farClipPlane)) {
                    if (hit.transform.GetComponent<Field>()) {
                        field = hit.transform.GetComponent<Field>();
                        midPoints.Add(field);
                        if (midPoints.Count == 1) {
                            allOffsetCoords.AddRange(OffsetLinedrawColor(startField, midPoints[0], Color.green));
                        } else if (midPoints.Count > 1) {
                            for (int i = 1; i < midPoints.Count; i++) {
                                allOffsetCoords.AddRange(OffsetLinedrawColor(midPoints[i - 1], midPoints[i], Color.green));
                            }
                        }
                        field.SetColor(Color.yellow);
                        field = null;
                    }
                }
            }
            // Sets Endpoint
            if (Input.GetMouseButtonDown(1)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Camera.main.farClipPlane)) {
                    if (hit.transform.GetComponent<Field>()) {
                        field = hit.transform.GetComponent<Field>();
                        if (field != startField) {
                            endField = field;
                            if (midPoints.Count == 0) {
                                allOffsetCoords.AddRange(OffsetLinedrawColor(startField, endField, Color.green));
                            } else {
                                allOffsetCoords.AddRange(OffsetLinedrawColor(midPoints[midPoints.Count - 1], endField, Color.green));
                            }
                        }
                    }
                }
            }
            yield return null;
        }
        endField.SetColor(Color.red);

        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < allOffsetCoords.Count; i++) {
            //var hexcoord = HexUtils.CubeToOffset(allOffsetCoords[i]);
            var curField = FieldManager.I.GetField(allOffsetCoords[i]);
            if (curField != null) {
                curField.SetColor(Color.white);
            }
        }
        Debug.Log("Testline Done");
        isRunning = false;

    }

    private static Vector2[] OffsetLinedrawColor (Field startField, Field endField, Color color)
    {
        Vector2[] coords;
        Field curField;

        Vector3 coordStart = HexUtils.OffsetToCube(startField.x, startField.y);
        Vector3 coordEnd = HexUtils.OffsetToCube(endField.x, endField.y);

        //Debug.LogWarning("coordStart: " + coordStart.ToString("f2") + " , end: " + coordEnd.ToString("f2"));

        coords = HexUtils.OffsetLinedraw(coordStart, coordEnd);

        // Colors Fields inbetween start & endfield
        for (int i = 0; i < coords.Length; i++) {
            //hexcoord = HexUtils.CubeToOffset(coords[i]);
            curField = FieldManager.I.GetField(coords[i]);
            if (curField != null && curField != startField && curField != endField) {
                curField.SetColor(color);
            }
        }
        return coords;
    }

    public List<List<Vector2>> fringes = new List<List<Vector2>>();
    
    public Vector2[] OffsetReachable (Vector2 start, int movement)
    {
        Vector2 neighbor;
        List<Vector2> visited = new List<Vector2>();
        visited.Add(start);
        fringes.Clear();
        var tmp = new List<Vector2>();
        tmp.Add(start);
        fringes.Add(tmp);

        for (int i = 1; i <= movement; i++) {
            fringes.Add(new List<Vector2>());
            for (int j = 0; j < fringes[i - 1].Count; j++) {
                Debug.LogWarning(fringes[i - 1].Count + " i: " + i);
                for (int n = 0; n < 6; n++) {
                    if(fringes[i - 1][j].y % 2 == 0)
                        neighbor = HexUtils.GetValueFromHexDirEven((HexUtils.HexDirection) n);
                    else
                        neighbor = HexUtils.GetValueFromHexDirOdd((HexUtils.HexDirection) n);
                    //Debug.LogWarning(neighbor.ToString("f4") + " | " + fringes[i - 1][j].ToString("f4"));
                    //neighbor.x += fringes[i - 1][j].x;
                    //neighbor.y += fringes[i - 1][j].y;
                    neighbor += fringes[i - 1][j];

                    //TODO Blocked Hex
                    if (!visited.Contains(neighbor)) {
                        visited.Add(neighbor);
                        fringes[i].Add(neighbor);
                    }
                }
            }
        }
        
        return visited.ToArray();
    }    
}