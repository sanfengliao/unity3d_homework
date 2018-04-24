using Com.Action;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCFlyActionFactory : MonoBehaviour {

    Queue<CCFlyAction> actions = new Queue<CCFlyAction>();
    public CCFlyAction GetAction()
    {
        CCFlyAction action = null;
        if (actions.Count > 0)
        {
            action = actions.Dequeue();
        }
        else
        {
            action = CCFlyAction.GetAction();
        }
        return action;
    }
    public void FreeAction(CCFlyAction action)
    {
        action.reset();
        actions.Enqueue(action);
    }
}
