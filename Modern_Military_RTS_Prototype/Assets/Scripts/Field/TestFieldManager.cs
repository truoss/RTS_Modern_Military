using UnityEngine;
using System.Collections;

public class TestFieldManager : MonoBehaviour {

    public int x;
    public int y;
    public Field result;

    [ContextMenu("GetField")]
    public void GetField () {
        result = FieldManager.I.GetField(x, y);
    }
}
