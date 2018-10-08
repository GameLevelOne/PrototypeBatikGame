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
	public AudioClip[] BGMs;

	public void PlayBGM(BGM bgm, bool loop = true)
	{
		if(!bgmSource.isPlaying || bgmSource.clip != BGMs[(int)bgm]){
			bgmSource.Stop();
			bgmSource.clip = BGMs[(int)bgm];
			bgmSource.loop = loop;
			bgmSource.Play();
		}
	}

	public void StopBGM()
	{
		if(bgmSource.isPlaying) bgmSource.Stop();
	}
}