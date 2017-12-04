using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public const int num = 13;
    public const float maxVec = 5f;

    int whichPlayer = 0;
    bool canMaintain = true;
    bool wasPass = false;

    bool canClick = true;

    public BoardManager boardManager;
    public UIManager uiManager;

    public Player[] players = new Player[2];
    public int turn = 0;
    public int destroyNum = 0;

    public int houseBuiltCost;
    public int houseMaintenanceCost;
    public int treeBuiltCost;
    public int treeIncome;

	// Use this for initialization
	void Start () {
        whichPlayer = 0;
        boardManager.BoardInit(num, maxVec, players);
        PropertyDecision();
        boardManager.DisplayCanPutField(whichPlayer);

        uiManager.UIUpdate();
	}

    public void OnBoardClicked(BaseEventData e)
    {
        PointerEventData pData = e as PointerEventData;
        Vector3 clickedPosition = pData.pointerCurrentRaycast.worldPosition;
        Vector2Int coordination = CalcCoordination(clickedPosition);
        //Debug.Log(coordination);

        Turn(false, coordination);
    }

    public void OnPassClicked()
    {
        Turn(true, Vector2Int.zero);
    }

    public void Turn(bool isPass,Vector2Int coodination)
    {
        if (!canClick)
        {
            return;
        }

        if (canMaintain)
        {
            if (isPass)
            {
                if (wasPass)
                {
                    PassEndLog();
                    canClick = false;
                    boardManager.ClearDisplay();
                    return;
                }
                wasPass = true;
                PassLog();
            }
            else 
            {
                wasPass = false;

                int cost = uiManager.selectedItem[whichPlayer] == 0 ? houseBuiltCost : treeBuiltCost;
				if (players [whichPlayer].money < cost) {
					LessMoneyLog ();
					return;
				} 
				else if (uiManager.selectedItem [whichPlayer] != UIManager.Item.RemoveHouse) {
					if (Build (whichPlayer, coodination)) {
						players [whichPlayer].money -= cost;
						PlayerNumUpdate (1);
						BuildLog ();
					} else {
						CannotPutLog ();
						return;
					}
				} 
				else //RemoveHouse
				{
					bool wasDestroy = DestroyHouse (whichPlayer, coodination);
					MyDestroyLog (wasDestroy);
					return;
				}
            }
        }
        else
        {
            if (DestroyHouse(whichPlayer, coodination))
            {
                destroyNum--;
                PlayerNumUpdate(-1);
                DestroyLog();
            }
            if (destroyNum > 0)
            {
                uiManager.UIUpdate();
                return;
            }
            else
            {
                canMaintain = true;
                Maintenance(whichPlayer);
                uiManager.UIUpdate();
                boardManager.DisplayCanPutField(whichPlayer);
                return;
            }
        }

        whichPlayer = (whichPlayer + 1) % 2;

        TreeIncome(whichPlayer);
        IncomeLog();
        if (!Maintenance(whichPlayer))
        {
            canMaintain = false;

            int lessMoney = players[whichPlayer].houseNum * houseMaintenanceCost - players[whichPlayer].money;
            destroyNum = lessMoney / houseMaintenanceCost;
            if(lessMoney % houseMaintenanceCost != 0)
            {
                destroyNum++;
            }
            boardManager.ClearDisplay();
            NeedDestroyLog();
        }
        else
        {
            canMaintain = true;
            MaintainLog();
            boardManager.DisplayCanPutField(whichPlayer);
        }

        if (!CheckCanRestart(whichPlayer))
        {
            canClick = false;
            boardManager.ClearDisplay();
            NoMoneyEndLog();
        }


        uiManager.UIUpdate();
    }
	
    private Vector2Int CalcCoordination(Vector3 pos)
    {
        float unit = maxVec * 2 / num;
        int x = -1;
        int y = -1;

        for(int i = 0; i < num; i++)
        {
            if(-maxVec + unit * i <= pos.x && pos.x < -maxVec + unit * (i + 1))
            {
                x = i;
                break;
            }
        }

        for (int i = 0; i < num; i++)
        {
            if (-maxVec + unit * i <= pos.z && pos.z < -maxVec + unit * (i + 1))
            {
                y = i;
                break;
            }
        }

        if(x >= 0 && y >= 0)
        {
            return new Vector2Int(x, y);
        }
        else
        {
            return new Vector2Int(-1,-1);
        }
    }

    public void Reset()
    {
        whichPlayer = 0;
        PropertyDecision();
        boardManager.BoardReset();
        boardManager.DisplayCanPutField(whichPlayer);
        wasPass = false;
        canClick = true;

        for(int i = 0; i < 2; i++)
        {
            players[i].Reset();

            /*
             *  This is Test Code !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             */

            int m;
            if((m = uiManager.GetInitialMoney()) > 0)
            {
                players[i].money = m;
            }
        }

        uiManager.UIUpdate();
        uiManager.ClearLog();
    }

    private bool CheckCanRestart(int whichPlayer)
    {
        Player p = players[whichPlayer];

        if(p.treeNum == 0 && p.money < Mathf.Min(houseBuiltCost, treeBuiltCost))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void PropertyDecision()
    {
        houseBuiltCost = Random.Range(8, 13) * 10;
        houseMaintenanceCost = Random.Range(3, 7) * 10;
		treeBuiltCost = Random.Range(3, 7) * 10;
		treeIncome = Random.Range(4, 11) * 10;
    }

    private void TreeIncome(int playerIndex)
    {
        Player p = players[playerIndex];
        p.money += p.treeNum * treeIncome;
    }

    private bool Maintenance(int playerIndex)
    {
        Player p = players[playerIndex];
		if(p.money >= p.houseNum * houseMaintenanceCost)
        {
            p.money -= p.houseNum * houseMaintenanceCost;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Build(int playerIndex, Vector2Int coordination)
    {
        BoardManager.Objects obj;
        if(playerIndex == 0)
        {
            obj = uiManager.selectedItem[0] == UIManager.Item.House ? BoardManager.Objects.redHouse : BoardManager.Objects.redTree;
        }
        else
        {
            obj = uiManager.selectedItem[1] == UIManager.Item.House ? BoardManager.Objects.blueHouse : BoardManager.Objects.blueTree;
        }

        return boardManager.PutObject(coordination.x, coordination.y, obj);
    }

    private bool DestroyHouse(int playerIndex, Vector2Int coodination)
    {
        return boardManager.DestroyHouse(coodination.x, coodination.y, playerIndex);
    }

    private void PlayerNumUpdate(int plusminus)
    {
        if (plusminus > 0)
        {
            if (uiManager.selectedItem[whichPlayer] == UIManager.Item.House)
            {
                players[whichPlayer].houseNum++;
            }
            else
            {
                players[whichPlayer].treeNum++;
            }
        }
        else
        {
            players[whichPlayer].houseNum--;
        }

    }

    private void BuildLog()
    {
        string playerName = whichPlayer == 0 ? "赤" : "青";
        string objName = uiManager.selectedItem[whichPlayer] == UIManager.Item.House ? "家を建てました。" : "木を植えました。";

        uiManager.AddLog(playerName + "が" + objName);
    }

    private void IncomeLog()
    {
        string playerName = whichPlayer == 0 ? "赤" : "青";

        int income = treeIncome * players[whichPlayer].treeNum;

        if (income != 0)
        {
            uiManager.AddLog(playerName + "が金のなる木から￥" + income + "ゲット");
        }
    }

    private void MaintainLog()
    {
        string playerName = whichPlayer == 0 ? "赤" : "青";

        int maintenance = houseMaintenanceCost * players[whichPlayer].houseNum;

        if(maintenance != 0)
        {
            uiManager.AddLog(playerName + "は家の維持費￥" + maintenance + "を支払った");
        }
    }

    private void LessMoneyLog()
    {
        string playerName = whichPlayer == 0 ? "赤" : "青";
        string v = uiManager.selectedItem[whichPlayer] == UIManager.Item.House ? "家を建てる" : "木を植える";

        uiManager.AddLog(playerName + "は" + v + "のに必要なお金を持っていない");
    }

    private void CannotPutLog()
    {
        if(uiManager.selectedItem[whichPlayer] == UIManager.Item.House)
        {
            uiManager.AddLog("そこには家は置けません");
        }
        else
        {
            uiManager.AddLog("そこには木は植えられません");
        }
    }

    private void NeedDestroyLog()
    {
        string playerName = whichPlayer == 0 ? "赤" : "青";

        uiManager.AddLog(playerName + "は家の維持費が払えません。家を" + destroyNum + "件手放してください");
    }

    private void DestroyLog()
    {
        string playerName = whichPlayer == 0 ? "赤" : "青";
        string log = playerName + "家を手放しました。";
        if(destroyNum > 0)
        {
            log += "あと" + destroyNum + "件手放してください";
        }
        else
        {
            log += "完了です。" + playerName + "のターンを続けます";
        }

        uiManager.AddLog(log);
    }

    private void PassLog()
    {
        string playerName = whichPlayer == 0 ? "赤" : "青";

        uiManager.AddLog(playerName + "はパスした");
    }

    private void PassEndLog()
    {
        int[] count = boardManager.CountField();

        string log = "赤=" + count[0] + ":青=" + count[1] + "。よって";
        if(count[0] == count[1])
        {
            log += "引き分けです";
        }
        else
        {
            string winnerName = count[0] > count[1] ? "赤" : "青";
            log += winnerName + "の勝ちです!!";
        }

        uiManager.AddLog(log);
    }

    private void NoMoneyEndLog()
    {
        string playerName = whichPlayer == 0 ? "赤" : "青";

        uiManager.AddLog(playerName + "が再起不能になったため" + playerName + "の負けです");
    }

	private void MyDestroyLog(bool wasDestroy){
		string playerName = whichPlayer == 0 ? "赤" : "青";

		if (wasDestroy) {
			uiManager.AddLog (playerName + "が自分の家を手放しました");
		} else {
			uiManager.AddLog (playerName + "の家しか手放せません");
		}
	}
}
