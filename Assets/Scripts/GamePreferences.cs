using UnityEngine;
using System.Collections;

public static class GamePreferences {
	
	public static string saved_score = "saved_score";
	public static string unlocked_ships = "unlocked_ships";
	public static string unlocked_levels = "unlocked_levels";

	public static string thrust_button = "thrust_button";
	public static string left_button = "left_button";
	public static string right_button = "right_button";
	public static string action_button = "action_button";

	public static int GetSavedScore(){
		return PlayerPrefs.GetInt(GamePreferences.saved_score);
	}

	public static void SetSavedScore(int score){
		PlayerPrefs.SetInt(GamePreferences.saved_score, score);
	}

	public static void IncreaseSavedScore(){
		PlayerPrefs.SetInt(GamePreferences.saved_score, PlayerPrefs.GetInt(GamePreferences.saved_score)+1);
	}

	public static int GetUnlockedShips(){
		return PlayerPrefs.GetInt(GamePreferences.unlocked_ships);
	}

	public static void SetUnlockedShips(int ships){
		PlayerPrefs.SetInt(GamePreferences.unlocked_ships, ships);
	}

	public static int GetUnlockedLevels(){
		return PlayerPrefs.GetInt(GamePreferences.unlocked_levels);
	}

	public static void SetUnlockedLevels(int levels){
		PlayerPrefs.SetInt(GamePreferences.unlocked_levels, levels);
	}

	public static string GetThrustButton(){
		return PlayerPrefs.GetString(GamePreferences.thrust_button);
	}

	public static void SetThrustButton(string button){
		PlayerPrefs.SetString(GamePreferences.thrust_button, button);
	}
	
}
