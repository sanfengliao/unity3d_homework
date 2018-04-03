###太阳系
##### 1. 游戏对象运动的本质是什么
	简单来说就是游戏对象跟随每一帧的变化，空间上发生变化。这里的空间变化包括了游
	戏对象的transform组件中的position和rotation两个属性，前者是绝对或者相对
	位置的改变，后者是所处位置的角度的旋转变化。

##### 2. 使用三种以上的方法实现物体的抛物线运动
	抛物线运动的本质就是改变物体的position属性,以下三种方法都是改变物体position的，只是实现不同
 方法一:  
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    private float speed = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.left * 5.0f ;
        transform.position += Vector3.down * speed * Time.deltaTime;
        speed += 9.8f * Time.deltaTime;
	}
}
```
方法二
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    private float speed = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += new Vector3(Time.deltaTime * 5.0f, Time.deltaTime * speed, 0);
        speed -= Time.deltaTime * 9.8f;
	}
}

```
方法三:
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    private float speed = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(Time.deltaTime * 5.0f, Time.deltaTime * speed, 0));
        speed -= Time.deltaTime * 9.8f;
	}
}
```
##### 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。
**添加GameObject**
![这里写图片描述](http://img.blog.csdn.net/20180401234024570?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
**给每个物体添加不同的材料(简单点的话就是直接把图片拖到GameObject上)**

**给行星添加公转行为(orgin为sun)(月球的公转行为也一样,orgin为earth）**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    public Transform orgin;
    public float speed;
    float x, z;
	// Use this for initialization
	void Start () {
		//使每个行星的法平面不同，不过每次启动行星的法平面都会发生变化，最好可以固定下来吧
        x = Random.Range(0.1f, 0.3f);
        z = Random.Range(0.1f, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 axios = new Vector3(x, 1, z);
        this.transform.RotateAround(orgin.position, axios, speed * Time.deltaTime);
	}
}
```

**给太阳、行星和月球添加自转行为**
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selfRotate : MonoBehaviour {

    public float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.RotateAround(this.transform.position, Vector3.up, speed * Time.deltaTime);
	}
}
```
**添加背景**
		*创建skybox的Materials，将Shader设为panoramic，将宇宙的图片拖入Spherial中
![这里写图片描述](http://img.blog.csdn.net/20180401234934170?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
	将windows下的lighting的skybox Material设为刚刚创建的Matertial
然后特别难看的太阳系就出来了
![这里写图片描述](http://img.blog.csdn.net/20180401235306889?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
![这里写图片描述](http://img.blog.csdn.net/20180401235324892?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
### 魔鬼与牧师

#### 游戏规则
	* 你要运用智慧帮助3个牧师（方块）和3个魔鬼（圆球）渡河。
	* 船最多可以载2名游戏角色。
	* 船上有游戏角色时，你才可以点击这个船，让船移动到对岸。
	* 当有一侧岸的魔鬼数多余牧师数时（包括船上的魔鬼和牧师），魔鬼就会失去控制，吃掉牧师（如果这一侧没有牧师则不会失败），游戏失败。
	* 当所有游戏角色都上到对岸时，游戏胜利。
#### 项目截图:
![这里写图片描述](http://img.blog.csdn.net/20180403193054228?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
![这里写图片描述](http://img.blog.csdn.net/20180403193226891?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
![这里写图片描述](http://img.blog.csdn.net/20180403193710106?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
##### 游戏使用MVC结构
	unity的每个GameObject都是model
	model有Controller来控制,boat由BoatController来控制，coast由CoastController控制,character有characterController来控制,同时FirstController为最高一层的Controller，控制其他Controller，同时来控制场景的加载
	同时还有一个Director对象，一个项目只能有一个director，用来协调各个controller之间的通信
	View就是UserGUI和ClickGUI，它们展示游戏结果，并提供用户交互的渠道（点击物体和按钮）。

##### Director的定义
```
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
```
director在游戏中只有一个实例，方便协调各个controller之间的通信.
Director类使用了单例模式。第一次调用Director.getInstance()时，会创建一个新的Director对象，保存在_instance，此后每次调用getInstance，都回返回_instance。也就是说Director最多只有一个实例。这样，我们在任何Script中的任何地方通过Director.getInstance()都能得到同一个Director对象，也就可以获得同一个currentSceneController，这样我们就可以轻易实现类与类之间的通信

#### SceneController 加载场景的接口，需要由firstController实现
```
  public interface SceneController
    {
        void loadResources();
    }
