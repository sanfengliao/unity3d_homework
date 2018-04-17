这次我们用Unity3d开发一个简单的打飞碟游戏  
[视频连接](http://v.youku.com/v_show/id_XMzU0NTU2OTM3Ng==.html?spm=a2h3j.8428770.3416059.1)
#### 游戏简介
游戏有3个回合，每个回合会发射n中颜色的飞碟，击中飞碟会得到相应的分数，其中，击中黄色飞碟得1分，击中蓝色飞碟得2分，红色飞碟4分，击不中不扣分
#### 游戏思想
为了减少开销，提升游戏的运行速度，采用工厂模式对飞碟进行管理，飞碟的生产与回收由工厂来执行，剩下的场景管理，动作管理和上次牧师与魔鬼相同：[牧师与魔鬼](https://blog.csdn.net/qq_36297981/article/details/79886475)
#### 代码实现
飞碟的工厂类
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {

	//存放飞碟的队列，获取飞碟出队，回收飞碟入队
    private Queue<GameObject> diskFactory = new Queue<GameObject>();
    //飞碟生产方法
    public GameObject GetDisk(int round)
    {
        GameObject newDisk = null;
        if (diskFactory.Count > 0)
        {
            newDisk = diskFactory.Dequeue();
        }
        else
        {
            newDisk = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefab/disk"), Vector3.zero, Quaternion.identity);
            newDisk.AddComponent<DiskData>();
            newDisk.SetActive(false);
            
        }
        /*
			通过回合生产不同颜色的飞碟，第一回合黄色，第二回合黄色和蓝色，第三回合黄色、蓝色和红色，不同颜色的飞行速度不一样
		*/
        round = Random.Range(0, round + 1);
        switch (round)
        {
            case 0:
                
                newDisk.GetComponent<Renderer>().material.color = Color.yellow;
                newDisk.GetComponent<DiskData>().speed = 4.0f;
                int diraction = UnityEngine.Random.Range(-1f, 1f) > 0 ? -1 : 1;
                newDisk.GetComponent<DiskData>().flyDiraction = diraction;
                break;
            case 1:
                newDisk.GetComponent<Renderer>().material.color = Color.blue;
                newDisk.GetComponent<DiskData>().speed = 8.0f;
                int diraction1 = UnityEngine.Random.Range(-1f, 1f) > 0 ? -1 : 1;
                newDisk.GetComponent<DiskData>().flyDiraction = diraction1;
                break;
            case 2:
                newDisk.GetComponent<Renderer>().material.color = Color.red;
                newDisk.GetComponent<DiskData>().speed = 10.0f;
                int diraction2 = UnityEngine.Random.Range(-1f, 1f) > 0 ? -1 : 1;
                newDisk.GetComponent<DiskData>().flyDiraction = diraction2;
                break;
        }
        return newDisk;
    }
   //回收飞碟，入队
    public void FreeDisk(GameObject disk)
    {
        disk.SetActive(false);
        diskFactory.Enqueue(disk);
    }
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
       
	}
}
```
场景控制之baseCode.cs
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;
namespace Com.MyGame
{
    public enum GameState { ROUND_START,ROUND_FINISH, RUNNING, PAUSE,START}
    //游戏导演
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

    public interface UserAction
    {

        GameState GetGameState();
        void SetGameState(GameState state);
        void GameOver();
        int getScore();
        void restart();
        void hit();
        int GetRound();
    }
    /*
		感觉积分器放在这里不是很好，不过管它呢
	*/
    public class ScoreRecorder : MonoBehaviour
    {

        /** 
         * score是玩家得到的总分 
         */

        public int score;

        /** 
         * scoreTable是一个得分的规则表，每种飞碟的颜色对应着一个分数 
         */

        private Dictionary<Color, int> scoreTable = new Dictionary<Color, int>();

        // Use this for initialization  
        void Start()
        {
            score = 0;
            scoreTable.Add(Color.yellow, 1);
            scoreTable.Add(Color.blue, 2);
            scoreTable.Add(Color.red, 4);
        }

        public void Record(GameObject disk)
        {
            score += scoreTable[disk.GetComponent<Renderer>().material.color];
        }

        public void Reset()
        {
            score = 0;
        }
    }

}
```
场景控制之firstController:负责加载资源
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class FirstController : MonoBehaviour, SceneController, UserAction
{
	//动作管理器
    public CCActionManager actionManger { get; set; }
    //飞碟
    public Queue<GameObject> disks = new Queue<GameObject>();
    //分数
    public ScoreRecorder scoreRecorder { get; set; }
    //飞碟数量
    public int diskNum;
    //当前回合
    private int currentRound = -1;
    //回合数
    private int round = 3;
    //发射飞碟得时间间隔
    private float time;
    //游戏状态
    private GameState state = GameState.START;
    UserGUI userGUI;

    void Awake()
    {
        Director dir = Director.getInstance();
        dir.currentSceneController = this;
        this.gameObject.AddComponent<ScoreRecorder>();
        this.gameObject.AddComponent<DiskFactory>();
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
		
        diskNum = 10;
        userGUI = this.gameObject.AddComponent<UserGUI>()  as UserGUI;
    }

    private void Update()
    {
        if (currentRound == 3)
        {
            return;
        }
        if (actionManger.diskNum == 0 && state == GameState.RUNNING)
        {
            state = GameState.ROUND_FINISH;
        }
        if (actionManger.diskNum ==0 && state == GameState.ROUND_START)
        {
            currentRound = currentRound + 1;
            NextRound();
            actionManger.diskNum = 10;
            state = GameState.RUNNING;
        }
        if (time > 1)
        {
            if (state == GameState.RUNNING)
            {
                ThrowDisk();
            }
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }
    }
    //加载飞碟，每个回合获取10飞碟
    public void NextRound()
    {
        DiskFactory df = Singleton<DiskFactory>.Instance;
        for (int i = 0; i < diskNum; ++i)
        {
            disks.Enqueue(df.GetDisk(currentRound));
        }
        actionManger.StartThrow(disks);
    }
	//发射飞碟
    public void ThrowDisk()
    {
        if (disks.Count != 0)
        {
            GameObject disk = disks.Dequeue();
            float y = Random.Range(0, 7);
            float x = Random.Range(-8, 8);
            disk.transform.position = new Vector3(x, y, 0);
            disk.SetActive(true);
        }
    }
    /*
		因为页面比较单一，所以这个方法没有实现，如果想让场景多彩一点，可以试着实现该方法
	*/
    public void loadResources()
    {
       
    }

    
    public void restart()
    {
        currentRound = -1;
        //state = GameState.ROUND_START;
        state = GameState.START;
    }
	
    public GameState GetGameState()
    {
        return state;
    }

    public void SetGameState(GameState state)
    {
        this.state = state;
    }
	/*
	因为游戏没有设置Gameover这个功能，所以就没有实现该功能
	*/
    public void GameOver()
    {
        //throw new System.NotImplementedException();
    }

    public int getScore()
    {
        //throw new System.NotImplementedException();
        return scoreRecorder.score;
    }
  
	//监视鼠标点击的方法
    public void hit()
    {
        GameObject gameObject = null;
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) gameObject = hit.collider.gameObject;
        }
        if (gameObject != null && gameObject.GetComponent<DiskData>()!=null){
            scoreRecorder.Record(gameObject);
            gameObject.transform.position = new Vector3(0, -5, 0);
        }
      

    }

    public int GetRound()
    {
        return currentRound;
        //throw new System.NotImplementedException();
    }
}

