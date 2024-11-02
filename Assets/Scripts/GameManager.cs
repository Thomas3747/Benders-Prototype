using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{

    public GameObject startUi;
    public GameObject pauseUI;
    public GameObject inGameUI;
    public GameObject startGame;
    public ParticleSystem explosiveParticle;
    public TextMeshProUGUI scoreText;
   // public TextMesh scoreText;
    public GameObject gameoverText;
    public GameObject HUD;
    public GameObject playerRef;
    private GameObject Enemy;
    public int score;
    public bool isGameActive;
    public bool playerAlive = false;

    private void Awake()
    {
        HUD.SetActive(true);
        startGame.SetActive(false);
        pauseUI.gameObject.SetActive(false);
        inGameUI.gameObject.SetActive(false);

    }

    void Start()
    {
        score = 0;
        startUi.gameObject.SetActive(true);
        playerRef.GetComponent<PlayerController>();
    }
    //enemy death particle spawn
    public void Splatter(Vector3 here)
    {
        Instantiate(explosiveParticle, here, explosiveParticle.transform.rotation);
    }
    //game score updater
    public void UpdateScore()
    {
        int scorer = 1;
        score += scorer;
        scoreText.text = "Score: " + score;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerRef.GetComponent<PlayerController>().playerHealth == 0)
        {
            GameOver();
        }
    }
    //gameover tracker
    public void GameOver()
    {
        gameoverText.gameObject.SetActive(true);        
        isGameActive = false;
        CancelInvoke();
    }
    //game restart setup
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //gamestart tracker
    public void StartGame()
    {
        isGameActive = true;
        score = 0;
        playerRef.GetComponent<PlayerController>().startPos = new Vector3(0.6f, 1.6f, -25f);
    }

}
