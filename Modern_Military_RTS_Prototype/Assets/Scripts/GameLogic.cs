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

    public BaseUnit SelectedUnit;
    public BaseUnit LastSelectedUnit;

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
