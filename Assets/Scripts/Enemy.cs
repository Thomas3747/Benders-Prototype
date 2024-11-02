using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : MonoBehaviour
{
    public float hitForce = 250;
    private int enemyHealth = 2;
    public Rigidbody playerRb;
    public PlayerController playerRef;
    public int pointValue;
    public bool enemyIsDead = false;
    public bool isPaused = false;
    public float range = 20;
    public GameObject fireBend;
    public GameObject target;
    public GameObject enemyFire;
    private Transform playerPos;
    public GameObject targetFire;
    public float efireSpeed = 50;
    private GameManager gameManager;    
    public float spawnRate = 2f;
    public float spawnDelayInterval = 1f;
    public float fireSpeed = 8.0f;
    private float catchSpeed = 2.5f;
    private float dtotalTime = 5;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRef = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive == true && isPaused == false)
        {
            FollowPlayer();
            DifficultyTimer();
        }

        else if (gameManager.isGameActive == true && isPaused == true)
        {
            catchSpeed = 0;
        }
    }

    //timer to vary enemy speed
    public void DifficultyTimer()
    {
        if (dtotalTime > 0)
        {
            dtotalTime -= Time.deltaTime;
        }

        else if (dtotalTime <= 0)
        {
            TimerRestart();
        }
    }
    //resatart timer
    void TimerRestart()
    {
        catchSpeed++;
        dtotalTime = 5;
    }

    //enemy follows player
    public void FollowPlayer()
    {
        LookAtPlayer();
        transform.position += transform.forward * catchSpeed * Time.deltaTime;
    }

    //enemy turns towards player
    public void LookAtPlayer()
    {
        gameObject.transform.LookAt(playerPos);
    }

    //enemy damage section
    public void EnemyHit()
    {
        if (playerRef.inAvatarState == true || playerRef.hasFirePowerup == true)
        {
            enemyHealth -= 2;
        }
        else
        {
            enemyHealth--;
        }
    }

    public void Enemypush()
    {
        Vector3 awayFromPlayer = GameObject.Find("Player").transform.position - transform.position;
        playerRb.AddForce(-awayFromPlayer * hitForce, ForceMode.Impulse);
    }
    //for when an enemy dies
    public void EnemyIsDead()
    {
       // gameManager.Splatter(transform.position);
        gameManager.UpdateScore();
        Destroy(gameObject);
    }

    //pause mode checker
    public void Pause()
    {
        isPaused = true;
        catchSpeed = 0;
    }

    //resume state checker
    public void Resume()
    {
        isPaused = false;
        catchSpeed = 2.5f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Activator"))
        {
            Enemypush();
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Enemypush();
        }
        if (collision.gameObject.CompareTag("Firebend"))
        {
            if (enemyHealth >= 0)
            {
                EnemyHit();
            }

            else if (enemyHealth < 0)
            {
                EnemyIsDead();

            }
        }

    }

}
