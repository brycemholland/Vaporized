using UnityEngine;
using System.Collections;
using System.Linq;

public class RespawnArea : MonoBehaviour {

	public float size_multiplyer;

	[HideInInspector]
	public int left_boundary, right_boundary, top_boundary, bottom_boundary;
	private Vector3[] entry_arr = new Vector3[4];

	[HideInInspector]
	public GameObject asteroid;
	public float number_of_asteroids;

	public GameObject star;
	public float number_of_stars;

	[HideInInspector]
	public GameObject enemy;
	private GameObject[] enemies;

	private GameObject gameplay_controller;
	private GameplayController gameplay_script;
	private GameController game_controller;

	// Use this for initialization
	void Start () {
		game_controller = GameObject.Find("GameController").GetComponent<GameController>();
		gameplay_controller = GameObject.Find("GameplayController");
		gameplay_script = gameplay_controller.GetComponent<GameplayController>();

		enemy = game_controller.enemy;
		asteroid = game_controller.asteroid;

		transform.localScale = new Vector3((Camera.main.aspect*Camera.main.orthographicSize)*size_multiplyer, Camera.main.orthographicSize*size_multiplyer, transform.localScale.z);

		left_boundary = (int) -transform.localScale.x/2;
		right_boundary = (int) transform.localScale.x/2;
		top_boundary = (int) transform.localScale.y/2;
		bottom_boundary = (int) -transform.localScale.y/2;

	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
	}

	void FixedUpdate(){
		CreateEnemies();
	}

	public void CreateRespawnable(){
		for (int i=0; i < number_of_stars; i++){
			Instantiate(star, new Vector3(Random.Range(left_boundary, right_boundary), Random.Range(top_boundary, bottom_boundary), 0), Quaternion.identity);
		}

		for (int i=0; i < number_of_asteroids; i++){
			Vector3 new_location = new Vector3(Random.Range(left_boundary, right_boundary), Random.Range(top_boundary, bottom_boundary), 0);

			bool reposition = false;
			if ( Camera.main.WorldToViewportPoint(new_location).x > 0 && Camera.main.WorldToViewportPoint(new_location).x < 1 && Camera.main.WorldToViewportPoint(new_location).y > 0 && Camera.main.WorldToViewportPoint(new_location).y < 1 ){
				reposition = true;
			}

			while (reposition == true){
				new_location = new Vector3(Random.Range(left_boundary, right_boundary), Random.Range(top_boundary, bottom_boundary), 0);
				if ( Camera.main.WorldToViewportPoint(new_location).x > 0 && Camera.main.WorldToViewportPoint(new_location).x < 1 && Camera.main.WorldToViewportPoint(new_location).y > 0 && Camera.main.WorldToViewportPoint(new_location).y < 1 ){
					reposition = true;
				} else {
					reposition = false;
				}
			}

			Instantiate(asteroid, new_location, Quaternion.identity);
		}

	}

	public void DestroyRespawnable(){
		GameObject[] respawnable = GameObject.FindGameObjectsWithTag("Asteroid").Concat(GameObject.FindGameObjectsWithTag("Star")).ToArray();
		if (respawnable.Length > 0){
			foreach (GameObject r in respawnable){
				Destroy(r);
			}
		}
	}

	public void CreateEnemies(){
		if (GameObject.FindGameObjectWithTag("Player")){
			entry_arr[0] = new Vector3(Random.Range(-transform.localScale.x/2, transform.localScale.x/2), transform.localScale.y/3, 0) + transform.position;
			entry_arr[1] = new Vector3(transform.localScale.x/3, Random.Range(-transform.localScale.y/2, transform.localScale.y/2), 0) + transform.position;
			entry_arr[2] = new Vector3(Random.Range(-transform.localScale.x/2, transform.localScale.x/2), -transform.localScale.y/3, 0) + transform.position;
			entry_arr[3] = new Vector3(-transform.localScale.x/3, Random.Range(-transform.localScale.y/2, transform.localScale.y/2), 0) + transform.position;

			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			if (enemies.Length < gameplay_script.opposition){
				Instantiate(enemy, entry_arr[(int)Random.Range(0, entry_arr.Length-1)], Quaternion.identity);
			}
		}
	}

	public void DestroyEnemies(){
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
		if (enemies.Length > 0){
			foreach (GameObject e in enemies){
				Destroy(e);
			}
		}
	}
}
