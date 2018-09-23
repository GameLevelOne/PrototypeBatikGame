using Unity.Entities;
using UnityEngine;

public class CameraMovementSystem : ComponentSystem {

	public struct CameraMovementComponent{
		public readonly int Length;
		public ComponentArray<Camera> camera;
		public ComponentArray<Transform> cameraTransform;
		public ComponentArray<CameraMovement> cameraMovement;
	}

	public struct LevelDataComponent{
		public readonly int Length;
		public ComponentArray<LevelData> levelData;
	}

	[InjectAttribute] CameraMovementComponent cameraMovementComponent;
	Camera currCamera;
	Transform currCameraTransform;
	CameraMovement currCameraMovement;

	[InjectAttribute] LevelDataComponent levelDataComponent;
	LevelData currLevelData;

	float deltaTime;

	int mapWidth;
	int mapHeight;
	bool initLevelData;

	float cameraSize;
	float cameraWidth;
	float cameraHeight;

	float tZoom = 0f;

	protected override void OnUpdate()
	{
		
		for(int i = 0;i<levelDataComponent.Length;i++){
			currLevelData = levelDataComponent.levelData[i];
			GetLevelData();
		}
	
		for(int i = 0;i<cameraMovementComponent.Length;i++){
			currCamera = cameraMovementComponent.camera[i];
			currCameraTransform = cameraMovementComponent.cameraTransform[i];
			currCameraMovement = cameraMovementComponent.cameraMovement[i];
			GetCameraData();
			MoveCamera();
		}

		if((currCameraMovement!=null) && (currCameraMovement.isZooming)){    //<---------Mot: Check if currCameraMovement not Null
			Zoom();
		}
		
	}

	void GetLevelData()
	{
		if(!currLevelData.isInitialied){
			currLevelData.isInitialied = true;

			mapWidth = currLevelData.mapWidth;
			mapHeight = currLevelData.mapHeight;

			deltaTime = Time.fixedDeltaTime;
		}
	}

	void GetCameraData()
	{
		cameraSize = currCamera.orthographicSize;
		cameraHeight = cameraSize * 2;
		cameraWidth = cameraHeight * Screen.width / Screen.height;
	}

	void Zoom()
	{
		float startSize = currCamera.orthographicSize;
		float zoomValue = currCameraMovement.zoomValue;

		currCamera.orthographicSize = Mathf.Lerp(startSize,zoomValue,Mathf.SmoothStep(0,1,tZoom));
		tZoom += deltaTime * currCameraMovement.zoomSpeed;
		if(tZoom >= 0.8f || Mathf.Abs(currCamera.orthographicSize-zoomValue) <= 0.01f){
			currCamera.orthographicSize = zoomValue;
			currCameraMovement.isZooming = false;
			tZoom = 0f;
		}
	}

	void MoveCamera()
	{
		Debug.Log("CameraMOve");
		Vector3 destPos = currCameraMovement.playerTransform.position + currCameraMovement.offset;
		Vector3 smoothedPos = Vector3.Lerp(currCameraTransform.position,destPos,currCameraMovement.smoothSpeed * deltaTime);
		currCameraTransform.position = smoothedPos;
		currCameraTransform.position = ValidateCamBoundraries();
	}	

	Vector3 ValidateCamBoundraries()
	{
		float x = GetX();
		float y = currCameraMovement.offset.y;
		float z = GetZ();
		return new Vector3(x,y,z);
	}

	float GetX()
	{
		if(currCameraTransform.position.x <= cameraWidth/2f){
			return cameraWidth/2f;
		}else if(currCameraTransform.position.x >= mapWidth-(cameraWidth/2f)){
			return mapWidth-(cameraWidth/2f);
		}else{
			return currCameraTransform.position.x;
		}
	}

	float GetZ()
	{
		// Debug.Log((cameraHeight/2f)-9.4f);
		// Debug.Log((mapHeight-(cameraHeight/2f)) - 14.4f);
		if(currCameraTransform.position.z <= 7f + currCameraMovement.offset.z ){
			return 7f + currCameraMovement.offset.z;
		}else if(currCameraTransform.position.z >= (mapHeight- 7f) + currCameraMovement.offset.z){
			return (mapHeight- 7f) + currCameraMovement.offset.z;
		}else{
			return currCameraTransform.position.z;
		}
	}
}