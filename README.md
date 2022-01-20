# GPP_Research_Topic
Predictive aim for AI targeting.

## 1. Description of the topic

The goal of this research topic is to get familiar with predictive aim for an AI. 
If an object with a given velocity is moving in the world, how would the AI have to aim to guarantee a hit with an instantiated bullet taking into account gravity.

## 2. Design/implementation
Assuming zero gravity acceleration at first: what we want to calculate is the velocity vector of the bullet at which point it will intersect with the moving target.
This gives us our first equation with unknown t : Length(finalTargetPosition - initialBulletPosition) = distanceTraveledByProjectile. Or: 
**Equation 1: Length(InitialTargetPosition + targetVelocity * t - InitialBulletPosition) = BulletSpeed * t**

Our second equation will use this calculated t to find the unknown bullet velocity vector : finalBulletPosition = finalTargetPosition. Or:
**Equation 2: InitialBulletPosition + BulletVelocity * t = InitialTargetPosition + TargetVelocity * t**

To find t in Equation 1 we first use the law of cosines as below:
![Law Of Cosines](https://user-images.githubusercontent.com/97388368/150339295-f258d3ff-bb60-4245-9134-29a1c3c18691.jpg)

**The law of cosine according to our problem:**
Length(InitialTargetPosition - InitialBulletPosition)² + Length(targetVelocity\*t)² - 2\*Length(InitialTargetPosition - InitialBulletPosition)\*Length(TargetVelocity\*t)\*cos(theta) = (BulletSpeed \* t)²
 
 
 **putting this equation in the form of a quadratic formula** a\***t²** + b\***t** + c = 0 gives us 
 
 (BulletSpeed² - TargetSpeed²)\***t²** + 2\*Length(InitialTargetPosition - InitialBulletPosition)\*TargetSpeed\*cos(theta)\***t** - (Length(InitialTargetPosition - InitialBulletPosition))² = 0
 
 t0 = (-b + Sqrt(b² - 4\*a\*c))/(2\*a)
 
 t1 = (-b - Sqrt(b² - 4\*a\*c))/(2\*a)
 
 We now have 2 values for t: if both are negative there is no valid solution (there is no way of hitting the target in the future, if we would have shot sooner we would have been able to hit it; but alas the past is the past!)
 if both t values are positive, we go with the smallest one: the sooner we hit the better!
 
 Now that we know our t value, let's plug it in equation 2 and derive it to find the unknown BulletVelocity:
 BulletVelocity = TargetVelocity + ((InitialTargetPosition - InitialBulletPosition)/t)
 We now have a solution for the bulletVelocity **But hold up! we assumed zero gravity in this solution**, what about accounting for the gravity?
 We still want to use the calculated t, but we add the reverse gravity to the bulletVelocity. **Our Previous Equation 2 becomes**
 
 **InitialBulletPosition + BulletVelocity \*t + 0.5\*GravityVector\*t² = InitialTargetPosition + TargetVelocity * t**
 or reworked to find the final bullet velocity:
 **BulletVelocity = TargetVelocity - 0.5\*GravityVector\*t + ((InitialTargetPosition - InitialBulletPosition)/t)**
 
 ## 3. Result
 ![GPP_Research](https://user-images.githubusercontent.com/97388368/150355400-4d7f934c-8ee1-45d4-8ba8-5f56ea35c964.gif)

 
 ## 4. Conclusion
 While the solution to this problem is not purely mathematically correct since we don't account for gravity for finding our unknown t initially, I still think this is an elegant solution to implement into a fake game world. While adding the reverse gravity velocity to the bulletvelocity in the end instead of just pure direction does indeed increase the force the bullet leaves the muzzle (which isn't physically correct), it still works out to let the bullet hit the target over a distance that looks and feels correct to the player.
