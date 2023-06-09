using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class M_Sonar : Singleton<M_Sonar>
{
    public Material mat_Wave;

    public float mat_Wave_Size;
    public float mat_Wave_Magnification;
    public float mat_Wave_Speed;
    public float mat_Wave_SizeRatio;

    public GameObject pre_Sonar;
    private bool isWaving = false;
    private float wavingTimer = 0;

    void Start()
    {
        StopWave();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, int.MaxValue)) SonarGeneration(hit.point);
        }

        if (isWaving)
        {
            wavingTimer += Time.deltaTime;
            mat_Wave.SetFloat("_TimeControl", wavingTimer);
            if (wavingTimer >= 1)
            {
                StopWave();
            }
        }
    }

    private void SonarGeneration(Vector3 worldPoint)
    {
        Instantiate(pre_Sonar, worldPoint+new Vector3(0,2,0), Quaternion.identity);
    }

    public void CallWave(Vector3 worldPoint)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPoint);
        float focalResetX = screenPos.x / 1920;
        float focalResetY = screenPos.y / 1080;
        Vector2 newFocal = new Vector2(focalResetX, focalResetY);
        SetNewWaveValue(newFocal);
        //StopWave(2.5f);
    }

    private void StopWave()
    {
        mat_Wave.SetFloat("_Size", 0);
        mat_Wave.SetFloat("_Magnification", 0);
        mat_Wave.SetFloat("_Speed", 0);
        mat_Wave.SetFloat("_SizeRatio", 0);
        mat_Wave.SetVector("_FocalPoint", new Vector2(0, 0));
        isWaving = false;
        wavingTimer = 0;
        mat_Wave.SetFloat("_TimeControl", wavingTimer);
    }

    void SetNewWaveValue(Vector2 newFocal)
    {
        mat_Wave.SetFloat("_TimeControl", 0);
        mat_Wave.SetFloat("_Size", mat_Wave_Size);
        mat_Wave.SetFloat("_Magnification", mat_Wave_Magnification);
        mat_Wave.SetFloat("_Speed", mat_Wave_Speed);
        mat_Wave.SetFloat("_SizeRatio", mat_Wave_SizeRatio);
        mat_Wave.SetVector("_FocalPoint", newFocal);
        isWaving = true;
    }
}
