using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject playerPosition;
    public GameObject gameStart;
    private AudioSource audioSource;
    float ZplayerPosition;
    float playerRange;
    bool gameOn;

    
    private void FixedUpdate()
    {//camera moves along when player is accessing portions of the level out of view
        if (gameOn)
        {
            ZplayerPosition = playerPosition.transform.position.z;
            Vector3 normalState = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 anchorState = new Vector3(transform.position.x, transform.position.y, ZplayerPosition);

            if (ZplayerPosition > 7 || ZplayerPosition < -20)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                //transform.position = Vector3.Lerp(transform.position, normalState, 1 * Time.deltaTime);
            }
            else if (ZplayerPosition <= 7 || ZplayerPosition >= -2)
            {
                //transform.position = new Vector3(transform.position.x, transform.position.y, ZplayerPosition);
                transform.position = Vector3.Lerp(normalState, anchorState, 10 * Time.deltaTime);
            }
        }
    }
    //executed when game turns on to find player
    public void Search()
    {
        gameOn = true;
        playerPosition = GameObject.Find("Player");
    }


}
