using UnityEngine;
using System.Collections;

public class TestFieldManager : MonoBehaviour {

    public int x;
    public int y;
    public Field result;

    public FieldManager.FieldDirection dir;
    public Field neighbour;

    [ContextMenu("GetField")]
    public void GetField () {
        result = FieldManager.I.GetField(x, y);
    }

    public void GetNeighbour () {
        GetField();
        neighbour = FieldManager.I.GetOffsetNeighbour(result, dir);
    }
}