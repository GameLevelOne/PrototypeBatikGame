using Unity.Entities;
using UnityEngine;

public class HookSystem : ComponentSystem {

	public struct HookComponent
	{
		public Hook hook;
	}

	public struct Facing2DComponent
	{
		public Facing2D facing2D;
	}
	
	bool hasSetDirection = false;

	Vector2 hookStartPos;

	int hookDirection;
	float deltaTime;
	float t = 0f;

	protected override void OnUpdate()
	{
		if(Input.GetKeyDown(KeyCode.C)){
			if(!hasSetDirection){
				foreach(var e in GetEntities<Facing2DComponent>()){
					hasSetDirection = true;
					hookDirection = e.facing2D.DirID;
				}
			}else{
				foreach( var e in GetEntities<HookComponent>()){
					if(e.hook.hookState == HookState.Idle && hasSetDirection){
						Launch(e);
					}else if(e.hook.hookState == HookState.Launch){
						Launching(e);
					}else if(e.hook.hookState == HookState.Catch){
						Catch(e);
					}else if(e.hook.hookState == HookState.Return){
						Return(e);
					}
				}
			}
		}
	}

	void Launch(HookComponent e)
	{
		e.hook.destination = GetDestinationPos(e.hook.rb.position,hookDirection,e.hook.range);		
		e.hook.hookState = HookState.Launch;
		deltaTime = Time.deltaTime;
	}

	void Launching(HookComponent e)
	{

		//if t <= 1 and or hook hit solid object
		if(e.hook.attachedObject == null && t < 1f){
			e.hook.rb.position = Vector2.Lerp(e.hook.startPos,e.hook.destination,t);
			t += e.hook.speed * deltaTime;
		}else{
			if(t >= 1f){ //return
				e.hook.hookState = HookState.Return;
			}else if(e.hook.attachedObject != null){
				e.hook.hookState = HookState.Catch;
			}
		} 
	}

	void Catch(HookComponent e)
	{
		//if solid object, pull player
		//if enemy, pull enemy
		if(e.hook.attachedObject.tag == Constants.Tag.ENEMY){
			//enemy lock to hook

			if(t > 0f){
				e.hook.rb.position = Vector2.Lerp(e.hook.startPos,e.hook.destination,t);
				t -= e.hook.speed * deltaTime;
			}

		}else if(e.hook.attachedObject.tag == ""){

		}

	}

	void Return(HookComponent e)
	{
		if(t > 0f){
			e.hook.rb.position = Vector2.Lerp(e.hook.startPos,e.hook.destination,t);
			t -= e.hook.speed * deltaTime;
			
		}else{
			t = 0;
			e.hook.hookState = HookState.Idle;
			hasSetDirection = false;
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