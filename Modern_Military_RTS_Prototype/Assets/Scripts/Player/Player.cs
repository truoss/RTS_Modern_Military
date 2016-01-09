using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Side
    {
        Blue,
        Red,
        Neutral
    }

    public string Name = "Player";
    public Side side = Side.Neutral;
    public bool isLocal = false;

    public StateMachine statemachine;

    public int spawnLimit = 3;
    public int curUnitsSpawned = 0;
    //public List<Unit> Units = new List<Unit>();

    void Update()
    {
        if(statemachine != null)
            statemachine.Update();
    }

    public void CreateInfantry () {
        if (GameLogic.I.GetLocalPlayer().side != Player.Side.Neutral && GameLogic.I.GetLocalPlayer().curUnitsSpawned < GameLogic.I.GetLocalPlayer().spawnLimit) {
            UnitManager.I.CmdCreateUnit("Inf",  GameLogic.I.GetLocalPlayer().side);
            GameLogic.I.GetLocalPlayer().curUnitsSpawned++;
        }
    }

    void OnDestroy()
    {
        if (statemachine != null)
            statemachine.Abort();
    }
}
