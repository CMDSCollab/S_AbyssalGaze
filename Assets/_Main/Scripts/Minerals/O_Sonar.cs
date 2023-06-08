using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class O_Sonar : MonoBehaviour
{
    public float detectRadius;
    public Color c_MUndetected;
    public Color c_MDetected;

    private GameObject bloommingComponent;
    public Material mat_Bloom;
    public MMF_Player mmf_Blink;

    void Start()
    {
        bloommingComponent = transform.Find("Sonar_White").gameObject;
        bloommingComponent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            M_Sonar.Instance.CallWave(transform.position);
            Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectRadius);
            //Debug.Log("There is " + detectedColliders.Length + " Colliders");
            List<Transform> detectedMinerals = new List<Transform>();

            foreach (var item in detectedColliders) if (item.CompareTag("Mineral")) detectedMinerals.Add(item.transform);

            Debug.Log("There is " + detectedMinerals.Count + " M");
            if (detectedMinerals.Count == 0)
            {
                StartCoroutine(Blink(c_MUndetected, 1));
            }
            else
            {
                StartCoroutine(Blink(c_MDetected, detectedMinerals.Count));
            }
        }
    }

    private IEnumerator Blink(Color bloomingColor, int blinkTime)
    {
        mat_Bloom.SetColor("_EmissionColor", bloomingColor);
        for (int i = 0; i < blinkTime; i++)
        {
            bloommingComponent.SetActive(true);
            mmf_Blink.PlayFeedbacks();
            yield return new WaitForSeconds(1f);
            bloommingComponent.SetActive(false);
            yield return new WaitForSeconds(0.4f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, detectRadius);
    }
}
