using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class M_Depth : Singleton<M_Depth>
{
    private float currentDepth;
    private int currentLayer;

    public int initialLayers;
    public float apartYDistance;
    public TMP_Text txt_Depth;

    public Transform depthTextContainer;
    public float flipTime;
    public float flipInterval;
    private string currentString;

    RectTransform NumA;
    RectTransform NumB;

    private void Start()
    {
        StartCoroutine(DepthSnyc());
    }

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
        //text_Depth.text = "Depth: " + currentDepth.ToString("f2") + " Layer: " + currentLayer.ToString();
        //txt_Depth.text = currentDepth.ToString("f2");
        //txt_Depth.text = (int)(currentDepth * 100) + " M";
    }

    public void GenerateIntinialLevel()
    {
        for (int i = 0; i < initialLayers; i++) GetComponent<M_GroundMesh>().GenerateGroundInDepth(i, apartYDistance);
    }

    IEnumerator DepthSnyc()
    {
        while (true)
        {

            string depthToShow = Mathf.Abs(currentDepth / 10).ToString();
            if (depthToShow != currentString)
            {
                currentString = depthToShow;

                List<char> charArray = new List<char>();
                foreach (char letter in depthToShow.ToString())
                {
                    if (letter.ToString() != ".")
                        charArray.Add(letter);
                }
                //Debug.Log(charArray.Count);
                for (int i = 0; i < depthTextContainer.childCount; i++)
                {

                    RectTransform inScreenT = depthTextContainer.GetChild(i).GetChild(0).GetComponent<RectTransform>();
                    //Debug.Log(inScreenT.name);
                    RectTransform outScreenT = depthTextContainer.GetChild(i).GetChild(1).GetComponent<RectTransform>();
                    //Debug.Log(outScreenT.name);
                    outScreenT.GetComponent<TMP_Text>().text = charArray.Count > i ? charArray[i].ToString() : 0.ToString();

                    VerticalMove(inScreenT, -32);
                    VerticalMove(outScreenT, 0);
                    yield return new WaitForSeconds(flipInterval);
                }

                yield return new WaitForSeconds(flipTime + 0.1f);

                for (int i = 0; i < depthTextContainer.childCount; i++)
                {
                    RectTransform inScreenT = depthTextContainer.GetChild(i).GetChild(0).GetComponent<RectTransform>();
                    RectTransform outScreenT = depthTextContainer.GetChild(i).GetChild(1).GetComponent<RectTransform>();
                    inScreenT.anchoredPosition = new Vector2(inScreenT.anchoredPosition.x, 32);
                    outScreenT.transform.SetAsFirstSibling();
                }

            }


            yield return new WaitForSeconds(0.05f);
        }

        void VerticalMove(RectTransform rect, float targetY)
        {
            DOTween.To(() => rect.anchoredPosition, x => rect.anchoredPosition = x, new Vector2(rect.anchoredPosition.x, targetY), flipTime);
            //else DOTween.To(() => rect.anchoredPosition, x => rect.anchoredPosition = x, new Vector2(rect.anchoredPosition.x, targetY), flipTime);
            //.OnComplete(() => rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -targetY))
            //.OnComplete(() => rect.transform.SetAsFirstSibling());
        }
    }
}