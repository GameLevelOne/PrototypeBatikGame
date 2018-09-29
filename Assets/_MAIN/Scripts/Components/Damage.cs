﻿using UnityEngine;

public enum DamageCharacteristic {
	NONE,
	COUNTERABLE,
	PARRYABLE,
	COUNTER_AND_PARRYABLE
}

public class Damage : MonoBehaviour {
	public DamageCharacteristic damageChar;
	public float damage;
	public AudioSource audioSource;
	public AudioClip hitClip;

	[HeaderAttribute("Is Attack affects environtment ?")]
	public bool isAffectGrass = false;
	public bool isAffectBush = false;
	public bool isAffectTree = false;
}
