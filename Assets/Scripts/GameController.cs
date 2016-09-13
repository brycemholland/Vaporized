using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController instance = null;

	public List<GameObject> all_player_ships = new List<GameObject>();
	[HideInInspector]
	public List<GameObject> available_player_ships = new List<GameObject>();
	[HideInInspector]
	public int player_index;

	public List<GameObject> all_levels = new List<GameObject>();
	[HideInInspector]
	public List<GameObject> available_levels = new List<GameObject>();
	[HideInInspector]
	public int level_index, total_score;

	public int score_to_unlock_level, score_gap_between_ships;

	[HideInInspector]
	public GameObject player;

	[HideInInspector]
	public Color background_color, asteroid_color, enemy_color;

	[HideInInspector]
	public GameObject asteroid, enemy;

	void Awake(){
		if (instance == null){
			instance = this;
		}      
        else if (instance != this){
            Destroy(gameObject);
        }    
		DontDestroyOnLoad(gameObject);

		//ResetSavedInfo();
		UnlockAll();

		UpdateLevels();
		level_index = available_levels.Count-1;

		UpdateShips();
		player_index = available_player_ships.Count-1;

		if (GamePreferences.GetSavedScore() <= 0){
			GamePreferences.SetSavedScore(0);
		}

	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void GoToScene(string level_name){
		SceneManager.LoadScene(level_name);
	}

	public void UpdateLevels(){
		if (GamePreferences.GetUnlockedLevels() <= 0){
			GamePreferences.SetUnlockedLevels(1);
		}
		available_levels.Clear();
		for (int i=0; i < GamePreferences.GetUnlockedLevels(); i++){
			available_levels.Add(all_levels[i]);
		}
	}

	public void UpdateShips(){
		if (GamePreferences.GetUnlockedShips() <= 0){
			GamePreferences.SetUnlockedShips(1);
		}
		available_player_ships.Clear();
		for (int i=0; i < GamePreferences.GetUnlockedShips(); i++){
			available_player_ships.Add(all_player_ships[i]);
		}
	}

	void ResetSavedInfo(){
		GamePreferences.SetSavedScore(0);
		GamePreferences.SetUnlockedLevels(1);
		GamePreferences.SetUnlockedShips(1);
	}

	void UnlockAll(){
		GamePreferences.SetUnlockedLevels(all_levels.Count);
		GamePreferences.SetUnlockedShips(all_player_ships.Count);
	}

	public void QuitGame(){
		Application.Quit();
	}
}
