using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public enum Item : int {House, Tree, Remove};

    public Item [] selectedItem = new Item[2];

    public Toggle redHouse, redTree, blueHouse, blueTree;
    public Text upText, redText, blueText,logText;

    public Text testMoneyText;

	public GameObject winPanel;

	public Animator redEndPanel, blueEndPanel;

    public GameManager gameManager;
	public MoneyPanelManager mpManager;

	public Color [] winPanelColor;

    private string[] logs = {"","",""};

    public void IsToggleClicked(bool state)
    {
		//selectedItem[0] = redHouse.isOn ? Item.House : Item.Tree;
		//selectedItem[1] = blueHouse.isOn ? Item.House : Item.Tree;
        //Debug.Log(selectedItem[0]+":"+selectedItem[1]);
		if (redHouse.isOn)
			selectedItem [0] = Item.House;
		else if (redTree.isOn)
			selectedItem [0] = Item.Tree;
		else
			selectedItem [0] = Item.Remove;

		if (blueHouse.isOn)
			selectedItem [1] = Item.House;
		else if (blueTree.isOn)
			selectedItem [1] = Item.Tree;
		else
			selectedItem [1] = Item.Remove;

		//Debug.Log (selectedItem[0]+":"+selectedItem[1]);

    }

    public void UIUpdate()
    {
        UpTextUpdate();
        RBtextUpdate(0);
        RBtextUpdate(1);
    }

    public void AddLog(string log)
    {
        //Debug.Log("ADDLOG");
        bool flg = true;
        for(int i = 0; i < logs.Length; i++)
        {
            if(logs[i] == "")
            {
                logs[i] = log;
                flg = false;
                break;
            }
        }

        if (flg)
        {
            for(int i = 0; i < logs.Length - 1; i++)
            {
                logs[i] = logs[i + 1];
            }

            logs[logs.Length - 1] = log;
        }

        LogUpdate();
    }

    public void ClearLog()
    {
		for (int i = 0; i < logs.Length; i++) {
			logs [i] = "";
		}
        logText.text = "";
    }

    private void UpTextUpdate()
    {
		upText.text = "家の建設費:$" + gameManager.houseBuiltCost + ",家の維持費:$" + gameManager.houseMaintenanceCost
		              + "木の設置費:$" + gameManager.treeBuiltCost + ",木の収入:$" + gameManager.treeIncome;

    }

    private void LogUpdate()
    {
        logText.text = "";
        for (int i = 0; i < logs.Length && logs[i] != ""; i++)
        {
            logText.text += logs[i] + "\n";
        }
    }

    private void RBtextUpdate(int index)
    {
        Player player = gameManager.players[index];

        Text text = index == 0 ? redText : blueText;

        text.text = "所持金 =＄" + player.money + "\n家の数 = " + player.houseNum + "\n木の数 = " + player.treeNum
            + "\n総維持費 =＄" + player.houseNum * gameManager.houseMaintenanceCost + "\n総収入 =＄" + player.treeNum * gameManager.treeIncome;
    }

	/*public int GetInitialMoney()
    {
        int value;
        if (int.TryParse(testMoneyText.text, out value))
        {
            return value;
        }
        else
        {
            return -1;
        }
    }*/
    
	public void EnablePanel(Animator panel){
		panel.SetTrigger ("Big");
	}

	public void DisablePanel(Animator panel){
		panel.SetTrigger ("Small");
	}
    
	public void BackToMenu(){
		SceneManager.LoadSceneAsync ("Menu");
	}

	public void WinEnd(int [] panelNum){
		Image winImage = winPanel.GetComponent<Image> ();
		Text winText = winPanel.transform.Find ("text").GetComponent<Text> ();
		winText.text = "赤:" + panelNum [0] + ",青:" + panelNum [1] + "\n";
		if (panelNum [0] == panelNum [1]) {
			winImage.color = winPanelColor [2];
			winText.text += "引き分け!";
		} else {
			string playerName;
			if (panelNum [0] > panelNum [1]) {
				playerName = "赤";
				winImage.color = winPanelColor [0];
			} else {
				playerName = "青";
				winImage.color = winPanelColor [1];
			}
			winText.text += playerName + "の勝ち!!";
		}

		EnablePanel (winPanel.GetComponent<Animator> ());
	}

	public void LoseEnd(int whichPlayerLose){
		Image winImage = winPanel.GetComponent<Image> ();
		Text winText = winPanel.transform.Find ("text").GetComponent<Text> ();
		string winName, loseName;

		if (whichPlayerLose == 1) {
			winName = "赤";
			loseName = "青";
			winImage.color = winPanelColor [0];
		} else {
			winName = "青";
			loseName = "赤";
			winImage.color = winPanelColor [1];
		}

		winText.text = loseName + "はお金も木もなくなりました\n" + winName + "の勝ち!!";
		EnablePanel (winPanel.GetComponent<Animator> ());
	}

	public void EndPanelEnable(int whichPlayer){
		if (whichPlayer == 0) {
			EnablePanel (redEndPanel);
		} else {
			EnablePanel (blueEndPanel);
		}
	}

	public void EndPanelDisenable(){
		DisablePanel (redEndPanel);
		DisablePanel (blueEndPanel);
	}
}
