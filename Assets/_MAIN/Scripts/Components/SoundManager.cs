using UnityEngine;

public enum BGM{
	Title,
	CutScene1,
	MainBeforeCutScene22,
	CutScene22,
	MainAfterCutScene22,
	LevelJatayu,
	JatayuFight,
	GotTreasure
}

public class SoundManager : MonoBehaviour {

#region SINGLETON
	private static SoundManager instance = null;
	public static SoundManager Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<SoundManager>();
			} 

			return instance;
		}
	}

	void Awake () {
		if (instance != null && instance != this) {
			GameObject.Destroy(gameObject);
		}else{
			instance = this;
		}
		DontDestroyOnLoad(gameObject);
	}
#endregion


	public AudioSource bgmSource;
	public AudioSource gotTreasureSource;
	public float fadeSpeed = 1f;
	public AudioClip[] BGMs;

	bool isChangingBGM = false;
	bool loop;
	BGM bgmToChange;

	public void PlayBGM(BGM bgm, bool loop = true)
	{
		if(!bgmSource.isPlaying || bgmSource.clip != BGMs[(int)bgm]){

			if(bgm == BGM.GotTreasure){
				PlayGotTreasure();
			}else if(bgm == BGM.JatayuFight){
				bgmSource.clip = BGMs[(int)bgm];
				bgmSource.volume = 1f;
				bgmSource.Play();
			}else{
				if(bgmSource.clip == null){
					bgmSource.clip = BGMs[(int)bgm];
					bgmSource.volume = 1f;
					bgmSource.Play();
				}else{
					isChangingBGM = true;
					bgmToChange = bgm;
					this.loop = loop;
				}
			}
		}
	}

	void PlayGotTreasure()
	{
		gotTreasureSource.PlayOneShot(BGMs[(int)BGM.GotTreasure]);
	}

	public void StopBGM()
	{
		if(bgmSource.isPlaying) bgmSource.Stop();
	}

	void Update()
	{
		if(isChangingBGM){
			bgmSource.volume -= Time.unscaledDeltaTime * fadeSpeed;
		
			if(bgmSource.volume <= 0f){
				bgmSource.Stop();
				bgmSource.clip = BGMs[(int)bgmToChange];
				bgmSource.loop = this.loop;
				bgmSource.volume = 1f;
				bgmSource.Play();
				isChangingBGM = false;
			}
		}
	}
}