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

    [ClientRpc]
    public void RpcCreateUnit (string UnitName, int ID, Player.Side side) {
        //Jeder Client
        Debug.LogWarning("RpcCreateUnit: " + UnitName + " , " + side, this);
        UnitData data = null;
        for (int i = 0; i < UnitDataAsset.I.UnitLibrary.Length; i++) {
            if (UnitDataAsset.I.UnitLibrary[i].Name == UnitName) {
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
        } else
            Destroy(unit.gameObject);
    }

    [Command]
    public void CmdMoveUnit (NetworkInstanceId playerID, int UnitID, int FieldID) //TODO: WaypointArray
    {
        Debug.LogWarning("CmdMoveUnit: " + playerID + ", " + UnitID + ", " + FieldID, this);
        //Validate cmd
        //check for player
        //check for unit
        //check for field

        //TODO: set unit move target on all Clients
        if (ValidatePlayerID(playerID) && ValidateUnitID(UnitID) && ValidateFieldID(FieldID))
            RpcMoveUnit(UnitID, FieldID);
    }

    bool ValidatePlayerID (NetworkInstanceId playerID) {
        for (int i = 0; i < GameLogic.I.player.Count; i++) {
            if (GameLogic.I.player[i].GetComponent<NetworkIdentity>().netId == playerID) {
                return true;
            }
        }
        return false;
    }

    bool ValidateUnitID (int UnitID) {
        for (int i = 0; i < GameLogic.I.Units.Count; i++) {
            if (GameLogic.I.Units[i].UnitID == UnitID) {
                return true;
            }
        }
        return false;
    }

    bool ValidateFieldID (int FieldID) {
        for (int y = 0; y < FieldManager.I.gridHeightInHexes; y++) {
            for (int x = 0; x < FieldManager.I.gridWidthInHexes; x++) {
                if (FieldManager.I.Map[x, y].FieldID == FieldID)
                    return true;
            }
        }
                return false;
    }

    /*
    [Command]
    public void CmdMoveUnit (int UnitID, int[] Waypoints)
    {
        //TODO: set unit move target on all Clients
    }
    */
    [ClientRpc]
    public void RpcMoveUnit (int UnitID, int FieldID) 
    {
        Field field = null;
        Unit unit = null;
        
        //get unit from id
        for (int i = 0; i < GameLogic.I.Units.Count; i++) {
            if (GameLogic.I.Units[i].UnitID == UnitID) {
                unit = GameLogic.I.Units[i];
            }
        }
        if (unit == null) {
            Debug.LogError("Could not find Unit!", this);
            return;
        }

        //get field from id
        for (int y = 0; y < FieldManager.I.gridHeightInHexes; y++) {
            for (int x = 0; x < FieldManager.I.gridWidthInHexes; x++) {
                if (FieldManager.I.Map[x, y].FieldID == FieldID)
                    field = FieldManager.I.Map[x, y];
            }
        }
        if (field == null) {
            Debug.LogError("Could not find Field!", this);
            return;
        }

        unit.TargetField = field.gameObject;
    }
}
