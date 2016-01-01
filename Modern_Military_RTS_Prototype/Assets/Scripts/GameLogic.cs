using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{
    public static GameLogic I;
    void Awake()
    {
        I = this;
    }

    public FieldManager FieldManager;

    public Field SelectedField;
    public Field LastSelectedField;
    public void SelectField(Field field)
    {
        LastSelectedField = SelectedField;
        SelectedField = field;
    }

    public BaseUnit SelectedUnit;
    public BaseUnit LastSelectedUnit;
    public void SelectUnit(BaseUnit unit)
    {
        LastSelectedUnit = SelectedUnit;
        SelectedUnit = unit;
    }

    public List<BaseUnit> Units = new List<BaseUnit>();

    void StartGame()
    {
        //Create Map here;
        //place units here;
    }

    void FixedUpdate()
    {
        //Update Units Movement
    }

    void Update()
    {
        //Update Units
    }
}
