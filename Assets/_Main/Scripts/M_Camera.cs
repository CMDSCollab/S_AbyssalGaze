using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Camera : Singleton<M_Camera>
{
    private float verDistanceOffset;

    void Start()
    {
        verDistanceOffset = transform.position.y - M_Machine.Instance.transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(M_Machine.Instance.transform.position.x, M_Machine.Instance.transform.position.y + verDistanceOffset, M_Machine.Instance.transform.position.z);
    }
}
