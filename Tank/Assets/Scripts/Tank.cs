using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

	public Transform turret;
	public Transform gun;
	public Transform wheels;
	public Transform tracks;

	// Use this for initialization
	void Start () {
		// turret = transform.Find("turret")
		// gun = transform.Find("gun")
		// wheels = transform.Find("wheels")
		// tracks = transform.Find("tracks")
		
	}
	
	// Update is called once per frame
	void Update () {
		float speed = 1;
		Debug.Log("Update ********** 1");
		if(Input.GetKey(KeyCode.UpArrow)){
			Debug.Log("Update ********** UpArrow");
			transform.eulerAngles = new Vector3(0,0,0);
			transform.position += transform.forward * speed;
		}else if(Input.GetKey(KeyCode.DownArrow)){
			Debug.Log("Update ********** DownArrow");
			transform.eulerAngles = new Vector3(0,180,0);
			transform.position += transform.forward * speed;
		}else if(Input.GetKey(KeyCode.LeftArrow)){
			Debug.Log("Update ********** LeftArrow");
			transform.eulerAngles = new Vector3(0,270,0);
			transform.position += transform.forward * speed;
		}else if(Input.GetKey(KeyCode.RightArrow)){
			Debug.Log("Update ********** RightArrow");
			transform.eulerAngles = new Vector3(0,90,0);
			transform.position += transform.forward * speed;
		}
	}

	public void TurretRotation(){

	}

	public void TurretRoll(){

	}

	public void WheelsRotation(WheelCollider collider){

	}

	public void TrackMove(){

	}
}
