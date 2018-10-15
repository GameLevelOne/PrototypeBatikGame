using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

[UpdateAfter(typeof(UnityEngine.Experimental.PlayerLoop.FixedUpdate))]
public class PlayerMovementSystem : ComponentSystem {
	public struct MovementData {
		public readonly int Length;
		public ComponentArray<Transform> Transform;
		public ComponentArray<PlayerInput> PlayerInput;
		public ComponentArray<Player> Player;
		public ComponentArray<Movement> Movement;
		public ComponentArray<Sprite2D> Sprite;
		public ComponentArray<Rigidbody> Rigidbody;
		public ComponentArray<Facing2D> Facing;
		// public ComponentArray<TeleportBulletTime> TeleportBulletTime;
	}
	public struct UIGameOverData
	{
		public readonly int Length;
		public ComponentArray<UIGameOver> uiGameOver;
	}
	[InjectAttribute]UIGameOverData uiGameOverData;
	[InjectAttribute] MovementData movementData;
	[InjectAttribute] ToolSystem toolSystem;
	[InjectAttribute] SwimSystem SwimSystem;
	[InjectAttribute] ManaSystem manaSystem;
	[InjectAttribute] GameStorageSystem gameStorageSystem;
	
	public PlayerInput input;
	public Facing2D facing;
	public Player player;

	Movement movement;
	PlayerTool tool;
	PlayerState state;
	// TeleportBulletTime teleportBulletTime;

	Transform tr;
	Rigidbody rb;
	// SpriteRenderer spriteRen;

	float deltaTime;
	float unscaledDeltaTime;
	float moveSpeed;
	float engageSpeed;
	bool isDodgeMove = false;
	// bool isAttackMove = false;
	// bool isStartDashing = false;
	float brakeTime = 0f;
	float dashDelay = 0f;
	float dashTime = 0f;
	int attackMode;
	Vector3 moveDir;
	// Vector3 currentMoveDir;

	bool gameOverCalled = false;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		unscaledDeltaTime = Time.unscaledDeltaTime;
		// if (movementData.Length == 0) return;
		
