using Com.Action;
using Com.MyGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCPhysicsManager : SSActionManager, ISSActionCallback, IActionManager
{


    public int diskNum;
    public FirstController scene;
    float speed = 20f;
    CCFlyActionFactory flyActionFactory;
    void ISSActionCallback.SSActionEvent(SSAction source, SSActionEventType events, int intParam, string strParam, Object objectParam)
    {
        if (source is CCFlyAction)
        {
            diskNum--;
            DiskFactory df = Singleton<DiskFactory>.Instance;
            df.FreeDisk(source.gameObject);
            flyActionFactory.FreeAction((CCFlyAction)source);
        }
    }

    void Start()
    {
        scene = Director.getInstance().currentSceneController as FirstController;
        scene.actionManger = this;
        flyActionFactory = Singleton<CCFlyActionFactory>.Instance;
    }
    //重写一下SSActionManager的方法, 让其执行Action的FixedUpdate方法
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
                ac.FixedUpdate();
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
        if(!action.gameObject.GetComponent<Rigidbody>())
            action.gameObject.AddComponent<Rigidbody>();
        action.transform = gameobject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
    public void StartThrow(Queue<GameObject> disks)
    {
        foreach (GameObject disk in disks)
        {
            RunAction(disk, flyActionFactory.GetAction(), (ISSActionCallback)this);
        }
    }

    public int getDiskNum()
    {
        return diskNum;
    }

    public void setDiskNum(int num)
    {
        diskNum = num;
    }
}
