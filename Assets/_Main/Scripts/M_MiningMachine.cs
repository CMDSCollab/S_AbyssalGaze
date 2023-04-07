using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_MiningMachine : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float moveSpeed; 
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
        if (direction!= Vector3.zero)
        {
            rb.velocity = direction * moveSpeed;
        }

    }
}
