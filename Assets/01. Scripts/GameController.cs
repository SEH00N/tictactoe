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
    private string computerSide;
    public bool isPlayerMove;

    private int moveCount = 0;
    public Player playerX;
    public Player playerO;
    public PlayerColor activePlayercolor;
    public PlayerColor inactivePlayerColor;
    public TextMeshProUGUI textInfo;
    private float delay;

    private void Awake()
    {
        SetGameControllerReferenceOnButtons();
        playerSide = "X";

        gameOverPanel.SetActive(false);

        moveCount = 0;
        
        #region 버릴 부분
        isPlayerMove = true;
        #endregion
    }

    #region 갔다 버릴 부분
        
    private void Update()
    {
        if(isPlayerMove) return;

        delay += delay * Time.deltaTime;

        if(delay >= 100)
        {
            int randIndex = Random.Range(0, 8);
            if(tmps[randIndex].GetComponent<Button>().interactable)
            {
                tmps[randIndex].text = GetComputerSide();
                tmps[randIndex].GetComponent<Button>().interactable = false;
                EndTurn();
            }
        }
    }
    #endregion

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
        {
            computerSide = "O";
            SetPlayerColors(playerX, playerO);
        }
        else
        {
            computerSide = "X";
            SetPlayerColors(playerO, playerX);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public string GetComputerSide()
    {
        return computerSide;
    }

    public void ChangeSides()
    {
        // playerSide = playerSide == "X" ? "O" : "X";
        isPlayerMove = isPlayerMove ? false : true;

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

        delay = 10f;

        if(CheckMatch(playerSide))
        {
            GameOver(playerSide);
        }
        if(CheckMatch(computerSide))
        {
            GameOver(computerSide);
        }

        // if(CheckMatch())
        // {
        //     GameOver();
        //     Client.Instance.blockPanel.SetActive(false);
        // }

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
        
    private bool CheckMatch(string _turn)
    {
        return CheckWin(0, 1, 2, _turn) || CheckWin(3, 4, 5, _turn) || CheckWin(6, 7, 8, _turn) 
            || CheckWin(0, 3, 6, _turn) || CheckWin(1, 4, 7, _turn) || CheckWin(2, 5, 8, _turn) 
            || CheckWin(0, 4, 8, _turn) || CheckWin(2, 4, 6, _turn);
    }

    private bool CheckWin(int _i, int _j, int _k)
    {
        bool matched = tmps[_i].text == playerSide && tmps[_j].text == playerSide && tmps[_k].text == playerSide;
        return matched;
    }


    private bool CheckWin(int _i, int _j, int _k, string _turn)
    {
        bool matched = tmps[_i].text == _turn && tmps[_j].text == _turn && tmps[_k].text == _turn;
        return matched;
    }

    private void GameOver()
    {
        SetBoardInteractable(false);

        gameOverPanel.SetActive(true);
        gameOverTMP.text = playerSide + "  WINS!";
    }

    private void GameOver(string _turn)
    {
        SetBoardInteractable(false);

        gameOverPanel.SetActive(true);
        gameOverTMP.text = _turn + "  WINS!";
    }

    public void RestartGame()
    {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);
        
        SetPlayerButtons(false);
        SetBoardInteractable(false);

        isPlayerMove = true;

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
