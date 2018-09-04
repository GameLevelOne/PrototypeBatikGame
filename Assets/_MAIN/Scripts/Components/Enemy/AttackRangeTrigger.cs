using UnityEngine;

public class AttackRangeTrigger : MonoBehaviour {

	public delegate void ExecuteAttack(bool attack);
	public event ExecuteAttack OnExecuteAttack;

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == Constants.Tag.PLAYER){
			if(OnExecuteAttack != null) OnExecuteAttack(true);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if(other.tag == Constants.Tag.PLAYER){
			if(OnExecuteAttack != null) OnExecuteAttack(false);
		}
	}
}
