using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestBoardSet : MonoBehaviour
{

    static string s;

    static float y = 0.01f;

    static Transform[] childlenlines = new Transform[1000];

    [MenuItem("Test/BoardSet")]
    public static void BoardSet()
    {
        int num = GameManager.num;
        float maxVec = GameManager.maxVec;

        GameObject line = Resources.Load("Line") as GameObject;
        GameObject lines = GameObject.Find("Lines");

        GameObject redQuad = Resources.Load("RedQuad") as GameObject;
        GameObject blueQuad = Resources.Load("BlueQuad") as GameObject;
        GameObject redDisplayQuad = Resources.Load("RedDisplayQuad") as GameObject;
        GameObject blueDisplayQuad = Resources.Load("BlueDisplayQuad") as GameObject;
        GameObject quads = GameObject.Find("Quads");

        if (!line || !redQuad || !blueQuad)
        {
            Debug.LogError("BoardSet:Object Not Found");
            return;
        }

        //もともとあったlineを削除
        Transform linet = lines.transform;
        int count = 0;
        foreach (Transform child in linet)
        {
            //Debug.Log(child.name);
            childlenlines[count++] = child;
        }

        for (int i = 0; i < childlenlines.Length; i++)
        {
            if (childlenlines[i] == null)
            {
                break;
            }
            DestroyImmediate(childlenlines[i].gameObject);
        }

        //もともとあったquadを削除
        Transform quadt = quads.transform;
        count = 0;
        foreach (Transform child in quadt)
        {
            Debug.Log(count);
            childlenlines[count++] = child;
        }

        for (int i = 0; i < childlenlines.Length; i++)
        {
            if (childlenlines[i] == null)
            {
                break;
            }
            DestroyImmediate(childlenlines[i].gameObject);
        }

        //lineの追加
        Quaternion q = Quaternion.Euler(90, 0, 0);
        for (int i = 1; i < num; i++)
        {
            float z = 2 * maxVec / num * i - maxVec;
            GameObject child = Instantiate(line, new Vector3(0, y, z), q);
            child.transform.parent = linet;
        }

        q = Quaternion.Euler(90, 90, 0);
        for (int i = 1; i < num; i++)
        {
            float x = 2 * maxVec / num * i - maxVec;
            GameObject child = Instantiate(line, new Vector3(x, y, 0), q);
            child.transform.parent = linet;
        }

        //Quadの追加
        float unitLength = maxVec * 2 / num;
        float offset = unitLength / 2 - maxVec;
        q = Quaternion.Euler(90, 0, 0);

        GameObject[] quadArray = { redQuad, blueQuad, redDisplayQuad, blueDisplayQuad };
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < num; j++)
            {
                for (int k = 0; k < quadArray.Length; k++)
                {
                    GameObject obj = quadArray[k];
                    Vector3 pos = new Vector3(unitLength * i + offset, 0.001f, unitLength * j + offset);
                    GameObject redQ = Instantiate(obj, pos, q);
                    switch (k)
                    {
                        case 0:
                            redQ.name = "RedQuad(" + i + "," + j + ")";
                            break;
                        case 1:
                            redQ.name = "BlueQuad(" + i + "," + j + ")";
                            break;
                        case 2:
                            redQ.name = "RedDisplayQuad(" + i + "," + j + ")";
                            break;
                        case 3:
                            redQ.name = "BlueDisplayQuad(" + i + "," + j + ")";
                            break;
                    }
                    Transform redt = redQ.transform;
                    redt.localScale *= unitLength;
                    redt.parent = quadt;
                }
            }
        }

    }

}
