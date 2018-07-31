using Unity.Entities;
using UnityEngine;

public class HookSystem : ComponentSystem {

	public struct HookComponent
	{
		public Hook hook;
	}

	[InjectAttribute] PlayerMovementSystem playerMovementSystem;
	[InjectAttribute] PlayerInputSystem playerInputSystem;

	// public struct Facing2DComponent
	// {
	// 	public Facing2D facing2D;
	// }
	
	// bool hasSetDirection = true;

	// Hook hook;
	Facing2D facing;

	Vector2 hookStartPos;

	int hookDirection;
	float deltaTime;
	float t = 0f;

	protected override void OnUpdate()
	{
		deltaTime = Time.deltaTime;

		foreach( var e in GetEntities<HookComponent>()){
			Hook hook = e.hook;
			facing = playerMovementSystem.facing;

			hookDirection = facing.DirID;

			if(hook.hookState == HookState.Idle && !hook.IsHookLaunched){
				hook.IsHookLaunched = true;
				playerInputSystem.player.IsHooking = true;
				hook.startPos = hook.transform.position;
				Launch(hook);
			} else if(hook.hookState == HookState.Launch){
				Debug.Log("Launching");
				Launching(hook);
			}
			else if(e.hook.hookState == HookState.Catch){
				Catch(hook);
			}else if(e.hook.hookState == HookState.Return){
				Return(hook);
				// update
			}
		}

		// if(Input.GetKeyDown(KeyCode.C)){
			// if(!hasSetDirection){
			// 	foreach(var e in GetEntities<Facing2DComponent>()){
			// 		hasSetDirection = true;
			// 		hookDirection = e.facing2D.DirID;
			// 	}
			// }else{
			// 	foreach( var e in GetEntities<HookComponent>()){
			// 		if(e.hook.hookState == HookState.Idle && hasSetDirection){
			// 			Launch(e);
			// 		}else if(e.hook.hookState == HookState.Launch){
			// 			Launching(e);
			// 		}else if(e.hook.hookState == HookState.Catch){
			// 			Catch(e);
			// 		}else if(e.hook.hookState == HookState.Return){
			// 			Return(e);
			// 		}
			// 	}
			// }
		// }
	}

	void Launch(Hook hook)
	{
		hook.destination = GetDestinationPos(hook.rb.position,hookDirection,hook.range);
		hook.hookState = HookState.Launch;
		// deltaTime = Time.deltaTime;
	}

	void Launching(Hook hook)
	{
		//if t <= 1 and or hook hit solid object
		if(hook.attachedObject == null && t < 1f){
			hook.rb.position = Vector2.Lerp(hook.startPos,hook.destination,t);
			t += hook.speed * deltaTime;
		}else{
			if(t >= 1f){ //return
				hook.hookState = HookState.Return;
			}else if(hook.attachedObject != null){
				hook.hookState = HookState.Catch;
			}
		} 
	}

	void Catch(Hook hook)
	{
		//if solid object, pull player
		//if enemy, pull enemy
		if(hook.attachedObject.tag == Constants.Tag.ENEMY){
			//enemy lock to hook

			hook.hookState = HookState.Return;
		}
		// else if(hook.attachedObject.tag == ""){

		// }
	}

	void Return(Hook hook)
	{
		if(t > 0f){
			hook.rb.position = Vector2.Lerp(hook.startPos,hook.destination,t);
			t -= hook.speed * deltaTime;
			
		}else{
			t = 0;
			hook.hookState = HookState.Idle;
			// hasSetDirection = false;
			hook.IsHookLaunched = false;
			playerInputSystem.player.IsHooking = false;
			GameObjectEntity.Destroy(hook.gameObject);
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