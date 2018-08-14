using Unity.Entities;
using UnityEngine;

public class CameraMovementSystem : ComponentSystem {

	public struct CameraMovementComponent{
		public readonly int Length;
		public ComponentArray<Transform> cameraTransform;
		public ComponentArray<CameraMovement> cameraMovement;
	}

	[InjectAttribute] CameraMovementComponent cameraMovementComponent;
	public Transform currCameraTransform;
	public CameraMovement currCameraMovement;

	float deltaTime;

	protected override void OnUpdate()
	{
		for(int i = 0;i<cameraMovementComponent.Length;i++){
			currCameraTransform = cameraMovementComponent.cameraTransform[i];
			currCameraMovement = cameraMovementComponent.cameraMovement[i];
			CheckBoundaries();
			MoveCamera();
		}
	}

	void CheckBoundaries()
	{
		if( currCameraMovement.playerTransform.position.x <= currCameraMovement.minBound.x || 
			currCameraMovement.playerTransform.position.x >= currCameraMovement.maxBound.x ){
			currCameraMovement.isMovingX = false;
		}else{
			currCameraMovement.isMovingX = true;
		}	

		if( currCameraMovement.playerTransform.position.y <= currCameraMovement.minBound.y || 
			currCameraMovement.playerTransform.position.y >= currCameraMovement.maxBound.y ){
			currCameraMovement.isMovingY = false;
		}else{
			currCameraMovement.isMovingY = true;
		}
	}

	void MoveCamera()
	{
		deltaTime = Time.fixedDeltaTime;
		Vector3 playerPos = currCameraMovement.playerTransform.position;

		Vector3 camPos = 
			new Vector3(
				currCameraMovement.isMovingX ? playerPos.x : currCameraTransform.position.x, 
				currCameraMovement.isMovingY ? playerPos.y : currCameraTransform.position.y, 
				-10f);
		
		currCameraTransform.position = Vector3.Lerp(currCameraTransform.position,camPos,currCameraMovement.smoothSpeed*deltaTime);
	}	
}