```

#### UserAction 用于相应用户的点击,同样需要firstController实现
```
public interface UserAction
    {
        void moveBoat();//用户点击船时，船移动
        void characterIsClicked(MyCharactorController characterCtrl);//用户点击角色是，角色上船或上岸
        void restart();//用户点击restart按钮，重新开始游戏
    }
```
#### Moveable
Moveable是一个可以挂载在GameObject上的类，用于控制GameObject的移动
```
 public class Moveable:MonoBehaviour
    {
        float speed = 20f;
        Vector3 dest;
        private void Update()
        {
                if (transform.position == dest)
                {
                    return;
                }
                    transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
        }
        public void setDestionation(Vector3 _dest)
        {
            dest = _dest;
        }
    }
```
#### ClickUI用来相应用户点击
```
	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class ClickGUI : MonoBehaviour
{
    UserAction action;
    MyCharactorController characterController;

    public void setController(MyCharactorController characterCtrl)
    {
        characterController = characterCtrl;
    }

    void Start()
    {
        action = Director.getInstance().currentSceneController as UserAction;
    }

    void OnMouseDown()
    {
        if (gameObject.name == "boat")//点击了船，船移动
        {
            action.moveBoat();
        }
        else //否则是角色上船/上岸
        {
            action.characterIsClicked(characterController);
        }
    }
}
```
#### BoatController
```
	 public class BoatController
    {
        GameObject boat;
        Moveable moveable;
        Vector3 startPosition = new Vector3(4, 0.7f, 0);
        Vector3 endPosition = new Vector3(-4, 0.7f, 0);
        Vector3[] start_positions;
        Vector3[] end_positions;
        MyCharactorController[] passenger = new MyCharactorController[2];
        int start_or_end; // 1 start -1 end
        public BoatController()
        {
            start_or_end = 1;
            start_positions = new Vector3[] { new Vector3(3F, 1.5F, 0), new Vector3(4.5F, 1.5F, 0) };
            end_positions = new Vector3[] { new Vector3(-5F, 1.5F, 0), new Vector3(-3.5F, 1.5F, 0) };
            boat = Object.Instantiate(Resources.Load("prefab/boat", typeof(GameObject)), startPosition, Quaternion.identity, null) as GameObject;
            boat.name = "boat";

            moveable = boat.AddComponent(typeof(Moveable)) as Moveable;
            boat.AddComponent(typeof(ClickGUI));//给船添加点击事件
        }
        public void move()
        {
            if (start_or_end == 1)
            {
                moveable.setDestionation(endPosition);
                start_or_end = -1;
            }
            else
            {
                moveable.setDestionation(startPosition);
                start_or_end = 1;
            }
        }
        public int getEmptyIndex()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }

        public bool isEmpty()
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null)
                {
                    return false;
                }
            }
            return true;
        }

        public Vector3 getEmptyPosition()
        {
            Vector3 pos;
            int emptyIndex = getEmptyIndex();
            if (start_or_end == -1)
            {
                pos = end_positions[emptyIndex];
            }
            else
            {
                pos = start_positions[emptyIndex];
            }
            return pos;
        }
		//魔鬼或牧师上船时，添加船员
        public void addCharacterCtrl(MyCharactorController characterCtrl)
        {
            int index = getEmptyIndex();
            passenger[index] = characterCtrl;
        }
        //魔鬼或牧师上岸时，添加船员
        public MyCharactorController removeCharacterCtrl(string passenger_name)
        {
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] != null && passenger[i].getName() == passenger_name)
                {
                    MyCharactorController charactorCtrl = passenger[i];
                    passenger[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("Cant find passenger in boat: " + passenger_name);
            return null;
        }

        public GameObject getGameobj()
        {
            return boat;
        }

        public int get_start_or_end()
        {
            return start_or_end;
        }

        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < passenger.Length; i++)
            {
                if (passenger[i] == null)
                    continue;
                if (passenger[i].getType() == 0)
                {   // 0->priest, 1->devil
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            moveable.reset();
            if (start_or_end == -1)
            {
                move();
            }
            passenger = new MyCharactorController[2];
        }

    }
