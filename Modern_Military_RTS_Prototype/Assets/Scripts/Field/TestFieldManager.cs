using UnityEngine;
using System.Collections;

public class TestFieldManager : MonoBehaviour
{

    public int x;
    public int y;
    public Field result;

    public HexUtils.HexDirection dir;
    public HexUtils.HexDiagonal dig;
    public Field neighbour;
    public Field diagonal;

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
            StartCoroutine(AsyncTestLinedrawing());
    }

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
        Debug.LogWarning("Startfeld gesetzt: " + startField);

        // Waits for selection after startfield was selected
        while (endField == null) {
            if (GameLogic.I.SelectedField != null && GameLogic.I.SelectedField != startField) {
                endField = GameLogic.I.SelectedField;
            }
            yield return new WaitForFixedUpdate();
        }
        endField.SetColor(Color.red);
        Debug.LogWarning("Endfeld gesetzt: " + endField);

        Vector3 coordStart = HexUtils.OffsetToCube(startField.x, startField.y);
        Vector3 coordEnd = HexUtils.OffsetToCube(endField.x, endField.y);

        Debug.LogWarning("coordStart: " + coordStart.ToString("f2") + " , end: " + coordEnd.ToString("f2"));

        Vector3[] coords = HexUtils.CubeLinedraw(coordStart, coordEnd);
        Vector2 hexcoord;
        Field curField;

        // Colors Fields inbetween start & endfield
        for (int i = 0; i < coords.Length; i++) {
            hexcoord = HexUtils.CubeToOffset(coords[i]);
            curField = FieldManager.I.GetField(hexcoord);
            if (curField != null && curField != startField && curField != endField) {
                curField.SetColor(Color.green);
            }
        }

        // Resets Colors after time
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < coords.Length; i++) {
            hexcoord = HexUtils.CubeToOffset(coords[i]);
            curField = FieldManager.I.GetField(hexcoord);
            if (curField != null) {
                curField.SetColor(Color.white);
            }
        }
        Debug.LogWarning("Testline Done");
        isRunning = false;
    }
}