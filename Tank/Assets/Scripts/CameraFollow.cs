using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	private GameObject target;

	public float distance = 8;

	public float rot = 0;
	public float rotSpeed = 0.2f;

	private float roll = 30f * Mathf.PI * 2 / 360;
	private float rollSpeed = 0.2f;
	private float maxRoll = 70f * Mathf.PI * 2 / 360;
	private float minRoll = -10f * Mathf.PI * 2 / 360;

	public float maxDistance = 22f;
	public float minDistance = 5f;

	public float zoomSpeed = 12f;

	// Use this for initialization
	void Start () {
		SetTarget(GameObject.Find("Tank"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate(){
		if (target == null || Camera.main == null)
			return;
		Vector3 targetPos = target.transform.position;
		Vector3 cameraPos;

		float d = distance * Mathf.Cos(roll);
		float height = distance * Mathf.Sin(roll);

		cameraPos.x = targetPos.x + d * Mathf.Cos(rot);
		cameraPos.z = targetPos.z + d * Mathf.Sin(rot);
		cameraPos.y = targetPos.y + height;

		Camera.main.transform.position = cameraPos;
		Camera.main.transform.LookAt(target.transform);

		Rotate();
		Roll();
		Zoom();
	}

	public void SetTarget(GameObject target){
		if (target.transform.Find("cameraPoint") != null)
			this.target = target.transform.Find("cameraPoint").gameObject;
		else
			this.target = target;
	}

	public void Zoom(){
		float wheelDistance = Input.GetAxis("Mouse ScrollWheel");
		// Debug.Log("Zoom--------------- wheelDistance = " + wheelDistance);
		if (wheelDistance > 0){
            if (distance > minDistance)
                distance -= zoomSpeed;
		}else if(wheelDistance < 0){
            if (distance < maxDistance)
                distance += zoomSpeed;
		}
	}

	public void Roll(){
		float w = Input.GetAxis("Mouse Y") * rollSpeed;// * 0.5f
		// Debug.Log("Roll--------------- w = " + w);
		roll -= w;
		if (roll > maxRoll)
			roll = maxRoll;
		if (roll < minRoll)
			roll = minRoll;
	}

	//横向
	public void Rotate(){
		float w = Input.GetAxis("Mouse X") * rotSpeed;
		// Debug.Log("Rotate--------------- w = " + w);
		rot -= w;
	}
}
