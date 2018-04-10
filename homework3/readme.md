  上次用了unity3d做了一个，虽然用了MVP的的设计模式，但是把动作管理放在场景里让项目的耦合程度太高，所以应该把动作从场景类分离出来，场景只需要控制视图就可以了。(这都是我随便乱吹的，其实我也不清楚为什么要动作分离)
 盗用别人的uml图
![这里写图片描述](https://github.com/cyulei/Unity3d-learning/blob/HW3/%E5%8A%A8%E4%BD%9C%E7%B1%BB%E5%9B%BE.png?raw=true)
1. 向上周的作业一样，先把场景构建好
basecode
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;
namespace Com.MyGame
{
    public class Director : System.Object
    {
        private static Director _instance;
        public SceneController currentSceneController { get; set; }
        public static Director getInstance()
        {
            if (_instance == null)
            {
                _instance = new Director();
            }
            return _instance;
        }
    }

    public interface SceneController
    {
        void loadResources();
    }
}
```
UserGUI
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class UserGUI : MonoBehaviour
{
    public FirstController scene;
    public int status = 0;
    GUIStyle style;
    GUIStyle buttonStyle;
    float time = 60;
    float second = 0;
    void Start()
    {
        scene = Director.getInstance().currentSceneController as FirstController;
      
        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
    }
    private void Update()
    {
        scene.check_game_over();
    }
    void OnGUI()
    {
        second += Time.deltaTime / 2;
        if (second >= 1)
        {
            time--;
            second = 0;
        }
        if (time > 0 && status == 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), time.ToString(), style);
            
        }
        if (status == 1 || time <= 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You Fail!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                time = 60;
                second = 0;
                scene.restart();
            }
        }
        else if (status == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                time = 60;
                second = 0;
                scene.restart();
            }
        }
    }
}
```
FirtstController
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class FirstController2 : MonoBehaviour, SceneController
{
	 public CCActionManager actionManger { get; set; }
    public List<GameObject> LeftObjList { get; set; }
    public List<GameObject> RightObjList { get; set; }
    public GameObject[] boat_people { get; set; }
    Vector3 river_pos = new Vector3(0, 0, 0);
    public GameObject startShore { get; set; }
    public GameObject endShore { get; set; }
    public GameObject boat { get; set; }
    Vector3 startShorePos = new Vector3(11, 0.5f, 0);
    Vector3 endShorePos = new Vector3(-11, 0.5f, 0);
    Vector3 boatStartPos = new Vector3(4, 0.7f, 0);
    Vector3 boatEndPos = new Vector3(-4, 0.7f, 0);
    UserGUI userGUI;


    void Awake()
    {
        LeftObjList = new List<GameObject>();
        RightObjList = new List<GameObject>();
        boat_people = new GameObject[2];
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        loadResources();
    }

    public void loadResources()
    {
        GameObject river = Instantiate(Resources.Load("prefab/river", typeof(GameObject)), river_pos, Quaternion.identity, null) as GameObject;
        river.name = "river";
        startShore = Instantiate(Resources.Load("prefab/coast", typeof(GameObject)), startShorePos, Quaternion.identity, null) as GameObject;
        startShore.name = "startShore";
        endShore = Instantiate(Resources.Load("prefab/coast", typeof(GameObject)), endShorePos, Quaternion.identity, null) as GameObject;
        endShore.name = "endShore";
        boat = Instantiate(Resources.Load("prefab/boat", typeof(GameObject)), boatStartPos, Quaternion.identity, null) as GameObject;
        boat.name = "boat";
        boat.transform.parent = startShore.transform;
        GameObject priest, devil;
        for (int i = 0; i < 3; ++i)
        {
            priest = Instantiate(Resources.Load("prefab/priest")) as GameObject;
            priest.name = i.ToString();
            priest.transform.position = new Vector3(7f + i * 1.5f, 2f, 0);
            priest.transform.parent = startShore.transform;
            LeftObjList.Add(priest);

            devil = Instantiate(Resources.Load("prefab/devil")) as GameObject;
            devil.name = (i + 3).ToString();
            devil.transform.position = new Vector3(7f + (i + 3) * 1.5f, 2f, 0);
            devil.transform.parent = startShore.transform;
            LeftObjList.Add(devil);
        }

    }
    
    public void check_game_over()
    {
        int start_priests = 0;
        int start_devil = 0;
        int end_priests = 0;
        int end_devil = 0;
        foreach(GameObject gb in LeftObjList)
        {
            if (gb.transform.tag == "priest")
            {
                start_priests++;
            }
            else
            {
                start_devil++;
            }
            
        }
        foreach(GameObject gb in RightObjList)
        {
            if (gb.transform.tag == "priest")
            {
                end_priests++;
            }
            else
            {
                end_devil++;
            }
        }
        foreach(GameObject gb in boat_people)
        {
            if (gb == null)
                continue;
            if (boat.transform.parent == startShore .transform)
            {
                if (gb.transform.tag == "priest")
                {
                    start_priests++;
                }
                else
                {
                    start_devil++;
                }
            }
            else
            {
                if (gb.transform.tag == "priest")
                {
                    end_priests++;
                }
                else
                {
                    end_devil++;
                }
            }
        }

        if (end_priests + end_devil == 6)
        {
            userGUI.status = 2;
            return;
        }
        if ((start_priests < start_devil && start_priests > 0 )|| (end_priests < end_devil && end_priests > 0))
        {
             userGUI.status = 1;
            return;
        }

        userGUI.status = 0;
    }

    public void restart()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

   
}
```
#####接下来就是动作管理了(主要是参考老师的ppt)
1. 定义动作基类：使用 virtual 申明虚方法，通过重写实现多态。这样继承者就明确使用Start 和 Update 编程游戏对象行为，利用接口实现消息通知，避免与动作管理者直接依赖。
```c#
public class SSAction : ScriptableObject
    {
        public bool enable = true;
        public bool distroy = false;
        // Use this for initialization
        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        public ISSActionCallback callback { get; set; }

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }

    }
