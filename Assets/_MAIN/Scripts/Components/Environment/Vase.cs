using UnityEngine;

public class Vase : MonoBehaviour {
	public Collider vaseCollider;
	public GameObject vaseIdle;
	public GameObject vaseGreyIdle;
	public GameObject vaseBroken;
	public ParticleSystem particle;
	[SpaceAttribute(10f)]
	public float lootDropProbability;
	[HeaderAttribute("Current")]
	public bool destroy = false;
	
}
