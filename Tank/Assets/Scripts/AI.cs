using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {

	public Tank tank;
	public enum Status
	{
		Patrol,
		Attack,
	}
	private Status status = Status.Patrol;

	private GameObject target;
	private float sightDistance = 30;
	private float lastSearchTargetTime = 0;
	private float searchTargetInterval = 3;

	private float lastUpdateWaypointTime = float.MinValue;
	private float updateWaypointInterval = 10;

	private Path path = new Path();

	void InitWayPoint(){
		GameObject obj = GameObject.Find ("WaypointContainer");
		if (obj && obj.transform.GetChild(0) != null) {
			Vector3 targetPos = obj.transform.GetChild (0).position;
			path.InitByNavMeshPath (this.transform.position,targetPos);
		}
			
	}

	public void ChangeStatus(Status status){
		if(status == Status.Patrol){
			this.PatrolStart ();
		}else if(status == Status.Attack){
			this.AttackStart ();
		}
	}

	void TargetUpdate(){
		float interval = Time.time - this.lastSearchTargetTime;
		if (interval < this.searchTargetInterval)
			return;
		this.lastSearchTargetTime = Time.time;

		if (target != null) {
			this.HasTarget ();		
		} else {
			this.NoTarget ();
		}
	}

	void HasTarget(){
		Tank targetTank = this.target.GetComponent<Tank>();
		Vector3 pos = this.transform.position;
		Vector3 targetPos = this.target.transform.position;

		if(targetTank.ctrlType == Tank.CtrlType.none){
			Debug.Log ("目标死亡，丢失目标");
			this.target = null;
		}else if(Vector3.Distance(pos,targetPos) > this.sightDistance){
			Debug.Log ("距离过远，丢失目标");
			this.target = null;
		}

	}

	void NoTarget(){
		float minHp = float.MaxValue;
		GameObject[] targets = GameObject.FindGameObjectsWithTag ("TankTag");
		for(int i = 0; i < targets.Length; i++){
			Tank tank = targets[i].GetComponent<Tank>();
			if (tank == null)
				continue;
			if (tank == this.gameObject)
				continue;
			if (tank.ctrlType == Tank.CtrlType.none)
				continue;
			Vector3 pos = this.transform.position;
			Vector3 targetPos = targets[i].transform.position;
			if (Vector3.Distance (pos, targetPos) > this.sightDistance)
				continue;
			if (minHp > tank.hp)
				this.target = tank.gameObject;
		}
		if (this.target != null)
			Debug.Log ("获取目标 " + this.target.name);
	}

	public void OnAttacked(GameObject attackTank){
		this.target = attackTank;
	}

	public Vector3 GetTurretTarget(){
		if (this.target == null) {
			float y = transform.eulerAngles.y;
			Vector3 rot = new Vector3 (0, y, 0);
			return rot;
		} else {
			Vector3 pos = this.transform.position;
			Vector3 targetPos = this.target.transform.position;
			Vector3 vec = targetPos - pos;
			return Quaternion.LookRotation (vec).eulerAngles;
		}
	}

	public bool IsShoot(){
		if(this.target == null){
			return false;
		}
		float turretRoll = tank.turret.eulerAngles.y;
		float angle = turretRoll - this.GetTurretTarget ().y;
		if(angle < 0){
			angle += 360;
		}
		if (angle < 30 || angle > 330) {
			return true;
		} else {
			return false;
		}
	}

	void OnDrawGizmos(){
		this.path.DrawWaypoints ();
	}

	// Use this for initialization
	void Start () {
		this.InitWayPoint ();
	}
	
	// Update is called once per frame
	void Update () {
		if (tank.ctrlType != Tank.CtrlType.computer)
			return;

		if(status == Status.Patrol){
			this.PatrolUpdate ();
		}else if(status == Status.Attack){
			this.AttackUpdate ();
		}

		this.TargetUpdate ();
	}

	void PatrolStart(){
		
	}

	void AttackStart(){
		Vector3 targetPos = this.target.transform.position;
		this.path.InitByNavMeshPath (this.transform.position,targetPos);
	}

	void PatrolUpdate(){
		if (this.target != null)
			this.ChangeStatus (Status.Attack);
		float interval = Time.time - this.lastUpdateWaypointTime;
		if (interval < this.updateWaypointInterval)
			return;
		this.lastUpdateWaypointTime = Time.time;
		if (path.wayPoints == null || path.isFinish) {
			GameObject obj = GameObject.Find ("WaypointContainer");
			int count = obj.transform.childCount;
			if (count == 0)
				return;
			int index = Random.Range (0,count);
			Vector3 targetPos = obj.transform.GetChild (index).position;
			path.InitByNavMeshPath (this.transform.position,targetPos);
		}
	}

	void AttackUpdate(){
		if (this.target == null)
			this.ChangeStatus (Status.Patrol);

		float interval = Time.time - this.lastUpdateWaypointTime;
		if (interval < this.updateWaypointInterval)
			return;
		this.lastUpdateWaypointTime = Time.time;

		Vector3 targetPos = this.target.transform.position;
		path.InitByNavMeshPath (this.transform.position,targetPos);
	}


	//获取转向角
	public float GetSteering()
	{
		if (tank == null)
			return 0;

		Vector3 itp = transform.InverseTransformPoint(path.wayPoint);
		if (itp.x > path.deviation / 5)
			return tank.maxSteeringAngle;
		else if (itp.x < -path.deviation / 5)
			return -tank.maxSteeringAngle;
		else
			return 0;
	}

	//获取马力
	public float GetMotor()
	{

		if (tank == null)
			return 0;

		Vector3 itp = transform.InverseTransformPoint(path.wayPoint);
		float x = itp.x;
		float z = itp.z;
		float r = 6;

		if (z < 0 && Mathf.Abs(x) < -z && Mathf.Abs(x) < r)
			return -tank.maxMotorTorque;
		else
			return tank.maxMotorTorque;
	}

	//获取刹车
	public float GetBrakeTorque()
	{
		if (path.isFinish)
			return tank.maxMotorTorque;
		else
			return 0;
	}
}
