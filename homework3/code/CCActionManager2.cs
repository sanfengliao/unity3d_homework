using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Action;
using Com.MyGame;

public class CCActionManager2 :SSActionManager, ISSActionCallback {
    public SSAction action1, action2;
    public CCSequenceAction sAction;
    public FirstController2 scene;
    float speed = 20f;
    void ISSActionCallback.SSActionEvent(SSAction source, SSActionEventType events, int intParam, string strParam, Object objectParam)
    {
        
    }

    // Use this for initialization
    void Start () {
        scene = Director.getInstance().currentSceneController as FirstController2;
        scene.actionManger = this;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject gameObj = null;
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) gameObj = hit.transform.gameObject;
        }
        
        if (gameObj != null)
        {
            if (gameObj.transform.tag == "priest" || gameObj.transform.tag == "devil")
            {
                int seatNum, shoreNum;
                if (gameObj.transform.parent == scene.boat.transform.parent && (scene.boat_people[0] == null || scene.boat_people[1] == null))
                {
                    seatNum = scene.boat_people[0] == null ? 0 : 1;
                    if (gameObj.transform.parent == scene.startShore.transform)
                    {
                        shoreNum = 0;
                        for (int i = 0; i < scene.LeftObjList.Count; ++i)
                        {
                            if (gameObj.name == scene.LeftObjList[i].name)
                            {
                                getOnBoat(gameObj, shoreNum, seatNum);
                                scene.LeftObjList.Remove(gameObj);
                            }
                        }
                    }
                    else
                    {
                        shoreNum = 1;
                        for (int i = 0; i < scene.RightObjList.Count; ++i)
                        {
                            if (gameObj.name == scene.RightObjList[i].name)
                            {
                                getOnBoat(gameObj, shoreNum, seatNum);
                                scene.RightObjList.Remove(gameObj);
                            }
                        }
                    }
                    scene.boat_people[seatNum] = gameObj;
                    base.Update();
                    gameObj.transform.parent = scene.boat.transform;

                }else if (gameObj.transform.parent == scene.boat.transform)
                {
                    shoreNum = scene.boat.transform.parent == scene.startShore.transform ? 0 : 1;
                    seatNum =(scene.boat_people[0]!=null && scene.boat_people[0] == gameObj) ? 0 : 1;
                    getOffBoat(gameObj, shoreNum);
                    base.Update();
                    scene.boat_people[seatNum] = null;
                    if (shoreNum == 0)
                    {
                        scene.LeftObjList.Add(gameObj);
                        gameObj.transform.parent = scene.startShore.transform;
                    }
                    else
                    {
                        scene.RightObjList.Add(gameObj);
                        gameObj.transform.parent = scene.endShore.transform;
                    }
                }
              
            }
            else if(gameObj.transform.name == "boat")
            {
                if (scene.boat_people[0] != null || scene.boat_people[1] != null)
                {
                    moveBoat(scene.boat);
                    scene.boat.transform.parent = scene.boat.transform.parent == scene.startShore.transform ?
                        scene.endShore.transform : scene.startShore.transform;
                }
            }
            
        }

        scene.check_game_over();
    }
    
    public void moveBoat(GameObject boat)
    {
        Vector3 pos = boat.transform.position == new Vector3(4, 0.7f, 0) ? new Vector3(-4, 0.7f, 0) : new Vector3(4, 0.7f, 0);
        action1 = CCMoveAction.GetCCMoveAction(pos, speed);
        this.RunAction(boat, action1, this);
        
    }

    public void getOnBoat(GameObject people, int shore, int seat)
    {
        if (shore == 0 && seat == 0)
        {
            action1 = CCMoveAction.GetCCMoveAction(new Vector3(3f, 2f, 0), speed);
            action2 = CCMoveAction.GetCCMoveAction(new Vector3(3f, 1.5f, 0), speed);
        }
        else if (shore == 0 && seat == 1)
        {
            action1 = CCMoveAction.GetCCMoveAction(new Vector3(4.5f, 2f, 0), speed);
            action2 = CCMoveAction.GetCCMoveAction(new Vector3(4.5f, 1.5f, 0), speed);
        }
        else if (shore == 1 && seat == 0)
        {
            action1 = CCMoveAction.GetCCMoveAction(new Vector3(-5f, 2f, 0), speed);
            action2 = CCMoveAction.GetCCMoveAction(new Vector3(-5f, 1.5f, 0), speed);
        }
        else if (shore == 1 && seat == 1)
        {
            action1 = CCMoveAction.GetCCMoveAction(new Vector3(-3.5f, 2f, 0), speed);
            action2 = CCMoveAction.GetCCMoveAction(new Vector3(-3.5f, 1.5f, 0), speed);
        }

        CCSequenceAction saction = CCSequenceAction.GetSSAction(0, 0, new List<SSAction> { action1, action2 });
        this.RunAction(people, saction, this);
    }

    public void getOffBoat(GameObject people, int shoreNum)
    {
        action1 = CCMoveAction.GetCCMoveAction(new Vector3(people.transform.position.x, 2f, 0), speed);
        if (shoreNum == 0) action2 = CCMoveAction.GetCCMoveAction(new Vector3(7 + 1.5f * System.Convert.ToInt32(people.name), 2f, 0), speed);
        else action2 = CCMoveAction.GetCCMoveAction(new Vector3(-7f - 1.5f * System.Convert.ToInt32(people.name), 2f, 0), speed);

        CCSequenceAction saction = CCSequenceAction.GetSSAction(0, 0, new List<SSAction> { action1, action2 });
        this.RunAction(people, saction, this);
    }
}
