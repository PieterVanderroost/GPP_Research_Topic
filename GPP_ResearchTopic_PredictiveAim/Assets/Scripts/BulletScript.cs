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
    public void InstantiateBullet(Vector3 muzzlePosition, float projectileSpeed, Vector3 targetPosition, Vector3 targetVelocity, float gravity)
    {
		//2 Equations to calculate unknown bulletVelocity:

		//1. Length(finalTargetPosition - muzzlePosition) = distanceTraveledByProjectile
		//(targetPosition+targetVelocity*t - muzzlePosition) = projectileSpeed * t
		//Solve for t

		//2. finalProjectilePosition = finalTargetPosition
		//(muzzlePosition + bulletVelocity*t = targetPosition + targetVelocity * t
		//Solve for bulletVelocity
		_gravity = gravity;

		float targetSpeed = targetVelocity.magnitude;
		Vector3 targetToMuzzle = muzzlePosition - targetPosition;
		float targetToMuzzleDistance = targetToMuzzle.magnitude;
		Vector3 targetToMuzzleDirection = targetToMuzzle;
		targetToMuzzleDirection.Normalize();

		//Law of Cosines: A*A + B*B - 2*A*B*cos(theta) = C*C
		//A is distance from muzzle to target targetToMuzzleDistance
		//B is distance traveled by target until impact targetSpeed * t
		//C is distance traveled by projectile until impact projectileSpeed * t
		float cosTheta = Vector3.Dot(targetToMuzzleDirection, targetVelocity.normalized);

		bool validSolutionFound = true;
		float t;
		//If the speed of the target and the projectile are the same
        if (Mathf.Approximately(projectileSpeed, targetSpeed))
        {

            //Law of Cosines: 0.5f * targetToMuzzleDistance / cos(theta) = targetSpeed * t
			//If we have a sharp angle, the bullet can hit the object
            if (cosTheta > 0)
            {
                t = 0.5f * targetToMuzzleDistance / (targetSpeed * cosTheta);
            }
            else
            {
                validSolutionFound = false;
                Debug.Log("No Valid Solution\n");
                _isInstanciated = false;
                return;
            }
        }
        else
        {
			//Quadratic formula:
			//t = [ -b + Sqrt( b*b - 4*a*c ) ] / (2*a)
			//t = [ -b - Sqrt( b*b - 4*a*c ) ] / (2*a)
			float a = Mathf.Pow(projectileSpeed, 2) - Mathf.Pow(targetSpeed,2);
			float b = 2.0f * targetToMuzzleDistance * targetSpeed * cosTheta;
			float c = -(Mathf.Pow(targetToMuzzleDistance, 2));
			float discriminant = b * b - 4.0f * a * c;

			if (discriminant < 0)
			{
			
				validSolutionFound = false;
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
				t = Mathf.Min(t0, t1);
				if (t < 0)
				{
					t = Mathf.Max(t0, t1);
				}

				if (t < 0)
				{
					//Make sure t is positive
					validSolutionFound = false;
					Debug.Log("No Valid Solution, t has to be positive for positive time.\n");
					_isInstanciated = false;
					return;
				}
			}
		}

		//Vb = Vt - 0.5*Ab*t + [(Pti - Pbi) / t]
		m_BulletVelocity = targetVelocity + (-targetToMuzzle / t);
		if (!validSolutionFound)
		{
			m_BulletVelocity = projectileSpeed * m_BulletVelocity.normalized;
		}

		//If we have gravity
		//Equation 2 becomes muzzlePosition + bulletVelocity*t + 0.5*gravity*t*t = targetPosition + targetVelocity * t
		//We basically add reverse gravityVelocity to y, to compensate for the gravity
		if (!Mathf.Approximately(gravity, 0))
		{
			float fallDistance = (t * m_BulletVelocity).y;
			float gravityCompensationSpeed = (fallDistance + 0.5f * gravity * t * t) / t;
			m_BulletVelocity.y = gravityCompensationSpeed;
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
