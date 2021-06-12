using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{ 
    [SerializeField] GameObject barrel;
    [SerializeField] Transform nozzle;
    [SerializeField] GameObject explosionObj;
    [SerializeField] protected GameObject enemyShotPrefab;

    protected GameObject[] bullets = new GameObject[10];
    protected abstract float _fireRate { get; }
    protected float timeStamp;
    protected int bulletIndex;
    public float bulletSpeed;

    protected Vector2 movementVector;

    public event Action OnDeath;

    protected void Start()
    {
        // Instantiate a bullet pool
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(enemyShotPrefab, new Vector3(-150, 0, 0), transform.rotation);
            bullets[i].GetComponent<ShotBehavior>().speed = 0;
        }
        bulletIndex = 0;
    }

    protected void Update()
    {
        BarrelFacePlayer();

        ShootAtPlayer(_fireRate);
    }

    /// <summary>
    /// Rotates barrel towards player
    /// </summary>
    protected void BarrelFacePlayer()
    {
        Vector2 facingDir = new Vector2(
            MainCharControl.mainCharPos.x - barrel.transform.position.x,
            MainCharControl.mainCharPos.y - barrel.transform.position.y
        );
        barrel.transform.up = facingDir;
    }

    /// <summary>
    /// Shoot at player on an interval
    /// </summary>
    protected void ShootAtPlayer(float rate)
    {
        if (Time.time >= timeStamp + rate)
        {
            AudioManager.Instance.Play("EnemyShoot");

            timeStamp = Time.time;

            bullets[bulletIndex].transform.position = nozzle.transform.position;
            bullets[bulletIndex].transform.rotation = nozzle.transform.rotation * Quaternion.Euler(-90, 0, 0);
            bullets[bulletIndex].GetComponent<ShotBehavior>().speed = bulletSpeed;
            
            bulletIndex++;
            if (bulletIndex == bullets.Length)
            {
                bulletIndex = 0;
            }
        }
    }

    /// <summary>
    /// WHat to do when the enemy collides with another enemy
    /// </summary>
    protected abstract void FriendlyTrigger();

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shot"))
        {
            // Play Explosion
            if (explosionObj is GameObject)
            {
                Instantiate(explosionObj, transform.position, other.transform.rotation);
            }
            AudioManager.Instance.Play("EnemyDeath");

            OnDeath?.Invoke();

            // Unsubscribe all listeners from this event
            Delegate[] clientList = OnDeath.GetInvocationList();
            foreach (var d in clientList)
                OnDeath -= (d as Action);

            if(OnDeath != null)
            foreach (var d in OnDeath.GetInvocationList())
                OnDeath -= (d as Action);

            // Set the state of the bullet that hit it
            other.transform.position = new Vector3(-150, 0, 0);
            other.GetComponent<ShotBehavior>().speed = 0;

            Terminate();
        }

        if (other.CompareTag("Enemy"))
        {
            FriendlyTrigger();
        }
    }

    /// <summary>
    /// Destroy this game object and all asscociated bullets
    /// </summary>
    internal void Terminate()
    {
        // Destroy all the bullets associated with this enemy
        foreach (var shot in bullets)
        {
            if (shot is GameObject && shot.activeSelf)
            {
                Destroy(shot, 3f);
            }
            else
            {
                Destroy(shot);
            }
        }

        // Destroy this enemy
        Destroy(gameObject);
    }
}
