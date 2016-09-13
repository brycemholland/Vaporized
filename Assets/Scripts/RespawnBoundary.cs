using UnityEngine;
using System.Collections;

public class RespawnBoundary : MonoBehaviour {

	public GameObject respawn_area;
	private RespawnArea respawn_area_script;

	// Use this for initialization
	void Start () {
		respawn_area_script = respawn_area.GetComponent<RespawnArea>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Asteroid" || col.gameObject.tag == "Star"){

			int left_boundary = (int) -respawn_area.transform.localScale.x/2 + (int) respawn_area.transform.position.x;
			int right_boundary = (int) respawn_area.transform.localScale.x/2 + (int) respawn_area.transform.position.x;
			int top_boundary = (int) respawn_area.transform.localScale.y/2 + (int) respawn_area.transform.position.y;
			int bottom_boundary = (int) -respawn_area.transform.localScale.y/2 + (int) respawn_area.transform.position.y;

			Vector3 new_location = new Vector3(Random.Range(left_boundary, right_boundary), Random.Range(top_boundary, bottom_boundary), 0);

			bool reposition = false;
			Vector3 obj_width = new Vector3(col.gameObject.transform.localScale.x, 0, 0);
			Vector3 obj_height = new Vector3(0, col.gameObject.transform.localScale.y, 0);
			if ( Camera.main.WorldToViewportPoint(new_location + obj_width).x > 0 && Camera.main.WorldToViewportPoint(new_location - obj_width).x < 1 && Camera.main.WorldToViewportPoint(new_location + obj_height).y > 0 && Camera.main.WorldToViewportPoint(new_location - obj_height).y < 1 ){
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

			Destroy(col.gameObject);
			if (col.gameObject.tag == "Asteroid"){
				Instantiate(respawn_area_script.asteroid, new_location, Quaternion.identity);
			} else if (col.gameObject.tag == "Star"){
				Instantiate(respawn_area_script.star, new_location, Quaternion.identity);
			}
		}
	}


}
