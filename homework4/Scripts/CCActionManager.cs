using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Action;
using Com.MyGame;

public class CCActionManager :SSActionManager, ISSActionCallback {
    public int diskNum;
    public FirstController scene;
    float speed = 20f;
    Queue<SSAction> actions = new Queue<SSAction>();
    void ISSActionCallback.SSActionEvent(SSAction source, SSActionEventType events, int intParam, string strParam, Object objectParam)
    {
        if (source is CCFlyAction) {
            diskNum --;
            DiskFactory df = Singleton<DiskFactory>.Instance;
            df.FreeDisk(source.gameObject);
            
            FreeAction(source);
        }
    }
    public void FreeAction(SSAction action)
    {
        action.reset();
        actions.Enqueue(action);
    }
    void Start()
    {
        scene = Director.getInstance().currentSceneController as FirstController;
        scene.actionManger = this;
    }
    void Update()
    {
        if (scene.GetGameState() == GameState.RUNNING)
             base.Update();
    }
    SSAction GetAction()
    {
        SSAction action = null;
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
    public void StartThrow(Queue<GameObject> disks)
    {
        foreach(GameObject disk in disks) {
            RunAction(disk, GetAction(),(ISSActionCallback)this);
        }
    }

}
