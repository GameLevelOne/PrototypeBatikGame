// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public enum waveDirections{Vertical,Horizontal};

public enum objSides{Top,Right,Bottom,Left,None}

[RequireComponent(typeof(MeshFilter)),RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class Wavy : MonoBehaviour {
	[HideInInspector]
	public MeshRenderer mr;

	public Texture2D texture;

	public Color tint=Color.white;

	[Range(0,30)]
	public int divisionsX=10;

	[Range(0,30)]
	public int divisionsY=10;

	public waveDirections waveDirection=waveDirections.Vertical;
	public objSides staticSide=objSides.Bottom;

	[Range(-10,10)]
	public float waveFrequency=10f;

	[Range(0f,1f)]
	public float waveForce=0.03f;

	[Range(0f,10f)]
	public float waveSpeed=1f;

	[HideInInspector]
	public int sortingLayer=0;

	[HideInInspector]
	public int orderInLayer=0;

	// void OnEnable(){
	// 	mr=GetComponent<MeshRenderer>();
	// 	mf=GetComponent<MeshFilter>();
	// 	// SetMeshAndMaterial();
	// 	// GenerateMesh();
	// 	sortingLayer=mr.sortingLayerID;
	// 	orderInLayer=mr.sortingOrder;
    //     // Update();
    // }
}
