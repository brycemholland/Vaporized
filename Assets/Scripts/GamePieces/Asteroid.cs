using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	private GameController game_controller;

	private Rigidbody2D myBody;
	public float min_size, max_size, speed, spin;

	// Use this for initialization
	void Start () {
		game_controller = GameObject.Find("GameController").GetComponent<GameController>();

		myBody = GetComponent<Rigidbody2D>();
		myBody.AddForce(new Vector2(Random.Range(-speed, speed), Random.Range(-speed, speed)));

		float new_size = Random.Range(min_size, max_size);
		transform.localScale = new Vector3(new_size, new_size, transform.localScale.z);

		myBody.AddTorque(Random.Range(-spin, spin));

		GetComponent<SpriteRenderer>().color = game_controller.asteroid_color;

		float color_adjust = Random.Range(-0.2f, 0.2f);
		Color new_color = new Color(color_adjust, color_adjust, color_adjust, 1);
		GetComponent<SpriteRenderer>().color += new_color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
