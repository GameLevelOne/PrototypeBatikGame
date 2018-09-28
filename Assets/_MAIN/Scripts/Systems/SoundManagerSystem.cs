using Unity.Entities;
using UnityEngine;

public class SoundManagerSystem : ComponentSystem {

	public struct SoundManagerComponent{
		public readonly int Length;
		public ComponentArray<SoundManager> soundManager;
	}

	[InjectAttribute] SoundManagerComponent soundManagerComponent;
	SoundManager soundManager;

	protected override void OnUpdate()
	{
		for(int i = 0;i<soundManagerComponent.Length;i++){
			soundManager = soundManagerComponent.soundManager[i];

			CheckInit();
		}
	}

	void CheckInit()
	{
		if(!soundManager.init){
			soundManager.init = true;
			
		}
	}

	public void PlaySFX(SFX sfx)
	{
		soundManager.sfxSource.PlayOneShot(soundManager.SFXs[(int)sfx]);
	}

	public void PlayBGM(BGM bgm)
	{
		soundManager.bgmSource.Stop();
		soundManager.bgmSource.clip = soundManager.BGMs[(int)bgm];
		soundManager.bgmSource.Play();
	}
}
