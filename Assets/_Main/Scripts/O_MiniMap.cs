using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class O_MiniMap : MonoBehaviour
{
    public Transform radar;

    void Start()
    {
        
    }

    void Update()
    {
        radar.Rotate(-Vector3.forward, 0.1f);
    }
}
