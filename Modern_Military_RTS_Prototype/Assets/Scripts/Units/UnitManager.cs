using UnityEngine;
using UnityEngine.Networking;

public class UnitManager : NetworkBehaviour
{
    int lastUnitID = 0;

    public static UnitManager I;
    void Awake() {
        I = this;
    }

    [Command]
    public void CmdCreateUnit(string UnitName, Player.Side side)
    {
        //Server
        Debug.LogWarning("CmdCreateInfantry: " + side, this);
        //Generate ID
        //string ID = lastUnitID++;
        RpcCreateUnit(UnitName, lastUnitID++, side);
        /*
        var tmp = CreateUnit(UnitName, lastUnitID++, side);
        if (tmp)
            NetworkServer.Spawn(tmp.gameObject);
        else
            Debug.LogError("Could not create Unit!", this);
        */
    }

    /*
    Unit CreateUnit (string UnitName, int ID, Player.Side side) {
        UnitData data = null;
        for (int i = 0; i < UnitDataAsset.I.UnitLibrary.Length; i++) {
            if (UnitDataAsset.I.UnitLibrary[i].Name == UnitName) {
                data = UnitDataAsset.I.UnitLibrary[i];
                break;
            }
        }

        if (data == null) {
            Debug.LogError("No UnitData found: " + UnitName, this);
            return null;
        }

        var gObj = new GameObject("Unit");
        gObj.AddComponent<NetworkIdentity>();    
        gObj.layer = 10;
        var unit = gObj.AddComponent<Unit>();
        unit.UnitID = ID;
        unit.Side = side;
        unit.InitData(data);
        var rigid = gObj.AddComponent<Rigidbody>();
        rigid.isKinematic = true;

        Field field = null;
        for (int i = 0; i < GameLogic.I.FieldManager.SpawnableFields.Length; i++) {
            if (GameLogic.I.FieldManager.SpawnableFields[i].PlayerSide == side) {
                field = GameLogic.I.FieldManager.SpawnableFields[i];
                break;
            }
        }

        if (field != null) {
            field.Units.Add(unit);
            unit.CurrentField = field.gameObject;
            unit.UpdateStackPosition();
            GameLogic.I.Units.Add(unit);
            //player.Units.Add(unit);
            unit.transform.position = field.transform.position;
            return unit;
        } else 
            Destroy(unit.gameObject);

        return null;
    }
    */
    [Command]
    public void CmdMoveUnit (int UnitID, int FieldID) //TODO: WaypointArray
    {
        Debug.LogWarning("CmdMoveUnit: " + UnitID + ", " + FieldID, this);
        //TODO: set unit move target on all Clients
    }

    /*
    [Command]
    public void CmdMoveUnit (int UnitID, int[] Waypoints)
    {
        //TODO: set unit move target on all Clients
    }
    */

    
    [ClientRpc]
    public void RpcCreateUnit(string UnitName, int ID, Player.Side side)
    {
        //Jeder Client
        Debug.LogWarning("RpcCreateUnit: " + UnitName + " , " + side, this);
        UnitData data = null;
        for (int i = 0; i < UnitDataAsset.I.UnitLibrary.Length; i++)
        {
            if (UnitDataAsset.I.UnitLibrary[i].Name == UnitName)
            {
                data = UnitDataAsset.I.UnitLibrary[i];
                break;
            }
        }

        if (data == null) {
            Debug.LogError("No UnitData found: " + UnitName, this);
            return;
        }
        
        var gObj = new GameObject("Unit");        
        gObj.layer = 10;
        var unit = gObj.AddComponent<Unit>();
        unit.UnitID = ID;
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
}
