using PlayerStates;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerScript : NetworkBehaviour
{
    public override void OnStartLocalPlayer()
    {
        var player = GetComponent<Player>();
        player.isLocal = true;
        //player.statemachine = new StateMachine();
        gameObject.name = "LOCAL Player";

        if (!GameLogic.I.player.Contains(player))
        {
            GameLogic.I.player.Add(player);
        }

        Debug.Log("OnStartLocalPlayer");
        base.OnStartLocalPlayer();
    }

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");
        var player = GetComponent<Player>();
        if (!GameLogic.I.player.Contains(player))
        {
            GameLogic.I.player.Add(player);
        }
        base.OnStartClient();
    }

    void OnDestroy () {
        //NetworkManager.Shutdown();
    }


}
