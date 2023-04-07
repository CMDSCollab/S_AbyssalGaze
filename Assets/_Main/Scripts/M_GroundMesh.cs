using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_GroundMesh : MonoBehaviour
{
    public GameObject pre_GroundMesh;
    public int initialLayers;
    public float apartYDistance;
    public Transform parent_Ground;
    private Vector3[] pivotPos = new Vector3[]
    {
        new Vector3(-10,0,-10f),
         new Vector3(-10,0,10f),
          new Vector3(10,0,10f),
           new Vector3(10,0,-10f),
    };

    public bool isOnGround = false;

    // Start is called before the first frame update
    void Start()
    {
        GenerateIntinialLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnGround) GroundMoveUpwards();
    }

    public void GenerateIntinialLevel()
    {
        for (int i = 0; i < initialLayers; i++)
        {
            int targetYRotation = 0;
            int randomPosIndex = Random.Range(0, pivotPos.Length);
            switch (randomPosIndex)
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
            Vector3 targetPos = new Vector3(pivotPos[randomPosIndex].x, i * apartYDistance, pivotPos[randomPosIndex].z);
            Instantiate(pre_GroundMesh, targetPos, Quaternion.Euler(0,targetYRotation,0), parent_Ground);
        }
    }

    //public void InstantiateNewGround()
    //{
    //    Instantiate(pre_GroundMesh);
    //}

    public void GroundMoveUpwards()
    {

    }
}
