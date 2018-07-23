using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class ToolSystem : ComponentSystem {

	public struct ToolComponent
	{
		public ToolType currentTool;
	}

	protected override void OnUpdate()
	{
		//if player input button action, do use tool.
	}

	public void ChangeTool(ToolComponent e, ToolType toolType)
	{
		e.currentTool = toolType;
	}

	public void UseTool(ToolType toolType)
	{
		if(toolType == ToolType.None) return;
		
		if(toolType == ToolType.Bow){
			UseBow();
		}else if(toolType == ToolType.Hook){
			UseHook();
		}else if(toolType == ToolType.Bomb){
			UseBomb();
		}else if(toolType == ToolType.Hammer){
			UseHammer();
		}else if(toolType == ToolType.Net){
			UseNet();
		}else if(toolType == ToolType.FishingRod){
			UseFisingRod();
		}else if(toolType == ToolType.Container1){
			UseContainer1();
		}else if(toolType == ToolType.Container2){
			UseContainer2();
		}else if(toolType == ToolType.Container3){
			UseContainer3();
		}else if(toolType == ToolType.Container4){
			UseContainer4();
		}else if(toolType == ToolType.Shovel){
			UseShovel();
		}else if(toolType == ToolType.Lantern){
			UseLantern();
		}else if(toolType == ToolType.InvisibilityCloak){
			UseInvisibilityCloack();
		}else if(toolType == ToolType.MagicMedallion){
			UseMagicMedallion();
		}else if(toolType == ToolType.FastTravel){
			UseFastTravel();
		}else if(toolType == ToolType.Flippers){
			UseFlippers();
		}else if(toolType == ToolType.Boots){
			UseBoots();
		}
	}

	void UseBow()
	{
		Debug.Log("Using Bow");

		//shoots projectile (arrow, limited amount).
		//shoots immediately on face direction
		//projectiles flies until hit obstacle
		//can break certain objects.

		//medium damage to enemies
	}
	
	void UseHook()
	{
		Debug.Log("Using Hook");
		//shoots projectile with rope. shoot in face direction. has range.
		//if the projectiles reaches something, do thigs:
		//1. enemy: pull the enemy towards player
		//2. solid objects: pull player towards the object
		
		//small damage to enemies
	}
	
	void UseBomb()
	{
		Debug.Log("Using Bomb");

		//plant a bomb that explodes after certain time
		//can be dropped or thrown
		//limited amount

		//big damage to enemies
	}
	
	void UseHammer()
	{
		Debug.Log("Using Hammer");
		//hammer destroy stones or destroyable solid object
		//break some enemy armor
		
		//small damage to enemy
	}
	
	void UseNet()
	{
		Debug.Log("Using Net");
		//catch certain objects (land/air)
	}
	
	void UseFisingRod()
	{
		//catch water object
		//mini game fishing when triggered
	}
	

	//containers can store certain enemies or items. (caught enemies will be stored in containers)
	void UseContainer1(){}
	
	void UseContainer2(){}
	
	void UseContainer3(){}
	
	void UseContainer4(){}

	void UseShovel()
	{
		Debug.Log("Using Shovel");
		//dig items from ground
	}
	
	void UseLantern(){}
	
	void UseInvisibilityCloack(){}
	
	void UseMagicMedallion()
	{
		Debug.Log("Using Magic Medallion");

		//big AoE Damage to all enemies on screen (?) 
		//affect some objects (destroy objects)

		//big damage
	}
	
	void UseFastTravel(){}
	
	void UsePowerBracelet()
	{
		Debug.Log("Using Power Bracelet");

		//allow to lift objects
	}
	
	void UseFlippers()
	{
		Debug.Log("Using Flippers");

		//allow to swim on water
	}
	
	void UseBoots()
	{
		Debug.Log("Using Boots");
		
		//allow dash attack
		//dash straight until hit an obstacle
		//bounce back on impact
	}
}