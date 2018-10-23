using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChunkSystem : ComponentSystem {
	
	public struct MapChunkComponent
	{
		public readonly int Length;
		public ComponentArray<LevelData> levelData;
		public ComponentArray<MapData> mapData;
	}

	public struct LevelDataComponent{
		public LevelData levelData;
	}

	public struct MapDataComponent{
		public MapData mapData;
	}

	[InjectAttribute] MapChunkComponent mapChunkComponent;
	LevelData currLevelData;
	MapData currMapData;

	bool getMapData = false;
	bool chunkLoaded = false;
	Vector2Chunk currentChunk = new Vector2Chunk();
	public Vector3 playerPos;

	int unitPerChunkX;
	int unitPerChunkY;
	int chunkLength;
	int chunkWidth;

	protected override void OnUpdate()
	{
		// if(getMapData == false){
		// 	foreach(var e in GetEntities<MapDataComponent>()){
		// 		unitPerChunkX = e.mapData.unitPerChunkX;
		// 		unitPerChunkY = e.mapData.unitPerChunkY;
		// 		chunkLength = e.mapData.chunkLength;
		// 		chunkWidth = e.mapData.chunkWidth;
		// 	}
		// 	getMapData = true;
		// }

		// foreach(var e in GetEntities<LevelDataComponent>()){
		// 	if(e.levelData.currentPlayer != null) CheckPlayerPosition(e);
		// }

		for(int i = 0;i<mapChunkComponent.Length;i++){
			currLevelData = mapChunkComponent.levelData[i];
			currMapData = mapChunkComponent.mapData[i];

			CheckPlayerPosition();
		}

	}

	void CheckPlayerPosition()
	{
		Vector2 playerPos = currLevelData.currentPlayer.transform.position;
		Vector2Chunk playerChunkPos = new Vector2Chunk(Mathf.FloorToInt(playerPos.x / currMapData.unitPerChunkX), Mathf.FloorToInt(playerPos.y / currMapData.unitPerChunkY));

		if(currentChunk.x != playerChunkPos.x || currentChunk.y != playerChunkPos.y){
			currentChunk = new Vector2Chunk(playerChunkPos.x,playerChunkPos.y);
			chunkLoaded = false;
		}

		if(!chunkLoaded){
			chunkLoaded = true;
			LoadChunks();
		}
	}

	// void CheckPlayerPosition(LevelDataComponent e)
	// {
	// 	playerPos = e.levelData.currentPlayer.transform.position;
		
	// 	Vector2Chunk playerChunkPos = new Vector2Chunk(Mathf.FloorToInt(playerPos.x / unitPerChunkX),Mathf.FloorToInt(playerPos.y / unitPerChunkY));
	// 	//  // Debug.Log("PlayerPos     : "+playerPos.x+","+playerPos.y);
	// 	//  // Debug.Log("PlayerPosChunk: "+playerChunkPos.x+","+playerChunkPos.y);
		
	// 	if(currentChunk.x != playerChunkPos.x || currentChunk.y != playerChunkPos.y){
	// 		currentChunk = new Vector2Chunk(playerChunkPos.x,playerChunkPos.y);
	// 		chunkLoaded = false;
	// 	}
		
	// 	if(!chunkLoaded){
	// 		chunkLoaded = true;
	// 		LoadChunks(currentChunk);
	// 	}
	// }

	void LoadChunks()
	{
		for(int y = -1;y<currMapData.chunkWidth;y++){
			for(int x = -1;x<currMapData.chunkLength;x++){
				if((x >= currentChunk.x-1 && x <= currentChunk.x+1) &&
				   (y >= currentChunk.y-1 && y <= currentChunk.y+1) ){
					string sceneName = GetSceneName(x,y);

					if(!SceneManager.GetSceneByName(sceneName).isLoaded){
						SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
					}
				}else{
					string sceneName = GetSceneName(x,y);
					if(SceneManager.GetSceneByName(sceneName).isLoaded){
						SceneManager.UnloadSceneAsync(sceneName);
					}
				}
			}
		}
	}


	// void LoadChunks(Vector2Chunk currentChunk)
	// {		
	// 	for(int y = -1;y<chunkWidth;y++){
	// 		for(int x = -1;x<chunkLength;x++){
	// 			if( (x >= currentChunk.x-1 && x <= currentChunk.x+1) &&
	// 			    (y >= currentChunk.y-1 && y <= currentChunk.y+1) ){
	// 				//  // Debug.Log(x+","+y);
	// 				string sceneName = GetSceneName(x,y);
	// 				// // Debug.Log(sceneName);
	// 				if(!SceneManager.GetSceneByName(sceneName).isLoaded){
	// 					SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
	// 				}
	// 			}else{
	// 				string sceneName = GetSceneName(x,y);
	// 				if(SceneManager.GetSceneByName(sceneName).isLoaded){
	// 					SceneManager.UnloadSceneAsync(sceneName);
	// 				}
	// 			}
	// 		}
	// 	}
	// }

	string GetSceneName(int x,int y)
	{
		return "MainWorld("+x+","+y+")";
	}
}

public class Vector2Chunk{
	public int x;
	public int y;

	public Vector2Chunk(){
		this.x = 0;
		this.y = 0;
	}

	public Vector2Chunk(int x, int y){
		this.x = x;
		this.y = y;
	}
}