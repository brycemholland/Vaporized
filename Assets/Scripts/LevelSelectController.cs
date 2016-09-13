using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour {

	private GameController game_controller;

	private GameObject level_select_text, level_select_indicator, level_select_image, help_text;

	// Use this for initialization
	void Start () {
		game_controller = GameObject.Find("GameController").GetComponent<GameController>();

		help_text = GameObject.Find("HelpText");

		level_select_text = GameObject.Find("Level Select Text");
		level_select_indicator = GameObject.Find("Level Select Indicator");
		level_select_image = GameObject.Find("Level Select Image");

		SelectLevel(true);

		help_text.GetComponent<Text>().text = "Unlock new levels by defeating "+game_controller.score_to_unlock_level+" enemies in a single mission at the newest level";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			SelectLevel(true);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			SelectLevel(false);
		}
		if (Input.GetKeyDown(KeyCode.Return)){
			game_controller.GoToScene("ShipSelectScene");
		}
		if (Input.GetKeyDown(KeyCode.Escape)){
			game_controller.GoToScene("WelcomeScene");
		}
	}

	public void SelectLevel(bool increase){
		if (increase){
			if (game_controller.level_index < game_controller.available_levels.Count-1){
				game_controller.level_index++;
			} else {
				game_controller.level_index = 0;
			}
		} else {
			if (game_controller.level_index > 0){
				game_controller.level_index--;
			} else {
				game_controller.level_index = game_controller.available_levels.Count-1;
			}
		}

		game_controller.asteroid = game_controller.available_levels[game_controller.level_index].GetComponent<LevelPreview>().asteroid;
		game_controller.enemy = game_controller.available_levels[game_controller.level_index].GetComponent<LevelPreview>().enemy;

		GameObject level_preview = game_controller.all_levels[game_controller.level_index];
		game_controller.background_color = level_preview.GetComponent<LevelPreview>().background_color;
		game_controller.asteroid_color = level_preview.GetComponent<LevelPreview>().asteroid_color;
		game_controller.enemy_color = level_preview.GetComponent<LevelPreview>().enemy_color;

		Camera.main.backgroundColor = game_controller.background_color;
		level_select_image.GetComponent<SpriteRenderer>().color = game_controller.asteroid_color;
		level_select_image.GetComponent<SpriteRenderer>().sprite = level_preview.GetComponent<SpriteRenderer>().sprite;
		level_select_indicator.GetComponent<Text>().text = game_controller.level_index+1+" / "+game_controller.available_levels.Count;

		string level_name;
		switch (game_controller.level_index){
			case 0:
				level_name = "Alpha";
				break;
			case 1:
				level_name = "Bravo";
				break;
			case 2:
				level_name = "Charlie";
				break;
			case 3:
				level_name = "Delta";
				break;
			case 4:
				level_name = "Echo";
				break;
			case 5:
				level_name = "Foxtrot";
				break;
			case 6:
				level_name = "Golf";
				break;
			case 7:
				level_name = "Hotel";
				break;
			case 8:
				level_name = "India";
				break;
			case 9:
				level_name = "Juliett";
				break;
			case 10:
				level_name = "Kilo";
				break;
			case 11:
				level_name = "Lima";
				break;
			case 12:
				level_name = "Mike";
				break;
			default:
				level_name = "No Name";
				break;
		}
		level_select_text.GetComponent<Text>().text = level_name;

	}
}
