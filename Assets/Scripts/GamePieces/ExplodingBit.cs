using UnityEngine;
using System.Collections;

public class ExplodingBit : MonoBehaviour {

	public float min_size, max_size, explosive_force;

	[HideInInspector]
	public Color start_color;

	private float new_size;

	private float fade_timer;

	private Rigidbody2D my_body;
	private SpriteRenderer my_sprite;

	// Use this for initialization
	void Start () {
		my_body = gameObject.GetComponent<Rigidbody2D>();
		my_sprite = gameObject.GetComponent<SpriteRenderer>();
		new_size = Random.Range(min_size, max_size);
		fade_timer = Time.time;

		transform.localScale = new Vector3(new_size, new_size, transform.position.z);

		float color_adjust = Random.Range(-0.2f, 0.2f);
		Color new_color = new Color(color_adjust, color_adjust, color_adjust, 1);
		my_sprite.color = new_color + start_color;

		my_body.AddForce(new Vector2(Random.Range(-explosive_force, explosive_force), Random.Range(-explosive_force, explosive_force)));
		my_body.AddTorque(Random.Range(100, 1000));

		Destroy(gameObject, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > fade_timer ) {
	    	fade_timer += 0.1f;
			if (my_sprite.color.a > 0){
				my_sprite.color -= new Color(0f, 0f, 0f, Time.deltaTime*10);
			}
		}
	}
}
