using UnityEngine;
using UnityEngine.Networking;

public class UnitManager : NetworkBehaviour
{
    public void CreateInfantry()
    {
        if (GameLogic.I.GetLocalPlayer().side != Player.Side.Neutral && GameLogic.I.GetLocalPlayer().curUnitsSpawned < GameLogic.I.GetLocalPlayer().spawnLimit)
        {
            CmdCreateInfantry(GameLogic.I.GetLocalPlayer().side);
            GameLogic.I.GetLocalPlayer().curUnitsSpawned++;
        }
    }

    [Command]
    void CmdCreateInfantry(Player.Side side)
    {
        Debug.LogWarning("CmdCreateInfantry: " + side, this);
        RpcCreateUnit(UnitDataAsset.I.UnitLibrary[0].Name, side);
    }

    
    [ClientRpc]
    public void RpcCreateUnit(string unitName, Player.Side side)
    {
        Debug.LogWarning("RpcCreateUnit: " + unitName + " , " + side, this);
        UnitData data = null;
        for (int i = 0; i < UnitDataAsset.I.UnitLibrary.Length; i++)
        {
            if (UnitDataAsset.I.UnitLibrary[i].Name == unitName)
            {
                data = UnitDataAsset.I.UnitLibrary[i];
                break;
            }
        }

        if (data == null)
            return;
        
        var gObj = new GameObject("Unit");
        //NetworkServer.SpawnWithClientAuthority(gObj, GetComponent<Player>().gameObject);
        gObj.layer = 10;
        var unit = gObj.AddComponent<Unit>();
        unit.Side = side;
        unit.InitData(data);
        var rigid = gObj.AddComponent<Rigidbody>();
        rigid.isKinematic = true;

        Field field = null;
        for (int i = 0; i < GameLogic.I.FieldManager.SpawnableFields.Length; i++)
        {
            if (GameLogic.I.FieldManager.SpawnableFields[i].PlayerSide == side)
            {
                field = GameLogic.I.FieldManager.SpawnableFields[i];
                break;
            }
        }

        if (field != null)
        {
            field.Units.Add(unit);
            unit.CurrentField = field.gameObject;
            unit.UpdateStackPosition();
            GameLogic.I.Units.Add(unit);
            //player.Units.Add(unit);
            unit.transform.position = field.transform.position;
        }
        else
            Destroy(unit.gameObject);
    }


    internal void Init (FieldManager FieldManager) {
        /*
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
        */
    }

    /*
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
    */
}
