using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Camera : Singleton<M_Camera>
{
    private float verDistanceOffset;
    [SerializeField] Transform cam_MiniMap;


    void Start()
    {
        verDistanceOffset = transform.position.y - M_Machine.Instance.transform.position.y;
    }

    void Update()
    {
        Vector3 newMachinePos = M_Machine.Instance.transform.position;
        transform.position = new Vector3(newMachinePos.x, newMachinePos.y + verDistanceOffset, newMachinePos.z);
        cam_MiniMap.position = new Vector3(newMachinePos.x, cam_MiniMap.position.y, newMachinePos.z);
    }
}
