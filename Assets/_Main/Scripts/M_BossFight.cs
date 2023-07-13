using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class M_BossFight : MonoBehaviour
{
    public GameObject pre_NoCenterGround;
    public GameObject pre_TransGround;
    public GameObject pre_BossMouth;
    public GameObject pre_BossSide;

    public Image allBlack;

    private

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateBossGround(Vector3 targetPos)
    {
        Instantiate(pre_NoCenterGround, targetPos, Quaternion.identity);

        void GroundCenterRemove()
        {

        }
    }

    public void GenerateBossMouth()
    {
       
    }

    public void GenerateBossSide(Vector3 targetPos)
    {
        Instantiate(pre_BossSide, targetPos, Quaternion.Euler(0,0,0));
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => DOTween.To(() => allBlack.color, x => allBlack.color = x, new Color(0, 0, 0, 0), 1));
        s.AppendInterval(1);
        s.AppendCallback(() => allBlack.gameObject.SetActive(false));
    }
}
