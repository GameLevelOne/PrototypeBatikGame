using UnityEngine;
using UnityEngine.UI;

public class UIHPManaTool : MonoBehaviour {
	public Player player;

	[HeaderAttribute("UI HP Attributes")]
	public Health playerHealth;
	// public Image clothHP;
	public Image clothHPMask;
	public ParticleSystem reducedHPFX;
	// public float initClothPosX;
	// public float maxClothWidth;
	// public float healthReduceValue;
	public bool isHPChange = false;
	
	[HeaderAttribute("UI Mana Attributes")]	
	public Mana playerMana;
	public Image imageMP;
	public Image imageShadowMP;
	public float manaReduceValue;
	public bool isMPChange = false;
	public bool isReducingShadowMP = false;
	public GameObject keyIcon;
	
	[SpaceAttribute(10f)]
	// public Wavy wavy;
	// public Sprite[] spritesHP;
	// public Transform clothHP;
	public Image imageTool;
	public Sprite[] toolSprite;
	public Sprite[] containerSprite;
	public bool isInitHPManaImage = false;
	public bool isToolChange = false;

	public bool showMoney;
	public Animator moneyAnim;
	public Text moneyLabel;
	public int curMoney;
	public float showMoneyDelay;

	void OnEnable () {
		playerHealth.OnHPChange += OnHPChange;
		playerMana.OnManaChange += OnManaChange;
	}

	void OnDisable () {
		playerHealth.OnHPChange -= OnHPChange;
		playerMana.OnManaChange -= OnManaChange;
	}

	void OnHPChange () {
		isHPChange = true;
	}
	
	void OnManaChange () {
		isMPChange = true;
	}
}
