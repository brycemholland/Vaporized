using UnityEngine;
using System.Collections;

public class EMP : MonoBehaviour {

	[HideInInspector]
	public GameObject shooter;

	public float emp_length, recover_time;

	private float emp_timer, progress;

	// Use this for initialization
	void Start () {
		if (shooter){
			emp_timer = Time.time + emp_length;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (shooter){
			transform.position = shooter.transform.position;
		}
		transform.localScale = Vector3.Lerp (transform.localScale, new Vector3(20f, 20f, 1), progress);
		gameObject.GetComponent<SpriteRenderer>().color += new Color(0,0,0,-0.05f);
		progress += Time.deltaTime * 1;
		if (Time.time > emp_timer){
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Player"){
			Starship starship = col.gameObject.GetComponent<Starship>();
			starship.disabled_timer = Time.time + recover_time;
			starship.disabled = true;
		}
	}
}
