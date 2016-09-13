using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public float min_size, max_size;
	private Light star_light;

	// Use this for initialization
	void Start () {

		float new_size = Random.Range(min_size, max_size);
		transform.localScale = new Vector3(new_size, new_size, transform.position.z);
		star_light = gameObject.GetComponent<Light>();
		star_light.range = new_size;
		star_light.intensity = Random.Range(1f, 3f);
		star_light.color = new Color( Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1), 1);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
