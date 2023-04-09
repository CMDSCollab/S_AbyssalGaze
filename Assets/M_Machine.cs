using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Machine : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed;
    private float currentDepth;
    public TMPro.TMP_Text text_Depth;
    public bool isOnGround = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horiAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horiAxis, 0, verAxis).normalized;
        if (direction != Vector3.zero)
        {
            rb.velocity = direction * moveSpeed;
            //rb.AddForce(direction * moveSpeed);
        }

        if (!isOnGround) SetCurrentDepth(transform.position.y);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
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
        text_Depth.text = "Depth: "+currentDepth.ToString("f2");
    }

    public void GetCurrentDepth()
    {
        
    }
}
