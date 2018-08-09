// using Unity.Entities;
// using UnityEngine;

// public class EnemyMovementSystem : ComponentSystem {
	
// 	public struct EnemyMovementComponent{
// 		public readonly int Length;
// 		public ComponentArray<Enemy> enemy;
// 		public ComponentArray<Transform> transform;
// 		public ComponentArray<EnemyMovement> enemyMovement;
// 		public ComponentArray<Rigidbody2D> rigidbody;
// 		public ComponentArray<Facing2D> facing2D;
// 	}

// 	#region injected component
// 	[InjectAttribute] EnemyMovementComponent enemyMovementComponent;
// 	Enemy currEnemy;
// 	Transform currTransform;
// 	EnemyMovement currEnemyMovement;
// 	Rigidbody2D currRigidbody;
// 	Facing2D currFacing2D;
// 	#endregion
	
// 	//system
// 	[InjectAttribute] EnemyAISystem enemyAISystem;
// 	[InjectAttribute] EnemyAnimationSystem enemyAnimationSystem;
// 	[InjectAttribute] EnemyAttackSystem enemyAttackSystem;
// 	[InjectAttribute] EnemyInputSystem enemyInputSystem;

// 	float deltaTime;

// 	protected override void OnUpdate()
// 	{
// 		// for(int i = 0;i<enemyMovementComponent.Length;i++){
// 		//	currEnemy = enemyMovementComponent.enemy[i];
// 		// 	currEnemyMovement = enemyMovementComponent.enemyMovement[i];
// 		// 	currTransform = enemyMovementComponent.transform[i];
// 		// 	currRigidbody = enemyMovementComponent.rigidbody[i];
// 		//	currFacing2D = enemyMovementComponent.facing2D[i];
// 		// 	Move();
// 		// }
		
// 	}

// 	// public void InitMove()
// 	// {
// 	// 	if(!currEnemyMovement.isChasing == !currEnemyMovement.isMoving){
// 	// 		currEnemyMovement.isMoving = true;
// 	// 		currEnemyMovement.targetPos = GetRantomPatrolTarget(currTransform);
// 	// 		deltaTime = Time.deltaTime;
// 	// 	}
// 	// }

// 	// void Move()
// 	// {
// 	// 	if(!currEnemyMovement.isChasing && currEnemyMovement.isMoving){
// 	// 		currRigidbody.position = Vector2.MoveTowards(currRigidbody.position, currEnemyMovement.targetPos, currEnemyMovement.speed * deltaTime);
// 	// 		currEnemyMovement.faceDirection = GetDirection(currRigidbody.position,currEnemyMovement.targetPos);
// 	// 		if(Vector2.Distance(currRigidbody.position,currEnemyMovement.targetPos) <= 0.1f){
// 	// 			currEnemyMovement.isMoving = false;
// 	// 			// enemyAISystem.SetEnemyState(EnemyState.Idle);
// 	// 		}
// 	// 	}
// 	// }

// 	// public void InitChase()
// 	// {
// 	// 	if(!currEnemyMovement.isChasing){
// 	// 		currEnemyMovement.isMoving = false;
// 	// 		currEnemyMovement.isChasing = true;
// 	// 		currEnemyMovement.TChase = currEnemyMovement.chaseDuration;
// 	// 		deltaTime = Time.deltaTime;
// 	// 	}
// 	// }

// 	// void Chase()
// 	// {
// 	// 	if(currEnemyMovement.isChasing){
// 	// 		currEnemyMovement.TChase -= deltaTime;
// 	// 		currRigidbody.position = Vector2.MoveTowards(currRigidbody.position, currEnemyMovement.chaseTransform.position, currEnemyMovement.speed * deltaTime);

// 	// 		if(currEnemyMovement.TChase <= 0f){
// 	// 			//chase end

// 	// 			currEnemyMovement.isChasing = false;
// 	// 			currEnemyMovement.chaseTransform = null;
// 	// 			// enemyAISystem.SetEnemyState(EnemyState.Idle);
// 	// 		}
// 	// 	}
// 	// }

// 	Vector2 GetRantomPatrolTarget(Transform current)
// 	{
// 		Vector3 currPos = current.position;
// 		float xRnd = currPos.x += Random.Range(-3f,3f);
// 		float yRnd = currPos.y += Random.Range(-3f,3f);

// 		return new Vector2(xRnd,yRnd);
// 	}

// 	// Direction GetDirection(Vector3 self, Vector3 target)
// 	// {
// 	// 	Vector3 distance = target - self;
// 	// 	float magnitude = distance.magnitude;
// 	// 	Vector3 direction = distance / magnitude;

// 	// 	float x = direction.x;
// 	// 	float y = direction.y;
		
// 	// 	if(x < 0f){
// 	// 		if(y < 0f) return Direction.DL;
// 	// 		else return Direction.UL;
// 	// 	}else{
// 	// 		if(y < 0f) return Direction.DR;
// 	// 		else return Direction.UR;
// 	// 	}
// 	// }
// }