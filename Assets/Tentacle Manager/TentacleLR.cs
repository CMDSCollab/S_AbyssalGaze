using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TentacleLR : MonoBehaviour
{
    public int length;
    public LineRenderer lr;
    public Vector3[] segmentPoses;
    protected Vector3[] segmentVelocities;
    public Transform targetDir;
    public float targetDistance;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    private void Start()
    {
        InitializeTentacle();
    }

    public void Update()
    {
        TentacleBehavior();
    }

    protected virtual void InitializeTentacle()
    {
        lr.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVelocities = new Vector3[length];
        ResetPos();
    }

    protected void ResetPos()
    {
        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < length; i++)
        {
            segmentPoses[i] = segmentPoses[i - 1] - targetDir.right * targetDistance;
        }
        lr.SetPositions(segmentPoses);
    }

    protected void RegularSwayBehavior()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] - targetDir.right * targetDistance, ref segmentVelocities[i], smoothSpeed + i / trailSpeed);
        }
        lr.SetPositions(segmentPoses);
    }

    protected virtual void TentacleBehavior()
    {
        RegularSwayBehavior();
    }
}
