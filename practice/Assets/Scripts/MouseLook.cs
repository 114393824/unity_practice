﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
	public enum RotationAxes
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2,
	}

	public RotationAxes aexs = RotationAxes.MouseXAndY;
	public float sensitivityHor = 9.0f;
	public float sensitivityVert = 9.0f;

	public float minimumVert = -45.0f;
	public float maxinumVert = 45.0f;

	private float _rotationX = 0;

	// Use this for initialization
	void Start () {
		Rigidbody body = GetComponent<Rigidbody> ();
		if(body != null){
			body.freezeRotation = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (aexs == RotationAxes.MouseX) {
			transform.Rotate (0,sensitivityHor * Input.GetAxis("Mouse X"),0);
		} else if (aexs == RotationAxes.MouseY) {
			_rotationX -= Input.GetAxis ("Mouse Y") * sensitivityVert;
			_rotationX = Mathf.Clamp (_rotationX,minimumVert,maxinumVert);

//			transform.Rotate (Input.GetAxis ("Mouse Y") * sensitivityVert,0,0);
			float rotationY = transform.localEulerAngles.y;
			transform.localEulerAngles = new Vector3 (_rotationX,rotationY,0);
		} else {
			_rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
			_rotationX = Mathf.Clamp (_rotationX,minimumVert,maxinumVert);

			float delta = Input.GetAxis ("Mouse X") * sensitivityHor;
			float rotationY = transform.localEulerAngles.y + delta;

			transform.localEulerAngles = new Vector3 (_rotationX,rotationY,0);
		}
	}
}
