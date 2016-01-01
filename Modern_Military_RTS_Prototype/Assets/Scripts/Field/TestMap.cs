using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestMap : MonoBehaviour
{
    public List<Field> fields;
    
	public void DoStart ()
    {
        fields = new List<Field>(GetComponentsInChildren<Field>());
	}	
}