```
2.简单的动作实现(船移动，牧师魔鬼移动)
```c#
 public class CCMoveAction : SSAction
    {
        public Vector3 target;
        public float speed;
        public static CCMoveAction GetCCMoveAction(Vector3 target, float speed)
        {
            CCMoveAction action = ScriptableObject.CreateInstance<CCMoveAction>();
            action.target = target;
            action.speed = speed;
            return action;
        }
        public override void Start()
        {

        }

        public override void Update()
        {

            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if (this.transform.position == target)
            {
                this.distroy = true;
                this.callback.SSActionEvent(this);
            }
        }
    }
```
 * 组合动作实现，组合动作可以用来定义一系列动作，让这一系列动作可以按顺序执行(借用了老师的代码)
```c#
 public class CCSequenceAction : SSAction, ISSActionCallback
    {

        public List<SSAction> sequence;
        public int repeat = -1;
        public int start = 0;


        public static CCSequenceAction GetSSAction(int repeat, int start, List<SSAction> sequence)
        {
            CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
            action.repeat = repeat;
            action.sequence = sequence;
            action.start = start;
            return action;
        }

        public override void Update()
        {
            if (sequence.Count == 0) return;
            if (start < sequence.Count)
            {
                sequence[start].Update();
            }
        }

        public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int Param = 0, string strParam = null, Object objectParam = null)
        {
            source.distroy = false;
            this.start++;
            if (this.start >= sequence.Count)
            {
                this.start = 0;
                if (repeat > 0) repeat--;
                if (repeat == 0)
                {
                    this.distroy = true;
                    this.callback.SSActionEvent(this);
                }
            }
        }
        // Use this for initialization
        public override void Start()
        {
            foreach (SSAction action in sequence)
            {
                action.gameObject = this.gameObject;
                action.transform = this.transform;
                action.callback = this;
                action.Start();
            }
        }

        void OnDestory() { }
    }
```
* 动作时间接口定义
```c#
 public enum SSActionEventType : int { Started, Competeted }

    public interface ISSActionCallback
    {
        void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0, string strParam = null, Object objectParam = null);
    }
```
* 动作管理器基类
```c#
public class SSActionManager : MonoBehaviour
{

    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();
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

    protected void Start() {
       
    }
```
* 从动作管理器基类派生出一个类来管理具体过河动作
```c#
public class CCActionManager :SSActionManager, ISSActionCallback {
    public SSAction action1, action2;
    public CCSequenceAction sAction;
    public FirstController scene;
    float speed = 20f;
    void ISSActionCallback.SSActionEvent(SSAction source, SSActionEventType events, int intParam, string strParam, Object objectParam)
    {
        
    }

    // Use this for initialization
    void Start () {
        scene = Director.getInstance().currentSceneController as FirstController;
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
```

最后小小抱怨一句：实训已经开始了，希望潘老师在接下来几周呢手下留情一点