```
##### BoastController
```
    public class CoastController
    {
        GameObject coast;
        Vector3 start = new Vector3(11, 0.5f, 0);
        Vector3 end = new Vector3(-11, 0.5f, 0);
        int start_or_end; //1 start -1 end
        Vector3[] positions;
        MyCharactorController[] passengerPlaner;
        public CoastController(string _to_or_from)
        {
            positions = new Vector3[] {new Vector3(7F,2F,0), new Vector3(8.5F,2f,0), new Vector3(10,2,0),
                new Vector3(11.5F,2,0), new Vector3(13,2F,0), new Vector3(14.5F,2F,0)};

            passengerPlaner = new MyCharactorController[6];

            if (_to_or_from == "start")
            {
                coast = Object.Instantiate(Resources.Load("prefab/coast", typeof(GameObject)), start, Quaternion.identity, null) as GameObject;
                coast.name = "start";
                start_or_end = 1;
            }
            else
            {
                coast = Object.Instantiate(Resources.Load("prefab/coast", typeof(GameObject)), end, Quaternion.identity, null) as GameObject;
                coast.name = "end";
                start_or_end = -1;
            }
        }

        public int getEmptyIndex()
        {
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
        public Vector3 getEmptyPosition()
        {
            Vector3 pos = positions[getEmptyIndex()];
            pos.x *= start_or_end;
            return pos;
        }
        public int getStartOrEnd()
        {
            return start_or_end;
        }
        //上岸时，增加成员
        public void addCharacter(MyCharactorController characterCtrl)
        {
            int index = getEmptyIndex();
            passengerPlaner[index] = characterCtrl;
        }
        //上船时，减少岸上的成员
        public MyCharactorController removeCharacter(string passenger_name)
        {   // 0->priest, 1->devil
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] != null && passengerPlaner[i].getName() == passenger_name)
                {
                    MyCharactorController charactorCtrl = passengerPlaner[i];
                    passengerPlaner[i] = null;
                    return charactorCtrl;
                }
            }
            Debug.Log("cant find passenger on coast: " + passenger_name);
            return null;
        }
		//分别得到岸上牧师和魔鬼的数量
        public int[] getCharacterNum()
        {
            int[] count = { 0, 0 };
            for (int i = 0; i < passengerPlaner.Length; i++)
            {
                if (passengerPlaner[i] == null)
                    continue;
                if (passengerPlaner[i].getType() == 0)
                {   // 0->priest, 1->devil
                    count[0]++;
                }
                else
                {
                    count[1]++;
                }
            }
            return count;
        }

        public void reset()
        {
            passengerPlaner = new MyCharactorController[6];
        }

    }
