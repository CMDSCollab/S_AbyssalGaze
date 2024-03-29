using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class M_Firearm : MonoBehaviour
{
    public Transform parent_Firearm;
    public Transform parent_Rotation;
    public GunList[] firearmList;

    public GameObject pre_Bullet;
    public GameObject fx_MuzzleFlash;
    private Transform currentFirearm;
    private static GunType currentType;
    private static int currentIndex;

    private Vector3 aimDirection;
    public LayerMask layer_Laser;
    public GameObject fx_Explosion;
    public GameObject fx_ExplosionSmall;

    //private PlayerInput playerInput;
    public Image weaponButtonImage;
    private float fireTimer;

    void Start()
    {
        //playerInput = FindObjectOfType<PlayerInput>();
        EquipWeapon(0);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, int.MaxValue))
        {
            aimDirection = new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z);
            float angle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
            parent_Rotation.localRotation = Quaternion.Euler(0, angle, 0);
        }

        if (currentType != GunType.Laser && Input.GetMouseButtonDown(0))
        {
            //Debug.Log("ENtered");
            Firing(); 
        }
        if (currentType == GunType.Laser)
        {
            if (Input.GetMouseButtonDown(0)) EnableLaser();
            if (Input.GetMouseButton(0)) FiringLaser();
            if (Input.GetMouseButtonUp(0)) DisableLaser();
        }

        if (Input.GetKeyDown(KeyCode.Q)) SwitchWeapon(1);


        //Vector2 inputDirection = playerInput.actions["Shoot"].ReadValue<Vector2>();
        //aimDirection = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;
        //float angle = Mathf.Atan2(aimDirection.x, aimDirection.z) * Mathf.Rad2Deg;
        //parent_Rotation.localRotation = Quaternion.Euler(0, angle, 0);

        fireTimer -= Time.deltaTime;

        //if (currentType != GunType.Laser && aimDirection != Vector3.zero && fireTimer<0) Firing();
        //if (currentType == GunType.Laser) {
        //    if (aimDirection != Vector3.zero) EnableLaser();
        //    if (aimDirection != Vector3.zero) FiringLaser();
        //    if (aimDirection == Vector3.zero) DisableLaser();
        //} 

        //if (playerInput.actions["Weapon"].triggered) SwitchWeapon(1);
    }

    void EquipWeapon(int weaponIndex)
    {
        if (currentFirearm != null) Destroy(currentFirearm.gameObject);

        currentFirearm = Instantiate(firearmList[weaponIndex].pre_Gun, parent_Firearm).transform;
        weaponButtonImage.sprite = firearmList[weaponIndex].icon;
        currentType = firearmList[weaponIndex].gunType;
        currentIndex = weaponIndex;
    }

    void Firing()
    {
        if (M_AmmoRepo.Instance.GetAmmo > 1)
        {
            switch (currentType)
            {
                case GunType.Rifle:
                    fireTimer = firearmList[0].fireRate;
                    GameObject muzzleFlash = Instantiate(fx_MuzzleFlash, currentFirearm.Find("Muzzle"));
                    GameObject bullet = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle"));
                    bullet.GetComponent<O_Bullet>().BulletSetUp(aimDirection, 10);
                    M_Audio.PlayOneShotAudio("Gunshot");
                    break;
                case GunType.Shot:
                    fireTimer = firearmList[1].fireRate;
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject shotFlash = Instantiate(fx_MuzzleFlash, currentFirearm.Find("Muzzle"));
                        GameObject shotBullet = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle"));

                        if (i < 2) shotBullet.transform.Rotate(Vector3.forward, -20 * (i + 1));
                        else if (i > 2) shotBullet.transform.Rotate(Vector3.forward, 20 * (i - 2));

                        shotBullet.GetComponent<O_Bullet>().BulletSetUp(shotBullet.transform.up, 5);
                    }
                    M_Audio.PlayOneShotAudio("Shotgun");
                    break;
                case GunType.Mini:
                    fireTimer = firearmList[2].fireRate;
                    GameObject leftFlash = Instantiate(fx_MuzzleFlash, currentFirearm.Find("Muzzle Left"));
                    GameObject rightFlash = Instantiate(fx_MuzzleFlash, currentFirearm.Find("Muzzle Right"));
                    GameObject miniBulletLeft = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle Left"));
                    GameObject miniBulletRight = Instantiate(pre_Bullet, currentFirearm.Find("Muzzle Right"));
                    miniBulletLeft.GetComponent<O_Bullet>().BulletSetUp(aimDirection, 8);
                    miniBulletRight.GetComponent<O_Bullet>().BulletSetUp(aimDirection, 8);
                    M_Audio.PlayOneShotAudio("Gunshot");
                    break;
            }
            M_AmmoRepo.Instance.BulletConsume();
        }
    }

    void EnableLaser()
    {
        M_Audio.PlayOneShotAudio_StopInCertainTime("Laser");
        Transform muzzle = currentFirearm.Find("Muzzle");
        muzzle.GetChild(0).gameObject.SetActive(true);
        muzzle.GetChild(1).gameObject.SetActive(true);
        muzzle.GetChild(2).gameObject.SetActive(true);
        muzzle.GetChild(3).gameObject.SetActive(true);
    }

    void FiringLaser()
    {
        if (M_AmmoRepo.Instance.GetAmmo > 1)
        {
            Transform muzzle = currentFirearm.Find("Muzzle");
            Transform beamStart = muzzle.Find("Beam");
            RaycastHit hit;
            Physics.Raycast(muzzle.position, muzzle.forward, out hit, Mathf.Infinity, layer_Laser);
            LineRenderer lr = muzzle.Find("Laser").GetComponent<LineRenderer>();
            float distance = (new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(beamStart.position.x, 0, beamStart.position.z)).magnitude;
            //lr.SetPosition(0, muzzle.Find("Beam").position);
            lr.SetPosition(1, new Vector3(0, 0, distance));
            muzzle.Find("BeamHit 1").transform.position = hit.point;
            muzzle.Find("BeamHit 2").transform.position = hit.point;


            if (hit.collider.gameObject.GetComponent<O_BaseEnemy>() != null)
                hit.collider.gameObject.GetComponent<O_BaseEnemy>().DamagedByLaser();
            if (hit.collider.gameObject.GetComponent<OE_BossMouth>() != null)
                hit.collider.gameObject.GetComponent<OE_BossMouth>().DamagedByLaser();
            if (hit.collider.gameObject.GetComponent<OE_BossSide>() != null)
                hit.collider.gameObject.GetComponent<OE_BossSide>().DamagedByLaser();
            M_AmmoRepo.Instance.LaserConsume();
        }
    }

    void DisableLaser()
    {
        M_Audio.OneShotAudio_StopCertain();
        Transform muzzle = currentFirearm.Find("Muzzle");
        muzzle.GetChild(0).gameObject.SetActive(false);
        muzzle.GetChild(1).gameObject.SetActive(false);
        muzzle.GetChild(2).gameObject.SetActive(false);
        muzzle.GetChild(3).gameObject.SetActive(false);
    }

    void SwitchWeapon(int IndexChangeAmount)
    {
        currentIndex += IndexChangeAmount;
        if (currentIndex > 3) currentIndex = 0;
        EquipWeapon(currentIndex);
        if (currentIndex == 3) DisableLaser();
    }

    //private void OnDrawGizmos()
    //{
    //    Transform muzzle = currentFirearm.Find("Muzzle");
    //    RaycastHit hit;
    //    Physics.Raycast(muzzle.position, muzzle.forward, out hit, Mathf.Infinity, layer_Laser);
    //    Debug.DrawRay(muzzle.position, muzzle.forward, Color.red);
    //}
}

[System.Serializable]
public class GunList
{
    public GameObject pre_Gun;
    public GunType gunType;
    public Sprite icon;
    public float fireRate;
}

public enum GunType { Rifle, Shot, Mini, Laser }