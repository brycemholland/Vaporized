using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	public float shield_time, appear_time;
	private float shield_timer, color_timer;

	[HideInInspector]
	public GameObject owner;

	private SpriteRenderer sprite_renderer;

	private Color start_color, target_color;

	// Use this for initialization
	void Start () {
		shield_timer = Time.time + shield_time;
		sprite_renderer = GetComponent<SpriteRenderer>();
		sprite_renderer.color = owner.GetComponent<SpriteRenderer>().color;
		sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.b, sprite_renderer.color.g, 0f);
		Appear();
	}
	
	// Update is called once per frame
	void Update () {
		if (owner){
			transform.position = owner.transform.position;
		}
		if (Time.time > shield_timer || !owner){
			ShutDown();
		}
		if (color_timer < 1){
			color_timer += Time.deltaTime / appear_time;
		}
		sprite_renderer.color = Color.Lerp(start_color, target_color, color_timer);
	}

	void Appear(){
		start_color = sprite_renderer.color;
		color_timer = 0;
		target_color = new Color(sprite_renderer.color.r, sprite_renderer.color.b, sprite_renderer.color.g, 1f);
	}

	void Disappear(){
		start_color = sprite_renderer.color;
		color_timer = 0;
		target_color = new Color(sprite_renderer.color.r, sprite_renderer.color.b, sprite_renderer.color.g, 0f);
	}

	void ShutDown(){
		if (owner){
			owner.GetComponent<PolygonCollider2D>().isTrigger = false;
			owner.GetComponent<CircleCollider2D>().isTrigger = true;
			if (owner.GetComponent<Player>()){
				owner.GetComponent<Player>().shield_enabled = false;
			} else if (owner.GetComponent<Enemy>()){
				owner.GetComponent<Enemy>().shield_enabled = false;
			}
			Disappear();
		}
		Destroy(gameObject, appear_time);
	}
}
