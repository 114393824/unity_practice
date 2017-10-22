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

	// Use this for initialization
	void Start () {
		this.turret = transform.Find ("turret");
		this.gun = this.turret.Find ("gun");
		this.wheels = transform.Find("wheels");
		this.tracks = transform.Find("tracks");

		this.motorAudioSource = gameObject.AddComponent<AudioSource> ();
		this.motorAudioSource.spatialBlend = 1;
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

		TurretRotation ();
		TurretRoll ();

		MotorSound();
	}

	public void PlayerCtrl(){
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

		this.turretRotTarget = Camera.main.transform.eulerAngles.y;
		this.turretRollTarget = Camera.main.transform.eulerAngles.x;
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
}
