using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.AI;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.XR;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 450.0f;
    public int playerHealth = 5;
    public bool inAvatarState = false;
    private bool hasAirPowerup = false;
    public bool hasWaterPowerup = false;
    public bool hasFirePowerup = false;
    public GameObject airbendIndicator;
    public GameObject WaterbendIndicator;
    public TrailRenderer FirePowerupIndicator;
    public GameObject enemyRef;
    public GameObject fireBend;
    public GameObject lighting;
    public GameObject glow;
    public GameObject fire;
    public Rigidbody waterRb;
    private Rigidbody playerRb;
    private Rigidbody enemyRb;
    public TrailRenderer airRenderer;
    public float lerpTime = 1.0f;
    public float spawnRate = 2f;
    public float spawnDelayInterval = 3f;
    private float fireSpeed = 90.0f;
    private float powerupTimer = 6.0f;
    public int enemyCount;
    public TextMeshProUGUI playerHP;
    public float range = 6;
    public ParticleSystem dust;
    public Vector3 startPos;
    public GameManager gameManager;
    public bool isPaused = false;
    public GameObject activator;
    public float stotalTime = 1;
    public bool stimeUp = false;
    public bool isDead = false;    
    
    public PlayerInputControls inputs;

  

    void Awake()
    {
        fire.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        playerHealth = 5;
        playerRb = GetComponent<Rigidbody>();
        playerRb.mass = 15;
        enemyRb = enemyRef.GetComponent<Rigidbody>();
        Rigidbody waterRb = WaterbendIndicator.GetComponent<Rigidbody>();
        hasFirePowerup = false;              
    }
    
        

    private void LeftMouseClicked()
    {
        print("LeftMouseClicked");
    }
    public void Start()
    {
        transform.position = startPos;        
    }
    void Update()
    {
      
        PlayerMovement();
        
        if (playerHealth == 0)
        {
            StopAllCoroutines();
        }
        else
        {
            Timer();
            Shoot();
        }
         
        airbendIndicator.transform.Rotate(0, 0, 1, Space.Self);
        WaterbendIndicator.transform.Rotate(0, 1, 0, Space.Self);
        waterRb.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        playerHP.text = "HP: " + playerHealth;
        activator.transform.position = new Vector3(transform.position.x, -0.3f, transform.position.z);    
    }

  
    //player boundary setup
    private void FixedUpdate()
    {
        if (transform.position.z < -29)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z);
        }

       
    }
    //pause mode checker
    public void Pause()
    {
        isPaused = true;
    }

    //resume state checker
    public void Resume()
    {
        isPaused = false;    
    }


    //simple shoot timer
    public void Timer()
    {
        if (stotalTime > 0)
        {
            stotalTime -= Time.deltaTime;
            stimeUp = false;
        }

        else if (stotalTime <= 0)
        {
            stimeUp = true;
            stotalTime = 1;
        }
    }
    //normal attack coroutine setup
    void Shoot()
    {
        if (stimeUp == true)
        {
            Firebend();
        }
    }

    //airbend coroutine setup
    IEnumerator AirPowerupCountdown()
    {
        yield return new WaitForSeconds(powerupTimer);
        hasAirPowerup = false;
        airbendIndicator.SetActive(false);
        gameObject.GetComponent<Rigidbody>().mass = 20;
        airRenderer.enabled = false;
    }
    //waterbend coroutine setup
    IEnumerator WaterPowerupCountdown()
    {
        yield return new WaitForSeconds(powerupTimer - 3);
        hasWaterPowerup = false;
        WaterbendIndicator.SetActive(false);
    }
    //attackup coroutine setup
    IEnumerator FirePowerupCountdown()
    {
        yield return new WaitForSeconds(powerupTimer);
        hasFirePowerup = false;
        fire.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        FirePowerupIndicator.enabled = false;
    }
    //avatarstate coroutine setup
    IEnumerator AvatarStateCountdown()
    {
        yield return new WaitForSeconds(powerupTimer + 2);
        inAvatarState = false;
        lighting.SetActive(true);
        glow.SetActive(false);
        fire.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        spawnDelayInterval = 3;
        fireSpeed = 500;
        spawnRate = 1;
        powerupTimer = 8.0f;
        airbendIndicator.SetActive(false);
    }
    //airbend mode
    public void AirpowerUp()
    {
        hasAirPowerup = true;
        Debug.Log("Air");
        airRenderer.enabled = true;
        gameObject.GetComponent<Rigidbody>().mass = 5;
        StartCoroutine(AirPowerupCountdown());
        airbendIndicator.SetActive(true);
    }

    //hp up by 1 unit
    void WaterpowerUp()
    {
        WaterbendIndicator.SetActive(true);
        hasWaterPowerup = true;
        Debug.Log("Water");
        playerHealth++;
        StartCoroutine(WaterPowerupCountdown());
    }
    //attack up mode
    void AttackPowerUp()
    {
        hasFirePowerup = true;
        Debug.Log("Fire");
        fire.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        spawnRate = 2 * Time.deltaTime;
        FirePowerupIndicator.enabled = true;
        StartCoroutine(FirePowerupCountdown());
    }
    //avatar state mode
    void AvatarState()
    {
        inAvatarState = true;
        Debug.Log("AvatarState");
        lighting.SetActive(false);
        glow.SetActive(true);
        fire.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        spawnDelayInterval = 1f;
        fireSpeed = 2000;
        spawnRate = 3f;
        powerupTimer = 10.0f;
        AirpowerUp();
        StartCoroutine(AvatarStateCountdown());
    }

    //player movement
    void PlayerMovement()
    {
        float verticalInput = inputs.JoystickValue.y;
        float horizontalInput = inputs.JoystickValue.x;

        playerRb.AddForce((Vector3.forward) * verticalInput * speed * (playerRb.mass + 30) * Time.deltaTime);
        playerRb.AddForce((Vector3.right) * horizontalInput * speed * (playerRb.mass + 30) * Time.deltaTime);


        if (verticalInput != 0 || horizontalInput != 0)
        {
            dust.Play();
        }
    }

     //normal attack
    void Firebend()
    {
        if (isPaused == false)
        {
            GameObject fire = Instantiate(fireBend, GameObject.Find("hands").transform.position, fireBend.transform.rotation, gameObject.transform);
            fire.GetComponent<Rigidbody>().AddForce(transform.forward * fireSpeed, ForceMode.Impulse);
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    //picking up powerups
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AirPowerup"))
        {
            AirpowerUp();
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("WaterPowerup"))
        {
            WaterpowerUp();
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("AvatarState"))
        {
            AvatarState();
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Fire Powerup"))
        {
            AttackPowerUp();
            Destroy(other.gameObject);
        }
    }

    //Hp reduction upon damage 
    public void IsAttackedSmall()
    {
        if (playerHealth > 0 && !hasAirPowerup)
        {
            playerHealth--;
        }

        else if (playerHealth == 0)
        {
            {
                isDead = true;                
               // gameObject.SetActive(false);
               Destroy(gameObject);
            }
        }
    }

    void playerHit()
    {
        Vector3 awayFromPlayer = transform.position - enemyRef.transform.position;
        playerRb.AddForce(awayFromPlayer * 40, ForceMode.Impulse);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hasAirPowerup)
        {
            IsAttackedSmall();
            playerHit();
        }
    }




    /* void Inputs()
     {

         if (playerVelocity.z < 0 )
         {
             playerVelocity.z = 0f;
         }

         Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
         controller.Move(move * Time.deltaTime * playerSpeed);

         if (move != Vector3.zero)
         {
             gameObject.transform.forward = move;
         }

         playerVelocity.y += gravityValue * Time.deltaTime;
         controller.Move(playerVelocity * Time.deltaTime);
     }
    */

    

}
