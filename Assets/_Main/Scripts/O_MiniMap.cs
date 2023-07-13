using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class O_MiniMap : MonoBehaviour
{
    public Transform radar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        radar.Rotate(-Vector3.forward, 1);
    }
}
