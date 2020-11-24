//using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    private float distanceMax = 205f;
    public GameObject bulletFactory;
    //public ParticleSystem bulletFx;
    float lastAtkTime = 0f;
    public float atkTime = 1f;

    public Camera cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        if(Input.GetButtonDown("Fire1") && Time.time > lastAtkTime+atkTime)
        {
          Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward,out hit,distanceMax))
        {
            hitPosition = hit.point;
            GameObject bullet = Instantiate(bulletFactory);
            bullet.transform.position = hitPosition;
            lastAtkTime = Time.time;

        }

    }
}
