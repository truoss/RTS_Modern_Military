using PlayerStates;
using UnityEngine;

namespace GameStates {
    public class GameRunningState : IGameState {
        RaycastHit hit;

        Unit unit;
        Field field;
        BattleSystem BattleSystem;

        float LastMovementTick;
        float LastBattleTick;

        public void Finished () {
            //GameLogic.I.SelectField(null);
            //GameLogic.I.SelectUnit(null);
        }

        public void Init () {
            //reset all selections
            LastMovementTick = Time.time;

            GameLogic.I.GetLocalPlayer().statemachine = new StateMachine();
            GameLogic.I.GetLocalPlayer().statemachine.SetState(new PlayGameState(GameLogic.I.GetLocalPlayer()));

            
            //BattleSystem
            LastBattleTick = Time.time;
            if (BattleSystem == null && GameLogic.I.GetLocalPlayer().GetComponent<UnityEngine.Networking.NetworkIdentity>().isServer) {
                BattleSystem = new BattleSystem();
            }
        }

        public IGameState Update ()
        {
            //Update units movement
            //Debug.Log(Time.time);
            if ((LastMovementTick + GameLogic.I.TickTimeMovement) < Time.time)
            {
                GameLogic.I.TickUpdateMovement();
                LastMovementTick = Time.time;
            }

            //BattleTickRate (Kampfauswertung)
            if (BattleSystem != null && (LastBattleTick + GameLogic.I.TickTimeBattle) < Time.time) {
                BattleSystem.BattleTick(GameLogic.I.Units.ToArray());
                LastBattleTick = Time.time;
            }
            //Check wincondition
            // true -> Change player states

            //Check for async

            return this;
        }
    }
}
