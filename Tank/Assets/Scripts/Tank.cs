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

		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
			transform.eulerAngles = new Vector3(0,0,0);
			transform.position += transform.forward * speed;
		}else if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
			transform.eulerAngles = new Vector3(0,180,0);
			transform.position += transform.forward * speed;
		}else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
			transform.eulerAngles = new Vector3(0,270,0);
			transform.position += transform.forward * speed;
		}else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
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
