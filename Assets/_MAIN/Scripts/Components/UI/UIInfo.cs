using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIInfo : MonoBehaviour {
	public AnimationControl animationControl;
	public Animator animator;
	public GameObject panelUIInfo;
	public Sprite initContainerSprite;
	public Sprite hpPotSprite;
	public Sprite mpPotSprite;
	public Sprite initToolSprite;
	public Sprite selectedToolSprite;
	// public CanvasGroup canvasInfoGroup;
	// public float showMultiplier;
	// public float hideMultiplier;
	[HeaderAttribute("Button Tools")]
	public List<Button> listOfButtonToolsNSummons;
	public Text descLabel;
	public string[] toolsDesc;
	public string[] containerDesc;

	[HeaderAttribute("Area Info")]
	public Text areaName;
	public Transform playerIndicator;
	public string[] areaNames;
	public Transform[] areaCoordinates;

	[HeaderAttribute("Current")]
	public bool isInitUIInfo = false;
	public bool isPlayingAnimation;
	[HeaderAttribute("Sound")]
	public AudioSource uiAudio;
	public AudioClip openClip;
	public AudioClip selectClip;
	public AudioClip chooseClip;
	public AudioClip backClip;

	void OnEnable () {
		animationControl.OnExitAnimation += OnExitAnimation;
	}

	void OnDisable () {
		animationControl.OnExitAnimation -= OnExitAnimation;
	}

	void OnExitAnimation () {
		isPlayingAnimation = false;
	}
}
