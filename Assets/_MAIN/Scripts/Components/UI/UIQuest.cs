using UnityEngine;
using UnityEngine.UI;

public class UIQuest : MonoBehaviour {
	// public Text mainQuestText;
	public Text[] questTexts;
	public Image[] questImages;
	// public Text[] completeTexts;
	public Sprite unCompleteSprite;
	public Sprite completeSprite;
	public Color initTextColor = new Color (110, 40, 0);
	public Color completedTextColor = new Color (50, 150, 150);
	// public Outline completedOutline;

	[HeaderAttribute("Current")]
	public bool isInitUIQuest;
	public bool isModifyUI;
}
