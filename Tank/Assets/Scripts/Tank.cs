using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

	public Transform turret;
	public Transform gun;
	public Transform wheels;
	public Transform tracks;

	private float turretRotSpeed = 0.5f;
	private float turretRotTarget;

	private float maxRoll = 10f;
	private float minRoll = -4f;
	private float turretRollTarget = 0;

	public List<AxleInfo> axleInfos;

	private float motor = 0;
	public float maxMotorTorque;

	private float brakeTorque = 0;
	public float maxBrakeTorque = 100;

	private float steering = 0;
	public float maxSteeringAngle;

	public AudioSource motorAudioSource;
	public AudioClip motorClip;

	public GameObject bullet;

	public float lastShootTime = 0;
	private float shootInterval = 0.5f;

	public enum CtrlType
	{
		none,
		player,
		computer
	}
	public CtrlType ctrlType = CtrlType.player;

	private float maxHp = 100;
	public float hp = 100;

	public GameObject destroyEffect;

	public Texture2D centerSight;
	public Texture2D tankSight;

	public Texture2D hpBarBg;
	public Texture2D hpBar;

	public Texture2D killUI;

	private float killUIStartTime = float.MinValue;

	public AudioSource shootAudioSource;
	public AudioClip shootClip;

	private AI ai;

	// Use this for initialization
	void Start () {
		this.turret = transform.Find ("turret");
		this.gun = this.turret.Find ("gun");
		this.wheels = transform.Find("wheels");
		this.tracks = transform.Find("tracks");

		this.motorAudioSource = gameObject.AddComponent<AudioSource> ();
		this.motorAudioSource.spatialBlend = 1;

		this.shootAudioSource = gameObject.AddComponent<AudioSource> ();
		this.shootAudioSource.spatialBlend = 1;

		if(this.ctrlType == CtrlType.computer){
			this.ai = gameObject.AddComponent<AI> ();
			this.ai.tank = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
//		float speed = 1;
//
//		if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
//			transform.eulerAngles = new Vector3(0,0,0);
//			transform.position += transform.forward * speed;
//		}else if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
//			transform.eulerAngles = new Vector3(0,180,0);
//			transform.position += transform.forward * speed;
//		}else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
//			transform.eulerAngles = new Vector3(0,270,0);
//			transform.position += transform.forward * speed;
//		}else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
//			transform.eulerAngles = new Vector3(0,90,0);
//			transform.position += transform.forward * speed;
//		}
		PlayerCtrl ();
		this.ComputerCtrl ();
		this.NoneCtrl ();

		foreach(AxleInfo axleInfo in axleInfos){
			if(axleInfo.steering){
				axleInfo.leftWheel.steerAngle = this.steering;
				axleInfo.rightWheel.steerAngle = this.steering;
			}
			if(axleInfo.motor){
				axleInfo.leftWheel.motorTorque = this.motor;
				axleInfo.rightWheel.motorTorque = this.motor;
			}
			if(true){
				axleInfo.leftWheel.brakeTorque = this.brakeTorque;
				axleInfo.rightWheel.brakeTorque = this.brakeTorque;
			}
			if (axleInfos[1] != null && axleInfo == axleInfos[1])
			{
				WheelsRotation(axleInfos[1].leftWheel);
				TrackMove();
			}
		}

		TurretRotation ();
		TurretRoll ();

		MotorSound();

	}

	public void Shoot(){
		if(Time.time - this.lastShootTime < this.shootInterval){
			return;
		}
		if (this.bullet == null)
			return;

		Vector3 pos = this.gun.position + this.gun.forward * 5;
		GameObject bulletObj = Instantiate (this.bullet,pos,gun.rotation);

		Bullet bulletCmp = bulletObj.GetComponent<Bullet> ();
		if (bulletCmp != null)
			bulletCmp.attackTank = this.gameObject;

		this.lastShootTime = Time.time;

		this.shootAudioSource.PlayOneShot (this.shootClip);

//		this.BeAttacked (30);
	}

	public void PlayerCtrl(){
		if (this.ctrlType != CtrlType.player)
			return;
		
		this.motor = this.maxMotorTorque * Input.GetAxis ("Vertical");
		this.steering = this.maxSteeringAngle * Input.GetAxis ("Horizontal");

		this.brakeTorque = 0;
		foreach(AxleInfo axleInfo in axleInfos){
			if(axleInfo.leftWheel.rpm > 5 && this.motor < 0){//前进时
				this.brakeTorque = this.maxBrakeTorque;
			}else if(axleInfo.leftWheel.rpm < -5 && this.motor > 0){
				this.brakeTorque = this.maxBrakeTorque;
			}
			continue;
		}


//		this.turretRotTarget = Camera.main.transform.eulerAngles.y;
//		this.turretRollTarget = Camera.main.transform.eulerAngles.x;

		this.TargetSignPos ();

		if (Input.GetMouseButton(0))
			Shoot();
	}

	public void ComputerCtrl(){
		if(this.ctrlType != CtrlType.computer){
			return;
		}
		Vector3 rot = ai.GetTurretTarget ();
		this.turretRotTarget = rot.y;
		this.turretRollTarget = rot.x;

		if(this.ai.IsShoot()){
			this.Shoot ();
		}

		//移动
		this.steering = ai.GetSteering();
		this.motor = ai.GetMotor();
		this.brakeTorque = ai.GetBrakeTorque();
	}

	public void NoneCtrl(){
		if(this.ctrlType != CtrlType.none){
			return;
		}

		this.motor = 0;
		this.steering = 0;
		this.brakeTorque = this.maxBrakeTorque / 2;
	}

	public void TurretRotation(){
		if (Camera.main == null)
			return;
		if (this.turret == null)
			return;
		
		float angle = this.turret.transform.eulerAngles.y - this.turretRotTarget;
		if (angle < 0)
			angle += 360;
		if (angle > this.turretRotSpeed && angle < 180) {
			this.turret.Rotate (0,-this.turretRotSpeed,0);
		}else if(angle > 180 && angle < 360 - this.turretRotSpeed){
			this.turret.Rotate (0,this.turretRotSpeed,0);
		}
	}

	public void TurretRoll(){
		if (Camera.main == null)
			return;
		if (this.turret == null)
			return;
		Vector3 worldEuler = gun.eulerAngles;
		Vector3 localEuler = gun.localEulerAngles;

		worldEuler.x = this.turretRollTarget;
		gun.eulerAngles = worldEuler;

		Vector3 euler = gun.localEulerAngles;
		if (euler.x > 180)
			euler.x -= 360;

		if (euler.x > maxRoll)
			euler.x = maxRoll;
		if (euler.x < minRoll)
			euler.x = minRoll;

		gun.localEulerAngles = new Vector3 (euler.x,localEuler.y,localEuler.z);

	}

	public void WheelsRotation(WheelCollider collider){
		if (this.wheels == null)
			return;

		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose (out position,out rotation);

		foreach(Transform wheel in this.wheels){
			wheel.rotation = rotation;
		}
	}

	public void TrackMove(){
		if (tracks == null)
			return;
		float offset = 0;
		if (wheels.GetChild (0) != null)
			offset = wheels.GetChild (0).localEulerAngles.x / 90f;
		foreach(Transform track in tracks){
			MeshRenderer mr = track.gameObject.GetComponent<MeshRenderer> ();
			if (mr == null)
				continue;
			Material mtl = mr.material;
			mtl.mainTextureOffset = new Vector2 (0,offset);
		}
	}

	public void MotorSound(){
		if(this.motor != 0 && !motorAudioSource.isPlaying){
			this.motorAudioSource.loop = true;
			this.motorAudioSource.clip = this.motorClip;
			this.motorAudioSource.Play();
		}else if(this.motor == 0){
			this.motorAudioSource.Pause ();
		}
	}

	public void BeAttacked(float att,GameObject attackTank){
		if(this.hp <= 0)
			return;
		if (this.hp > 0) {
			this.hp -= att;
			if(this.ai != null){
				ai.OnAttacked(attackTank);
			}
		}
		if(this.hp <= 0){
			GameObject destoryObj = (GameObject)Instantiate (this.destroyEffect);
			destoryObj.transform.SetParent (transform,false);
			destoryObj.transform.localPosition = Vector3.zero;
			this.ctrlType = CtrlType.none;

			if(attackTank != null){
				Tank tankCmp = attackTank.GetComponent<Tank> ();
				if (tankCmp != null && tankCmp.ctrlType == CtrlType.player)
					tankCmp.StartDrawKill ();
			}
		}
	}

	public void TargetSignPos(){
		Vector3 hitPoint = Vector3.zero;
		RaycastHit raycastHit;

		Vector3 centerVec = new Vector3 (Screen.width / 2,Screen.height / 2,0);
		Ray ray = Camera.main.ScreenPointToRay (centerVec);

		if (Physics.Raycast (ray, out raycastHit, 400.0f)) {
			hitPoint = raycastHit.point;
		} else {
			hitPoint = ray.GetPoint (400);
		}

		Vector3 dir = hitPoint - turret.position;
		Quaternion angle = Quaternion.LookRotation (dir);

		this.turretRotTarget = angle.eulerAngles.y;
		this.turretRollTarget = angle.eulerAngles.x;

	}

	public Vector3 CalExplodePoint(){
		Vector3 hitPoint = Vector3.zero;
		RaycastHit hit;

		Vector3 pos = this.gun.position + gun.forward * 5;
		Ray ray = new Ray (pos,gun.forward);

		if (Physics.Raycast (ray, out hit, 400.0f)) {
			hitPoint = hit.point;	
		} else {
			hitPoint = ray.GetPoint (400);
		}

		return hitPoint;
	}

	public void DrawSight(){
		Vector3 explodePoint = CalExplodePoint ();
		Vector3 screenPoint = Camera.main.WorldToScreenPoint (explodePoint);

		Rect tankRect = new Rect(screenPoint.x - tankSight.width / 2,Screen.height - screenPoint.y - tankSight.height / 2,tankSight.width,tankSight.height);
		GUI.DrawTexture (tankRect,tankSight);

		Rect centerRect = new Rect (Screen.width / 2 - centerSight.width / 2,Screen.height / 2 - centerSight.height / 2,centerSight.width,centerSight.height);
	
		GUI.DrawTexture (centerRect,centerSight);
	}

	public void DrawHp(){
		Rect bgRect = new Rect (30,Screen.height - hpBarBg.height - 15,hpBarBg.width,hpBarBg.height);
		GUI.DrawTexture (bgRect,hpBarBg);

		float width = hp * 102 / maxHp;

		Rect hpRect = new Rect (bgRect.x + 29,bgRect.y + 9,width,hpBar.height);
		GUI.DrawTexture (hpRect,hpBar);

		string text = Mathf.Ceil (hp).ToString () + "/" + Mathf.Ceil(maxHp).ToString();

		Rect textRect = new Rect (bgRect.x + 80,bgRect.y - 10,50,50);
		GUI.Label (textRect,text);
	}

	public void StartDrawKill(){
		this.killUIStartTime = Time.time;
	}

	private void DrawKillUI(){
		if(Time.time - this.killUIStartTime < 1f){
			Rect rect = new Rect (Screen.width / 2 - killUI.width / 2,30,killUI.width,killUI.height);
			GUI.DrawTexture (rect,killUI);
		}
	}

	void OnGUI(){
		if (ctrlType != CtrlType.player)
			return;
		DrawSight ();
		DrawHp ();
		DrawKillUI ();
	}
}
