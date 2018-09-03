using UnityEngine;
using System.Collections.Generic;

public class Dissolver : MonoBehaviour {
	public List<Renderer> mRenderer;
	[RangeAttribute(0f,1f)]
	public List<float> dissolveValue;

	public float dissolveSpeed = 0.25f;
	public bool init = false;
	public bool dissolve = false;
	public bool unDissolve = false;
}
