using UnityEngine;

public enum FaderState{
	Clear,
	Black,
	FadeIn,
	FadeOut
}

public class UIFader : MonoBehaviour {
	public FaderState state;
	public bool initClear = false;
	public bool initBlack = false;
	public bool initFadingIn = false;
	public bool initFadingOut = false;

}
