using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

    GameObject[,] objects;
    int num;
    float maxVec;

    float unit;
    float offset;

    public GameObject redHouse, blueHouse, redTree, blueTree, redBase, blueBase;

    public void Init(int num,float maxVec)
    {
        this.num = num;
        this.maxVec = maxVec;

        objects = new GameObject[num, num];

        unit = maxVec * 2 / num;
        offset = unit / 2 - maxVec;
    }

    public void PutObject(int i,int j,BoardManager.Objects obj)
    {
        GameObject g = null;
        switch (obj)
        {
            case BoardManager.Objects.redHouse:
                g = redHouse;
                break;
            case BoardManager.Objects.blueHouse:
                g = blueHouse;
                break;
            case BoardManager.Objects.redTree:
                g = redTree;
                break;
            case BoardManager.Objects.blueTree:
                g = blueTree;
                break;
            case BoardManager.Objects.redBase:
                g = redBase;
                break;
            case BoardManager.Objects.blueBase:
                g = blueBase;
                break;
        }
        objects[i, j] = Instantiate(g, new Vector3(offset + unit * i, 0f, offset + unit * j), Quaternion.identity);
    }

    public void DestroyHouse(int i, int j)
    {
        Destroy(objects[i, j]);
        objects[i, j] = null;
    }

    public void ChangeTree(int i, int j, BoardManager.Objects obj)
    {
        Destroy(objects[i, j]);
        GameObject tree;
        if(obj == BoardManager.Objects.redTree)
        {
            tree = redTree;
        }
        else
        {
            tree = blueTree;
        }
        Instantiate(tree, new Vector3(offset + unit * i, 0f, offset + unit * j), Quaternion.identity);
    }

    public void Reset()
    {
        for(int i = 0; i < num; i++)
        {
            for(int j = 0; j < num; j++)
            {
                if (objects[i, j] != null)
                {
                    Destroy(objects[i, j]);
                    objects[i, j] = null;
                }
            }
        }
    }

}
