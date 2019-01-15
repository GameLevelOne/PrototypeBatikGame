using UnityEngine;
using System.Collections.Generic;

public class Dissolver : MonoBehaviour {
	public TriggerDetection playerAttackTriggerDetection;
	public GameObject greyLayerObj;
	public List<Renderer> mRenderer;
	[RangeAttribute(0f,1f)]
	public List<float> dissolveValue;

	public float dissolveSpeed = 0.25f;
	public bool init = false;
	public bool dissolve = false;
	public bool unDissolve = false;
	public bool isDissolved = false;

	void OnEnable()
    {   
        if (playerAttackTriggerDetection!=null)
            playerAttackTriggerDetection.OnTriggerEnterObj += OnTriggerEnterObj;
    }   

    void OnDisable()
    {
        if (playerAttackTriggerDetection!=null)
            playerAttackTriggerDetection.OnTriggerEnterObj -= OnTriggerEnterObj;
    }

	void OnTriggerEnterObj(GameObject g)
	{
		dissolve = true;
	}
}
