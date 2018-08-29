using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Dialog : MonoBehaviour {
	public NPC npc;
	public GameObject panelDialog;
	public Text textDialog;

	public int dialogIndex;
	public int letterIndex;
	public float showDialogDuration;
	public float showDialogTime;
	public float dialogTime;
	public float showDialogDelay;
	public bool isInitShowingDialog;
	public bool isShowingDialog;
	public bool isFinishShowingDialog;

	public string[] idleDialogs = new string[] {
		"Hi !",
		"Have fun !",
		"Nice to meet you !"
	};
	
	public string[] interactDialogs = new string[] {
		"Welcome to JavaTale",
		"Are you ready for next dungeon ?",
		"Good luck and have fun !"
	};

	public List<string> letterList;
}
