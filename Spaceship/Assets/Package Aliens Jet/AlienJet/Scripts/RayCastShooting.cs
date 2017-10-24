using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastShooting : MonoBehaviour {

	public float Range = 1000;
	public float Force = 1000;
	public int Clips = 20;
	public int BulletPerClip = 20;
	public float ReloadTime = 3.3f;
	public static int BulletsLeft = 0;
	public float ShootTimer = 0;
	public float ShootCooler = 0.9f;
	public AudioClip ShootAudio;
	public AudioClip ReloadAudio;
	public Transform FireBallPrefab;
	public int Damage = 10;
	public GUIStyle guiStyleBullets;


	// Use this for initialization
	void Start () {
		BulletsLeft = this.BulletPerClip;
	}
	
	// Update is called once per frame
	void Update () {
		if( this.ShootTimer > 0 ){
			this.ShootTimer -= Time.deltaTime;
		}
		if( this.ShootTimer < 0 ){
			this.ShootTimer = 0;
		}

		if( Input.GetKey("e") && BulletsLeft > 0){
			if( this.ShootTimer == 0 ){
				this.PlayShootAudio();
				this.RayShoot();
				this.ShootTimer = ShootCooler;

				Transform fireballs = Instantiate( this.FireBallPrefab, this.transform.position, this.transform.rotation );
				fireballs.GetComponent<Rigidbody> ().AddForce( this.transform.forward * 1000 );
			}
		}

	}

	public void RayShoot(){
		RaycastHit Hit;
		Vector3 DirectionRay = this.transform.TransformDirection (Vector3.forward);
		Debug.DrawRay(this.transform.position , DirectionRay * this.Range , Color.cyan);

		if (Physics.Raycast (this.transform.position , DirectionRay , out Hit , this.Range)) {
			if (Hit.rigidbody) {
				Hit.rigidbody.AddForceAtPosition ( DirectionRay * this.Force , Hit.point);
//				Hit.collider.SendMessageUpwards("ApplyDamage" , this.Damage, SendMessageOptions.DontRequireReceiver);
			}
		}

		BulletsLeft --;
		
		if(BulletsLeft < 0){
			BulletsLeft = 0;
		}
		if(BulletsLeft == 0){
			this.Reload();
		}
	}

	public IEnumerator Reload(){
		this.PlayReloadAudio();

		yield return new WaitForSeconds(this.ReloadTime);
		if (this.Clips > 0){
			BulletsLeft = BulletPerClip;
		}
	}

	public void PlayShootAudio(){
		this.GetComponent<AudioSource> ().PlayOneShot (ShootAudio);
	}

	public void PlayReloadAudio(){
		this.GetComponent<AudioSource> ().PlayOneShot (ReloadAudio);
	}

	void OnGUI(){
//		GUI.Label(Rect(1650,890,100,40), BulletsLeft.ToString(),guiStyleBullets);
	}
}
