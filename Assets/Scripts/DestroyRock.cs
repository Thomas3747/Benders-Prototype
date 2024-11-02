using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyRock : MonoBehaviour
{
    public GameObject enemies;
    private float rockSpeed = 30.0f;


    void Update()
    {   //destroy rock when it leaves the game area
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * rockSpeed, ForceMode.Impulse);
        if (transform.position.z < -31 || transform.position.z > 32)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    { //destroy rock when it hits anything except the player
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }
}
