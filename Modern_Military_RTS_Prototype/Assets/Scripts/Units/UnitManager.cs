using UnityEngine;
using System.Collections;

public class UnitManager : MonoBehaviour
{
    [ContextMenu("Create Red Unit")]
    public Unit CreateRedRandomUnit()
    {
        return CreateRandomUnit(Player.Side.Red);
    }

    [ContextMenu("Create Blue Unit")]
    public Unit CreateBlueRandomUnit()
    {
        return CreateRandomUnit(Player.Side.Blue);
    }

    //[ContextMenu("Create Unit")]
    public Unit CreateRandomUnit(Player.Side side)
    {
        var gObj = new GameObject("Unit");
        var unit = gObj.AddComponent<Unit>();
        unit.Side = side;
        unit.InitData(UnitDataAsset.I.UnitLibrary[Random.Range(0, UnitDataAsset.I.UnitLibrary.Length)]);
        var rigid = gObj.AddComponent<Rigidbody>();
        rigid.isKinematic = true;

        return unit;
    }
}
