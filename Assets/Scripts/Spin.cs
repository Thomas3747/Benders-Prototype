using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{

    public float totalTime = 10;
    public bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (isPaused == false)
        {
            transform.Rotate(0, 0, 1, Space.Self);
            Timer();
        }
    }

    //timer to self destruct after 10 seconds
    void Timer()
    {
        if (totalTime > 0)
        {
            //Subtract elapsed time every frame
            totalTime -= Time.deltaTime;
        }

        else
        {
            Destroy(gameObject);
        }
    }
    //Pause state checker
    public void Pause()
    {
        isPaused = true;
    }

    //resume state checker
    public void Resume()
    {
        isPaused = false;
    }
}
