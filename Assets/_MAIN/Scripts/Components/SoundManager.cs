using UnityEngine;

public enum BGM{

}

public enum SFX{

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
		}
		DontDestroyOnLoad(gameObject);
	}
#endregion
	public AudioSource bgmSource;
	public AudioSource sfxSource;

	public AudioClip[] BGMs;
	public AudioClip[] SFXs;
	[HeaderAttribute("Current")]
	public bool init = false;

	public void PlaySFX(SFX sfx)
	{
		sfxSource.PlayOneShot(SFXs[(int)sfx]);
	}

	public void PlayBGM(BGM bgm)
	{
		bgmSource.Stop();
		bgmSource.clip = BGMs[(int)bgm];
		bgmSource.Play();
	}	
}