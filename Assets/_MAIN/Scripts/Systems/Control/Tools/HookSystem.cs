using Unity.Entities;
using UnityEngine;

public class HookSystem : ComponentSystem {

	public struct HookComponent
	{
		public readonly int Length;
		public ComponentArray<Hook> hook;
		public ComponentArray<Rigidbody2D> rigidbody;
	}

	#region injected component
	[InjectAttribute] HookComponent hookComponent;
	Hook currHook;
	Rigidbody2D currRigidbody;
	#endregion

	#region injected system
	[InjectAttribute] PlayerMovementSystem playerMovementSystem;
	[InjectAttribute] PlayerInputSystem playerInputSystem;

	Facing2D facing;
	Vector2 hookStartPos;
	#endregion
		
	int hookDirection;
	float deltaTime;
	float t = 0f;

	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;

		for(int i = 0;i<hookComponent.Length;i++){
			currHook = hookComponent.hook[i];
			currRigidbody = hookComponent.rigidbody[i];
			facing = playerMovementSystem.facing;
			hookDirection = facing.DirID;

			CheckHookState();
		}
	}

	void CheckHookState()
	{
		if(currHook.hookState == HookState.Idle && !currHook.IsHookLaunched){
			currHook.IsHookLaunched = true;
			playerInputSystem.player.IsHooking = true;
			currHook.startPos = currHook.transform.position;
			Launch();
		}else if(currHook.hookState == HookState.Launch){
			Launching();
		}else if(currHook.hookState == HookState.Catch){
			Catch();
		}else if(currHook.hookState == HookState.Return){
			Return();
		}
	}

	void Launch()
	{
		currHook.destination = GetDestinationPos(currRigidbody.position,hookDirection,currHook.range);
		currHook.hookState = HookState.Launch;
		// deltaTime = Time.deltaTime;
	}

	void Launching()
	{
		//if t <= 1 and or hook hit solid object
		if(currHook.attachedObject == null && t < 1f){
			currRigidbody.position = Vector2.Lerp(currHook.startPos,currHook.destination,t);
			t += currHook.speed * deltaTime;
		}else{
			if(t >= 1f){ //return
				currHook.hookState = HookState.Return;
			}else if(currHook.attachedObject != null){
				currHook.hookState = HookState.Catch;
			}
		} 
	}

	void Catch()
	{
		//if solid object, pull player
		//if enemy, pull enemy
		if(currHook.attachedObject.tag == Constants.Tag.ENEMY){
			//enemy lock to hook

			currHook.hookState = HookState.Return;
		}
		// else if(hook.attachedObject.tag == ""){

		// }
	}

	void Return()
	{
		if(t > 0f){
			currRigidbody.position = Vector2.Lerp(currHook.startPos,currHook.destination,t);
			t -= currHook.speed * deltaTime;
			
		}else{
			t = 0;
			currHook.hookState = HookState.Idle;
			currHook.IsHookLaunched = false;
			playerInputSystem.player.IsHooking = false;
			GameObjectEntity.Destroy(currHook.gameObject);
			UpdateInjectedComponentGroups();
		}
	}

	Vector2 GetDestinationPos(Vector2 hookInitPos, int dirID, float range)
	{
		Vector3 destination = hookInitPos;
		float x = hookInitPos.x;
		float y = hookInitPos.y;

		if(dirID == 1){ //bottom
			y-=range;
		}else if(dirID == 2){ //bottom left
			x-=range;
			y-=range;
		}else if(dirID == 3){ //left
			x-=range;
		}else if(dirID == 4){ //top left
			x-=range;
			y+=range;
		}else if(dirID == 5){ //top
			y+=range;
		}else if(dirID == 6){ //top right
			x+=range;
			y+=range;
		}else if(dirID == 7){ //right
			x+=range;
		}else if(dirID == 8){ //bottom right
			x+=range;
			y-=range;
		}

		return new Vector2(x,y);
	}
	

}