using UnityEngine;

public class PlayerCloth : MonoBehaviour {
	public Player player;
	public Facing2D facing;
	public Health playerHealth;
	public Cloth cloth;
	public Renderer clothRenderer;
	public float addOnAccel;
	public float clothReduceValue;

	public int currentDirID;
	public bool isHPChange;
	public bool isInitCLoth;

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
