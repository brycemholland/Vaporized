using UnityEngine;
using System.Collections;

public class CameraMain : MonoBehaviour {

	private GameController game_controller;

	[HideInInspector]
	public GameObject player_instance;

	// Use this for initialization
	void Start () {
		game_controller = GameObject.Find("GameController").GetComponent<GameController>();
		gameObject.GetComponent<Camera>().backgroundColor =  game_controller.background_color;
	}
	
	// Update is called once per frame
	void Update () {
		if (player_instance){
			Vector3 player_position = new Vector3(player_instance.transform.position.x, player_instance.transform.position.y, transform.position.z);
			if (player_instance.GetComponent<Rigidbody2D>()){
				Vector3 player_velocity = player_instance.GetComponent<Rigidbody2D>().velocity/9;
				transform.position = player_position - player_velocity;
			}

		} else {
			player_instance = GameObject.FindGameObjectWithTag("Player");
		}
	}
}
