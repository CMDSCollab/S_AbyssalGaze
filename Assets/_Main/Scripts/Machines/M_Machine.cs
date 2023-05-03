using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class M_Machine : Singleton<M_Machine>
{
    private Rigidbody rb;
    public float moveSpeed;

    private bool isOnGround = false;
    public float MiningTime;
    private float timer_mining;
    [HideInInspector]public bool isOnMining = false;
    private O_GroundMesh currentGround;
    private static GameObject currentMine;


    public Action<float> MachineOnDive;
    public Action MachineOnGround;
    public Action<object> MachineOnHit;
    public Action<object> MineComplete;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
 
    }

    void Update()
    {
        MachineMovement();

        if (!isOnGround) MachineOnDive(transform.position.y);
        else MachineOnGround();

        if (currentMine != null && Input.GetKeyDown(KeyCode.Space) && !isOnMining)
        {
            M_MiningGame.Instance.StartMining(currentGround.GetMineralType(currentMine.transform));
            isOnMining = true;
        }
    }

    public void MachineMovement()
    {
        float horiAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horiAxis, 0, verAxis).normalized;
        if (direction != Vector3.zero)
            rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.z * moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            currentGround = collision.gameObject.GetComponent<O_GroundMesh>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
            currentGround = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            MachineOnHit(other.gameObject);
        }

        if (other.gameObject.CompareTag("Mineral"))
        {
            currentMine = other.gameObject;
            Debug.Log("There is Mineral");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.CompareTag("Mine"))
        //{
        //    timer_mining += Time.deltaTime;
        //    if (timer_mining > MiningTime)
        //    {
        //        Destroy(other.gameObject);
        //        MineComplete(other.gameObject);
        //        timer_mining = 0;
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Mineral"))
        {
            currentMine = null;
            Debug.Log("Leaved the Mineral");
        }
    }
}
