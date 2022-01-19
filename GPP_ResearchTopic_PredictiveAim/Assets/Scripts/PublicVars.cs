using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicVars : MonoBehaviour
{
    public float targetSpeed = 10.0f;
    public float gravity = 0.981f;
    public float bulletSpeed = 30.0f;
    public float targetDistance = 75.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeBulletSpeed(float speed)
    {
        bulletSpeed = speed;
    }
    public void ChangeGravity(float grav)
    {
        gravity = grav;
    }
    public void ChangeTargetSpeed(float tSpeed)
    {
        targetSpeed = tSpeed;
    }
    public void ChangeTargetDistance(float distance)
    {
        targetDistance = distance;
    }
}
