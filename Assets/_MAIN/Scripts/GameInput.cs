using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInput {

#region MOVEMENT
public static bool IsUpDirectionHeld {
	get {
		if (Input.GetAxis("Vertical Javatale")>0f)
			return true;
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
			return true;		
		return false;
	}
}
public static bool IsDownDirectionHeld {
	get {
		if (Input.GetAxis("Vertical Javatale")<0f)
			return true;
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
			return true;		
		return false;
	}
}
public static bool IsLeftDirectionHeld {
	get {
		if (Input.GetAxis("Horizontal Javatale")<0f)
			return true;
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
			return true;		
		return false;
	}
}
public static bool IsRightDirectionHeld {
	get {
		if (Input.GetAxis("Horizontal Javatale")>0f)
			return true;
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
			return true;		
		return false;
	}
}

#endregion

#region MAIN_ACTION
public static bool IsAttackPressed {
	get {
		if (Input.GetKeyDown(KeyCode.Joystick1Button1))
			return true;
		if (Input.GetKeyDown(KeyCode.KeypadEnter))
			return true;		
		if (Input.GetButtonDown("Fire1"))
			return true;
		return false;
	}
}
public static bool IsAttackReleased {
	get {
		if (Input.GetKeyUp(KeyCode.Joystick1Button1))
			return true;
		if (Input.GetKeyUp(KeyCode.KeypadEnter))
			return true;		
		if (Input.GetButtonUp("Fire1"))
			return true;
		return false;
	}
}
public static bool IsAttackHeld {
	get {
		if (Input.GetKey(KeyCode.Joystick1Button1))
			return true;
		if (Input.GetKey(KeyCode.KeypadEnter))
			return true;		
		if (Input.GetButton("Fire1"))
			return true;
		return false;
	}
}
public static bool IsDodgePressed {
	get {
		if (Input.GetKeyDown(KeyCode.Joystick1Button0))
			return true;
		if (Input.GetKeyDown(KeyCode.Keypad0))
			return true;		
		return false;
	}
}
public static bool IsDodgeReleased {
	get {
		if (Input.GetKeyUp(KeyCode.Joystick1Button0))
			return true;
		if (Input.GetKeyUp(KeyCode.Keypad0))
			return true;		
		return false;
	}
}
public static bool IsDodgeHeld {
	get {
		if (Input.GetKey(KeyCode.Joystick1Button0))
			return true;
		if (Input.GetKey(KeyCode.Keypad0))
			return true;		
		return false;
	}
}
public static bool IsToolsPressed {
	get {
		if (Input.GetKeyDown(KeyCode.Joystick1Button2)) 
			return true;
		if (Input.GetKeyDown(KeyCode.KeypadPlus)) 
			return true;		
		return false;
	}
}
public static bool IsToolsReleased {
	get {
		if (Input.GetKeyUp(KeyCode.Joystick1Button2))
			return true;
		if (Input.GetKeyUp(KeyCode.KeypadPlus))
			return true;		
		return false;
	}
}
public static bool IsToolsHeld {
	get {
		if (Input.GetKey(KeyCode.Joystick1Button2))
			return true;
		if (Input.GetKey(KeyCode.KeypadPlus))
			return true;		
		return false;
	}
}
public static bool IsGuardPressed {
	get {
		if (Input.GetKeyDown(KeyCode.Joystick1Button3))
			return true;
		if (Input.GetKeyDown(KeyCode.KeypadPeriod))
			return true;		
		if (Input.GetButtonDown("Fire2"))
			return true;
		return false;
	}
}
public static bool IsGuardReleased {
	get {
		if (Input.GetKeyUp(KeyCode.Joystick1Button3))
			return true;
		if (Input.GetKeyUp(KeyCode.KeypadPeriod))
			return true;		
		if (Input.GetButtonUp("Fire2"))
			return true;
		return false;
	}
}
public static bool IsGuardHeld {
	get {
		if (Input.GetKey(KeyCode.Joystick1Button3))
			return true;
		if (Input.GetKey(KeyCode.KeypadPeriod))
			return true;		
		if (Input.GetButton("Fire2"))
			return true;
		return false;
	}
}

#endregion

#region SUB_ACTION
public static bool IsBowPressed {
	get {
		if (Input.GetKeyDown(KeyCode.Joystick1Button6) || Input.GetKeyDown(KeyCode.Joystick1Button8))
			return true;
		if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Space))
			return true;		
		return false;
	}
}
public static bool IsBowReleased {
	get {
		if (Input.GetKeyUp(KeyCode.Joystick1Button6) || Input.GetKeyUp(KeyCode.Joystick1Button8))
			return true;
		if (Input.GetKeyUp(KeyCode.Keypad5) || Input.GetKeyUp(KeyCode.Space))
			return true;		
		return false;
	}
}
public static bool IsBowHeld {
	get {
		if (Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.Joystick1Button8))
			return true;
		if (Input.GetKey(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Space))
			return true;		
		return false;
	}
}
public static bool IsInventoryPressed {
	get {
		if (Input.GetKeyDown(KeyCode.Joystick1Button7))
			return true;
		if (Input.GetKeyDown(KeyCode.Tab))
			return true;		
		return false;
	}
}
public static bool IsInventoryReleased {
	get {
		if (Input.GetKeyUp(KeyCode.Joystick1Button7))
			return true;
		if (Input.GetKeyUp(KeyCode.Tab))
			return true;		
		return false;
	}
}
public static bool IsInventoryHeld {
	get {
		if (Input.GetKey(KeyCode.Joystick1Button7))
			return true;
		if (Input.GetKey(KeyCode.Tab))
			return true;		
		return false;
	}
}
public static bool IsQuickLPressed {
	get {
		if (Input.GetKeyDown(KeyCode.Joystick1Button4))
			return true;
		if (Input.GetKeyDown(KeyCode.Q))
			return true;		
		return false;
	}
}
public static bool IsQuickLReleased {
	get {
		if (Input.GetKeyUp(KeyCode.Joystick1Button4))
			return true;
		if (Input.GetKeyUp(KeyCode.Q))
			return true;		
		return false;
	}
}
public static bool IsQuickLHeld {
	get {
		if (Input.GetKey(KeyCode.Joystick1Button4))
			return true;
		if (Input.GetKey(KeyCode.Q))
			return true;		
		return false;
	}
}
public static bool IsQuickRPressed {
	get {
		if (Input.GetKeyDown(KeyCode.Joystick1Button5))
			return true;
		if (Input.GetKeyDown(KeyCode.E))
			return true;		
		return false;
	}
}
public static bool IsQuickRReleased {
	get {
		if (Input.GetKeyUp(KeyCode.Joystick1Button5))
			return true;
		if (Input.GetKeyUp(KeyCode.E))
			return true;		
		return false;
	}
}
public static bool IsQuickRHeld {
	get {
		if (Input.GetKey(KeyCode.Joystick1Button5))
			return true;
		if (Input.GetKey(KeyCode.E))
			return true;		
		return false;
	}
}
#endregion

#region CONVINIENCES
public static bool AnyButtonPressed {
	get {
		if (IsAttackPressed || IsDodgePressed || IsGuardPressed || IsToolsPressed || IsBowPressed || IsInventoryPressed || IsQuickLPressed || IsQuickRPressed)
			return true;
		return false;
	}
}
public static bool AnyButtonReleased {
	get {
		if (IsAttackReleased || IsDodgeReleased || IsGuardReleased || IsToolsReleased || IsBowReleased || IsInventoryReleased || IsQuickLReleased || IsQuickRReleased)
			return true;		
		return false;
	}
}
public static bool AnyButtonHeld {
	get {
		if (IsAttackHeld || IsDodgeHeld || IsGuardHeld || IsToolsHeld || IsBowHeld || IsInventoryHeld || IsQuickLHeld || IsQuickRHeld)
			return true;		
		return false;
	}
}
#endregion

}
