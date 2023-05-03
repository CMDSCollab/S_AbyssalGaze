using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Mineral : Singleton<M_Mineral>
{
    public float posShrinkage;
    public int spawnNum;
    public GameObject pre_Mineral;

    public Vector2 mapping_StartPos;
    public float mapping_WorldLength;
    public float mapping_Density;
    public List<Vector2> circlePivots = new List<Vector2>();

    private float circleSize;
    private float circleRadius;
    public LayerMask groundLayer;

    public Transform parent_MineralColliders;

    public void GenerateCirclePivots()
    {
        circleSize = mapping_WorldLength / mapping_Density;
        circleRadius = circleSize / 2;
        for (int i = 0; i < mapping_Density; i++)
            for (int j = 0; j < mapping_Density; j++)
                GenerateGrid(i, j);

        void GenerateGrid(int xIndex, int yIndex)
        {
            Vector2 genPoint = new Vector2(xIndex * circleSize + circleRadius, yIndex * circleSize + circleSize);
            genPoint += mapping_StartPos;
            circlePivots.Add(genPoint);
        }
    }

    public Transform GenerateMineSpot(float targetY)
    {
        Transform newLayer = new GameObject().transform;
        newLayer.name = "Layer: " + targetY;
        foreach (Vector2 pivot in circlePivots)
        {
            Vector3 spawnPivot = new Vector3(pivot.x, targetY, pivot.y);
            if (Physics.OverlapSphere(spawnPivot, circleRadius, groundLayer).Length != 0)
            {
                GameObject newSphere = new GameObject();
                newSphere.name = "Sphere: " + pivot.x + " " + pivot.y;
                newSphere.AddComponent<SphereCollider>().radius = circleRadius;
                newSphere.GetComponent<SphereCollider>().isTrigger = true;
                newSphere.transform.position = spawnPivot;
                newSphere.transform.SetParent(newLayer);
            }
        }
        newLayer.SetParent(parent_MineralColliders);
        return newLayer;
    }

    //private void OnDrawGizmos()
    //{
    //    foreach (Vector2 pivot in circlePivots)
    //    {
    //        Gizmos.DrawSphere(new Vector3(pivot.x, 0, pivot.y), circleRadius);
    //    }
    //}



}
