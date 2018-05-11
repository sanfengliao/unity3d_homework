using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolActionManager : SSActionManager {

	// Use this for initialization
    public void StartGoAction(GameObject gameObject)
    {
        SSAction action = PatrolGoAction.getPatrolGoAction();
        this.RunAction(gameObject, action, this);
    }
	
}
