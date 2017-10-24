using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsAllien : MonoBehaviour {

	public float rotationSpeed = -50.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		if(Input.GetKey("up")){
			this.GetComponent<Rigidbody> ().AddRelativeForce ( Vector3.forward * 45000 );
		}
		if(Input.GetKey("down")){
			this.GetComponent<Rigidbody> ().AddRelativeForce ( Vector3.forward * -30000 );
		}
		if(Input.GetKey("a")){
			this.GetComponent<Rigidbody> ().AddRelativeTorque ( 0, -500000, 0 );
		}
		if(Input.GetKey("d")){
			this.GetComponent<Rigidbody> ().AddRelativeTorque ( 0, 500000, 0 );
		}
		if(Input.GetKey("w")){
			this.GetComponent<Rigidbody> ().AddRelativeTorque ( -400000, 0, 0 );
		}
		if(Input.GetKey("s")){
			this.GetComponent<Rigidbody> ().AddRelativeTorque ( 400000, 0, 0 );
		}
		if(Input.GetKey("left shift")){
			Vector3 _temp = this.GetComponent<Rigidbody> ().velocity;
			_temp.y = 50;
		}

		float rotation = Input.GetAxis("Horizontal") * this.rotationSpeed * Time.deltaTime;
		this.transform.Rotate (0, 0, rotation);

		if( Vector3.Angle(Vector3.up,this.transform.up ) < 360){
			this.transform.rotation = Quaternion.Slerp( this.transform.rotation, Quaternion.Euler( 0, this.transform.rotation.eulerAngles.y, 0 ), Time.deltaTime * 1 );
		}
	}
}
