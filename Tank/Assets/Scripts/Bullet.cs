using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 100f;
	public GameObject explode;
	public float maxLiftTime = 2f;
	public float instantiateTime = 0f;

	public GameObject attackTank;

	public AudioClip explodeClip;

	// Use this for initialization
	void Start () {
		this.instantiateTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * speed * Time.deltaTime;

		if (Time.time - this.instantiateTime > this.maxLiftTime)
			Destroy (gameObject);
	}

	void OnCollisionEnter(Collision collisionInfo){
		if (collisionInfo.gameObject == attackTank)
			return;
		
		GameObject explodeObj = Instantiate (this.explode,transform.position,transform.rotation);
		AudioSource audioSource = explodeObj.AddComponent<AudioSource> ();
		audioSource.spatialBlend = 1;
		audioSource.PlayOneShot (explodeClip);

		Destroy (gameObject);

		Tank tank = collisionInfo.gameObject.GetComponent<Tank> ();
		if(tank != null){
			float att = GetAtt ();
			tank.BeAttacked (att,attackTank);
		}
	}

	private float GetAtt(){
		float att = 100 - (Time.time - this.instantiateTime) * 40;
		if (att < 1)
			att = 1;
		return att;
	}
}
