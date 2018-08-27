﻿using UnityEngine;
using UnityEngine.UI;

public class UIHPManaTool : MonoBehaviour {
	public Player player;

	[HeaderAttribute("UI HP Attributes")]
	public Health playerHealth;
	public Image clothHP;

	// public float maxClothWidth;
	public float healthReduceValue;
	public bool isHPChange = false;
	
	[HeaderAttribute("UI HP Attributes")]
	public Mana playerMana;
	public Image imageMana;
	public bool isMPChange = false;
	
	[SpaceAttribute(10f)]
	// public Wavy wavy;
	// public Sprite[] spritesHP;
	// public Transform clothHP;
	public Image imageTool;

	void OnEnable () {
		playerHealth.OnHPChange += OnHPChange;
		playerMana.OnManaChange += OnManaChange;
	}

	void OnDisable () {
		playerHealth.OnHPChange -= OnHPChange;
		playerMana.OnManaChange -= OnManaChange;
	}

	void OnHPChange () {
		isHPChange = true;
	}
	
	void OnManaChange () {
		isMPChange = true;
	}
}
