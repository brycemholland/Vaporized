using UnityEngine;
using System.Collections;

public class Enemy : Starship {

	[HideInInspector]
	public GameObject player_instance;

	private GameController game_controller;
	private GameplayController gameplay_controller;

	public GameObject laser, emp, shield;

	private RaycastHit2D hit;

	private bool on_camera;

	private float normal_max_velocity;

	[HideInInspector]
	public bool player_in_sights, shield_enabled;

	// Use this for initialization
	void Awake () {
		my_body = GetComponent<Rigidbody2D>();
	}

	void Start (){
		game_controller = GameObject.Find("GameController").GetComponent<GameController>();
		gameplay_controller = GameObject.Find("GameplayController").GetComponent<GameplayController>();
		player_instance = GameObject.FindGameObjectWithTag("Player");
		normal_max_velocity = max_velocity;
		player_in_sights = false;
		on_camera = false;
		GetComponent<SpriteRenderer>().color = game_controller.enemy_color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		CheckVelocity();
		CheckIfOnScreen();

		if (player_instance){
			if (!disabled){
				PursuePlayer();
			}
		} else {
			player_instance = GameObject.FindGameObjectWithTag("Player");
		}
		if (gameObject.name == "EnemyShooter(Clone)"){
			EnemyShootLaser();
		} else if (gameObject.name == "EnemyEMP(Clone)"){
			EnemyEmitEMP();
		} else if (gameObject.name == "EnemyBasher(Clone)"){
			EnemyEnableShield();
		}
	}

	void CheckIfOnScreen(){
		if (Camera.main.WorldToViewportPoint(gameObject.transform.position).x > 0 && Camera.main.WorldToViewportPoint(gameObject.transform.position).x < 1 && Camera.main.WorldToViewportPoint(gameObject.transform.position).y > 0 && Camera.main.WorldToViewportPoint(gameObject.transform.position).y < 1){
			on_camera = true;
		} else {
			//on_camera = false;
		}
	}

	void PursuePlayer(){
		if (on_camera){
			max_velocity = normal_max_velocity;
		} else {
			max_velocity = normal_max_velocity*2;
		}
		Vector3 direction = player_instance.transform.position - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotation_speed);

		if (my_body){
			float vel = my_body.velocity.magnitude;
			my_body.AddRelativeForce(new Vector2(0, thrust));
			if (vel > max_velocity){
				my_body.velocity = Vector2.ClampMagnitude(my_body.velocity, max_velocity);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (on_camera){
			if (!shield_enabled){
				CrashCheck(col);
				if (col.collider.gameObject.name == "Laser(Clone)"){
					exploding = true;
				}
			}
			if (exploding){
				if (player_instance){
					gameplay_controller.IncreaseScore(1);
				}
				Explode();
			}
		}
	}

	void EnemyShootLaser(){
		if (on_camera){
			hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), transform.up);
			if (hit){
				if (hit.collider.gameObject.tag == "Player"){
					if (!player_in_sights){
						power_timer = Time.time+power_recharge;
						player_in_sights = true;
					}
				} else {
					power_timer = Time.time+power_recharge;
					player_in_sights = false;
				}
			}
			if (Time.time > power_timer && player_in_sights){
				power_timer = Time.time+power_recharge;
				GameObject new_laser = (GameObject)Instantiate(laser, transform.position + transform.up/2, transform.rotation);
				new_laser.GetComponent<Laser>().shooter = gameObject;
				Physics2D.IgnoreCollision(new_laser.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
			}
		}
	}

	void EnemyEmitEMP(){
		if (player_instance){
			if (Vector3.Distance(player_instance.transform.position, transform.position) < 3){
				
				if (Time.time > power_timer){
					power_timer = Time.time+power_recharge;
					GameObject new_emp = (GameObject)Instantiate(emp, transform.position, transform.rotation);
					new_emp.GetComponent<EMP>().shooter = gameObject;
					Physics2D.IgnoreCollision(new_emp.GetComponent<Collider2D>(),  GetComponent<Collider2D>());
				}
			}
		}
	}

	void EnemyEnableShield(){
		if (on_camera){
			hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), transform.up, my_body.velocity.magnitude);
			if (hit){
				if (Time.time > power_timer){
					power_timer = Time.time+power_recharge;
					GameObject new_shield = (GameObject)Instantiate(shield, transform.position, transform.rotation);
					gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
					gameObject.GetComponent<CircleCollider2D>().isTrigger = false;
					new_shield.GetComponent<Shield>().owner = gameObject;
					shield_enabled = true;
				}
			}
		}
	}
}
