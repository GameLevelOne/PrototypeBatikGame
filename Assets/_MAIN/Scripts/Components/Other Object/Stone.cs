using UnityEngine;

public class Stone : MonoBehaviour {
	// public Liftable liftable;
	public Collider stoneCollider;
	public Collider parentCollider;
	public TriggerDetection hammerTrigger;
	// public AudioSource audioSource;
	public GameObject stoneColorObj;
	public GameObject stoneGreyObj;
	// public GameObject stoneBrokenObj;
	// public ParticleSystem stoneParticle;
	public GameObject stoneCrushFX;

	[HeaderAttribute("Current")]
	public bool localhit = false;

	public bool hit {
		get {return localhit;}
		set {
			localhit = value;
			Debug.Log(value);
		}
	}

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