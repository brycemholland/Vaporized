using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	private Rigidbody2D my_body;

	[HideInInspector]
	public GameObject shooter;

	public float laser_length, speed;

	// Use this for initialization
	void Start () {
		if (shooter){
			Rigidbody2D shooter_body = shooter.GetComponent<Rigidbody2D>();
			if (gameObject.GetComponent<Rigidbody2D>()){
				my_body = GetComponent<Rigidbody2D>();
				my_body.velocity = shooter_body.velocity;
				my_body.AddRelativeForce(new Vector2(0, speed));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localScale.y < laser_length){
			transform.localScale = transform.localScale + new Vector3(0, 0.1f, 0) * Time.deltaTime * 200;
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		Destroy(gameObject);
	}
}
