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

	int hookDirection;
	Vector2 hookDestination;

	protected override void OnUpdate()
	{
		if(!hasSetDirection){
			foreach(var e in GetEntities<Facing2DComponent>()){
				SetHookDirection(e);
			}
		}else{
			foreach(var e in GetEntities<HookComponent>()){
					if(!e.hook.isLaunching){
						Launch(e);
					}else if(e.hook.isLaunching){
						Launching(e);
					}
				}
			
		}
		
	}
	
	void SetHookDirection(Facing2DComponent e)
	{
		hasSetDirection = true;
		hookDirection = e.facing2D.DirID;
	}

	void Launch(HookComponent e)
	{
		e.hook.isLaunching = true;
		hookDestination = GetDestinationPos(e.hook.transform.position, hookDirection,e.hook.range);
	}

	void Launching(HookComponent e)
	{
		e.hook.rb.position = Vector2.MoveTowards(e.hook.rb.position,hookDestination,e.hook.speed*Time.deltaTime);


	}

	Vector3 GetDestinationPos(Vector3 hookInitPos, int dirID, float range)
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