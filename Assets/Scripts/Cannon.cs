using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Transform firepoint;
    public GameObject cannonBall;
    public float timeBetween;
    public float startTimeBetween;

    void Start()
    {
        timeBetween = startTimeBetween;
    }

    
    void Update()
    {
        if(timeBetween <0)
        {
            Instantiate(cannonBall, firepoint.position, firepoint.rotation);
            timeBetween = startTimeBetween;
        }
        else
        {
            timeBetween -= Time.deltaTime;
        }
    }
}
