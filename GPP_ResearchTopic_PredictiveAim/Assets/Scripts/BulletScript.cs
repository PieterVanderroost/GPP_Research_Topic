using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Vector3 m_BulletVelocity;
    bool _isInstanciated = false;
	float _currentLifeTime;
	public float _maxLifeTime = 30.0f;
	float _gravity;

	[SerializeField]
	GameObject hudTxt;

    void Start()
    {

    }
    public void InstantiateBullet(Vector3 bulletPosition, float projectileSpeed, Vector3 targetPosition, Vector3 targetVelocity, float gravity)
    {
		//2 Equations to calculate unknown bulletVelocity:

		//1. Length(finalTargetPosition - bulletPosition) = distanceTraveledByProjectile
		//(targetPosition+targetVelocity*t - bulletPosition) = projectileSpeed * t
		//Using Law of Cosines A*A + B*B - 2*A*B*cos(theta) = C*C
		//Solve for t

		//2. finalProjectilePosition = finalTargetPosition
		//(bulletPosition + bulletVelocity*t = targetPosition + targetVelocity * t
		//Solve for bulletVelocity

		//Initialize variables
		_gravity = gravity;
		float targetSpeed = targetVelocity.magnitude;
		Vector3 targetToBullet = bulletPosition - targetPosition;
		float targetToBulletDistance = targetToBullet.magnitude;
		Vector3 targetToBulletDirection = targetToBullet;
		targetToBulletDirection.Normalize();

		////1.////
		//Law of Cosines: 
		//A is Length(targetPosition - bulletPosition)
		//B is Length(targetVelocity * t)
		//C is projectileSpeed * t
		float cosTheta = Vector3.Dot(targetToBulletDirection, targetVelocity.normalized);
		float t;

		//Law of Cosines derived to quadratic formula:a*t² + b*t + c = 0
			float a = Mathf.Pow(projectileSpeed, 2) - Mathf.Pow(targetSpeed,2);
			float b = 2.0f * targetToBulletDistance * targetSpeed * cosTheta;
			float c = -(Mathf.Pow(targetToBulletDistance, 2));
            if (a == 0)
            {
				Debug.Log("No Valid Solution\n");
				_isInstanciated = false;
				return;
			}

			float discriminant = b * b - 4.0f * a * c;

			if (discriminant < 0)
			{
			
				Debug.Log("Target is too fast for bullet speed!\n");
				GameObject UI = GameObject.FindWithTag("CantShootUI");
				UI.GetComponent<WarningHudTxt>().EnableShowText();
				_isInstanciated = false;
				return;
			}
			else
			{
				float sqrtDiscriminant = Mathf.Sqrt(discriminant);
				float t0 = 0.5f * (-b + sqrtDiscriminant) / a;
				float t1 = 0.5f * (-b - sqrtDiscriminant) / a;
			//Assign the lowest positive time to t to aim at the earliest hit
			//t = [ -b + Sqrt( b*b - 4*a*c ) ] / (2*a)
			//t = [ -b - Sqrt( b*b - 4*a*c ) ] / (2*a)
			t = Mathf.Min(t0, t1);
				if (t < 0)
				{
					t = Mathf.Max(t0, t1);
				}

				//Make sure t is positive
				if (t < 0)
				{
					Debug.Log("No Valid Solution, t has to be positive for positive time.\n");
					_isInstanciated = false;
					return;
				}
		}

		////2.////
		//m_BulletVelocity = targetVelocity  + ((targetPosition - bulletPosition) / t);
		m_BulletVelocity = targetVelocity + (-targetToBullet / t);

		//If we have gravity
		//Equation 2 becomes m_BulletVelocity = TargetVelocity - 0.5*GravityVector*t + ((targetPosition - bulletPosition)/t)
		//We basically add reverse gravityVelocity to y, to compensate for the gravity
		if (gravity > 0) 
		{
			float gravityCompensationSpeed = (0.5f * gravity * t * t) / t;
			m_BulletVelocity.y += gravityCompensationSpeed;
		}

		_isInstanciated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isInstanciated == false)
        {
			Destroy(gameObject);
			return;
        }
		m_BulletVelocity += Vector3.down * _gravity * Time.deltaTime;
		transform.position += m_BulletVelocity * Time.deltaTime;

		_currentLifeTime += Time.deltaTime;
        if (_currentLifeTime>=_maxLifeTime)
        {
			Destroy(gameObject);
        }
	
	}
}
