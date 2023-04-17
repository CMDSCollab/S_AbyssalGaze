using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Depth : Singleton<M_Depth>
{
    private float currentDepth;
    private int currentLayer;
    public TMPro.TMP_Text text_Depth;

    public void GetCurrentDepth(float targetValue)
    {
        currentDepth = targetValue;
        int newLayer = (int)currentDepth / 5;
        if (newLayer != currentLayer)
        {
            currentLayer = newLayer;
            GetComponent<M_GroundMesh>().GenerateNewLevel(-currentLayer);
        }
        text_Depth.text = "Depth: " + currentDepth.ToString("f2") + " Layer: " + currentLayer.ToString();
    }
}
