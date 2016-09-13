using UnityEngine;
using System.Collections;

public class Starship : MonoBehaviour {

	public float thrust, max_velocity, rotation_speed, impact_resistance, power_recharge;

	[HideInInspector]
	public float old_velocity, new_velocity, power_timer, disabled_timer;

	[HideInInspector]
	public Rigidbody2D my_body;

	public GameObject exploding_bit;

	public int explosion_size;

	[HideInInspector]
	public bool exploding, disabled;

	public Starship(){
		
	}

	void Awake(){
		exploding = false;
		power_timer = Time.time;
	}

	void Update(){
		if (disabled){
			if (Time.time > disabled_timer){
				disabled = false;
			}
		}
	}

	public void CheckVelocity(){
		old_velocity = my_body.velocity.magnitude;
	}

	public void CrashCheck(Collision2D col) {
		new_velocity = my_body.velocity.magnitude;
		if (old_velocity - new_velocity >= impact_resistance ){
			exploding = true;
		}
	}

	public void Explode(){
		for (int i=0; i<explosion_size; i++){
			GameObject b = (GameObject)Instantiate(exploding_bit, transform.position, Quaternion.identity);
			b.GetComponent<ExplodingBit>().start_color = gameObject.GetComponent<SpriteRenderer>().color;
		}
		Destroy(gameObject);
	}
}
