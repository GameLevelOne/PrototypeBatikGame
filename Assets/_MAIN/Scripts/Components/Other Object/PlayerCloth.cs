using UnityEngine;

public class PlayerCloth : MonoBehaviour {
	public Player player;
	public PlayerInput playerInput;
	public Facing2D facing;
	public Health playerHealth;
	public Cloth cloth;
	public Renderer clothRenderer;
	public float addOnAccel;
	public float clothReduceValue;
	public float randomCountdownTime;

	[HeaderAttribute("0 to 2")]
	public float randomMultiplierMin;
	public float randomMultiplierMax;

	[HeaderAttribute("Current")]
	public bool isInitCLoth = false;
	public int currentDirID;
	public bool isHPChange = false;
	public bool isReducingCloth = false;
	public float randomCountdownTimer;
	public bool randomMultiplierToggle = false;
	public float addRandomAccel = 0f;

	void OnEnable () {
		playerHealth.OnHPChange += OnHPChange;
	}

	void OnDisable () {
		playerHealth.OnHPChange -= OnHPChange;
	}

	void OnHPChange () {
		isHPChange = true;
	}
}
