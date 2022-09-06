using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GridSpace : MonoBehaviour
{
    public Button button = null;
    public TextMeshProUGUI tmp = null;
    public string playerSide = null;
    private GameController gameController;

    public void SetGameControllerReference(GameController _gameController)
    {
        this.gameController = _gameController;
    }

    public void SetSpace(bool _isSend = true)
    {
        tmp.text = gameController.GetPlayerSide();
        button.interactable = false;
        gameController.EndTurn();
        Client.Instance.blockPanel.SetActive(true);

        if(_isSend)
        {
            Client.Instance.SendMessages("game", "setted", Array.IndexOf(gameController.tmps, tmp).ToString());
        }
    }
}
