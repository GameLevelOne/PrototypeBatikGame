﻿using UnityEngine;
using UnityEngine.UI;

public class UIHPManaTool : MonoBehaviour {
	public Player player;
	public Health playerHealth;
	public Mana playerMana;

	public Image imageHP;
	public Image imageMana;
	public Image imageTool;

	public bool isHPChange = false;
	public bool isMPChange = false;

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
