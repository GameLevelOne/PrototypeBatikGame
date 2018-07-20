using Unity.Entities;

public class ToolSystem : ComponentSystem {

	public struct ToolSystemComponent
	{
		public ToolType currentTool;
	}

	protected override void OnUpdate()
	{
		//if player input button action, do use tool.
	}

	void UseTool(ToolType toolType)
	{
		if(toolType == ToolType.None) return;
		

		if(toolType == ToolType.Bow){

		}else if(toolType == ToolType.Hook){

		}else if(toolType == ToolType.Bomb){
			
		}else if(toolType == ToolType.Hammer){
			
		}else if(toolType == ToolType.Net){
			
		}else if(toolType == ToolType.FishingRod){
			
		}else if(toolType == ToolType.Container1){
			
		}else if(toolType == ToolType.Container2){
			
		}else if(toolType == ToolType.Container3){
			
		}else if(toolType == ToolType.Container4){
			
		}else if(toolType == ToolType.Shovel){
			
		}else if(toolType == ToolType.Lantern){
			
		}else if(toolType == ToolType.InvisibilityCloak){
			
		}else if(toolType == ToolType.MagicMedallion){
			
		}else if(toolType == ToolType.FastTravel){
			
		}else if(toolType == ToolType.Flippers){
			
		}else if(toolType == ToolType.Boots){
			
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
