using UnityEngine;

public class RendererOrder : MonoBehaviour {
	public string sortingLayer;
	public int order;

	public bool isMeshRenderer;

	void Awake () {
		if (!isMeshRenderer) {
			Renderer renderer = GetComponent<Renderer>();

			renderer.sortingLayerName = sortingLayer;
			renderer.sortingOrder = order;
			Debug.Log("Renderer "+renderer.sortingLayerName);
		} else {
			MeshRenderer renderer = GetComponent<MeshRenderer>();

			renderer.sortingLayerName = sortingLayer;
			renderer.sortingOrder = order;
			Debug.Log("MeshRenderer "+renderer.sortingLayerName);
		}
	}
}
