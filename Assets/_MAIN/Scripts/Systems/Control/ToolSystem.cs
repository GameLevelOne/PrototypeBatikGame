using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class ToolSystem : ComponentSystem {

	public struct ToolComponent
	{
		public PlayerTool playerTool;
	}

	protected override void OnUpdate()
	{
		//if player input button action, do use tool.
		if(Input.GetKeyDown(KeyCode.Space)){
			foreach(var e in GetEntities<ToolComponent>()){
				if(!e.playerTool.isUsingTool){
					e.playerTool.isUsingTool = true;
					UseTool(e);
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.C)){
			foreach(var e in GetEntities<ToolComponent>()){
				if(!e.playerTool.isUsingTool){
					ChangeTool(e);
				}
			}
		}
	}

	public void ChangeTool(ToolComponent e)
	{
		int current = (int) e.playerTool.currentTool;
		
		if(current >= ((int)ToolType.Boots)){
			current = 1;
		}else{
			current++;
		}

		e.playerTool.currentTool = (ToolType) current;
		e.playerTool.textToolName.text = ((ToolType) current).ToString();
	}

	public void UseTool(ToolComponent e)
	{
		if(e.playerTool.currentTool == ToolType.None) return;
		
		if(e.playerTool.currentTool == ToolType.Bow){
			UseBow();
		}else if(e.playerTool.currentTool == ToolType.Hook){
			UseHook();
		}else if(e.playerTool.currentTool == ToolType.Bomb){
			UseBomb();
		}else if(e.playerTool.currentTool == ToolType.Hammer){
			UseHammer();
		}else if(e.playerTool.currentTool == ToolType.Net){
			UseNet();
		}else if(e.playerTool.currentTool == ToolType.FishingRod){
			UseFisingRod();
		}else if(e.playerTool.currentTool == ToolType.Container1){
			UseContainer1();
		}else if(e.playerTool.currentTool == ToolType.Container2){
			UseContainer2();
		}else if(e.playerTool.currentTool == ToolType.Container3){
			UseContainer3();
		}else if(e.playerTool.currentTool == ToolType.Container4){
			UseContainer4();
		}else if(e.playerTool.currentTool == ToolType.Shovel){
			UseShovel();
		}else if(e.playerTool.currentTool == ToolType.Lantern){
			UseLantern();
		}else if(e.playerTool.currentTool == ToolType.InvisibilityCloak){
			UseInvisibilityCloack();
		}else if(e.playerTool.currentTool == ToolType.MagicMedallion){
			UseMagicMedallion();
		}else if(e.playerTool.currentTool == ToolType.FastTravel){
			UseFastTravel();
		}else if(e.playerTool.currentTool == ToolType.Flippers){
			UseFlippers();
		}else if(e.playerTool.currentTool == ToolType.Boots){
			UseBoots();
		}
		e.playerTool.isUsingTool = false;
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
		Debug.Log("Using Fishing Rod");
		//catch water object
		//mini game fishing when triggered
	}
	

	//containers can store certain enemies or items. (caught enemies will be stored in containers)
	void UseContainer1()
	{
		Debug.Log("Using Container1");

	}
	
	void UseContainer2()
	{
		Debug.Log("Using Container2");
	}
	
	void UseContainer3()
	{
		Debug.Log("Using Container3");
	}
	
	void UseContainer4()
	{
		Debug.Log("Using Container4");
	}

	void UseShovel()
	{
		Debug.Log("Using Shovel");
		//dig items from ground
	}
	
	void UseLantern()
	{
		Debug.Log("Using Lantern");
	}
	
	void UseInvisibilityCloack()
	{
		Debug.Log("Using Invisibility Cloak");
	}
	
	void UseMagicMedallion()
	{
		Debug.Log("Using Magic Medallion");

		//big AoE Damage to all enemies on screen (?) 
		//affect some objects (destroy objects)

		//big damage
	}
	
	void UseFastTravel()
	{
		Debug.Log("Using Fast Travel");
	}
	
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