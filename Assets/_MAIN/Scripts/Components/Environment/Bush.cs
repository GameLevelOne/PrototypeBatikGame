﻿using UnityEngine;

public class Bush : MonoBehaviour {
	public bool init = false;
	public bool animateEnd = false;
	public bool destroy = false;
	
	void OnBushAnimateEnd()
	{
		animateEnd = true;
	}
}