```
##### MycharacterController
```
public class MyCharactorController
    {
        GameObject character;
        Moveable moveable;
        int characterType;
        bool isOnBoat;
        CoastController coast;
        ClickGUI clickGUI;
        public  MyCharactorController(string type)
        {
            if (type == "priest")
            {
                character = Object.Instantiate(Resources.Load("prefab/priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                characterType = 0;
            }
            else
            {
                character = Object.Instantiate(Resources.Load("prefab/devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
                characterType = 1;
            }
            moveable = character.AddComponent(typeof(Moveable)) as Moveable;
            clickGUI = character.AddComponent(typeof(ClickGUI)) as ClickGUI;
            clickGUI.setController(this);
        }
        public void setName(string name)
        {
            character.name = name;
        }

        public void setPosition(Vector3 pos)
        {
            character.transform.position = pos;
        }

        public void moveToPosition(Vector3 destination)
        {
            moveable.setDestionation(destination);
        }

        public int getType()
        {   // 0->priest, 1->devil
            return characterType;
        }

        public string getName()
        {
            return character.name;
        }
		  public bool getIsOnBoat()
        {
            return isOnBoat;
        }
        public void GetOnBoat(BoatController boatController)
        {
            //下岸
            coast.removeCharacter(character.name);
            coast = null;
			//上船
            moveToPosition(boatController.getEmptyPosition());
            character.transform.parent = boatController.getGameobj().transform;
            isOnBoat = true;
            boatController.addCharacterCtrl(this);
            
        }
      
        public void getOnCoast(CoastController coastController, BoatController boatController)
        {
            //下船
            character.transform.parent = null;
            if (boatController != null)
                boatController.removeCharacterCtrl(character.name);
            //上岸
            moveToPosition(coastController.getEmptyPosition());
            coast = coastController;
            coastController.addCharacter(this);
            isOnBoat = false;

        }
        public void reset()
        {
            moveable.reset();
            coast = (Director.getInstance().currentSceneController as FirstController).startCoast;
            getOnCoast(coast, null);
            setPosition(coast.getEmptyPosition());
        }
        public CoastController GetCoastController()
        {
            return coast;
        }

    }
```
##### FirstController
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class FirstController : MonoBehaviour, SceneController, UserAction {

    Vector3 river_pos = new Vector3(0, 0, 0);
    UserGUI userGUI;
    public CoastController startCoast;
    public CoastController endCoast;
    public BoatController boat;
    private MyCharactorController[] characterControllers;
    void Awake()
    {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        characterControllers = new MyCharactorController[6];
        loadResources();
    }

    public void loadResources()
    {
        GameObject river = Instantiate(Resources.Load("prefab/river", typeof(GameObject)), river_pos, Quaternion.identity, null) as GameObject;
        river.name = "river";
        startCoast = new CoastController("start");
        endCoast = new CoastController("end");
        boat = new BoatController();
        loadCharacters();
    }
    private void loadCharacters()
    {
        for (int i = 0; i < 3; ++i)
        {
            MyCharactorController ch = new MyCharactorController("priest");
            ch.setName("priest" + i);
            ch.setPosition(startCoast.getEmptyPosition());
            ch.getOnCoast(startCoast, null);
            characterControllers[i] = ch;
        }
        for (int i = 0; i < 3; ++i)
        {
            MyCharactorController cha = new MyCharactorController("devil");
            cha.setName("devil" + i);
            cha.setPosition(startCoast.getEmptyPosition());
            cha.getOnCoast(startCoast, null);
            characterControllers[i + 3] = cha;

        }
    }
    //用户点击船时，船移动
    public void moveBoat()
    {
        if (boat.isEmpty())
        {
            return;
        }
        boat.move();
        userGUI.status = check_game_over();
    }
	//检查量变牧师和魔鬼的数量
    int check_game_over()
    {
        int start_priest = 0;
        int start_devil = 0;
        int end_priest = 0;
        int end_devil = 0;
        int[] start_count = startCoast.getCharacterNum();
        start_priest += start_count[0];
        start_devil += start_count[1];
        int[] end_count = endCoast.getCharacterNum();
        end_priest += end_count[0];
        end_devil += end_count[1];
        if (end_devil + end_priest == 6)
        {
            return 2;//WIN
        }
        int[] boat_num = boat.getCharacterNum();
        if (boat.get_start_or_end() == -1)
        {
            end_priest += boat_num[0];
            end_devil += boat_num[1];
        }
        else
        {
            start_priest += boat_num[0];
            start_devil += boat_num[1];
        }

        if (start_priest < start_devil && start_priest > 0)
        {
            return 1;//LOSE
        }
        if (end_devil > end_priest && end_priest > 0)
        {
            return 1;
        }
        return 0;//NOT FINISH
    }

    public void restart()
    {
        boat.reset();
        startCoast.reset();
        endCoast.reset();
        for (int i = 0; i < characterControllers.Length; ++i)
        {
            characterControllers[i].reset();
        }
    }
	//点击角色时的角色的行为
    public void characterIsClicked(MyCharactorController characterCtrl)
    {
        if (characterCtrl.getIsOnBoat())
        {
            if (boat.get_start_or_end() == -1)
            {
                characterCtrl.getOnCoast(endCoast, boat);
            }
            else
            {
                characterCtrl.getOnCoast(startCoast, boat);
            }
        }
        else
        {
            CoastController whichCoast = characterCtrl.GetCoastController();
            if (boat.getEmptyIndex() == -1)
            {
                return;
            }
            if (boat.get_start_or_end() != whichCoast.getStartOrEnd() )
            {
                return;
            }
            characterCtrl.GetOnBoat(boat);
        }
        userGUI.status = check_game_over();
    }
}
```

#### USERGUI
```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class UserGUI : MonoBehaviour
{
    private UserAction action;
    public int status = 0;
    GUIStyle style;
    GUIStyle buttonStyle;
    float time = 60;
    float second = 0;
    void Start()
    {
        action = Director.getInstance().currentSceneController as UserAction;

        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
    }
    void OnGUI()
    {
	    //倒计时
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
		//判断游戏状态
        if (status == 1 || time <= 0)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You Fail!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                time = 60;
                action.restart();
                second = 0;
            }
        }
        else if (status == 2)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 85, 100, 50), "You win!", style);
            if (GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle))
            {
                status = 0;
                time = 60;
                action.restart();
                second = 0;
            }
        }
    }
}
```

[参考链接](https://www.jianshu.com/p/07028b3da573)
如有雷同，纯属.......