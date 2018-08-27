using UnityEngine;
using Unity.Entities;

public class UIPlayerInfoSystem : ComponentSystem {
	public struct UIPlayerInfoData {
		public readonly int Length;
		public ComponentArray<UIPlayerInfo> UIPlayerInfo;
	}
	[InjectAttribute] UIPlayerInfoData uiPlayerInfoData;

	UIPlayerInfo uiPlayerInfo;

	protected override void OnUpdate () {
		if (uiPlayerInfoData.Length == 0) return;

		for (int i=0; i<uiPlayerInfoData.Length; i++) {
			uiPlayerInfo = uiPlayerInfoData.UIPlayerInfo[i];
		}
	}

	public void PrintHP (string value) {
		uiPlayerInfo.textHP.text = value;
	}

	public void PrintMP (string value) {
		uiPlayerInfo.textMP.text = value;
	}

	public void PrintTool (string value) {
		uiPlayerInfo.textTool.text = value;
	}
}
