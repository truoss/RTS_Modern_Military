using UnityEngine;
using System.Collections;

public class TestFieldManager : MonoBehaviour {

    public int x;
    public int y;
    public Field result;

    public HexUtils.HexDirection dir;
    public HexUtils.HexDiagonal dig;
    public Field neighbour;
    public Field diagonal;

    [ContextMenu("GetField")]
    public void GetField () {
        result = FieldManager.I.GetField(x, y);
    }

    public void GetNeighbour () {
        GetField();
        neighbour = FieldManager.I.GetOffsetNeighbour(result, dir);
    }

    public void GetDiagonal () {
        GetField();
        diagonal = FieldManager.I.GetDiagonalNeighbour(result, dig);
    }
}