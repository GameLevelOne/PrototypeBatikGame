// using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class WavySystem : ComponentSystem {
	public struct WavyData {
		public readonly int Length;
		public ComponentArray<Wavy> Wavy;
	}
	[InjectAttribute] WavyData wavyData;

	Wavy wavy;

	private Material mat;
	private MeshFilter mf;
	private Mesh mesh;
	private List<Vector3> vertices=new List<Vector3>(200);
	private List<Vector3> uvs=new List<Vector3>(200);
	private List<Color> colors=new List<Color>(200);
	private int[] triangles;
	private Texture2D _texture;
	private Color _tint;
	private int _divisionsX;
	private int _divisionsY;
	private waveDirections _waveDirection;
	private float _waveFrequency;
	private objSides _staticSide;
	private float _waveForce;
	private float _waveSpeed;
	float meshWidth=1;
	float meshHeight=1;
	private int _sortingLayer;
	private int _orderInLayer=0;

	bool isInitiatingWavy = false;

	protected override void OnUpdate () {
		if (wavyData.Length == 0) return;

		for (int i=0; i<wavyData.Length; i++) {
			wavy = wavyData.Wavy[i];

			if (!isInitiatingWavy) {
				InitWavy();
			}

			CheckWavySprite ();
		}
	}

	void InitWavy () {
		wavy.mr = wavy.GetComponent<MeshRenderer>();
		mf = wavy.GetComponent<MeshFilter>();
		SetMeshAndMaterial();
		GenerateMesh();
		wavy.sortingLayer = wavy.mr.sortingLayerID;
		wavy.orderInLayer = wavy.mr.sortingOrder;
        CheckWavySprite ();

		isInitiatingWavy = true;
	}

	void CheckWavySprite () {
		if (
			_texture!=wavy.texture || 
			_tint!=wavy.tint ||
			_divisionsX!=wavy.divisionsX || 
			_divisionsY!=wavy.divisionsY || 
			_waveDirection!=wavy.waveDirection || 
			_staticSide!=wavy.staticSide || 
			_waveFrequency!=wavy.waveFrequency || 
			_waveForce!=wavy.waveForce ||
			_waveSpeed!=wavy.waveSpeed ||
			_sortingLayer!=wavy.sortingLayer ||
			_orderInLayer!=wavy.orderInLayer
			){
				SetMeshAndMaterial();
				_divisionsX=wavy.divisionsX;
				_divisionsY=wavy.divisionsY;
				if(_waveDirection!=wavy.waveDirection){
					mat.SetFloat("_WaveDirection",wavy.waveDirection==waveDirections.Vertical?0:1);
					_waveDirection=wavy.waveDirection;
				}
				if(_staticSide!=wavy.staticSide){
					if(wavy.staticSide==objSides.Top) mat.SetFloat("_StaticSide",1);
					if(wavy.staticSide==objSides.Right) mat.SetFloat("_StaticSide",2);
					if(wavy.staticSide==objSides.Bottom) mat.SetFloat("_StaticSide",3);
					if(wavy.staticSide==objSides.Left) mat.SetFloat("_StaticSide",4);
					if(wavy.staticSide==objSides.None) mat.SetFloat("_StaticSide",0);
					_staticSide=wavy.staticSide;
				}
				_waveFrequency=wavy.waveFrequency;
				_waveForce=wavy.waveForce;
				_waveSpeed=wavy.waveSpeed;
				mat.SetFloat("_WaveFrequency",wavy.waveFrequency);
				mat.SetFloat("_WaveForce",wavy.waveForce);
				mat.SetFloat("_WaveSpeed",wavy.waveSpeed);
				if(_texture!=wavy.texture){
					_texture=wavy.texture;
					mat.SetTexture("_MainTex",wavy.texture);
					if(wavy.texture!=null){
						if(wavy.texture.width>wavy.texture.height){
							meshWidth=1f;
							meshHeight=(float)wavy.texture.height/(float)wavy.texture.width;
						}else{
							meshWidth=(float)wavy.texture.width/(float)wavy.texture.height;
							meshHeight=1f;
						}
					}else{
						meshWidth=1f;
						meshHeight=1f;
					}
				}

				_tint=wavy.tint;
				mat.SetColor("_Color",wavy.tint);

				if(_sortingLayer!=wavy.sortingLayer || _orderInLayer!=wavy.orderInLayer){
					wavy.mr.sortingLayerID=wavy.sortingLayer;
					wavy.mr.sortingOrder=wavy.orderInLayer;
					_sortingLayer=wavy.sortingLayer;
					_orderInLayer=wavy.orderInLayer;
				}
				GenerateMesh();
			}
	}
	

	void SetMeshAndMaterial(){
		if(mesh==null){
			mesh=new Mesh();
			mesh.name="WavySpriteMesh";
			if(mf.sharedMesh!=null) {
				// DestroyImmediate(mf.sharedMesh);
				GameObject.DestroyImmediate(mf.sharedMesh);
			}
		}
		if(mf.sharedMesh==null){
			mf.sharedMesh=mesh;
		}
		if(mat==null){
			mat=new Material(Shader.Find("Custom/WavySprite"));
			mat.name="WavySpriteMaterial";
			if(wavy.mr.sharedMaterial!=null) {
				// DestroyImmediate(wavy.mr.sharedMaterial);
				GameObject.DestroyImmediate(wavy.mr.sharedMaterial);
			}
		}
		if(wavy.mr.sharedMaterial==null){
			wavy.mr.sharedMaterial=mat;
		}
	}
	
	void GenerateMesh(){
		int pointsX=wavy.divisionsX+2;
		int pointsY=wavy.divisionsY+2;
		int verticeNum=0;
		int squareNum=-1;
		vertices.Clear();
		uvs.Clear();
		colors.Clear();
		triangles=new int[((pointsX*pointsY)*2)*3];
		for(int y=0;y<pointsY;y++){
			for(int x=0;x<pointsX;x++){
				vertices.Add(new Vector3(
					(((float)x/(float)(pointsX-1))-0.5f)*meshWidth,
					((float)y/(float)(pointsY-1))*meshHeight,
					0f
				));
				uvs.Add(new Vector3(
					((float)x/(float)(pointsX-1)),
					((float)y/(float)(pointsY-1)),
					0f
				));
				//Add triangles
				if(x>0 && y>0){
					verticeNum=x+(y*pointsX);
					squareNum++;
					triangles[squareNum*6]=verticeNum-pointsX-1;
					triangles[squareNum*6+1]=verticeNum-1;
					triangles[squareNum*6+2]=verticeNum;
					triangles[squareNum*6+3]=verticeNum;
					triangles[squareNum*6+4]=verticeNum-pointsX;
					triangles[squareNum*6+5]=verticeNum-pointsX-1;
				}
			}
		}
		mesh.Clear();
		mesh.SetVertices(vertices);
		mesh.SetUVs(0,uvs);
		mesh.SetColors(colors);
		mesh.SetTriangles(triangles,0);
	}
}
