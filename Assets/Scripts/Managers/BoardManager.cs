using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	enum States : int{none, red, blue, outer};
    public enum Objects : int{none, redHouse, blueHouse, redTree, blueTree, redBase, blueBase};

    public ObjectManager objectManager;

    GameObject[,,] quads;
    GameObject[,,] displayQuads;

    States [,] boardStates;
    Objects [,] objectStates;

    bool[,] canPutField;//置ける場所
    bool[,] searchField;//サーチに使う

    Player[] players;

    int num;

    int baseLandNum = 4;

	public void BoardInit(int num, float maxVec, Player[] players)
    {
        this.num = num;
        this.players = players;

        objectManager.Init(num,maxVec);

        quads = new GameObject[2,num,num];
        displayQuads = new GameObject[2, num, num];

        boardStates = new States[num, num];
        objectStates = new Objects[num, num];

        canPutField = new bool[num, num];
        searchField = new bool[num, num];

        Transform quadt = GameObject.Find("Quads").transform;

        for (int i = 0; i < num; i++) {
            for(int j = 0; j < num; j++)
            {
                quads[0, i, j] = quadt.Find("RedQuad(" + i + "," + j + ")").gameObject;
                quads[1, i, j] = quadt.Find("BlueQuad(" + i + "," + j + ")").gameObject;
                displayQuads[0, i, j] = quadt.Find("RedDisplayQuad(" + i + "," + j + ")").gameObject;
                displayQuads[1, i, j] = quadt.Find("BlueDisplayQuad(" + i + "," + j + ")").gameObject;
                //Debug.Log(quads[0, i, j] +":"+ quads[1, i, j]);
            }
        }

        BoardReset();
    }

    public void BoardReset()
    {
        //BoardStatesの初期化
        for(int i = 0; i < num; i++)
        {
            for(int j = 0; j < num; j++)
            {
                if (i >= num - baseLandNum && j >= num - baseLandNum)
                {
                    ChangeStatus(i, j, States.blue);
                }
                else if (i < baseLandNum && j < baseLandNum)
                {
                    ChangeStatus(i, j, States.red);
                }
                else
                {
                    ChangeStatus(i, j, States.none);
                }

                objectStates[i, j] = Objects.none;
            }
        }

        //Objectの初期化
        objectManager.Reset();

        PutObject(num - 1, num - 1, Objects.blueBase);
        PutObject(0, 0, Objects.redBase);
		objectStates[num - 1, num - 1] = Objects.blueBase;
		objectStates[0, 0] = Objects.redBase;
    }

    void ChangeStatus(int i, int j, States state)
    {
        //Debug.Log("ChangeStatus"+i+":"+j+":"+state);
        boardStates[i, j] = state;

        switch (state)
        {
            case States.red:
                quads[0, i, j].SetActive(true);
                quads[1, i, j].SetActive(false);
                break;
            case States.blue:
                quads[0, i, j].SetActive(false);
                quads[1, i, j].SetActive(true);
                break;
            case States.none:
                quads[0, i, j].SetActive(false);
                quads[1, i, j].SetActive(false);
                break;
        }
    }

    public bool PutObject(int i, int j, Objects obj)
    {
        if(CheckObjects(i,j) != Objects.none)
        {
            return false;
        }

        objectStates[i, j] = obj;

        switch (obj)
        {
            case Objects.redHouse:
                if (PutHouseCheck(i, j, 0))
                {
                    objectManager.PutObject(i, j, obj);
                    return true;
                }
                return false;
            case Objects.blueHouse:
                if (PutHouseCheck(i, j, 1))
                {
                    objectManager.PutObject(i, j, obj);
                    return true;
                }
                return false;
            case Objects.redTree:
                if (PutTreeCheck(i, j, 0))
                {
                    objectManager.PutObject(i, j, obj);
                    return true;
                }
                return false;
            case Objects.blueTree:
                if (PutTreeCheck(i, j, 1))
                {
                    objectManager.PutObject(i, j, obj);
                    return true;
                }
                return false;
            case Objects.none:
                break;
            case Objects.redBase:
                objectManager.PutObject(i, j, obj);
                break;
            case Objects.blueBase:
                objectManager.PutObject(i, j, obj);
                break;
        }

        return false;

    }

	public bool RemoveObject(int whichPlayer, Vector2Int coodination){
		Objects myHouse, myTree;
		if (whichPlayer == 0) {
			myHouse = Objects.redHouse;
			myTree = Objects.redTree;
		} else {
			myHouse = Objects.blueHouse;
			myTree = Objects.blueTree;
		}

		Objects currentobj = CheckObjects (coodination.x, coodination.y);
		if (currentobj == myHouse) {
			DestroyHouse (coodination.x, coodination.y, whichPlayer);
			return true;
		} else if (currentobj == myTree) {
			DestroyTree (coodination.x, coodination.y, whichPlayer);
			return true;
		} else {
			return false;
		}
	}

	public void DestroyTree(int i, int j, int whichPlayer){
		objectManager.DestroyObject (i, j);
		players [whichPlayer].treeNum--;
		objectStates [i, j] = Objects.none;
	}

	public bool DestroyHouse(int i, int j, int whichPlayer)
    {
		Objects obj = whichPlayer == 0 ? Objects.redHouse : Objects.blueHouse;
		if(objectStates[i,j] != obj)
        {
            return false;
        }

		objectManager.DestroyObject(i, j);
        objectStates[i, j] = Objects.none;

		players [whichPlayer].houseNum--;

        States mystate = whichPlayer == 0 ? States.red : States.blue;
        for(int x = i-1; x <= i+1; x++)
        {
            for(int y = j-1; y <= j+1; y++)
            {
				if(CheckStatus(x,y) == mystate && !IsBaseField(x,y,whichPlayer) && CheckObjects(x,y) != obj)
                {
                    ChangeStatus(x, y, States.none);
                }
            }
        }

		//canputfieldの再計算
		CanPutFieldUpdate (whichPlayer);

        for(int x = i - 2; x <= i + 2; x++)
        {
            for(int y = j - 2; y <= j + 2; y++)
            {
                //Debug.Log(x + ":" + y + ":" + objectStates[x, y]);
                if (CheckObjects(x, y) == obj)
                {
                    //Debug.Log(x + ":" + y);
                    CheckAndChangeHouse(x, y, mystate);
                }
            }
        }

		return true;
    }

    private void CheckAndChangeHouse(int x, int y, States mystate)
    {
        for (int nx = x - 1; nx <= x + 1; nx++)
        {
            for (int ny = y - 1; ny <= y + 1; ny++)
            {
				if (CheckStatus(nx, ny) == States.none && canPutField[nx,ny])
                {
                    ChangeStatus(nx, ny, mystate);
                }
            }
        }
    }

    private bool IsBaseField(int i, int j, int whichPlayer)
    {
        if(whichPlayer == 0)
        {
            if (i < baseLandNum && j < baseLandNum) return true;
            else return false;
        }
        else
        {
            if (i >= num - baseLandNum && j >= num - baseLandNum) return true;
            else return false;
        }
    }

    private bool PutHouseCheck(int i, int j, int whichPlayer)
    {
        States myState, oppositeState;
        Objects myTree, oppositeTree;

        if(whichPlayer == 0)
        {
            myState = States.red;
            oppositeState = States.blue;
            myTree = Objects.redTree;
            oppositeTree = Objects.blueTree;
        }
        else
        {
            myState = States.blue;
            oppositeState = States.red;
            myTree = Objects.blueTree;
            oppositeTree = Objects.redTree;
        }

        if (!canPutField[i, j])
        {
            return false;
        }

		/*if(CheckStatus(i,j) == oppositeState)
        {
            return false;
        }*/

        for(int x = i-1; x <= i+1; x++)
        {
            for(int y = j-1; y <= j+1; y++)
            {
				if(CheckObjects(x,y) == myTree && CheckStatus(x,y) == States.none)
                {
                    return false;
                }
            }
        }

        for (int x = i - 1; x <= i + 1; x++)
        {
            for (int y = j - 1; y <= j + 1; y++)
            {
                if (CheckStatus(x, y) == States.none && canPutField[x,y])
                {
                    ChangeStatus(x, y, myState);
                }
				if(CheckObjects(x,y) == oppositeTree && CheckStatus(x,y) == myState)
                {
					ChangeTree (x, y, whichPlayer, myTree);
                }
            }
        }


        return true;
    }

	private void ChangeTree(int i, int j, int whichPlayer, Objects tree){
		players [whichPlayer].treeNum++;
		players[(whichPlayer + 1) % 2].treeNum--;
		objectManager.ChangeTree (i, j, tree);
		objectStates [i, j] = tree;
	}

    private bool PutTreeCheck(int i, int j, int whichPlayer)
    {
        if(boardStates[i,j] == States.none && canPutField[i, j])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private States CheckStatus(int i, int j)
    {
        if(i >= num || j >= num || i < 0 || j < 0)
        {
            return States.outer;
        }
        else
        {
            //Debug.Log("CheckStatus:i=" + i + ":j=" + j);
            return boardStates[i, j];
        }
    }

    private Objects CheckObjects(int i, int j)
    {
        if (i >= num || j >= num || i < 0 || j < 0)
        {
            return Objects.none;
        }
        else
        {
            return objectStates[i, j];
        }
    }

    States tmpMyState;
    int tmpwhichPlayer;
    public void DisplayCanPutField(int whichPlayer)
    {
        //Debug.Log("Display:" + whichPlayer);
        for(int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                displayQuads[0, i, j].SetActive(false);
                displayQuads[1, i, j].SetActive(false);
                canPutField[i, j] = false;
                searchField[i, j] = false;
            }
        }


        tmpMyState = whichPlayer == 0 ? States.red : States.blue;
        tmpwhichPlayer = whichPlayer;
        for(int i = 0; i < num; i++)
        {
            for(int j = 0; j < num; j++)
            {
                if(CheckStatus(i,j) == tmpMyState && !searchField[i, j])
                {
                    Search(i, j);
                }
            }
        }

		for (int i = 0; i < num; i++) {
			for (int j = 0; j < num; j++) {
				if (canPutField [i, j]) {
					Display (i, j);
				}
			}
		}
    }

    public void ClearDisplay()
    {
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                displayQuads[0, i, j].SetActive(false);
                displayQuads[1, i, j].SetActive(false);
            }
        }
    }

	private void CanPutFieldUpdate(int whichPlayer){
		for(int i = 0; i < num; i++)
		{
			for (int j = 0; j < num; j++)
			{
				canPutField[i, j] = false;
				searchField[i, j] = false;
			}
		}


		tmpMyState = whichPlayer == 0 ? States.red : States.blue;
		tmpwhichPlayer = whichPlayer;
		for(int i = 0; i < num; i++)
		{
			for(int j = 0; j < num; j++)
			{
				if(CheckStatus(i,j) == tmpMyState && !searchField[i, j])
				{
					Search(i, j);
				}
			}
		}
	}


    private void Search(int i, int j)
    {
        searchField[i, j] = true;

        States currentState = CheckStatus(i, j);
        if(currentState == States.none)
        {
			//Display(i, j);
            canPutField[i, j] = true;
        }else if(currentState == tmpMyState)
        {
            canPutField[i, j] = true;
        }
        else
        {
            return;
        }

        if (!CheckSearchField(i + 1, j))
        {
            Search(i + 1, j);
        }
        if (!CheckSearchField(i - 1, j))
        {
            Search(i - 1, j);
        }
        if (!CheckSearchField(i, j + 1))
        {
            Search(i, j + 1);
        }
        if (!CheckSearchField(i, j - 1))
        {
            Search(i, j - 1);
        }
    }

    private void Display(int i, int j)
    {
        displayQuads[tmpwhichPlayer, i, j].SetActive(true);
    }

    private bool CheckSearchField(int i, int j)
    {
        if(i < 0 || j < 0 || i >= num || j >= num)
        {
            return true;
        }
        //Debug.Log(i + ":" + j);
        return searchField[i, j];
    }

    public int [] CountField()
    {
        int[] count = {0, 0};

        for(int i = 0; i < num; i++)
        {
            for(int j = 0; j < num; j++)
            {
                switch (boardStates[i, j])
                {
                    case States.red:
                        count[0]++;
                        break;
                    case States.blue:
                        count[1]++;
                        break;
                }
            }
        }

        return count;
    }


}
