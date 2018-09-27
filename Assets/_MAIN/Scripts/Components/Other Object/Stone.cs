using UnityEngine;

public class Stone : MonoBehaviour {
	public Collider stoneCollider;
	public TriggerDetection hammerTrigger;
	public GameObject stoneColorObj;
	public GameObject stoneGreyObj;
	public GameObject stoneBrokenObj;
	public ParticleSystem stoneParticle;
	[HeaderAttribute("Current")]
	public bool hit = false;

	void OnEnable()
	{
		hammerTrigger.OnTriggerEnterObj += OnHit;
	}

	void OnDisable()
	{
		hammerTrigger.OnTriggerEnterObj -= OnHit;
	}

	void OnHit(GameObject other)
	{
		if(other != null) hit = true;
	}
}