using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem;

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

    private bool isBossFightStart = false;
    //public PlayerInput playerInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MachineMovement();

        if (!isOnGround) MachineOnDive(transform.position.y);
        else MachineOnGround();

        //if (currentMine != null && playerInput.actions["Mine"].triggered && !isOnMining)
        //{
        //    M_MiningGame.Instance.StartMining(currentGround.GetMineralType(currentMine.transform));
        //    isOnMining = true;
        //}

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
        //Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>().normalized;
        //Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
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

        if (collision.gameObject.CompareTag("BossGround") && !isBossFightStart)
        {
            isBossFightStart = true;
            string[] world1BgAudio = new string[1] { "Boss" };
            M_Audio.PlayLoopAudio(world1BgAudio);
        }

        if (collision.gameObject.CompareTag("Monster"))
        {
            if (collision.gameObject.GetComponent<OE_Melee>() != null)
            {
                GetComponent<M_MachineValue>().HPDecrease(collision.gameObject.GetComponent<OE_Melee>().damageAmount);
                M_Audio.PlayOneShotAudio("Getting Hit");
            }
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
            if (other.GetComponent<O_EnemyBullet>()!=null)
            {
                GetComponent<M_MachineValue>().HPDecrease(other.GetComponent<O_EnemyBullet>().damageAmount);
                M_Audio.PlayOneShotAudio("Getting Hit");
            }
        }

        if (other.gameObject.CompareTag("Mineral"))
        {
            currentMine = other.gameObject;
            //Debug.Log("There is Mineral");
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
            //Debug.Log("Leaved the Mineral");
        }
    }

    public void CurrentMineMiningFinished()
    {
        Destroy(currentMine, 0.5f);
        currentMine = null;
    }
}
