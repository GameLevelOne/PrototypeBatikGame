using UnityEngine;

public class JatayuMovement : MonoBehaviour {
	public Jatayu jatayu;

#region animation event
	void DoAttack()
	{
		jatayu.endMove = true;
	}
#endregion
	
}
