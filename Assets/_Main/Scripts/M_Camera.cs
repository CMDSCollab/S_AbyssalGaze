using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Camera : MonoBehaviour
{
    public Transform obj_Machine;
    private float verDistanceOffset;
    // Start is called before the first frame update
    void Start()
    {
        verDistanceOffset = transform.position.y - obj_Machine.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, obj_Machine.position.y + verDistanceOffset, 0);
    }
}
