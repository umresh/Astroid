using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    [SerializeField]
    private float fireRate = 0.25f;

    [SerializeField]
    private Transform bulletSpawnerTransform;

    [SerializeField]
    private AudioClip sound;

    [SerializeField]
    private float soundVolume = 0.4f;

    private BulletsPool bulletsPool;
    private Player player;
    private float nextFire = 0.0f;
    private int burstCount = 3;
    private bool isBurstActive = false;

    private WeaponDataScriptableObject weaponData;

    void Start()
    {
        GameManager.Instance.OnFireWeapon += CheckAndFire;
        weaponData = GameManager.Instance.weaponDataSO;
        burstCount = weaponData.burstCount;
        fireRate = weaponData.fireRate;
        sound = weaponData.fireAudioClip;
        isBurstActive = weaponData.burst;

        bulletsPool = GetComponent<BulletsPool>();
        bulletsPool.InitializePool();
        player = GetComponent<Player>();
    }

    void CheckAndFire()
    {
        if (!bulletsPool.isInitialised) return;

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                FireWeapon();
            }
        }
        else
        {
            nextFire = Time.time;
        }
    }

    void FireWeapon()
    {
        Quaternion newRot = gameObject.transform.rotation;
        if (isBurstActive)
        {
            int spread = 10;
            for (int i = 0; i < burstCount; i++)
            {
                //Adds rotation to the projectiles for spread in burst shot
                float addedOffset = (i - (burstCount / 2)) * spread;
                newRot = Quaternion.Euler(gameObject.transform.localEulerAngles.x,
                gameObject.transform.localEulerAngles.y,
                gameObject.transform.localEulerAngles.z + (addedOffset));
                FireSingleWeapon(newRot);
            }
        }
        else
            FireSingleWeapon(newRot);

        AudioManager.Instance.PlaySFX(sound, soundVolume);
    }

    /// <summary>
    /// Get the bullet object from the pool and shoot
    /// </summary>
    /// <param name="roation"></param>
    void FireSingleWeapon(Quaternion roation)
    {
        GameObject weaponGO = bulletsPool.GetGameObject();
        weaponGO.transform.position = bulletSpawnerTransform.position;
        weaponGO.transform.rotation = roation;

        weaponGO.GetComponent<Bullet>().Init(bulletsPool, weaponData.damage, transform, 3);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnFireWeapon -= CheckAndFire;
    }
}


