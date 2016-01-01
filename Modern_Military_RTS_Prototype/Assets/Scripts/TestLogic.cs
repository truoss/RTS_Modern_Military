using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLogic : MonoBehaviour
{
    public static TestLogic I;
    void Awake()
    {
        I = this;
    }

    public TestMap TestMap;
    public List<BaseUnit> Units = new List<BaseUnit>();

    // Use this for initialization
    void Start () {
        TestMap.DoStart();
	}

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
}
