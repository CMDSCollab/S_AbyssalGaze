using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_GroundMesh : MonoBehaviour
{
    public GameObject pre_GroundMesh;
    public int initialLayers;
    public float apartYDistance;
    public Transform parent_Ground;
    private int previousLevelPosIndex;
    private Vector3[] pivotPos = new Vector3[]
    {
        new Vector3(-10,0,-10f),
        new Vector3(-10,0,10f),
        new Vector3(10,0,10f),
        new Vector3(10,0,-10f),
    };
    private Transform bottom;

    void Start()
    {
        bottom = GameObject.Find("Environment").transform.Find("Bottom");
        GenerateIntinialLevel();
    }

    public void GenerateIntinialLevel()
    {
        for (int i = 0,j=0; i < initialLayers; i++)
        {
            int targetYRotation = 0;
      
            switch (j)
            {
                case 0:
                    targetYRotation = 0;
                    break;
                case 1:
                    targetYRotation = 90;
                    break;
                case 2:
                    targetYRotation = 180;
                    break;
                case 3:
                    targetYRotation = 270;
                    break;
            }
            Vector3 targetPos = new Vector3(pivotPos[j].x, i * apartYDistance, pivotPos[j].z);
            Instantiate(pre_GroundMesh, targetPos, Quaternion.Euler(0,targetYRotation,0), parent_Ground);
            j++;
            if (j >= 4) j = 0;

            if (i == initialLayers-1)
            {
                previousLevelPosIndex = j;
                bottom.transform.position = new Vector3(bottom.transform.position.x, (i + 1) * apartYDistance, bottom.transform.position.x);
            }
        }
    }

    public void GenerateNewLevel(int newDepth)
    {
        int targetYRotation = 0;
        switch (previousLevelPosIndex)
        {
            case 0:
                targetYRotation = 0;
                break;
            case 1:
                targetYRotation = 90;
                break;
            case 2:
                targetYRotation = 180;
                break;
            case 3:
                targetYRotation = 270;
                break;
        }
        Vector3 targetPos = new Vector3(pivotPos[previousLevelPosIndex].x, (newDepth+initialLayers) * apartYDistance, pivotPos[previousLevelPosIndex].z);
        Instantiate(pre_GroundMesh, targetPos, Quaternion.Euler(0, targetYRotation, 0), parent_Ground);
        bottom.transform.position += new Vector3(0, apartYDistance, 0);
        previousLevelPosIndex++;
        if (previousLevelPosIndex >= 4) previousLevelPosIndex = 0;
    }
}
