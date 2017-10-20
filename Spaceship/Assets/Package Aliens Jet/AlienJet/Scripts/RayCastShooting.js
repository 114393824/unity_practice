var Range : float = 1000;
var Force : float = 1000;
var Clips : int = 20;
var BulletPerClip : int = 60;
var ReloadTime : float  = 3.3;
static var BulletsLeft : int = 0;
var ShootTimer : float = 0;
var ShootCooler : float = 0.9;
public var ShootAudio : AudioClip;
public var ReloadAudio : AudioClip;
var FireBallPrefab : Transform; 
var Damage : int = 10;
var guiStyleBullets : GUIStyle; 


function Start () {

BulletsLeft = BulletPerClip;
}


function Update () {
	

	if( ShootTimer > 0)
	{
	ShootTimer -= Time.deltaTime;
	}

	if( ShootTimer < 0)
	{
	ShootTimer=0;
	}

if(Input.GetKey ("e") && BulletsLeft){
		if ( ShootTimer == 0)
		{
		PlayShootAudio ();
		RayShoot();
		ShootTimer = ShootCooler;
		var fireballs = Instantiate(FireBallPrefab, transform.position, transform.rotation);		
		fireballs.GetComponent.<Rigidbody>().AddForce(transform.forward * 1000);
	
	
		}
	}
	
}

function RayShoot ()
{
		var Hit : RaycastHit;
		
			var DirectionRay = transform.TransformDirection (Vector3.forward);
			
		Debug.DrawRay(transform.position , DirectionRay * Range , Color.cyan);
		if (Physics. Raycast (transform.position , DirectionRay , Hit , Range)) 
		{
			if (Hit.rigidbody) {
			
			Hit.rigidbody.AddForceAtPosition ( DirectionRay * Force , Hit.point);
			Hit.collider.SendMessageUpwards("ApplyDamage" , Damage, SendMessageOptions.DontRequireReceiver);
			 
			}
				
		}
		
		BulletsLeft --;
		
		if(BulletsLeft < 0)
		{
		BulletsLeft = 0;
		}
			if(BulletsLeft == 0)
			{
			Reload();
			}
		
		
}

function Reload()
{
PlayReloadAudio();

	yield WaitForSeconds (ReloadTime) ; 
		if (Clips > 0)
			{
			BulletsLeft = BulletPerClip;
			}
	
}

function PlayShootAudio()
{

		GetComponent.<AudioSource>().PlayOneShot (ShootAudio);
}

function PlayReloadAudio()
{

	GetComponent.<AudioSource>().PlayOneShot (ReloadAudio);
	
}

function OnGUI ()
{
	
	GUI.Label(Rect(1650,890,100,40), BulletsLeft.ToString(),guiStyleBullets);
	
}



