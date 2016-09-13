using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipSelectController : MonoBehaviour {

	private GameController game_controller;

	private GameObject player_select_image, player_select_text, player_select_indicator, kills_remaining_text;

	void Awake(){
		
	}

	void Start () {
		game_controller = GameObject.Find("GameController").GetComponent<GameController>();

		kills_remaining_text = GameObject.Find("KillsRemainingText");

		player_select_image = GameObject.Find("Player Select Image");
		player_select_text = GameObject.Find("Player Select Text");
		player_select_indicator = GameObject.Find("Player Select Indicator");

		Camera.main.backgroundColor = game_controller.background_color;

		SelectPlayer(true);

		int kills_remaining = game_controller.score_gap_between_ships - (GamePreferences.GetSavedScore() % game_controller.score_gap_between_ships);
		kills_remaining_text.GetComponent<Text>().text = "Defeat "+kills_remaining+" more enemies to unlock an new starship";

	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			SelectPlayer(true);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)){
			SelectPlayer(false);
		}
		if (Input.GetKeyDown(KeyCode.Return)){
			game_controller.GoToScene("GameplayScene");
		}
		if (Input.GetKeyDown(KeyCode.Escape)){
			game_controller.GoToScene("LevelSelectScene");
		}
	}

	public void SelectPlayer(bool increase){
		if (increase){
			if (game_controller.player_index < game_controller.available_player_ships.Count-1){
				game_controller.player_index++;
			} else {
				game_controller.player_index = 0;
			}
		} else {
			if (game_controller.player_index > 0){
				game_controller.player_index--;
			} else {
				game_controller.player_index = game_controller.available_player_ships.Count-1;
			}
		}

		if (game_controller.player_index < game_controller.available_player_ships.Count && game_controller.player_index >= 0){
			game_controller.player = game_controller.available_player_ships[game_controller.player_index];
		} else {
			game_controller.player = game_controller.available_player_ships[0];
		}

		player_select_image.GetComponent<SpriteRenderer>().sprite = game_controller.available_player_ships[game_controller.player_index].GetComponent<SpriteRenderer>().sprite;
		player_select_indicator.GetComponent<Text>().text = game_controller.player_index+1+" of "+game_controller.available_player_ships.Count;

		string ship_name;
		switch (game_controller.available_player_ships[game_controller.player_index].name){
			case "PlayerShooter":
				ship_name = "Shooter";
				break;
			case "PlayerBooster":
				ship_name = "Booster";
				break;
			case "PlayerEMP":
				ship_name = "EMP";
				break;
			case "PlayerMiner":
				ship_name = "Miner";
				break;
			case "PlayerBasher":
				ship_name = "Basher";
				break;
			default:
				ship_name = "No Name";
				break;
		}
		player_select_text.GetComponent<Text>().text = ship_name;
	}
}
