/// <summary>
/// Contains all constant strings like tags, animator parameters, sorting layer names, and playerpref keys
/// </summary>
public static class Constants {

	public static class AnimatorParameter
	{
		public static class Int
		{
			// public const string INTERACT_VALUE = "InteractValue";
		}

		public static class Float
		{
			// public const string ATTACK_MODE = "AttackMode";
			// public const string MOVE_MODE = "MoveMode";
			// public const string SLASH_COMBO = "SlashCombo";
			
			// public const string IDLE_MODE = "IdleMode";
			public const string FACE_X = "FaceX";
			public const string FACE_Y = "FaceY";
			// public const string TOOL_TYPE = "ToolType";
			// public const string INTERACT_MODE = "InteractMode";
			// public const string LIFTING_MODE = "LiftingMode";
		}

		public static class Trigger
		{
			public const string EXPLODE = "Explode";
		}

		public static class Bool
		{
			// public const string IS_ATTACKING = "IsAttacking";
			// public const string IS_MOVING = "IsMoving";
			// public const string IS_RAPID_SLASHING = "IsRapidSlashing";
			// public const string IS_USING_TOOL = "IsUsingTool";
			// public const string IS_INTERACT = "IsInteract";
		}

	}

	public static class SortingLayerName
	{

	}

	public static class Tag
	{
		public const string PLAYER = "Player"; 
		public const string ENEMY = "Enemy"; 
		public const string PLAYER_SLASH = "Player Slash";
		public const string PLAYER_DASH_ATTACK = "Player Dash Attack"; 
		public const string PLAYER_COUNTER = "Player Counter"; 
		public const string ENEMY_ATTACK = "Enemy Attack"; 
		public const string STONE = "Stone"; 
		public const string DIG_AREA = "Dig Area"; 
		public const string DIG_RESULT = "Dig Result"; 
		public const string SWIM_AREA = "Swim Area"; 
		public const string FISHING_AREA = "Fishing Area"; 

		public const string HAMMER = "Hammer"; 
		public const string NET = "Net"; 
		public const string BOW = "Bow"; 
		public const string MAGIC_MEDALLION = "Magic Medallion";
		public const string FISH = "Fish"; 
		// public const string LIFTABLE = "Liftable"; 
		public const string LOOTABLE = "Lootable"; 
		public const string BOUNDARIES = "Boundaries";
		public const string CHEST = "Chest";
	}

	public static class PlayerPrefKey
	{

		public const string PLAYER_STATS_MAXHP = "Player/Stats/MaxHP";
		public const string PLAYER_STATS_HP = "Player/Stats/HP";
		public const string PLAYER_STATS_MAXMANA = "Player/Stats/MaxMana";
		public const string PLAYER_STATS_MANA = "Player/Stats/Mana";

		public const string PLAYER_TOOL_BOW = "Player/Tool/Bow";
		public const string PLAYER_TOOL_HOOK = "Player/Tool/Hook";
		public const string PLAYER_TOOL_BOMB = "Player/Tool/Bomb";
		public const string PLAYER_TOOL_HAMMER = "Player/Tool/Hammer";
		public const string PLAYER_TOOL_NET = "Player/Tool/Net";
		public const string PLAYER_TOOL_FISHINGROD = "Player/Tool/FishingRod";
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

	public static class BlendTreeName
	{
		#region Player
		public const string IDLE_STAND = "IdleStand";
		public const string MOVE_RUN = "MoveRun";
		public const string MOVE_DODGE = "MoveDodge";
		public const string MOVE_DASH = "MoveDash";
		public const string IDLE_DIE = "IdleDie";
		public const string IDLE_BRAKE = "IdleBrake";
		public const string IDLE_CHARGE = "IdleCharge";
		public const string MOVE_CHARGE = "MoveCharge";
		public const string IDLE_GUARD = "IdleGuard";
		public const string MOVE_GUARD = "MoveGuard";
		public const string IDLE_BULLET_TIME = "IdleBulletTime";
		public const string RAPID_SLASH_BULLET_TIME = "RapidSlashBulletTime";
		public const string IDLE_SWIM = "IdleSwim";
		public const string MOVE_SWIM = "MoveSwim";
		public const string GET_HURT = "GetHurt";
		public const string BLOCK_ATTACK = "BlockAttack";
		public const string COUNTER_ATTACK = "CounterAttack";
		public const string CHARGE_ATTACK = "ChargeAttack";
		public const string NORMAL_ATTACK_1 = "NormalAttack1";
		public const string NORMAL_ATTACK_2 = "NormalAttack2";
		public const string NORMAL_ATTACK_3 = "NormalAttack3";
		public const string GRABBING = "Grabbing";
		public const string SWEATING_GRAB = "SweatingGrab";
		public const string UNGRABBING = "UnGrabbing";
		public const string IDLE_PUSH = "IdlePush";
		public const string MOVE_PUSH = "MovePush";
		public const string LIFTING = "Lifting";
		public const string IDLE_LIFT = "IdleLift";
		public const string MOVE_LIFT = "MoveLift";
		public const string THROWING_LIFT = "ThrowingLift";
		public const string THROW_FISH_BAIT = "ThrowFishBait";
		public const string IDLE_FISHING = "IdleFishing";
		public const string RETURN_FISH_BAIT = "ReturnFishBait";
		public const string FISHING_FAIL = "FishingFail";
		public const string LIFTING_TREASURE = "LiftingTreasure";
		public const string IDLE_LIFT_TREASURE = "IdleLiftTreasure";
		public const string END_LIFT_TREASURE = "EndLiftTreasure";
		public const string TAKE_AIM_BOW = "TakeAimBow";
		public const string AIMING_BOW = "AimingBow";
		public const string OPENING_CHEST = "OpeningChest";
		public const string AFTER_OPEN_CHEST = "AfterOpenChest";
		public const string SHOT_BOW = "ShotBow";
		public const string USE_BOMB = "UseBomb";
		public const string USE_HAMMER = "UseHammer";
		public const string USE_SHOVEL = "UseShovel";
		public const string USE_MAGIC_MEDALLION = "UseMagicMedallion";
		public const string USE_CONTAINER = "UseContainer";
		#endregion

		#region Stand
		public const string STAND_TAKE_AIM_BOW = "StandTakeAimBow";
		public const string STAND_AIMING_BOW = "StandAimingBow";
		public const string STAND_SHOT_BOW = "StandShotBow";
		public const string STAND_GRABBING = "StandGrabbing";
		public const string STAND_UNGRABBING = "StandUnGrabbing";
		public const string STAND_IDLE_PUSH = "StandIdlePush";
		public const string STAND_MOVE_PUSH = "StandMovePush";
		public const string STAND_IDLE_LIFT = "StandIdleLift";
		public const string STAND_LIFTING = "StandLifting";
		public const string STAND_MOVE_LIFT = "StandMoveLift";
		public const string STAND_THROWING_LIFT = "StandThrowingLift";
		public const string STAND_DASH = "StandDash";
		public const string STAND_BOMB = "StandBomb";
		public const string STAND_MAGIC_MEDALLION = "StandMagicMedallion";
		public const string STAND_INACTIVE = "StandInactive";
		#endregion

		#region Enemy
		public const string ENEMY_IDLE = "Idle";
		public const string ENEMY_ATTACK = "Attack";
		#endregion
	}

	public static class AnimationName
	{
		public const string CHEST_OPEN = "ChestOpen";
	}
}
