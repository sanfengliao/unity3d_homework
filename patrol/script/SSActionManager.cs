using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSActionManager : MonoBehaviour,ISSActionCallback {

    protected Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    protected List<SSAction> waitingAdd = new List<SSAction>();
    protected List<int> waitingDelete = new List<int>();
    protected void Update()
    {
        foreach (SSAction ac in waitingAdd)
        {
            actions[ac.GetInstanceID()] = ac;
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            if (ac.distroy)
            {

                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable)
            {
                ac.Update();
            }
        }
        foreach (int key in waitingDelete)
        {
            SSAction ac = actions[key];
            actions.Remove(key);
            DestroyObject(ac);
        }
        waitingDelete.Clear();
    }
    public void DestroyAllAction()
    {
        foreach (KeyValuePair<int, SSAction> kv in actions)
        {
            SSAction ac = kv.Value;
            ac.gameObject.GetComponent<Animator>().SetBool("walk", false);
            ac.distroy = true;
        }
    }
    public void RunAction(GameObject gameobject, SSAction action, ISSActionCallback manager)
    {
        action.gameObject = gameobject;
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }

    protected void Start()
    {

    }

    public void SSActionEvent(SSAction source, int intParam = 0, GameObject objectParam = null)
    {
        if (intParam == 0)
        {
            //跟踪玩家
            source.distroy = true;
            GameObject player = objectParam.GetComponent<PatrolData>().player;
            SSAction follow= PatrolFollowAction.GetPatrolFollowAction(player);
            this.RunAction(objectParam,follow, this);
        }else if(intParam == 1) {
            //继续巡逻
            source.distroy = true;
            objectParam.GetComponent<PatrolData>().player = null;
            SSAction go = PatrolGoAction.getPatrolGoAction();
            this.RunAction(objectParam, go, this);
        }
    }
}
