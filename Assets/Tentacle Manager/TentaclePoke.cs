using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossTantacleState { Sway, Shrink, Poke ,PokeCam}
public class TentaclePoke : TentacleLR
{
    private Vector3 initialPos;
    private Vector3[] segmentShrinkPoses;
    private Vector3[] segmentPokePoses;
    public Transform pokeDetector;
    private Collider pokeCollider;
    [HideInInspector] public BossTantacleState state;
    private bool isPoked = false;

    protected override void InitializeTentacle()
    {
        base.InitializeTentacle();
        initialPos = targetDir.position;
        segmentShrinkPoses = new Vector3[length];
        segmentShrinkPoses[0] = targetDir.position;
        segmentPokePoses = new Vector3[length];
        segmentPokePoses[0] = targetDir.position;

        pokeCollider = pokeDetector.GetComponent<Collider>();
        pokeCollider.enabled = false;
    }

    protected override void TentacleBehavior()
    {
        switch (state)
        {
            case BossTantacleState.Sway:
                RegularSwayBehavior();
                break;
            case BossTantacleState.Shrink:
                ShrinkBehavior();
                break;
            case BossTantacleState.Poke:
                PokeBehavior();
                break;
            case BossTantacleState.PokeCam:
                PokeCamBehavior();
                break;
        }
    }

    private void ShrinkBehavior()
    {
        isPoked = false;
        Vector3 targetDirection = (segmentShrinkPoses[0]- M_Machine.Instance.transform.position ).normalized;
        targetDirection = new Vector3(targetDirection.x,0, targetDirection.z);

        for (int i = 1; i < segmentShrinkPoses.Length; i++)
            segmentShrinkPoses[i] = segmentShrinkPoses[i - 1] - targetDirection * targetDistance / 2;
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentShrinkPoses[i - 1], ref segmentVelocities[i], smoothSpeed + i / trailSpeed);
        }
        lr.SetPositions(segmentPoses);
        pokeDetector.position = segmentPoses[segmentPoses.Length - 2];
    }

    private void PokeBehavior()
    {
        if (!isPoked)
        {
            Vector3 targetDirection = (segmentPokePoses[0] - M_Machine.Instance.transform.position).normalized;
            targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
            for (int i = 1; i < segmentPokePoses.Length; i++)
                segmentPokePoses[i] = segmentPokePoses[i - 1] - targetDirection * targetDistance * 2;
            isPoked = true;
        }
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPokePoses[i - 1] , ref segmentVelocities[i], smoothSpeed + i / trailSpeed);
        }
        lr.SetPositions(segmentPoses);
        pokeDetector.position = segmentPoses[segmentPoses.Length - 2];
    }

    private void PokeCamBehavior()
    {
        if (!isPoked)
        {
            Vector3 targetDirection = (segmentPokePoses[0] - Camera.main.transform.position).normalized;
            //targetDirection = new Vector3(targetDirection.x, 0, targetDirection.z);
            for (int i = 1; i < segmentPokePoses.Length; i++)
                segmentPokePoses[i] = segmentPokePoses[i - 1] - targetDirection * targetDistance * 2;
            isPoked = true;
        }
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPokePoses[i - 1], ref segmentVelocities[i], smoothSpeed + i / trailSpeed);
        }
        lr.SetPositions(segmentPoses);
        pokeDetector.position = segmentPoses[segmentPoses.Length - 2];
    }
}
