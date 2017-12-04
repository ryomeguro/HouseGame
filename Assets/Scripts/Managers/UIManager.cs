using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public enum Item : int {House, Tree, RemoveHouse};

    public Item [] selectedItem = new Item[2];

    public Toggle redHouse, redTree, blueHouse, blueTree;
    public Text upText, redText, blueText,logText;

    public Text testMoneyText;

    public GameManager gameManager;

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
			selectedItem [0] = Item.RemoveHouse;

		if (blueHouse.isOn)
			selectedItem [1] = Item.House;
		else if (blueTree.isOn)
			selectedItem [1] = Item.Tree;
		else
			selectedItem [1] = Item.RemoveHouse;

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
        logText.text = "";
    }

    private void UpTextUpdate()
    {
        upText.text = "家建設費=￥" + gameManager.houseBuiltCost + ",家維持費=￥" + gameManager.houseMaintenanceCost
            + "木設置費=￥" + gameManager.treeBuiltCost + ",木収入=￥" + gameManager.treeIncome;

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

        text.text = "所持金 =￥" + player.money + "\n家の数 = " + player.houseNum + "\n木の数 = " + player.treeNum
            + "\n総維持費 =￥" + player.houseNum * gameManager.houseMaintenanceCost + "\n総収入 =￥" + player.treeNum * gameManager.treeIncome;
    }

    public int GetInitialMoney()
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
    }
    
}
