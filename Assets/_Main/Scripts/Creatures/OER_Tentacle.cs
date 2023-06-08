using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OER_Tentacle : MonoBehaviour
{
    public int length;
    public LineRenderer lr;
    private Vector3[] segmentPoses;
    private Vector3[] segmentVelocitys;
    public Transform targetDir;
    public float targetDistance;
    public float smoothSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    void Start()
    {
        lr.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVelocitys = new Vector3[length];
    }

    void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude-180);
        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDistance;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentVelocitys[i], smoothSpeed);
        }
        lr.SetPositions(segmentPoses);
    }
}
