using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesJetMovingScript : MonoBehaviour {

	public float emissionTime = 3.0f;
	public float emissionDelay = 3.0f;
	public float lastTime = 0;

	public static bool networkdestroyparticleemiterAlien = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("up")){
			this.GetComponent<ParticleAnimator> ().autodestruct = false;
			this.GetComponent<ParticleEmitter> ().emit = true;
			this.lastTime = Time.time;
		}
		if(this.GetComponent<ParticleEmitter> ().emit && (Time.time - this.lastTime) > this.emissionTime){
			this.GetComponent<ParticleEmitter> ().emit = false;
		}
		if(networkdestroyparticleemiterAlien){
			Destroy(this);
		}
	}
}
