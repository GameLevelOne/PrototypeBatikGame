using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraShaker : MonoBehaviour {
	public Vector3 originPos;
	public float shakeMagnitude;
	[RangeAttribute(0.02f,0.2f)]
	public float shakeDelay;
	[HeaderAttribute("Current")]
	public bool shake;
	public float tShakeDelay = 0;
	public PostProcessingBehaviour blurBehaviour;
}
