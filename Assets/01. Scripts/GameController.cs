using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image panel = null;
    public TextMeshProUGUI tmp = null;
    public Button button = null;
}

[System.Serializable]
public class PlayerColor
{
    public Color panelColor = new Color();
    public Color textColor = new Color();
}

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI[] tmps;
    public GameObject gameOverPanel = null;
    public TextMeshProUGUI gameOverTMP = null;
    private string playerSide = "";

    private int moveCount = 0;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayercolor;
    public PlayerColor inactivePlayerColor;
    public TextMeshProUGUI textInfo;

    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
        playerSide = "X";

        gameOverPanel.SetActive(false);

        moveCount = 0;
    }

    private void SetGameControllerReferenceOnButtons()
    {
        for(int i = 0; i < tmps.Length; i++)
        {
            tmps[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    private void StartGame()
    {
        textInfo.gameObject.SetActive(false);
        moveCount = 0;
        SetBoardInteractable(false);
        SetPlayerButtons(false);
    }

    public void SetStartingSide(string startingSide)
    {
        playerSide = startingSide;
        if(playerSide == "X")
            SetPlayerColors(playerX, playerO);
        else
            SetPlayerColors(playerO, playerX);
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void ChangeSides()
    {
        playerSide = playerSide == "X" ? "O" : "X";

        if(playerSide == "X")
            SetPlayerColors(playerX, playerO);
        else
            SetPlayerColors(playerO, playerX);
    }

    private void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer.panel.color = activePlayercolor.panelColor;
        newPlayer.tmp.color = activePlayercolor.textColor;

        oldPlayer.panel.color = inactivePlayerColor.panelColor;
        oldPlayer.tmp.color = inactivePlayerColor.textColor;
    }

    public void EndTurn()
    {
        moveCount++;

        if(CheckMatch())
        {
            GameOver();
            Client.Instance.blockPanel.SetActive(false);
        }

        if(moveCount >= 9)
        {
            gameOverPanel.SetActive(true);
            gameOverTMP.text = "DRAW!!";
            Client.Instance.blockPanel.SetActive(false);
        }

        ChangeSides();
    }

    private bool CheckMatch()
    {
        return CheckWin(0, 1, 2) || CheckWin(3, 4, 5) || CheckWin(6, 7, 8) 
            || CheckWin(0, 3, 6) || CheckWin(1, 4, 7) || CheckWin(2, 5, 8) 
            || CheckWin(0, 4, 8) || CheckWin(2, 4, 6);
    }

    private bool CheckWin(int _i, int _j, int _k)
    {
        bool matched = tmps[_i].text == playerSide && tmps[_j].text == playerSide && tmps[_k].text == playerSide;
        return matched;
    }

    private void GameOver()
    {
        SetBoardInteractable(false);

        gameOverPanel.SetActive(true);
        gameOverTMP.text = playerSide + "  WINS!";
    }

    public void RestartGame()
    {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
        
        SetPlayerButtons(false);
        SetBoardInteractable(false);


        SetPlayerColors(playerX, playerO);
        for(int i = 0; i < tmps.Length; i++)
            tmps[i].text = "";

        Client.Instance.Reset();
    }

    public void SetBoardInteractable(bool _toggle)
    {
        for(int i = 0; i < tmps.Length; i++)
            tmps[i].GetComponentInParent<Button>().interactable = _toggle;
    }

    public void SetPlayerButtons(bool _toggle)
    {
        playerX.button.interactable = _toggle;
        playerO.button.interactable = _toggle;
    }
}
