using UnityEngine;

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

    internal void Init (FieldManager FieldManager) {
        //Red
        var unit = CreateRedRandomUnit();
        Field field = null;
        for (int i = 0; i < FieldManager.SpawnableFields.Length; i++) {
            if (FieldManager.SpawnableFields[i].PlayerSide == Player.Side.Red) {
                field = FieldManager.SpawnableFields[i];
                break;
            }
        }
        if (field != null) {
            GameLogic.I.Units.Add(unit);
            unit.transform.position = field.transform.position;
        }
        //Blue
        unit = CreateBlueRandomUnit();
        for (int i = 0; i < FieldManager.SpawnableFields.Length; i++) {
            if (FieldManager.SpawnableFields[i].PlayerSide == Player.Side.Blue) {
                field = FieldManager.SpawnableFields[i];
                break;
            }
        }
        if (field != null) {
            GameLogic.I.Units.Add(unit);
            unit.transform.position = field.transform.position;
        }
    }
}
