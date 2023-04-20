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
    public int groundScaleOffset;
    public LayerMask groundLayer;

    private bool isPreviousGroundUpper = false;

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
        //bottom = GameObject.Find("Environment").transform.Find("Bottom");
        GenerateIntinialLevel();
    }

    public void GenerateIntinialLevel()
    {
        for (int i = 0; i < initialLayers; i++) GenerateGroundInDepth(i);
    }

    public void GenerateNewLevel(int newDepth)
    {
        GenerateGroundInDepth(newDepth + initialLayers);
    }

    //public void GenerateGroundInDepth(int targetDepth)
    //{
    //    int targetYRotation = 0;
    //    switch (previousLevelPosIndex)
    //    {
    //        case 0:
    //            targetYRotation = 0;
    //            break;
    //        case 1:
    //            targetYRotation = 90;
    //            break;
    //        case 2:
    //            targetYRotation = 180;
    //            break;
    //        case 3:
    //            targetYRotation = 270;
    //            break;
    //    }
    //    Vector3 targetPos = new Vector3(pivotPos[previousLevelPosIndex].x, (targetDepth) * apartYDistance, pivotPos[previousLevelPosIndex].z);
    //    Instantiate(pre_GroundMesh, targetPos, Quaternion.Euler(0, targetYRotation, 0), parent_Ground);
    //    //bottom.transform.position += new Vector3(0, apartYDistance, 0);

    //    previousLevelPosIndex++;
    //    if (previousLevelPosIndex >= 4) previousLevelPosIndex = 0;
    //}

    public void GenerateGroundInDepth(int targetDepth)
    {
        Vector3 targetPos = new Vector3(0, (targetDepth) * apartYDistance, 0);
        Transform newPlane = Instantiate(pre_GroundMesh, targetPos, Quaternion.Euler(0, 0, 0), parent_Ground).transform;
        float xOffset = (newPlane.GetComponent<O_GroundMesh>().xRange / 2) * newPlane.localScale.x;
        float zOffset = (newPlane.GetComponent<O_GroundMesh>().zRange / 2) * newPlane.localScale.z;
        newPlane.position -= new Vector3(xOffset, 0, zOffset);

        newPlane.RotateAround(Vector3.zero, Vector3.up, isPreviousGroundUpper ? Random.Range(0, 180) : Random.Range(180, 360));
        newPlane.gameObject.layer = LayerMask.NameToLayer("Ground");
        //Vector2 peripheryPoint = GetArcPoint(new Vector2(30, 20), newPlane.transform.localRotation.y);
        //float distance = Vector2.Distance(Vector2.zero, peripheryPoint)*0.1f;
        //Debug.Log(distance+" - "+ newPlane.transform.localRotation.y);

        newPlane.position -= newPlane.forward *0.5f;
        //Debug.Log(newPlane.transform.position);
        //bottom.transform.position += new Vector3(0, apartYDistance, 0);
        isPreviousGroundUpper = !isPreviousGroundUpper;
        previousLevelPosIndex++;
        if (previousLevelPosIndex >= 4) previousLevelPosIndex = 0;
    }

    public Vector2 GetArcPoint(Vector2 lpRect, float angle)
    {
        Vector2 pt = new Vector2();
        float a = lpRect.x / 2f;
        float b = lpRect.y / 2f;
        if (a == 0 || b == 0) return new Vector2(lpRect.x, lpRect.y);

        //弧度
        float radian = angle * Mathf.PI / 180.0f;

        //获取弧度正弦值
        float yc = Mathf.Sin(radian);
        //获取弧度余弦值
        float xc = Mathf.Cos(radian);
        //获取曲率  r = ab/\Sqrt((a.Sinθ)^2+(b.Cosθ)^2
        float radio = (a * b) / Mathf.Sqrt(Mathf.Pow(yc * a, 2) + Mathf.Pow(xc * b, 2));

        //计算坐标
        float ax = radio * xc;
        float ay = radio * yc;
        pt.x = (int)(lpRect.x + a + ax);
        pt.y = (int)(lpRect.y + b + ay);
        
        return pt;
    }
}