```
场景控制之UserGUI：用来显示按钮，分数等
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class UserGUI : MonoBehaviour
{
    public FirstController scene;
    public int status = 0;
    public bool isFirst = true;
    GUIStyle style;
    GUIStyle buttonStyle;
    float time = 60;
    float second = 0;
    void Start()
    {
        //scene = Director.getInstance().currentSceneController as FirstController;
        scene = Director.getInstance().currentSceneController as FirstController;
        style = new GUIStyle();
        style.fontSize = 40;
        style.normal.textColor = Color.red;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 25;
        
    }
    private void Update()
    {
        //scene.click();

    }
    void OnGUI() {
        scene.hit();
        GUI.Label(new Rect(1000, 0, 400, 400), scene.getScore().ToString(),style);
        int buttonX = Screen.width / 2 - 45;
        int buttonY = Screen.height / 2 - 45;
        int labelX = Screen.width / 2 - 45;
        int labelY = Screen.height;
        游戏的暂停与继续
        if (scene.GetGameState() == GameState.RUNNING && GUI.Button(new Rect(10, 10,90,90 ), "Pause",buttonStyle) )
        {
            scene.SetGameState(GameState.PAUSE);
        }
        if (scene.GetGameState() == GameState.PAUSE && GUI.Button(new Rect(10, 10, 90,90), "Run", buttonStyle))
        {
            scene.SetGameState(GameState.RUNNING);
        }
		//其实后面这几个用来控制游戏开始和进入下一轮的的按钮的出现逻辑我总觉得怪怪的
        if (scene.GetRound() == 2 && scene.GetGameState() == GameState.ROUND_FINISH)
        {
            GUI.Label(new Rect(labelX, labelY, 90, 90), "You Have Finish All The Round");
            if (GUI.Button(new Rect(buttonX, buttonY, 90, 90), "Restart", buttonStyle))
            {
                scene.restart();
            }
            return;
        }
        if (scene.GetRound() == -1 && GUI.Button(new Rect(buttonX, buttonY, 90, 90), "Start",buttonStyle)) {
            scene.SetGameState(GameState.ROUND_START);
        }

         if (scene.GetGameState() == GameState.ROUND_FINISH  && scene.GetRound() < 2 && GUI.Button(new Rect(buttonX, buttonY, 90, 90), "Next Round", buttonStyle))
         {
            scene.SetGameState(GameState.ROUND_START);
         }
    }
}

```

