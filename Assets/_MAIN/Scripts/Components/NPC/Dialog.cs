using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum DialogState {
	INIT,
	SHOW,
	WAITINPUT,
	WAITDELAY,
	IDLE
}

public class Dialog : MonoBehaviour {
	
	[HeaderAttribute("Player Only")]
	public Player player;
	
	[HeaderAttribute("NPC Only")]
	public NPC npc;

	[SpaceAttribute(10f)]
	public GameObject panelDialog;
	public Text textDialog;
	public GameObject dialogButton;

	public float showTextDuration;
	public float delayDialogDuration;

	[HeaderAttribute("Current")]
	public DialogState dialogState;
	public int dialogIndex;
	public int letterIndex;
	public float dialogDeltaTime;
	
	public string[] interactDialogs = new string[] {
		"1. Welcome to JavaTale",
		"2. Are you ready for next dungeon ?",
		"3. Good luck and have fun !"
	};
	
	public string[] openingDialogs = new string[] {
		"1. Opening",
		"2. Opening",
		"3. Opening"
	};

	public List<string> letterList;
}
