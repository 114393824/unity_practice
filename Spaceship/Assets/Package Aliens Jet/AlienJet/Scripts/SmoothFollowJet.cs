using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowJet : MonoBehaviour {

	public Transform target;
	public float distance = 10.0f;
	public float height = 5.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate(){
		if(!target){
			return;
		}

		float wantedRotationAngle = this.target.eulerAngles.y;
		float wantedHeight = this.target.position.y + this.height;

		float currentRotationAngle = this.transform.eulerAngles.y;
		float currentHeight = this.transform.position.y;

		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle,wantedRotationAngle,Time.deltaTime * this.rotationDamping);
		currentHeight = Mathf.Lerp (currentHeight,wantedHeight,Time.deltaTime * this.heightDamping);

		Quaternion currentRotation = Quaternion.Euler (0,currentRotationAngle,0);

		this.transform.position = this.target.position;
		this.transform.position -= currentRotation * Vector3.forward * this.distance;

		Vector3 _tmp = this.transform.position;
		_tmp.y = currentHeight;

		this.transform.position = _tmp;

		this.transform.LookAt (this.target);
	}
}
