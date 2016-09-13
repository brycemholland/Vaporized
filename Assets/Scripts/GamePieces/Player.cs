using UnityEngine;
using System.Collections;

public class Player : Starship {

	private GameplayController gameplay_controller;

	public GameObject laser, emp, mine, shield;

	private float boost_timer, normal_max_velocity, normal_thrust;

	[HideInInspector]
	public bool shield_enabled;

	public Player(){

	}

	// Use this for initialization
	void Awake () {
		my_body = gameObject.GetComponent<Rigidbody2D>();
		gameplay_controller = GameObject.Find("GameplayController").GetComponent<GameplayController>();

		boost_timer = Time.time;
		normal_max_velocity = max_velocity;
		normal_thrust = thrust;
	}
	
	void FixedUpdate () {
		CheckVelocity();
		PlayerMoveKeyboard();
		UsePower();
	}

	void PlayerMoveKeyboard(){
		float vel = my_body.velocity.magnitude;

		if (Time.time < boost_timer){
			power_timer = Time.time + power_recharge;
			thrust = normal_thrust * 2;
			max_velocity = normal_max_velocity * 2;
		} else {
			thrust = normal_thrust;
			if (max_velocity > normal_max_velocity){
				max_velocity -= 1 * Time.deltaTime*10;
			}
		}

		if (!disabled){
			Debug.Log(Input.inputString);
			if (Input.GetKey(KeyCode.UpArrow)){
				my_body.AddRelativeForce(new Vector2(0, thrust));
			}
			if (Input.GetKey(KeyCode.LeftArrow)){
				my_body.rotation += rotation_speed;
			}
			if (Input.GetKey(KeyCode.RightArrow)){
				my_body.rotation -= rotation_speed;
			}
		} else {
			Debug.Log("disabled");
		}

		if (vel > max_velocity){
			my_body.velocity = Vector2.ClampMagnitude(my_body.velocity, max_velocity);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (!shield_enabled){
			CrashCheck(col);
			if (col.collider.gameObject.name == "Laser(Clone)" || col.collider.gameObject.tag == "Enemy"){
				exploding = true;
			}
		}
		if (exploding){
			gameplay_controller.ToggleEndScreen();
			Explode();
		}
	}

	void UsePower(){
		if ( Time.time > power_timer ){
			if (Input.GetKey(KeyCode.Space)){
				power_timer = Time.time+power_recharge;
				switch (gameObject.name){
					case "PlayerShooter(Clone)":
						ShootLaser();
						break;
					case "PlayerBooster(Clone)":
						Boost();
						break;
					case "PlayerEMP(Clone)":
						EmitEMP();
						break;
					case "PlayerMiner(Clone)":
						PlaceMine();
						break;
					case "PlayerBasher(Clone)":
						EnableShield();
						break;
					default:
						break;
				}
			}
		}
	}

	void ShootLaser(){
		GameObject new_laser = (GameObject)Instantiate(laser, transform.position + transform.up/2, transform.rotation);
		new_laser.GetComponent<Laser>().shooter = gameObject;
		Physics2D.IgnoreCollision(new_laser.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
	}

	void Boost(){
		boost_timer = Time.time + 1;
	}

	void EmitEMP(){
		GameObject new_emp = (GameObject)Instantiate(emp, transform.position, transform.rotation);
		new_emp.GetComponent<EMP>().shooter = gameObject;
		Physics2D.IgnoreCollision(new_emp.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
	}

	void PlaceMine(){
		GameObject new_mine = (GameObject)Instantiate(mine, transform.position, transform.rotation);
		Physics2D.IgnoreCollision(new_mine.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
		new_mine.transform.Rotate(new Vector3(0, 0, 45));
	}

	void EnableShield(){
		GameObject new_shield = (GameObject)Instantiate(shield, transform.position, transform.rotation);
		gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
		gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
		new_shield.GetComponent<Shield>().owner = gameObject;
		shield_enabled = true;
	}

}
