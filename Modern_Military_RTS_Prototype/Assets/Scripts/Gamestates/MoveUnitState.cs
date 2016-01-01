using UnityEngine;
using System.Collections;
using System;

namespace GameStates
{
    public class MoveUnitState : IGameState
    {
        RaycastHit hit;
        
        BaseUnit unit;
        Field field;        

        public void Finished()
        {
            //GameLogic.I.SelectField(null);
            GameLogic.I.SelectUnit(null);
        }

        public void Init()
        {
            //reset all selections
            
        }

        public IGameState Update()
        {
            //State: 
            //Wait for player select unit
            //if unit selected wait until player select target field
            //after select field move selected unit

            unit = null;
            field = null;

            //raycast on mouse click
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Camera.main.farClipPlane))
                {
                    if (hit.transform.GetComponent<BaseUnit>())
                    {
                        // select unit deselect field
                        unit = hit.transform.GetComponent<BaseUnit>();
                        GameLogic.I.SelectUnit(unit);
                        GameLogic.I.SelectField(null);
                    }
                    else if (hit.transform.GetComponent<Field>() && !GameLogic.I.SelectedUnit)
                    {
                        //select field if no unit was selected before
                        field = hit.transform.GetComponent<Field>();
                        GameLogic.I.SelectField(field);
                    }
                    else if (hit.transform.GetComponent<Field>() && GameLogic.I.SelectedUnit)
                    {
                        //set move target if unit was selected
                        field = hit.transform.GetComponent<Field>();
                        GameLogic.I.SelectedUnit.TargetField = field.gameObject;

                        return new MoveUnitState();
                    }
                }                
            }

            return this;
        }
    }
}
