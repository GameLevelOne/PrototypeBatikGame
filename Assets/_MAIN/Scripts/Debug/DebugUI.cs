using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {

	[HeaderAttribute("MAIN")]
	public GameObject debugPanel;

	[HeaderAttribute("QUEST DATA")]
	public InputField area11Ctr;
	public InputField area21Ctr;
	public InputField area22Ctr;
	public InputField area23Ctr;
	public InputField area31Ctr;
	public InputField area33Ctr;
	public Toggle area11Dissolve;
	public Toggle area21Dissolve;
	public Toggle area22Dissolve;
	public Toggle area23Dissolve;
	public Toggle area31Dissolve;
	public Toggle area33Dissolve;

	[HeaderAttribute("AREA DATA")]
	public Toggle area11CutScene;
	public Toggle area21VinesBurn;
	public Toggle area21GateUnlocked;
	public Toggle area22ChestUnlocked;

	[HeaderAttribute("PLAYER DATA")]
	public Toggle gateKey;
	public Toggle fireArrow;
	public Toggle fishingRod;

	// Use this for initialization
	void Start () {
		debugPanel.SetActive(false);
	}
	
	public void OpenDebug() {
		ReadData();
		debugPanel.SetActive(true);		
	}

	void ReadData() {
		//QUEST DATA	
		area11Ctr.text = PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_INDEX+"0", 0).ToString();
		area21Ctr.text = PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_INDEX+"1", 0).ToString();
		area22Ctr.text = PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_INDEX+"2", 0).ToString();
		area23Ctr.text = PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_INDEX+"3", 0).ToString();
		area31Ctr.text = PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_INDEX+"4", 0).ToString();
		area33Ctr.text = PlayerPrefs.GetInt(Constants.QuestPrefKey.QUEST_INDEX+"5", 0).ToString();
		area11Dissolve.isOn = PlayerPrefs.GetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"0", 0) == 1 ? true : false;
		area21Dissolve.isOn = PlayerPrefs.GetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"1", 0) == 1 ? true : false;
		area22Dissolve.isOn = PlayerPrefs.GetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"2", 0) == 1 ? true : false;
		area23Dissolve.isOn = PlayerPrefs.GetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"3", 0) == 1 ? true : false;
		area11Dissolve.isOn = PlayerPrefs.GetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"4", 0) == 1 ? true : false;
		area11Dissolve.isOn = PlayerPrefs.GetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"5", 0) == 1 ? true : false;

		//AREA DATA
		area11CutScene.isOn = PlayerPrefs.GetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+"OpeningMadaKari", 0) == 1 ? true : false;
		area21VinesBurn.isOn = PlayerPrefs.GetInt(Constants.EnvirontmentPrefKey.VINES_STATE + "0", 0) == 1 ? true : false;
		area21GateUnlocked.isOn = PlayerPrefs.GetInt(Constants.EnvirontmentPrefKey.GATES_STATE + "0", 0) == 1 ? true : false;
		area22ChestUnlocked.isOn = PlayerPrefs.GetInt(Constants.EnvirontmentPrefKey.CHEST_STATE + "0", 0) == 1 ? true : false;

		//PLAYER DATA
		gateKey.isOn = PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_SAVED_KEY + "0", 0) == 1 ? true : false;
	}

	public void SaveData() {
		//QUEST DATA	
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"0", int.Parse(area11Ctr.text));
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"1", int.Parse(area21Ctr.text));
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"2", int.Parse(area22Ctr.text));
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"3", int.Parse(area23Ctr.text));
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"4", int.Parse(area31Ctr.text));
		PlayerPrefs.SetInt(Constants.QuestPrefKey.QUEST_INDEX+"5", int.Parse(area33Ctr.text));
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"0", area11Dissolve.isOn ? 1 : 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"1", area21Dissolve.isOn ? 1 : 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"2", area22Dissolve.isOn ? 1 : 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"3", area23Dissolve.isOn ? 1 : 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"4", area31Dissolve.isOn ? 1 : 0);
		PlayerPrefs.SetInt(Constants.DissolvedLevelPrefKey.LEVEL_INDEX+"5", area33Dissolve.isOn ? 1 : 0);

		//AREA DATA
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.FINISHED_TIMELINE+"OpeningMadaKari", area11CutScene.isOn ? 1 : 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.VINES_STATE + "0", area21VinesBurn.isOn ? 1 : 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.GATES_STATE + "0", area21GateUnlocked.isOn ? 1 : 0);
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CHEST_STATE + "0", area22ChestUnlocked.isOn ? 1 : 0);

		//PLAYER DATA
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_KEY + "0", gateKey.isOn ? 1 : 0);


		debugPanel.SetActive(false);
	}

	public void CancelDebug() {
		debugPanel.SetActive(false);
	}
}
