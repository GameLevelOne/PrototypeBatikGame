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

	public TriggerDetection playerAttackTriggerDetection;
	public Dissolver dissolver;
	public Liftable liftable;

	[HeaderAttribute("Current")]
	public bool init = false;
	public bool localhit = false;

	public bool hit {
		get {return localhit;}
		set {
			localhit = value;
			 // Debug.Log(value);
		}
	}

	void OnEnable()
	{
		hammerTrigger.OnTriggerEnterObj += OnHit;
		playerAttackTriggerDetection.OnTriggerEnterObj += OnTriggerEnterObj;
	}

	void OnDisable()
	{
		hammerTrigger.OnTriggerEnterObj -= OnHit;
		playerAttackTriggerDetection.OnTriggerEnterObj -= OnTriggerEnterObj;
	}

	void OnTriggerEnterObj(GameObject g)
	{
		dissolver.dissolve = true;
		liftable.liftableType = LiftableType.LIFTABLE;
	}

	void OnHit(GameObject other)
	{
		if(other != null) hit = true;
	}
}