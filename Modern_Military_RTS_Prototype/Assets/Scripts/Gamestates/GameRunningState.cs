using PlayerStates;
using UnityEngine;

namespace GameStates {
    public class GameRunningState : IGameState {
        RaycastHit hit;

        Unit unit;
        Field field;
        Player localPlayer;

        float LastTick;

        public void Finished () {
            //GameLogic.I.SelectField(null);
            GameLogic.I.SelectUnit(null);
        }

        public void Init () {
            //reset all selections
            LastTick = Time.time;

            GameLogic.I.GetLocalPlayer().statemachine = new StateMachine();
            GameLogic.I.GetLocalPlayer().statemachine.SetState(new PlayGameState(GameLogic.I.GetLocalPlayer()));
        }

        public IGameState Update ()
        {
            //Update units movement
            //Debug.Log(Time.time);
            if (LastTick + GameLogic.I.TickTimeMovement < Time.time)
            {
                GameLogic.I.TickUpdateMovement();
                LastTick = Time.time;
            }

            //BattleTickRate (Kampfauswertung)

            //Check wincondition
            // true -> Change player states

            //Check for async

            return this;
        }
    }
}
