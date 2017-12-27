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
	public GameObject smokeAndStar,smoke;

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
		Quaternion q = Quaternion.identity;
		bool particleFlg = true;

		switch (obj) {
		case BoardManager.Objects.redHouse:
			g = redHouse;
			break;
		case BoardManager.Objects.blueHouse:
			g = blueHouse;
			break;
		case BoardManager.Objects.redTree:
			g = redTree;
			q = Quaternion.Euler (0, Random.Range (0, 360), 0);
			break;
		case BoardManager.Objects.blueTree:
			g = blueTree;
			q = Quaternion.Euler (0, Random.Range (0, 360), 0);
			break;
		case BoardManager.Objects.redBase:
			g = redBase;
			particleFlg = false;
			break;
		case BoardManager.Objects.blueBase:
			g = blueBase;
			particleFlg = false;
			break;
		}

		Vector3 newPos = new Vector3 (offset + unit * i, 0f, offset + unit * j);
		objects[i, j] = Instantiate(g, newPos, q);

		if (particleFlg) {
			smokeAndStar.SetActive (false);
			smokeAndStar.transform.position = newPos;
			smokeAndStar.SetActive (true);
		}
    }

	public void DestroyObject(int i, int j)
    {
        Destroy(objects[i, j]);
        objects[i, j] = null;

		smoke.SetActive (false);
		smoke.transform.position = new Vector3 (offset + unit * i, 0f, offset + unit * j);
		smoke.SetActive (true);
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
		objects [i, j] = Instantiate (tree, new Vector3 (offset + unit * i, 0f, offset + unit * j), Quaternion.identity);
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
