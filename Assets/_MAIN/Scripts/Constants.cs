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
		}

		public static class Trigger
		{
			
		}

		public static class Bool
		{
			public const string IS_ATTACKING = "IsAttacking";
			public const string IS_DODGING = "isDodging";
			public const string IS_MOVING = "IsMoving";
		}

	}

	public static class SortingLayerName
	{

	}

	public static class Tag
	{
		public const string PLAYER_ATTACK = "Player Attack"; 
		public const string ENEMY_ATTACK = "Enemy Attack"; 
		public const string HAMMER = "Hammer"; 
	}

	public static class PlayerPrefKey
	{

	}
}
