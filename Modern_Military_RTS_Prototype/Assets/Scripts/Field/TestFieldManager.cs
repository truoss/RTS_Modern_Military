using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestFieldManager : MonoBehaviour
{

    public int x;
    public int y;
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

    private IEnumerator AsyncMultiLinedrawing ()
    {
        isRunning = true;
        Field startField = null;
        Field endField = null;

        List<Field> midPoints = new List<Field>();
        List<Vector3> allCubeCoords = new List<Vector3>();

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
                            allCubeCoords.AddRange(CubeLinedrawColor(startField, midPoints[0], Color.green));
                        } else if (midPoints.Count > 1) {
                            for (int i = 1; i < midPoints.Count; i++) {
                                allCubeCoords.AddRange(CubeLinedrawColor(midPoints[i - 1], midPoints[i], Color.green));
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
                                allCubeCoords.AddRange(CubeLinedrawColor(startField, endField, Color.green));
                            } else {
                                allCubeCoords.AddRange(CubeLinedrawColor(midPoints[midPoints.Count - 1], endField, Color.green));
                            }
                        }
                    }
                }
            }
            yield return null;
        }
        endField.SetColor(Color.red);

        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < allCubeCoords.Count; i++) {
            var hexcoord = HexUtils.CubeToOffset(allCubeCoords[i]);
            var curField = FieldManager.I.GetField(hexcoord);
            if (curField != null) {
                curField.SetColor(Color.white);
            }
        }
        Debug.Log("Testline Done");
        isRunning = false;

    }

    /*
    private IEnumerator AsyncTestLinedrawing ()
    {
        isRunning = true;
        Field startField = null;
        Field endField = null;

        // Waits for firstselection
        while (startField == null) {
            if (GameLogic.I.SelectedField != null) {
                startField = GameLogic.I.SelectedField;

            }
            yield return new WaitForFixedUpdate();
        }
        startField.SetColor(Color.red);

        // Waits for selection after startfield was selected
        while (endField == null) {
            if (GameLogic.I.SelectedField != null && GameLogic.I.SelectedField != startField) {
                endField = GameLogic.I.SelectedField;
            }
            yield return new WaitForFixedUpdate();
        }

        endField.SetColor(Color.red);

        Vector2 hexcoord;
        Field curField;
        Vector3[] coords = CubeLinedrawColor(startField, endField, Color.green);

        // Resets Colors after time
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < coords.Length; i++) {
            hexcoord = HexUtils.CubeToOffset(coords[i]);
            curField = FieldManager.I.GetField(hexcoord);
            if (curField != null) {
                curField.SetColor(Color.white);
            }
        }
        Debug.Log("Testline Done");
        isRunning = false;
    }
    */

    private static Vector3[] CubeLinedrawColor (Field startField, Field endField, Color color)
    {
        Vector3[] coords;
        Vector2 hexcoord;
        Field curField;

        Vector3 coordStart = HexUtils.OffsetToCube(startField.x, startField.y);
        Vector3 coordEnd = HexUtils.OffsetToCube(endField.x, endField.y);

        //Debug.LogWarning("coordStart: " + coordStart.ToString("f2") + " , end: " + coordEnd.ToString("f2"));

        coords = HexUtils.CubeLinedraw(coordStart, coordEnd);

        // Colors Fields inbetween start & endfield
        for (int i = 0; i < coords.Length; i++) {
            hexcoord = HexUtils.CubeToOffset(coords[i]);
            curField = FieldManager.I.GetField(hexcoord);
            if (curField != null && curField != startField && curField != endField) {
                curField.SetColor(color);
            }
        }

        return coords;
    }
}