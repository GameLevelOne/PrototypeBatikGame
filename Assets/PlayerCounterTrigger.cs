using UnityEngine;

public class PlayerCounterTrigger : MonoBehaviour {
	public delegate void CounterTrigger();
	public event CounterTrigger OnCounterTrigger;

	void OnTriggerEnter (Collider other) {
		if (other.GetComponent<Damage>() != null) {
			DamageCharacteristic damageChar = other.GetComponent<Damage>().damageChar;

			if (damageChar == DamageCharacteristic.COUNTERABLE || damageChar == DamageCharacteristic.COUNTER_AND_PARRYABLE) {
				if (OnCounterTrigger != null) {
					OnCounterTrigger();
				}
			}
		}
	}	
}
