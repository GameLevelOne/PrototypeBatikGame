using UnityEngine;

public class Tree : MonoBehaviour {
	public AnimSpeedRandom animSpeedRandom;
    public TriggerDetection playerAttackTriggerDetection;

    [HeaderAttribute("Current")]
    public bool hit = false;

#region init
    void OnEnable()
    {   
        playerAttackTriggerDetection.OnTriggerEnterObj += OnTriggerEnterObj;
    }   

    void OnDisable()
    {
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
