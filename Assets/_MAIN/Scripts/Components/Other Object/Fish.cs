﻿using UnityEngine;

public enum FishState {
	IDLE,
	PATROL,
	CHASE,
	CATCH,
	FLEE
}

public class Fish : MonoBehaviour {
	public FishState state;
	public GameObject[] lootableObjs;

	public Animator anim;
	public Collider2D selfCol;
	public Rigidbody2D rigidbody;
	public Transform parentPoolCol;
	public FishingRod fishingRod;

	public Vector2 targetPos;
	public float parentPoolRadius;
	public float moveSpeed;
	public float chaseSpeed;
	public float fleeSpeed;
	public float timeToCatch;
	public float idleDuration;

	[SerializeField] float timeIdle;

	public float TimeIdle {
		get {return timeIdle;}
		set {timeIdle = value;}
	}
}
