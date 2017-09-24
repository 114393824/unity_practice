using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {
	public float speed = 10.0f;
	public int damage = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0,0,speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		PlayerController player = other.GetComponent<PlayerController> ();
		if (player != null){
//			Debug.Log ("Player hit");
			player.Hurt(1);
		}
		Destroy (this.gameObject);
	}
}
