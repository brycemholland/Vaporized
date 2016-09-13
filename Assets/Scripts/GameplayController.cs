using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour {

	private GameController game_controller;

	public int opposition;
	private GameObject enemies_ui;
	private Text enemies_text;

	[HideInInspector]
	public int score, score_temp;
	private GameObject score_obj;
	private Text score_text;

	private GameObject pause_screen, end_screen, notification_text;

	private RespawnArea respawn_area;

	private GameObject power_meter;

	private float notification_timer;


	void Awake () {
		score = 0;
		score_temp = 0;
	}

	void Start(){
		game_controller = GameObject.Find("GameController").GetComponent<GameController>();

		pause_screen = GameObject.Find("Pause Screen");
		end_screen = GameObject.Find("End Screen");

		notification_text = GameObject.Find("Notification Text");
		notification_text.SetActive(false);

		score_obj = GameObject.Find("Score");
		score_text = score_obj.GetComponent<UnityEngine.UI.Text>();

		enemies_ui = GameObject.Find("Enemies");
		enemies_text = enemies_ui.GetComponent<UnityEngine.UI.Text>();

		power_meter = GameObject.Find("Power Meter");

		respawn_area = GameObject.Find("RespawnArea").GetComponent<RespawnArea>();

		ToggleEndScreen();
		StartGame();
	}

	void Update(){
		if (pause_screen.activeSelf){
			
		} else {
			if (GameObject.FindGameObjectWithTag("Player")){
				ManagePowerMeter();
			}
		}

		if (end_screen.activeSelf){
			if (Input.GetKeyDown(KeyCode.Return)){
				EndGame();
				StartGame();
				ToggleEndScreen();
				TogglePauseScreen();
			}
		} else {
			if (Input.GetKeyDown(KeyCode.Return)){
				TogglePauseScreen();
			}
		}
	}
	
	void FixedUpdate () {
		if (pause_screen.activeSelf){
			
		} else {
			if (score_temp >= opposition){
				opposition++;
				enemies_text.text = "ENEMIES: "+opposition+"";
				score_temp = 0;
			}
			if (Time.time > notification_timer){
				notification_text.SetActive(false);
			}
		}
	}

	public void TogglePauseScreen(){
		if (pause_screen.activeSelf){
			pause_screen.SetActive(false);
			Time.timeScale = 1;
		} else {
			pause_screen.SetActive(true);
			Time.timeScale = 0;
		}
	}

	public void ToggleEndScreen(){
		if (end_screen.activeSelf){
			end_screen.SetActive(false);
			Time.timeScale = 1;
		} else {
			end_screen.SetActive(true);
			Time.timeScale = 0.5f;
		}
	}

	public void EndGame(){
		respawn_area.DestroyEnemies();
		respawn_area.DestroyRespawnable();
		if (GameObject.FindObjectsOfType<Laser>().Length > 0){
			foreach (Laser l in GameObject.FindObjectsOfType<Laser>()){
				Destroy(l);
			}
		}
		if (GameObject.FindGameObjectsWithTag("Player").Length > 0){
			foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player")){
				Destroy(p);
			}
		}
		if (GameObject.FindGameObjectsWithTag("Laser").Length > 0){
			foreach(GameObject l in GameObject.FindGameObjectsWithTag("Laser")){
				Destroy(l);
			}
		}
	}

	public void StartGame(){
		Instantiate(game_controller.player, new Vector3(0, 0, 0), Quaternion.identity);
		respawn_area.CreateRespawnable();
		score_temp = 0;
		opposition = 1;
		enemies_text.text = "ENEMIES: "+opposition+"";
		score = 0;
		score_text.text = "SCORE: "+score+"";
		TogglePauseScreen();
	}

	public void IncreaseScore(int value){
		score += value;
		score_temp += value;
		score_text.text = "SCORE: "+score+"";
		GamePreferences.IncreaseSavedScore();

		if (score == game_controller.score_to_unlock_level){
			UnlockNextLevel();
		}

		if (GamePreferences.GetSavedScore() % game_controller.score_gap_between_ships == 0){
			UnlockNextShip();
		}
	}

	void ManagePowerMeter(){
		Player player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		Vector3 meter_scale = power_meter.transform.localScale;
		float meter_width = Mathf.Clamp( ((Time.time + player_script.power_recharge) - player_script.power_timer)/player_script.power_recharge, 0, 1);
		power_meter.transform.localScale = new Vector3(meter_width, meter_scale.y, meter_scale.z);
		power_meter.GetComponent<Image>().color = new Color(1,1,1, meter_scale.x);
		
	}

	void ShowNotificationText(string notification){
		notification_text.GetComponent<Text>().text = notification;
		notification_text.SetActive(true);
		notification_timer += Time.time + 5;
	}

	void UnlockNextLevel(){
		if (game_controller.level_index < game_controller.all_levels.Count-1){
			if (game_controller.level_index == GamePreferences.GetUnlockedLevels()-1){
				GamePreferences.SetUnlockedLevels(GamePreferences.GetUnlockedLevels()+1);
				game_controller.UpdateLevels();
				ShowNotificationText("New Level Unlocked");
			}
		}
	}

	void UnlockNextShip(){
		if (game_controller.player_index < game_controller.all_player_ships.Count-1){
			if (game_controller.player_index == GamePreferences.GetUnlockedShips()-1){
				GamePreferences.SetUnlockedShips(GamePreferences.GetUnlockedShips()+1);
				game_controller.UpdateShips();
				ShowNotificationText("New Starship Unlocked");
			}
		}
	}


}
