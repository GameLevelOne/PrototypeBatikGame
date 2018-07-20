using Unity.Entities;

public class ToolSystem : ComponentSystem {

	public struct ToolSystemComponent
	{
		public ToolType currentTool;
	}

	protected override void OnUpdate()
	{
		//if player input button action, do use tool.
		foreach(var e in GetEntities<ToolSystemComponent>())
		{
			UseTool(e.currentTool);
			this.Enabled = false;
		}

	}

	void UseTool(ToolType toolType)
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

	void UseBow(){}
	void UseHook(){}
	void UseBomb(){}
	void UseHammer(){}
	void UseNet(){}
	void UseFisingRod(){}
	
	void UseContainer1(){}
	void UseContainer2(){}
	void UseContainer3(){}
	void UseContainer4(){}

	void UseShovel(){}
	void UseLantern(){}
	void UseInvisibilityCloack(){}
	void UseMagicMedallion(){}
	void UseFastTravel(){}
	void UsePowerBracelet(){}
	void UseFlippers(){}
	void UseBoots(){}
}
