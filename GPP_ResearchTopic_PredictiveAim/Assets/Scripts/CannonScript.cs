using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    [SerializeField]
    GameObject _bulletPrefab;
    [SerializeField]
    PublicVars publicVars;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float gravity = publicVars.gravity;
        float bulletSpeed = publicVars.bulletSpeed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Fire!\n");
            GameObject target = GameObject.FindGameObjectWithTag("Target");
            PlayerMovement playerScript = target.GetComponent<PlayerMovement>();

            GameObject bullet = Instantiate(_bulletPrefab,transform.position,transform.rotation);
           BulletScript bulletScript  = bullet.GetComponent<BulletScript>();
            bulletScript.InstantiateBullet(transform.position,bulletSpeed,target.transform.position,playerScript.GetVelocity(),gravity);
        }
    }
}
