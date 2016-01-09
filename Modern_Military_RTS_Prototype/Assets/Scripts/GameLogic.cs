using UnityEngine;
using GameStates;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {
    public static GameLogic I;
    public static StateMachine StateMachine;

    Player localPlayer;
    public List<Player> player = new List<Player>();

    public float TickTimeMovement = 2;
    public float TickTimeCombat = 5;

    public FieldManager FieldManager;
    public TestMap TestMap;

    public Field SelectedField;
    public Field LastSelectedField;
    public void SelectField(Field field)
    {
        LastSelectedField = SelectedField;
        SelectedField = field;
    }

    public Unit SelectedUnit;
    public Unit LastSelectedUnit;
    public void SelectUnit(Unit unit)
    {
        LastSelectedUnit = SelectedUnit;
        SelectedUnit = unit;
    }

    public List<Unit> Units = new List<Unit>();


    void Awake () {
        I = this;
    }

    void Start () {
        StateMachine = new StateMachine();
        StateMachine.SetState(new StartGameState(FieldManager));
    }
        

    public void TickUpdateMovement () {
        if (Units.Count == 0)
            return;

        //Update Units Movement
        for (int i = 0; i < Units.Count; i++) {
            Units[i].DoUpdateMovement();
        }
        //Debug.LogWarning("Tick", this);
    }

    void Update ()
    {
        StateMachine.Update();
    }

    void OnDestroy () {
        StateMachine.Abort();
    }

    public Player GetLocalPlayer()
    {
        if (player.Count > 0 && localPlayer == null)
        {
            for (int i = 0; i < player.Count; i++)
            {
                if (player[i].isLocal)
                {
                    localPlayer = player[i];
                }
            }
        }

        return localPlayer;
    }
}
