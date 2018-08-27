using UnityEngine;

public enum DamageCharacteristic {
	NONE,
	COUNTERABLE,
	PARRYABLE,
	COUNTER_AND_PARRYABLE
}

public class Damage : MonoBehaviour {
	public DamageCharacteristic damageChar;
	public float damage;
}
