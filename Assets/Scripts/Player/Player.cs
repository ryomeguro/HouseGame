using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    PlayerData playerData;

    public int money;
    public int houseNum = 0;
    public int treeNum = 0;

    public void Reset()
    {
        money = playerData.money;
        houseNum = 0;
        treeNum = 0;
    }

    private void Awake()
    {
        money = playerData.money;
    }

}
