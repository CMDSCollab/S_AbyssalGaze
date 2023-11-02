using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_MiningLaser : MonoBehaviour
{
    public LineRenderer laser;
    public ParticleSystem beam_OnMachine;
    public ParticleSystem beam_LaserStart;
    public ParticleSystem beam_LaserSparkle;
    private float currentLength;
    public float stretchSpeed;
    public float shrinkAmount;
    public LayerMask layer_Mineral;
    [HideInInspector] public bool isLaserStretch = false;
    [HideInInspector] public bool isDigging = false;
    private Vector3 worldEnd;
    public bool isLeft;
    public float digRadius;
    public float speedOffset;

    void Start()
    {
        LaserEnd();
    }

    void Update()
    {
        if (isLaserStretch) LaserStretch();
    }

    public void LaserStart()
    {
        currentLength = 0;
        isLaserStretch = true;
        beam_OnMachine.Play();
        beam_LaserStart.Play();
        laser.SetPosition(1, new Vector3(0, 0, 0));
    }

    void LaserStretch()
    {
        currentLength += Time.deltaTime * stretchSpeed;
        Vector3 currentEnd = new Vector3(0, 0, currentLength);
        laser.SetPosition(1, currentEnd);
        worldEnd = laser.transform.position + new Vector3(isLeft? currentLength/2.1f:-currentLength/speedOffset, 0, 0);
        //Debug.Log(currentEnd+" - " + laser.transform.position + " - " + worldEnd) ;

        if (Physics.OverlapSphere(worldEnd, 0.1f).Length != 0)
        {
            foreach (Collider col in Physics.OverlapSphere(worldEnd, 0.1f))
            {
                Debug.Log(col.gameObject.name);
                if (col.CompareTag("Mineral"))
                {
                    beam_LaserSparkle.transform.position = worldEnd;
                    beam_LaserSparkle.transform.position = worldEnd;
                    if (!beam_LaserSparkle.isPlaying) beam_LaserSparkle.Play();
                    isDigging = true;
                }
            }
        }
        else
        {
            if (beam_LaserSparkle.isPlaying) beam_LaserSparkle.Stop();
            isDigging = false;
        }


        //RaycastHit hit;
        //if(Physics.Raycast(beam_LaserStart.transform.position, beam_LaserStart.transform.forward, out hit, currentLength, layer_Mineral))
        //{
        //    beam_LaserSparkle.transform.position = hit.point;
        //    beam_LaserSparkle.transform.position = hit.point;
        //    if (!beam_LaserSparkle.isPlaying) beam_LaserSparkle.Play();
        //    isDigging = true;
        //}
        //else
        //{
        //    if (beam_LaserSparkle.isPlaying) beam_LaserSparkle.Stop();
        //    isDigging = false;
        //}


        //float distance = (new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(beamStart.position.x, 0, beamStart.position.z)).magnitude;
  



        //if (hit.collider.gameObject.GetComponent<O_BaseEnemy>() != null)
        //        hit.collider.gameObject.GetComponent<O_BaseEnemy>().DamagedByLaser();
        //    if (hit.collider.gameObject.GetComponent<OE_BossMouth>() != null)
        //        hit.collider.gameObject.GetComponent<OE_BossMouth>().DamagedByLaser();
        //    if (hit.collider.gameObject.GetComponent<OE_BossSide>() != null)
        //        hit.collider.gameObject.GetComponent<OE_BossSide>().DamagedByLaser();
        //    M_AmmoRepo.Instance.LaserConsume();
    }

    public void LaserShrink()
    {
        currentLength -= shrinkAmount;
        laser.SetPosition(1, new Vector3(0, 0, currentLength));
    }

    public void LaserEnd()
    {
        currentLength = 0;
        isLaserStretch = false;
        laser.SetPosition(1, new Vector3(0, 0, 0));
        beam_LaserStart.Stop();
        beam_OnMachine.Stop();
        beam_LaserSparkle.Stop();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(worldEnd, 0.1f);
    }
}
