using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizedVector : MonoBehaviour {

	public Transform A,B;

	public void Execute()
	{
		Vector3 distance = B.position - A.position;
		// Debug.Log("Distance = "+distance);
		float magnitude = distance.magnitude;
		// Debug.Log("Distance Magnitude = "+magnitude);
		Vector3 direction = distance / magnitude;
		// Debug.Log("Direction = "+direction);

		float x = direction.x;
		float y = direction.y;
		// Debug.Log(x);
		// Debug.Log(y);

		if(Mathf.Abs (x) >= Mathf.Abs (y)){
			if(x >= 0) Debug.Log(Direction.Right);
			else Debug.Log (Direction.Left);
		}else{
			if(y >= 0) Debug.Log(Direction.Up);
			else Debug.Log(Direction.Down);
		}
	}
}
