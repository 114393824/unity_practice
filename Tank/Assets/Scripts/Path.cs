using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Path : MonoBehaviour {

	public Vector3[] wayPoints;
	public int index = -1;
	public Vector3 wayPoint;

	bool isLoop = false;
	public float deviation = 5;
	public bool isFinish = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitByNavMeshPath(Vector3 pos,Vector3 targetPos){
		this.wayPoints = null;
		this.index = -1;

		NavMeshPath navPath = new NavMeshPath ();
		bool hasFoundPath = NavMesh.CalculatePath (pos,targetPos,NavMesh.AllAreas,navPath);
		if (!hasFoundPath)
			return;
		int length = navPath.corners.Length;
		this.wayPoints = new Vector3[length];
		for (int i = 0; i < length; i++)
			this.wayPoints [i] = navPath.corners [i];

		this.index = 0;
		this.wayPoint = this.wayPoints [this.index];
		this.isFinish = false;
	}

	public void DrawWaypoints(){
		if (this.wayPoints == null)
			return;
		int length = this.wayPoints.Length;
		Debug.Log ("DrawWaypoints   length = " + length);
		for(int i = 0; i < length; i++){
			if (i == this.index) {
				Gizmos.DrawSphere (this.wayPoints[i],1);	
			} else {
				Gizmos.DrawCube (this.wayPoints[i],Vector3.one);
			}
		}
	}
}
