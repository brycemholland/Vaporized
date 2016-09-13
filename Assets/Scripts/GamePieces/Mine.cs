using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	public float explosion_size, detonation_time;
	public GameObject exploding_bit;

	private float mine_timer;

	// Use this for initialization
	void Start () {
		mine_timer = Time.time + detonation_time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > mine_timer){
			Explode();
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		Explode();
	}

	void Explode(){
		for (int i=0; i<explosion_size; i++){
			GameObject b = (GameObject)Instantiate(exploding_bit, transform.position, Quaternion.identity);
			b.GetComponent<ExplodingBit>().start_color = gameObject.GetComponent<SpriteRenderer>().color;
		}
		Destroy(gameObject);
	}
}
