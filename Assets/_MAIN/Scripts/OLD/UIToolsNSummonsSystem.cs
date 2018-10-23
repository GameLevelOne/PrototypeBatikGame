// using UnityEngine;
// using Unity.Entities;

// public class UIToolsNSummonsSystem : ComponentSystem {
// 	public struct UIToolsNSummonsData {
// 		public readonly int Length;
// 		public ComponentArray<UIToolsNSummons> UIToolsNSummons;
// 	}
// 	[InjectAttribute] UIToolsNSummonsData uiToolsNSummonsData;

// 	UIToolsNSummons uiToolsNSummons;

// 	protected override void OnUpdate () {
// 		if (uiToolsNSummonsData.Length == 0) return;

// 		for (int i=0; i<uiToolsNSummonsData.Length; i++) {
// 			uiToolsNSummons = uiToolsNSummonsData.UIToolsNSummons[i];

// 			CheckUI ();
// 		}
// 	}

// 	void CheckUI () {
// 		if (uiToolsNSummons.gameObject.activeSelf) {
// 			 // Debug.Log("Ok");
// 		} else {
// 			 // Debug.Log("No");
// 		}
// 	}
// }
