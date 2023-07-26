using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class M_Depth : Singleton<M_Depth>
{
    private float currentDepth;
    private int currentLayer;
    public TMPro.TMP_Text text_Depth;

    public int initialLayers;
    public float apartYDistance;
    public TMP_Text txt_Depth;

    public void GetCurrentDepth(float targetValue)
    {
        currentDepth = targetValue;
        int newLayer = (int)((currentDepth - 0.5f) / apartYDistance);
        if (newLayer != currentLayer)
        {
            currentLayer = newLayer;
            GetComponent<M_GroundMesh>().GenerateGroundInDepth(currentLayer + initialLayers - 1, apartYDistance);
            if (GetComponent<M_GroundMesh>().parent_Ground.childCount > 0)
                GetComponent<M_GroundMesh>().DestroyUpperGround();
        }
        text_Depth.text = "Depth: " + currentDepth.ToString("f2") + " Layer: " + currentLayer.ToString();
        txt_Depth.text = currentDepth.ToString("f2");
    }

    public void GenerateIntinialLevel()
    {
        for (int i = 0; i < initialLayers; i++) GetComponent<M_GroundMesh>().GenerateGroundInDepth(i, apartYDistance);
    }
}
