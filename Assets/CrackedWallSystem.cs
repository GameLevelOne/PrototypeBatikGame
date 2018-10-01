using UnityEngine;
using Unity.Entities;

public class CrackedWallSystem : ComponentSystem {
	public struct CrackedWallData {
		public readonly int Length;
		public ComponentArray<CrackedWall> CrackedWall;
	}
	[InjectAttribute] CrackedWallData crackedWallData;
	CrackedWall crackedWall;

	protected override void OnUpdate () {
		// if (GateOpenerData.Length == 0) return;
		for (int i=0; i<crackedWallData.Length; i++) {
			crackedWall = crackedWallData.CrackedWall[i];

			if (!crackedWall.isInitCrackedWall) {
				InitCrackedWall();
			} else {
				if (!crackedWall.isCrackedWallDestroyed) {
					CheckCrackedWall();
				} else {
					if (!crackedWall.crackedWallExplodeFX.isStopped) {	
						crackedWall.crackedWallObj.SetActive(false);
						crackedWall.crackedWallMainCol.enabled = false;
					}
				}
			}
		}
	}

	void InitCrackedWall () {
		crackedWall.isCrackedWallDestroyed = PlayerPrefs.GetInt(Constants.EnvirontmentPrefKey.CRACKED_WALL_STATE + crackedWall.crackedWallID, 0) == 1 ? true : false;
		
		//UNLOCKED GATE SPRITE BY SAVED GATE STATE
		if (crackedWall.isCrackedWallDestroyed) {
			crackedWall.crackedWallObj.SetActive(false);
			crackedWall.crackedWallMainCol.enabled = false;
		} else {
			crackedWall.crackedWallObj.SetActive(true);
			crackedWall.crackedWallMainCol.enabled = true;
		}

		crackedWall.isInitCrackedWall = true;
	}

	void CheckCrackedWall () {
		if (crackedWall.destroy) {
			//SAVE GATE STATE
			PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.CRACKED_WALL_STATE + crackedWall.crackedWallID, 1);
			crackedWall.isCrackedWallDestroyed = true;
		}
	}
}