		for (int i=0; i<movementData.Length; i++) {
			input = movementData.PlayerInput[i];
			player = movementData.Player[i];
			tr = movementData.Transform[i];
			// spriteRen = movementData.Sprite[i].spriteRen;
			rb = movementData.Rigidbody[i];
			movement = movementData.Movement[i];
			facing = movementData.Facing[i];
			// teleportBulletTime = movementData.TeleportBulletTime[i];

			if (!movement.isInitMovement) {
				InitMovement();

				continue;
			}

			state = player.state;
			tool = toolSystem.tool;

			if (state == PlayerState.DIE) {
				rb.velocity = Vector3.zero;
				input.moveDir = Vector3.zero;

				// if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 0")){
				// 	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				// 	gameStorageSystem.LoadState(50);
					// PlayerPrefs.SetInt(Constants.PlayerPrefKey.LEVEL_PLAYER_START_POS,0);
					// SceneManager.LoadScene(Constants.SceneName.SCENE_LEVEL_1);
				// }	
				if (!gameOverCalled) {
					for (int j=0;j<uiGameOverData.Length;j++) {
						uiGameOverData.uiGameOver[j].call = true;
					}
					gameOverCalled = true;
				}

				continue;
			}

#region TESTING 
			// if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 0")){
			// 	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			// }	
#endregion
			
			attackMode = input.attackMode;
			// int moveMode = input.MoveMode;
			
			// switch (moveMode) {
            // case 0:
			moveSpeed = movement.normalSpeed; //NORMAL
            //     break;
            // // case 1:
            // //     moveSpeed = movement.slowSpeed; //CHARGING
            // //     break;
			// }

			if (!CheckIfAllowedToMove()) {
				SetPlayerSpecificMove ();
				continue;
			} else if (state == PlayerState.POWER_BRACELET) {
				if (input.interactValue == 2 || input.interactValue == 0) {
					moveDir = Vector3.zero;
				} else {
					if (player.isPlayerKnockedBack && player.somethingThatHitsPlayer != null) {
						// Debug.Log("KNOCKEDBACK");
						KnockBack(movement.knockBackSpeedNormal * 10f);
					}

					moveDir = input.moveDir;
				}
			} else if (state == PlayerState.SWIM) {
				if (input.interactValue == 2 || input.interactValue == 0) {
					moveDir = Vector3.zero;
				} else {
					moveDir = input.moveDir;
				}
			} else if (input.moveMode == 2) {
				moveDir = Vector3.zero;
			}
			// else if (state == PlayerState.FISHING || state == PlayerState.GET_TREASURE) {
			// 	moveDir = Vector2.zero;
			// 	rb.velocity = moveDir;
			// } 
			// else if (state == PlayerState.BOW) {
			// 	moveDir = Vector2.zero;
			// } 
		 	else {
				dashDelay = movement.dashDelay;
				brakeTime = movement.brakeTime;
				player.isBouncing = false;
				// moveDir = input.moveDir;

				if (player.isHitJatayuAttack2) {
					moveDir = input.moveDir - Vector3.forward;
				} else {
					moveDir = input.moveDir;
				}
				// Debug.Log(moveDir +"\n"+ moveDir.normalized);
			}

			// Debug.Log(player.isPlayerKnockedBack);
			// rb.drag = movement.initRigidBodyDrag;
			SetPlayerStandardMove();
			// Debug.Log(currentMoveDir);
			// continue; //TEMP
		}
	}

	void InitMovement () {
		brakeTime = 0f;
		dashTime = 0f;
		dashDelay = 0f;
		isDodgeMove = false;
		moveDir = Vector3.zero;
		engageSpeed = movement.engageSpeed;

		movement.isInitMovement = true;
		gameOverCalled = false;
	}

	void SetPlayerStandardMove () {
		if (attackMode == 0) {
			// moveDir = input.moveDir;

			if (state == PlayerState.DODGE) {
				// Transform target = facing.attackArea.transform;
				
				//=====SPEED GOING SLOWER=====//
				// if (!isDodgeMove) { 
				// 	isDodgeMove = true;
				// 	rb.AddForce((target.position - tr.position) * movement.dodgeSpeed);
				// } 
				
				//=====SPEED CONSTANT=====//
				if (!isDodgeMove) { 
					isDodgeMove = true;
					// rb.velocity = -moveDir * movement.dodgeSpeed * deltaTime; //OLD
					
					rb.velocity = Vector3.zero;
					rb.AddForce(-moveDir * movement.dodgeSpeed, ForceMode.Impulse);

					// input.moveDir = -moveDir; //REVERSE
					// input.moveDir = Vector3.zero;
					// input.dirButtons =  new List<int>(4){0,0,0,0};
				} else {
					// input.dirButtons =  new List<int>(4){0,0,0,0};
					// moveDir = Vector3.zero;
					// input.moveDir = Vector3.zero;
				}
			} else {
				isDodgeMove = false;
				//SET VELOCITY
				// moveDir = moveDir.normalized * moveSpeed * deltaTime;
				rb.velocity = moveDir * moveSpeed * deltaTime;

				// if (player.isPlayerKnockedBack) Debug.Log("HERE");
				// Debug.Log("Velocity : "+rb.velocity+"\n MoveDir : "+moveDir+" | Normalized : "+moveDir.normalized);
				// Debug.Log("Velocity: "+rb.velocity+"= \n normalized: "+moveDir.normalized+ "x moveSpeed: "+moveSpeed+"x deltaTime: "+deltaTime);

				if (moveDir != Vector3.zero) {
					// currentMoveDir = moveDir;
					
					if (state != PlayerState.POWER_BRACELET && state != PlayerState.SWIM && state != PlayerState.OPEN_CHEST && state != PlayerState.SLOW_MOTION && state != PlayerState.RAPID_SLASH) {
						player.SetPlayerState(PlayerState.MOVE);
					}
				} else {
					if (state == PlayerState.MOVE && !player.isHitJatayuAttack2) {
						player.SetPlayerIdle();
					}
				}
			}
		} else if (attackMode >= -1 && attackMode <= 3) {
			// currentMoveDir = moveDir;
			moveDir = input.moveDir;

			if (player.isMoveAttack) {
				player.isMoveAttack = false;
				// Transform target = facing.attackArea.transform;
				// rb.velocity = Vector3.zero;
				// rb.AddForce(moveDir * movement.attackMoveForce);
				// rb.velocity = Vector3.zero;
				rb.velocity = moveDir * movement.attackMoveForce * deltaTime;
			} else {
				rb.velocity = Vector3.zero;
			}
		} else {
			rb.velocity = Vector3.zero;
		}

		// if (rb.velocity.y != 0f) {
		// 	spriteRen.sortingOrder = Mathf.RoundToInt(tr.position.y * 100f) * -1;
		// }
	}

	void SetPlayerSpecificMove () {
		// Transform target = facing.attackArea.transform;
		// Vector3 dir = target.position - tr.position;
		// if (state == PlayerState.HOOK) {
		// 	rb.velocity = Vector2.zero;
		// }

		if (state == PlayerState.SLOW_MOTION) {
			if (attackMode == 0) {
				SetCounterMovement();
			}
		} else if (state == PlayerState.ENGAGE) {
			SetEngageMovement();
		} else if (state == PlayerState.DASH) {
			if (input.interactValue == 0) {
				// player.isUsingStand = false;
				// if (dashDelay > 0f) {
				// 	dashDelay -= deltaTime;
					rb.velocity = Vector3.zero;
				// } else {
				// 	if (isHaveEnoughMana((int) ToolType.Boots, true, true)) {
				// 		input.interactValue = 1;
				// 		// UseMana((int) ToolType.Boots);
				// 	}
				// }
			} else if (input.interactValue == 1) {
				if (player.isBouncing) {
					input.interactValue = 2;
				} else {
					if (isHaveEnoughMana((int) ToolType.Boots, false, false)) {
						float dashSpeed = tool.GetObj((int) ToolType.Boots).GetComponent<Boots>().bootsSpeed;
						rb.velocity = moveDir * dashSpeed * deltaTime;

						if (dashTime <= 0.2f) {
							dashTime += deltaTime;
						} else {
							dashTime = 0f;
							// Debug.Log("Use mana dash");
							UseMana((int) ToolType.Boots, true);
						}
					}
				}
			} else if (input.interactValue == 2) {
				if (brakeTime > 0f) {
					brakeTime -= deltaTime;
					
					if (player.isBouncing) {
						rb.velocity = -moveDir * movement.bounceSpeed * deltaTime * brakeTime;
					} else {
						rb.velocity = moveDir * movement.bounceSpeed * deltaTime * brakeTime;
					}
				} else {
					input.moveDir = Vector3.zero;
					player.isBouncing = false;
					dashTime = 0f;
					// player.SetPlayerIdle();
				}
			} else {
				rb.velocity = Vector3.zero;
			}
		} else if (state == PlayerState.OPEN_CHEST) {
			rb.velocity = Vector3.zero;
		} else if (state == PlayerState.BOW) {
			rb.velocity = Vector3.zero;
		} else if (state == PlayerState.GET_HURT) {
			if (player.isPlayerKnockedBack && player.somethingThatHitsPlayer != null) {
				KnockBack(movement.knockBackSpeedNormal);
			} 
		// } else if (state == PlayerState.POWER_BRACELET) {
		// 	rb.velocity = Vector3.zero;
		} else if (state == PlayerState.BLOCK_ATTACK) {
			if (player.isPlayerKnockedBack && player.somethingThatHitsPlayer != null) {
				KnockBack(movement.knockBackSpeedGuard);
			}
		} else {
			input.moveDir = Vector3.zero;
			rb.velocity = Vector3.zero;
		}
	}

	bool CheckIfAllowedToMove () {
		if (
			state == PlayerState.USING_TOOL || 
			state == PlayerState.HOOK || 
			state == PlayerState.DASH || 
			state == PlayerState.BOW || 
			state == PlayerState.FISHING || 
			state == PlayerState.GET_TREASURE || 
			state == PlayerState.DIE || 
			// state == PlayerState.OPEN_CHEST || 
			state == PlayerState.GET_HURT || 
			state == PlayerState.BLOCK_ATTACK ||
			state == PlayerState.SLOW_MOTION ||
			state == PlayerState.ENGAGE ||
			state == PlayerState.RAPID_SLASH
			// state == PlayerState.POWER_BRACELET
			) {
			return false;
		// } else if (player.isPlayerKnockedBack && player.somethingThatHitsPlayer != null) {
		// 	return false;
		} else {
			return true;
		}
	}

	bool isHaveEnoughMana (int toolIdx, bool isUseMana, bool isUsingStand) {
		// Debug.Log("mana cost for tool " + toolIdx + " is " + tool.GetToolManaCost(toolIdx));
		if(manaSystem.isHaveEnoughMana(tool.GetToolManaCost(toolIdx), isUseMana, isUsingStand)) {
			return true;
		} else {
			return false;
		}
	}

	void UseMana (int toolIdx, bool isUsingStand) {
		manaSystem.UseMana(tool.GetToolManaCost(toolIdx), isUsingStand);
	}

	void KnockBack (float knockbackSpeed) {
		Vector3 enemyPos = player.somethingThatHitsPlayer.position;
		Vector3 resultPos = new Vector3 (tr.position.x-enemyPos.x, 0f, tr.position.z-enemyPos.z);
		// Debug.Log("enemyPos : "+enemyPos);
		rb.velocity = Vector3.zero;
		rb.AddForce(resultPos.normalized * knockbackSpeed, ForceMode.Impulse);

		player.isPlayerKnockedBack = false;
		player.somethingThatHitsPlayer = null;
	}

	void SetCounterMovement () {
		// Vector3 teleportPos = player.somethingThatHitsPlayer.GetComponent<Facing2D>().blindArea.transform.position;
		// rb.position = new Vector3 (teleportPos.x, rb.position.y, teleportPos.z);
		// Debug.Log("Slow Motion PlayerMovementSystem");
		Time.timeScale = 0.5f;
		input.attackMode = -3; //Set counterslash first
		rb.velocity = Vector3.zero;
	}

	void SetEngageMovement () {
		float distance = Vector3.Distance(rb.position, player.counterPos);
		if(distance < 0.2f){
			player.SetPlayerState(PlayerState.RAPID_SLASH);
		} else {
			// rb.position = MoveEngageToPos(player.counterPos, engageSpeed);
			
			rb.position = Vector3.Lerp(rb.position, player.counterPos,engageSpeed * deltaTime / distance);
		}
	}

	Vector3 MoveEngageToPos (Vector3 targetPos, float speed) {
		Vector3 deltaPos = targetPos-rb.position;
		// Debug.Log("DeltaPos "+deltaPos);
		
		if (deltaPos.z < -0.2f || deltaPos.z > 0.2f) {
			Vector3 vecticalizeVector = Vector3.Scale(deltaPos.normalized, new Vector3 (1f, 1f, GameStorage.Instance.settings.verticalMultiplier));
			return rb.position + vecticalizeVector * speed * deltaTime;
		} else {
			return rb.position + deltaPos * speed * deltaTime;
		}
	}
}