using UnityEngine;

public class Tree : MonoBehaviour {
	public AnimSpeedRandom animSpeedRandom;
    public TriggerDetection playerAttackTriggerDetection;
    public ParticleSystem particle;

    public AudioSource audioSource;
    [HeaderAttribute("Current")]
    public bool hit = false;

#region init
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
#endregion

    void OnTriggerEnterObj(GameObject obj)
    {
        if(obj != null){
            hit = true;
        }
    }

#region animation event
    void OnHitAnimStart()
    {
        animSpeedRandom.animSpeed = 1f;
    }
#endregion
}
