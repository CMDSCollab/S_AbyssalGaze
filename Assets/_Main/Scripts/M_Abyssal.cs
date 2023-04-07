using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Abyssal : MonoBehaviour
{
    public GameObject pre_Ground;
    public GameObject pre_Frame;

    public float intervalDistance;
    public int frameMaxCount;

    public Transform environmentParent;

    private void Start()
    {
        AbyssalCreation();
    }

    public void AbyssalCreation()
    {
        for (int i = 0; i < frameMaxCount; i++)
        {
            GameObject frame = Instantiate(pre_Frame, new Vector3(0, -i * intervalDistance, 0), Quaternion.identity, environmentParent);
            frame.name = "F" + i;
            Vector3 targetPos = Vector3.zero;
            int alignment = Random.Range(0, 2);
            if (alignment == 0)
            {
                int leftOrRight = Random.Range(0, 2);
                if (leftOrRight == 0) targetPos = new Vector3(Random.Range(-5f, 5f), -i * intervalDistance - intervalDistance / 2, 5);
                else targetPos = new Vector3(Random.Range(-5, 5), -i * intervalDistance - intervalDistance / 2, -5);
            }
            else
            {
                int upOrDown = Random.Range(0, 2);
                if (upOrDown == 0) targetPos = new Vector3(-5, -i * intervalDistance - intervalDistance / 2, Random.Range(-5f, 5f));
                else targetPos = new Vector3(5, -i * intervalDistance - intervalDistance / 2, Random.Range(-5f, 5f));
            }

            GameObject ground = Instantiate(pre_Ground, targetPos, Quaternion.identity, environmentParent);
            ground.name = "G" + i;
        }
    }

    public void Update()
    {
        
    }
}
