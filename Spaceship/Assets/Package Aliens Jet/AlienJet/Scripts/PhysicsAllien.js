var rotationSpeed : float = 100.0;


function FixedUpdate () {
if (Input.GetKey ("up")) 
	{
    GetComponent.<Rigidbody>().AddRelativeForce (Vector3.forward * 45000);
	}
	
if (Input.GetKey ("down")) 
	{
	GetComponent.<Rigidbody>().AddRelativeForce (Vector3.forward * -30000);
	}
if (Input.GetKey ("left shift")) {
        GetComponent.<Rigidbody>().velocity = Vector3(0,50,0);
    }

if (Input.GetKey ("a")) 
	{
	GetComponent.<Rigidbody>().AddRelativeTorque (0, -500000, 0);
	}

if (Input.GetKey ("d")) 
	{
	GetComponent.<Rigidbody>().AddRelativeTorque (0, 500000, 0);
	}
	
var rotation : float = Input.GetAxis ("Horizontal") * rotationSpeed;
	rotation *= Time.deltaTime;
	transform.Rotate (0, 0, rotation);
	
if ( Vector3.Angle( Vector3.up, transform.up ) < 360) {
			transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.Euler( 0, transform.rotation.eulerAngles.y, 0 ), Time.deltaTime * 1 );
	}
	
if (Input.GetKey ("w")) 
	{
	GetComponent.<Rigidbody>().AddRelativeTorque (-400000, 0, 0);
	}
if (Input.GetKey ("s")) 
	{
	GetComponent.<Rigidbody>().AddRelativeTorque (400000, 0, 0);
	}


}





