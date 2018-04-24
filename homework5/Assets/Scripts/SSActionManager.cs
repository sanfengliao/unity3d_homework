using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Action;
public class SSActionManager : MonoBehaviour
{

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
}
