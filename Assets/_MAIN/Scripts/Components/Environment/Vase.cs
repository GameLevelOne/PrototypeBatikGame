using UnityEngine;

public class Vase : MonoBehaviour {
	public Collider vaseCollider;
	public TriggerDetection vaseTrigger;
	public GameObject vaseIdle;
	public GameObject vaseGreyIdle;
	public GameObject vaseBroken;
	public ParticleSystem particle;
	[SpaceAttribute(10f)]
	public float lootDropProbability;
	[HeaderAttribute("Current")]
	public bool destroy = false;
	
	void OnEnable()
	{
		vaseTrigger.OnTriggerEnterObj += OnHit;
	}

	void OnDisable()
	{
		vaseTrigger.OnTriggerEnterObj -= OnHit;
	}

	void OnHit(GameObject other)
	{
		if(other != null) destroy = true;
	}
}
