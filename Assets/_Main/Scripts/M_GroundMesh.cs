using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_GroundMesh : MonoBehaviour
{
    public GameObject pre_GroundMesh;
    public Transform parent_Ground;
    private int previousLevelPosIndex;
    public LayerMask groundLayer;

    private bool isPreviousGroundUpper = false;
    private Transform bottom;

    public void GenerateGroundInDepth(int targetDepth, float apartYDistance)
    {
        Vector3 targetPos = new Vector3(0, (targetDepth) * apartYDistance, 0);
        Transform newPlane = Instantiate(pre_GroundMesh, targetPos, Quaternion.Euler(0, 0, 0), parent_Ground).transform;
        float xOffset = (newPlane.GetComponent<O_GroundMesh>().xRange / 2) * newPlane.localScale.x;
        float zOffset = (newPlane.GetComponent<O_GroundMesh>().zRange / 2) * newPlane.localScale.z;
        newPlane.position -= new Vector3(xOffset, 0, zOffset);

        newPlane.RotateAround(Vector3.zero, Vector3.up, isPreviousGroundUpper ? Random.Range(0, 180) : Random.Range(180, 360));
        newPlane.gameObject.layer = LayerMask.NameToLayer("Ground");
        newPlane.position -= newPlane.forward * 0.5f;

        newPlane.GetComponent<O_GroundMesh>().InitializeGroundValue(targetDepth);
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

        float radian = angle * Mathf.PI / 180.0f;
        float yc = Mathf.Sin(radian);
        float xc = Mathf.Cos(radian);
        float radio = (a * b) / Mathf.Sqrt(Mathf.Pow(yc * a, 2) + Mathf.Pow(xc * b, 2));

        float ax = radio * xc;
        float ay = radio * yc;
        pt.x = (int)(lpRect.x + a + ax);
        pt.y = (int)(lpRect.y + b + ay);
        
        return pt;
    }

    public void DestroyUpperGround()
    {
        Destroy(parent_Ground.GetChild(0).gameObject);
    }
}
