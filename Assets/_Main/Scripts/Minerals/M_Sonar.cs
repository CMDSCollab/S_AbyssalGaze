using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Sonar : MonoBehaviour
{
    public Material mat_Wave;

    public float mat_Wave_Size;
    public float mat_Wave_Magnification;
    public float mat_Wave_Speed;
    public float mat_Wave_SizeRatio;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    mat_Wave.SetFloat("_Size", mat_WaveInitial_Size);
        //    mat_Wave.SetFloat("_Magnification", mat_WaveInitial_Magnification);
        //    mat_Wave.SetFloat("_Speed", mat_WaveInitial_Speed);
        //    mat_Wave.SetFloat("_SizeRatio", mat_WaveInitial_SizeRatio);
        //    mat_Wave.SetVector("_FocalPoint", mat_WaveInitial_FocalPoint);
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    mat_Wave.SetFloat("_Size", 0);
        //    mat_Wave.SetFloat("_Magnification", 0);
        //    mat_Wave.SetFloat("_Speed", 0);
        //    mat_Wave.SetFloat("_SizeRatio", 0);
        //    mat_Wave.SetVector("_FocalPoint", new Vector2(0, 0));
        //}

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, int.MaxValue))
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(hit.point.x, hit.point.y, hit.point.z));
                float focalResetX = screenPos.x / 1920;
                float focalResetY = screenPos.y / 1080;
                Vector2 newFocal = new Vector2(focalResetX, focalResetY);
                //mat_Wave.SetVector("_FocalPoint", newFocal);
                SetNewWaveValue(newFocal);
            }
        }

    }

    void SetNewWaveValue(Vector2 newFocal)
    {
        mat_Wave.SetFloat("_Size", mat_Wave_Size);
        mat_Wave.SetFloat("_Magnification", mat_Wave_Magnification);
        mat_Wave.SetFloat("_Speed", mat_Wave_Speed);
        mat_Wave.SetFloat("_SizeRatio", mat_Wave_SizeRatio);
        mat_Wave.SetVector("_FocalPoint", newFocal);
    }
}
