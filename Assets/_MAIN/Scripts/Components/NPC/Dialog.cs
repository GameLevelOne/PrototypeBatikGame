using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Dialog : MonoBehaviour {
	
	[HeaderAttribute("Player Only")]
	public Player player;
	
	[HeaderAttribute("NPC Only")]
	public NPC npc;

	[SpaceAttribute(10f)]
	public GameObject panelDialog;
	public Text textDialog;

	public float showDialogDuration;
	public float showDialogDelay;

	[HeaderAttribute("Current")]
	public bool isInitShowingDialog;
	public int dialogIndex;
	public int letterIndex;
	public bool isStartDialog;
	public bool isShowingDialog;
	public bool isFinishShowingDialog;
	public float showDialogTime;
	public float dialogTime;
	// public string stringDialog;

	public string[] idleDialogs = new string[] {
		"Hi !",
		"Have fun !",
		"Nice to meet you !"
	};
	
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
