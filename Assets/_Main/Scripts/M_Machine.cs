using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class M_Machine : Singleton<M_Machine>
{
    private Rigidbody rb;
    public float moveSpeed;
    private float currentDepth;
    private int currentLayer;
    public TMPro.TMP_Text text_Depth;
    public bool isOnGround = false;
    public float maxOxygen;
    private float currentOxygen;
    public Slider slider_Oxygen;
    public TMPro.TMP_Text text_Oxygen;
    public float MiningTime;
    public float MineralOxygenAmount;
    private float timer_mining;
    public float dmgToTake;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slider_Oxygen.maxValue = maxOxygen;
        currentOxygen = maxOxygen;
        slider_Oxygen.value = currentOxygen;
    }

    void Update()
    {
        float horiAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horiAxis, 0, verAxis).normalized;
        if (direction != Vector3.zero)
            rb.velocity = new Vector3( direction.x*moveSpeed,rb.velocity.y,direction.z*moveSpeed);

        if (!isOnGround) SetCurrentDepth(transform.position.y);

        currentOxygen -= Time.deltaTime;
        SliderTextValueSync();
        if (currentOxygen<0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void SliderTextValueSync()
    {
        slider_Oxygen.value = currentOxygen;
        text_Oxygen.text = currentOxygen.ToString("f0") + " / " + maxOxygen.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            currentOxygen -= dmgToTake;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mine"))
        {
            timer_mining += Time.deltaTime;
            if (timer_mining > MiningTime)
            {
                Destroy(other.gameObject);
                currentOxygen += MineralOxygenAmount;
                timer_mining = 0;
                if (currentOxygen > maxOxygen) currentOxygen = maxOxygen;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mine"))
        {
            timer_mining = 0;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }

    }

    public void SetCurrentDepth(float targetValue)
    {
        currentDepth = targetValue;
        int newLayer = (int)currentDepth / 5;
        if (newLayer != currentLayer)
        {
            currentLayer = newLayer;
            FindObjectOfType<M_GroundMesh>().GenerateNewLevel(-currentLayer);
        }
        text_Depth.text = "Depth: " + currentDepth.ToString("f2") + " Layer: " + currentLayer.ToString();
    }

    public float GetCurrentDepth()
    {
        return currentDepth;
    }
}
