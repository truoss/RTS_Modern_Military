﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

namespace PlayerStates
{
    public class PlayGameState : IGameState {
        Player player;
        RaycastHit hit;

        Unit unit;
        Field field;

        float LastTick;

        public PlayGameState (Player player) {
            this.player = player;
        }

        public void Finished () {

        }

        public void Init () {

        }

        public IGameState Update () {
            unit = null;
            field = null;

            //raycast on mouse click
            if (Input.GetMouseButtonDown(0)) {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Camera.main.farClipPlane)) {
                    if (hit.transform.GetComponent<Unit>()) {
                        // select unit deselect field
                        unit = hit.transform.GetComponent<Unit>();
                        GameLogic.I.SelectUnit(unit);
                        GameLogic.I.SelectField(null);
                    } else if (hit.transform.GetComponent<Field>() && !GameLogic.I.SelectedUnit) {
                        //select field if no unit was selected before
                        field = hit.transform.GetComponent<Field>();
                        GameLogic.I.SelectField(field);
                    } else if (hit.transform.GetComponent<Field>() && GameLogic.I.SelectedUnit) {
                        //Unit same side?
                        if (GameLogic.I.SelectedUnit.Side == player.side) {
                            //set move target if unit was selected
                            field = hit.transform.GetComponent<Field>();

                            //TODO: Network cmd
                            var playerID = player.GetComponent<NetworkIdentity>().netId;
                            //Debug.LogWarning("Player " + playerID);
                            //Debug.LogWarning("Unit " + GameLogic.I.SelectedUnit.UnitID);
                            //Debug.LogWarning("Field " + field.FieldID);
                            UnitManager.I.CmdMoveUnit(playerID, GameLogic.I.SelectedUnit.UnitID, field.FieldID);
                            //GameLogic.I.SelectedUnit.TargetField = field.gameObject;

                            GameLogic.I.SelectUnit(null);
                            //return new MoveUnitState();
                        } else if (hit.transform.GetComponent<Field>()) {
                            //select field if no unit was selected before
                            field = hit.transform.GetComponent<Field>();
                            GameLogic.I.SelectField(field);
                            GameLogic.I.SelectUnit(null);
                        }
                    }
                }
            }
            return this;
        }
    }
}