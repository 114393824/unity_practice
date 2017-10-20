var emissionTime : float = 3.0;
var emissionDelay : float = 3.0;
var lastTime = 0.0;
static var networkdestroyparticleemiterAlien = false;
function Update()
{
 if (Input.GetKey ("up"))
 {
   GetComponent(ParticleAnimator).autodestruct = false;
   GetComponent.<ParticleEmitter>().emit = true;
   lastTime = Time.time;

 }
 if (GetComponent.<ParticleEmitter>().emit && (Time.time - lastTime) > emissionTime)
 {
    GetComponent.<ParticleEmitter>().emit = false;
 }
 if(networkdestroyparticleemiterAlien)
 {
 Destroy (this);
 }

}