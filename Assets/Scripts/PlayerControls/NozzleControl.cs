using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozzleControl : MonoBehaviour
{
    public GameObject shotPrefab;
    private GameObject[] bullets = new GameObject[10];

    PlayerInputAction inputAction;

    public float fireRate;
    public float bulletSpeed;
    private int bulletIndex;
    private float _timestamp;

    void Awake()
    {
        inputAction = new PlayerInputAction();
        inputAction.PlayerControls.Shoot.performed += _ => Shoot();
    }

    void Start()
    {
        Array.Clear(bullets, 0, bullets.Length);
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(shotPrefab, new Vector3(-150, 0, 0), transform.rotation);
            bullets[i].GetComponent<ShotBehavior>().speed = 0;
        }

        fireRate = 0.3f;
        bulletIndex = 0;
    }

    /// <summary>
    /// Create, move and destroy the projectile
    /// </summary>
    void Shoot()
    {
        if (Time.time >= _timestamp + fireRate && Time.timeScale != 0)
        {
            AudioManager.Instance.Play("PlayerShoot");

            _timestamp = Time.time;

            bullets[bulletIndex].transform.position = transform.position;
            bullets[bulletIndex].transform.rotation = transform.rotation * Quaternion.Euler(90, 0, 0);
            bullets[bulletIndex].GetComponent<ShotBehavior>().speed = bulletSpeed;
            
            bulletIndex++;
            if (bulletIndex == bullets.Length)
            {
                bulletIndex = 0;
            }
        }
    }

    //Enable and disable the input action
    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }
}
