using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  //  Rigidbody _rigidBody;
    Vector3 _targetVelocity;
    [SerializeField]
    PublicVars publicVars;
    float _movementSpeed;
    [SerializeField]
    bool _turnAround = true;
    [SerializeField]
    float _turnDistance = 300.0f;
    [SerializeField]
    private GameObject _hitVFX;
    // Start is called before the first frame update
    void Start()
    {
      //  _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
       // _rigidBody.velocity += _targetVelocity;
       // Debug.Log(_targetVelocity);
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, publicVars.targetDistance);
        _movementSpeed = publicVars.targetSpeed;
        if (_turnAround)
        {
            if (transform.position.x >= _turnDistance && _movementSpeed > 0)
            {
                publicVars.targetSpeed = -publicVars.targetSpeed;
            }
            else if (transform.position.x <= -_turnDistance && _movementSpeed < 0)
            {
                publicVars.targetSpeed = -publicVars.targetSpeed;
            }
        }

        _targetVelocity = transform.right * _movementSpeed;
        transform.position += _targetVelocity * Time.deltaTime;
    }
    public Vector3 GetVelocity()
    {
        return _targetVelocity;
    }

    public void ChangeTurnAround(bool toggle)
    {
        _turnAround = toggle;
    }
    public void ChangeTurnDistance(float turnDist)
    {
        _turnDistance = turnDist;
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
        if (other.CompareTag("Bullet"))
        {
            Instantiate(_hitVFX, transform.position, transform.rotation);
        }
    }
}
