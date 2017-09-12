using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour {
	public float speed = 6.0f;
	private CharacterController _charController;
	public float gravity = -9.8f;
	// Use this for initialization
	void Start () {
		_charController = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		float deltaX = Input.GetAxis("Horizontal") * speed;
		float deltaY = Input.GetAxis ("Vertical") * speed;

		Vector3 movement = new Vector3 (deltaX,0,deltaY);
		movement = Vector3.ClampMagnitude (movement,speed);
		movement.y = gravity;

		movement *= Time.deltaTime;
		movement = transform.TransformDirection (movement);

		_charController.Move (movement);

//		transform.Translate (deltaX * Time.deltaTime,0,deltaY * Time.deltaTime);
	}
}
