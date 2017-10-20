var MaxHealth : int = 20;
var CurrentHealth : int;
var damageTimer = 0.0;
public var sound : AudioClip;
static var metecount = 0;
static var metecountAllien = 0;

//apply Damage on the other objects with fireballs
function Start()
{
CurrentHealth = MaxHealth;
}

function ApplyDamage (Damage : float)
{
	if (CurrentHealth < 0){
	return;
	}
		CurrentHealth -= Damage;
			if (CurrentHealth == 0)
			{
		//	metecount +=5;
			//print("YOU NOW HAVE "+ metecount +" score");
		//	GameObject.Find("Score").guiText.text = ""+metecount;
			
			gameObject.GetComponent("Detonator").Explode();
			}
}
/*
function ApplyDamageAllien (AllienDamage : float)
{
	if (CurrentHealth < 0){
	return;
	}
		CurrentHealth -= AllienDamage;
			if (CurrentHealth == 0)
			{
			metecountAllien +=5;
			print("YOU NOW HAVE "+ metecountAllien +" score");
			GameObject.Find("ScoreAllien").guiText.text = ""+metecountAllien;
			
			gameObject.GetComponent("Detonator").Explode();
			}
}



function OnCollisionEnter ()
	{
	DoDamage();
	AudioSource.PlayClipAtPoint(sound, transform.position);
	gameObject.GetComponent("Detonator").Explode();
	}
*/