动作管理之SSAction: SSAction是所有动作的基类，通过实现SSAciton来指定不同的动作
```
 public class SSAction : ScriptableObject
    {
        public bool enable = false;
        public bool distroy = true;
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
        public void reset()
        {
            enable = false;
            distroy = true;
            gameObject = null;
            transform = null;
            callback = null;
        }

    }
```
动作管理之CCFlyAction: 飞碟的飞行动作
```
public class CCFlyAction: SSAction
    {
        private float gravityAcceleration = 9.8f;
        private int diraction;
        private float speed;
        private float flyTime;
        public override void Start()
        {
            
            diraction = gameObject.GetComponent<DiskData>().flyDiraction;
            speed = gameObject.GetComponent<DiskData>().speed;
            enable = true;
            distroy = false;
            flyTime = 0;
        }
        public override void Update()
        {
           
            if (gameObject.activeSelf)
            {
                flyTime += Time.deltaTime;
                this.transform.Translate(new Vector3(diraction * Time.deltaTime * speed, -1 * flyTime * Time.deltaTime * gravityAcceleration, 0));
                if (this.transform.position.y < -4)
                {
                    this.enable = false;
                    this.distroy = true;
                    this.callback.SSActionEvent(this);
                }
            }
            
        }
        public static CCFlyAction GetAction()
        {
            CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction>();
            return action;
        }
    }
```
动作管理之动作管理器SSActionManager：这是所有动作管理器的基类
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Action;
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
}

```
动作管理之动作管理器CCActionManager：对SSManager进行了加强，用于管理某个对象的具体动作，为了减少开销，CCActionManager也作为了一个工厂，用来管理CCFlyAction
```
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
	    //游戏处于运行状态时飞碟才会飞
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

```
动作管理之ISSActionCallback：这个我也不知道怎么描述他的功能，说是用来作为ActionManager和Action之间的通信，但我感觉也能用来规定动作执行完之后需要执行的行为
```
 public enum SSActionEventType : int { Started, Competeted }

    public interface ISSActionCallback
    {
        void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0, string strParam = null, Object objectParam = null);
    }
```
用来实现单例模式的模板
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: MonoBehaviour {

    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    
                }
            }
            return instance;
        }
    }
}

```
