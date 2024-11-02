using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendingActivator : MonoBehaviour
{

    float rockUp;
    float rockDown;
    float rockUpTime = 100.0f;
    public bool rockActive = false;
    Vector3 spawnPosition;
    //float spawnRate = 1.5f;
    // public GameObject rocks;
    // private Rigidbody rockForce;

    // Start is called before the first frame update
    void Start()
    {
        //Ground position for earth/rock shields
        rockDown = transform.position.y - 1.6f;
        rockUp = 0.6f;
        Vector3 downStateRock = new Vector3(transform.position.x, rockDown, transform.position.z);
        transform.position = downStateRock;
    }

    //activating shield on collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Activator"))
        {
            RockActive();
        }
    }

    //changing rock z position to move them up to serve as shields
    public void RockActive()
    {
        Vector3 downStateRock = new Vector3(transform.position.x, rockDown, transform.position.z);
        Vector3 upStateRock = new Vector3(transform.position.x, rockUp, transform.position.z);

        rockActive = true;
        transform.position = Vector3.Lerp(downStateRock, upStateRock, rockUpTime);
    }

    //reseting rock position when player moves away
    private void OnCollisionExit(Collision collision)
    {
        Vector3 downStateRock = new Vector3(transform.position.x, rockDown, transform.position.z);
        Vector3 upStateRock = new Vector3(transform.position.x, rockUp, transform.position.z);

        if (collision.gameObject.CompareTag("Activator"))
        {
            rockActive = false;
            transform.position = Vector3.Lerp(upStateRock, downStateRock, rockUpTime);
        }
    }
    //earthbend when rock is active for a while
    /*  IEnumerator EarthBend()
      {
          while (rockActive)
          {
              yield return new WaitForSeconds(spawnRate);
              Instantiate(rocks, spawnPosition, transform.rotation);
          }
      }*/


}
