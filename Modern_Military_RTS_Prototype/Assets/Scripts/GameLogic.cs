using UnityEngine;
using GameStates;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    public static GameLogic I;
    public static StateMachine StateMachine;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        if(TestMap)
            TestMap.DoStart();

        StateMachine = new StateMachine();
        StateMachine.SetState(new MoveUnitState());
    }

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
    

    void FixedUpdate()
    {
        if (Units.Count == 0)
            return;

        //Update Units Movement
        for (int i = 0; i < Units.Count; i++)
        {
            Units[i].DoFixedUpdate();
        }
    }

    void Update()
    {
        StateMachine.Update();
    }

    void OnDestroy()
    {
        StateMachine.Abort();
    }
}
