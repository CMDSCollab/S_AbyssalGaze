using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleDot : MonoBehaviour
{
    public int length;
    public Vector3[] segmentPoses;
    private Vector3[] segmentVelocities;
    public Transform targetDir;
    public float targetLineDistance;
    public float targetDotDistance;
    public float smoothSpeed;
    public float trailSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    public Transform bodyParent;
    private Transform[] bodyParts;
    private Vector2[] dotDirs;
    public float dotRotateSpeed;

    private Gradient gradient;

    private void Start()
    {
   
        segmentPoses = new Vector3[length];
        segmentVelocities = new Vector3[length];
        bodyParts = new Transform[bodyParent.childCount];
        dotDirs = new Vector2[bodyParent.childCount];
        for (int i = 0; i < bodyParts.Length; i++) bodyParts[i] = bodyParent.GetChild(i);
        ResetPos();
    }

    private void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] - targetDir.right * targetLineDistance, ref segmentVelocities[i], smoothSpeed + i / trailSpeed);
        }

        float apartRatio = length / bodyParts.Length;
        for (int i = 1; i < bodyParts.Length+1; i++)
        {
            int currentLineIndex = (int)(i * apartRatio);
            //Debug.Log(currentLineIndex);
            bodyParts[i - 1].transform.position = segmentPoses[currentLineIndex];
        }

        DotsRotation();
    }

    private void ResetPos()
    {
        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < length; i++)
        {
            segmentPoses[i] = segmentPoses[i - 1] - targetDir.right * targetLineDistance;
        }
    }

    private void DotsRotation()
    {
        for (int i = 1; i < dotDirs.Length; i++)
        {
            dotDirs[i] = bodyParts[i - 1].position - bodyParts[i].position;
            float angle = Mathf.Atan2(dotDirs[i].y, dotDirs[i].x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bodyParts[i].rotation = Quaternion.Slerp(bodyParts[i].rotation, rotation, dotRotateSpeed * Time.deltaTime);
        }
    }

    public void AppearanceChange_Dot(Sprite targetSprite)
    {
        foreach (Transform body in bodyParts)
        {
            body.GetComponent<SpriteRenderer>().sprite = targetSprite;
        }
    }
}
