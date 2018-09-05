using UnityEngine;

public class LevelData : MonoBehaviour {

	public int mapHeight;
	public int mapWidth;
	public float cameraBottomBound;
	public float cameraTopBound;

	[SpaceAttribute(20f)]	
	public GameObject playerObj;
	public Vector3 playerStartPos;
	public bool isInitialied = false;
	[Header("Current")]
	public GameObject currentPlayer;


}
