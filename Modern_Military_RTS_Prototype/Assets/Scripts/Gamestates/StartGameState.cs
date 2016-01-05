using UnityEngine;

namespace GameStates {
    public class StartGameState : IGameState {

        private FieldManager FieldManager;
        private UnitManager UnitManager;

        public StartGameState (FieldManager FieldManager, UnitManager UnitManager) {
            this.FieldManager = FieldManager;
            this.UnitManager = UnitManager;
        }

        public void Finished () {
        }

        public void Init () {
            FieldManager.Init();
            UnitManager.Init(FieldManager);
        }

        IGameState IGameState.Update () {
            return new MoveUnitState();
        }
    }
}