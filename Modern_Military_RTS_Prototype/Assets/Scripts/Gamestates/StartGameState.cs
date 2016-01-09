using UnityEngine;

namespace GameStates
{
    public class StartGameState : IGameState
    {

        private FieldManager FieldManager;

        float waitTime = 30;
        float startTime;

        public StartGameState(FieldManager FieldManager)
        {
            this.FieldManager = FieldManager;
        }

        public void Finished()
        {
        }

        public void Init()
        {
            FieldManager.Init();

            startTime = Time.time;
        }

        IGameState IGameState.Update()
        {
            //wait 30sec before start
            if (startTime + waitTime > Time.time)
            {
                TestHud.I.ShowCenterInfo();
                TestHud.I.SetCenterInfo(Mathf.RoundToInt(((startTime + waitTime) - Time.time)).ToString());

                for (int i = 0; i < GameLogic.I.player.Count; i++)
                {
                    if (IsOdd(i))
                        GameLogic.I.player[i].side = Player.Side.Red;
                    else
                        GameLogic.I.player[i].side = Player.Side.Blue;
                }

                return this;
            }
            else
            {
                TestHud.I.HideCenterInfo();
                TestHud.I.HideCreateInfantry();
                return new GameRunningState();
            }
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }
    }
}