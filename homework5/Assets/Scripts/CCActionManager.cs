using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Action;
using Com.MyGame;

public class CCActionManager :SSActionManager, ISSActionCallback, IActionManager {
    public int diskNum;
    public FirstController scene;
    float speed = 20f;
    CCFlyActionFactory flyActionFactory;
    void ISSActionCallback.SSActionEvent(SSAction source, SSActionEventType events, int intParam, string strParam, Object objectParam)
    {
        if (source is CCFlyAction) {
            diskNum --;
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
    void Update()
    {
        if (scene.GetGameState() == GameState.RUNNING)
             base.Update();
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
