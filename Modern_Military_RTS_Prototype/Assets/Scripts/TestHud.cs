using UnityEngine;
using UnityEngine.UI;

public class TestHud : MonoBehaviour
{
    public static TestHud I;
    Player localPlayer;
    public Text playerside;
    public Text playerState;
    public Text centerInfo;
    public GameObject CreateInfantry;

    void Awake()
    {
        I = this;
    }

    public void SetCenterInfo(string text)
    {
        centerInfo.text = text;
    }

    public void ShowCenterInfo()
    {
        centerInfo.enabled = true;
    }

    public void HideCenterInfo()
    {
        centerInfo.enabled = false;
    } 

    public void HideCreateInfantry()
    {
        CreateInfantry.SetActive(false);
    }

    public void OnCreateInfantry()
    {
        GameLogic.I.GetLocalPlayer().GetComponent<UnitManager>().CreateInfantry();
    }

    void Update()
    {
        if (localPlayer == null)
        {
            localPlayer = GameLogic.I.GetLocalPlayer();
        }

        if (localPlayer)
        {
            playerside.text = localPlayer.side.ToString();
            if(localPlayer.statemachine != null)
                playerState.text = localPlayer.statemachine.CurrentState.ToString();
        }
    }
}
