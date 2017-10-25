using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOntargetWithSpaceShip : MonoBehaviour {

	public int MaxHealth = 20;
	public float CurrentHealth = 0.0f;
	public float damageTimer = 0.0f;
	public AudioClip sound;

	public static int metecount = 0;
	public static int metecountAllien = 0;


	// Use this for initialization
	void Start () {	
		this.CurrentHealth = this.MaxHealth;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void ApplyDamage(float Damage){
		Debug.Log ("ApplyDamage   Damage = " + Damage);
		if(this.CurrentHealth < 0)
			return;

		this.CurrentHealth -= Damage;
	}
}
