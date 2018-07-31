/// <summary>
/// Contains all constant strings like tags, animator parameters, sorting layer names, and playerpref keys
/// </summary>
public static class Constants {

	public static class AnimatorParameter
	{
		public static class Int
		{
			
		}

		public static class Float
		{
			public const string ATTACK_MODE = "AttackMode";
			public const string MOVE_MODE = "MoveMode";
			public const string SLASH_COMBO = "SlashCombo";
			
			public const string IDLE_MODE = "IdleMode";
			public const string FACE_X = "FaceX";
			public const string FACE_Y = "FaceY";
			public const string TOOL_TYPE = "ToolType";
		}

		public static class Trigger
		{
			public const string EXPLODE = "Explode";
		}

		public static class Bool
		{
			public const string IS_ATTACKING = "IsAttacking";
			public const string IS_DODGING = "IsDodging";
			public const string IS_MOVING = "IsMoving";
			public const string IS_RAPID_SLASHING = "IsRapidSlashing";
			public const string IS_USING_TOOL = "IsUsingTool";
			public const string IS_DASHING = "IsDashing";
		}

	}

	public static class SortingLayerName
	{

	}

	public static class Tag
	{
		public const string PLAYER = "Player"; 
		public const string ENEMY = "Enemy"; 
		public const string PLAYER_ATTACK = "Player Attack"; 
		public const string PLAYER_COUNTER = "Player Counter"; 
		public const string ENEMY_ATTACK = "Enemy Attack"; 
		public const string STONE = "Stone"; 
		public const string DIG_AREA = "Dig Area"; 
		public const string DIG_RESULT = "Dig Result"; 

		public const string HAMMER = "Hammer"; 
		public const string NET = "Net"; 
		public const string BOW = "Bow"; 
		public const string MAGIC_MEDALLION = "Magic Medallion";

	}

	public static class PlayerPrefKey
	{

		public const string PLAYER_STATS_MAXHP = "Player/Stats/MaxHP";

		public const string PLAYER_TOOL_BOW = "Player/Tool/Bow";
		public const string PLAYER_TOOL_HOOK = "Player/Tool/Bow";
		public const string PLAYER_TOOL_BOMB = "Player/Tool/Bomb";
		public const string PLAYER_TOOL_HAMMER = "Player/Tool/Hammer";
		public const string PLAYER_TOOL_NET = "Player/Tool/Net";
		public const string PLAYER_TOOL_FISHINGROD = "Player/Tool/Net";
		public const string PLAYER_TOOL_CONTAINER1 = "Player/Tool/Container1";
		public const string PLAYER_TOOL_CONTAINER2 = "Player/Tool/Container2";
		public const string PLAYER_TOOL_CONTAINER3 = "Player/Tool/Container3";
		public const string PLAYER_TOOL_CONTAINER4 = "Player/Tool/Container4";
		public const string PLAYER_TOOL_SHOVEL = "Player/Tool/Shovel";
		public const string PLAYER_TOOL_LANTERN = "Player/Tool/Lantern";
		public const string PLAYER_TOOL_INVISIBILITYCLOAK = "Player/Tool/InvisibilityCloak";
		public const string PLAYER_TOOL_MAGICMEDALLION = "Player/Tool/MagicMedallion";
		public const string PLAYER_TOOL_FASTTRAVEL = "Player/Tool/FastTravel";
		public const string PLAYER_TOOL_POWERBRACELET = "Player/Tool/PowerBracelet";
		public const string PLAYER_TOOL_FLIPPERS = "Player/Tool/Flippers";
		public const string PLAYER_TOOL_BOOTS = "Player/Tool/Boots";
	}
}
