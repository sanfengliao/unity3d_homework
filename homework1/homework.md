######解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系
	* 官方解释
		* GameObject: GameObjects are the fundamental objects in Unity that represent characters, props and scenery. They do not accomplish much in themselves but they act as containers for Components, which implement the real functionality.
		* Assets: An asset is representation of any item that can be used in your game or project. An asset may come from a file created outside of Unity, such as a 3D model, an audio file, an image, or any of the other types of file that Unity supports. There are also some asset types that can be created within Unity, such as an Animator Controller, an Audio Mixer or a Render Texture.
	* 游戏对象是unity中表示角色，道具，场景等的基本对象，他们本身不能完成许多功能，但他们可以作为组件的容器，有组件完成现实场景的动作
	* 资源表示可以在你的工程或者游戏中使用的一切物体
	* 资源可以被一个或多个游戏对象使用，有些资源可以作为模板来实例化游戏对象
######下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）
![这里写图片描述](http://img.blog.csdn.net/20180325224507094?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
* 如上图图所示结构，资源中可以放置图像、声音、脚本等。
###### 编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件
1. 基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
2. 常用事件包括 OnGUI() OnDisable() OnEnable()
```c#
	public class NewBehaviourScript : MonoBehaviour {
	// 当前控制脚本实例被装载的时候调用。一般用于初始化整个实例使用。（建议少用，此刻物体还未实例化出来，会影响程序的执行顺序）
    void Awake() {
        Debug.Log ("onAwake");
    }
    // 当前脚本执行update之前执行
    void Start () {
        Debug.Log ("onStart");
    }
	// 每一帧都执行
    void Update () {
        Debug.Log ("onUpdate");
    }
    //每固定帧绘制时执行一次，和Update不同的是FixedUpdate是渲染帧执行，如果你的渲染效率底下的时候FixedUpdate的调用次数就会下降。FixedUpdate比较适用于物理引擎的计算，因为是跟每帧渲染有关，而Update比较适合做控制。（放置游戏基本物理行为的代码，在Update之后执行）
    void FixedUpdate() {
        Debug.Log ("onFixedUpdate");
    }
    // 每帧执行完毕调用，他在所有Update结束后才调用，比较适合于命令脚本的执行。官网上例子是摄像机的跟随，都是在所有Update操作完才跟进摄像机，不然就有可能出现摄像机已经推进了，但是视角里还未有角色的空帧出现
    void LateUpdate() {
        Debug.Log ("onLateUpdate");
    }
    // 类似OnUpdate 每一帧均执行,在绘制GUI是执行
    void OnGUI() {
        Debug.Log ("onGUI");
    }
    // 对象变为可用或激活状态时此函数被调用，OnEnable不能用于协同程序。（物体启动时被调用
    void OnEnable() {
	    Debug.Log("OnEnable")
	}
	// 当对象变为不可用或非激活状态时此函数被调用。当物体销毁时它被调用，并且可用于任意清理代码。当脚本编译完成之后被重新加载时，OnDisable将被调用，OnEnable在脚本被载入后调用。（物体被禁用时调用）
    void OnDisable() {
        Debug.Log ("onDisable");
    }
    // 当NewBehaviourScript将被销毁时，这个函数被调用
    void OnDestroy() {
        Debug.Log ("onDestroy");
    }
}
```
###### 查找脚本手册，了解 GameObject，Transform，Component 对象
1. 分别翻译官方对三个对象的描述（Description）
	* GameObject: GameObject是unity中游戏角色，游戏道具，游戏场景等的基本角色，GameOjbect本身不能完成许多功能，但它可以作为Component的容器，有Component来完成一系列功能
	* Transform: Component的一种，决定了GameObject的位置，旋转，缩放。每个GameObject必须有Transform组件
	* Component: 是游戏中对象和行为的细节，是每个对象的功能部分
2. 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件
	![这里写图片描述](http://img.blog.csdn.net/20180325223031013?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
	* Table对象的属性：Tag属性用于区分游戏中不同类型的对象，Tag可以理解为一类元素的标记，可用GameObject.FindWithTag()来查询对象。
	* Table的Transform属性：Position、Rotation、Scale
	* Table的部件：Mesh Filter、Box Collider、Mesh Renderer
3. 用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）
![这里写图片描述](http://img.blog.csdn.net/20180325224049925?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvcXFfMzYyOTc5ODE=/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)
###### 整理相关学习资料，编写简单代码验证以下技术的实现：
	* 查找对象
		* 通过名字查找: public static GameObject Find(string name) 
		* 通过标签名查找单个对象: public static GameObject FindWithTag(string tag) 
		* 通过标签名查找多个对象: public static GameObject[] FindGameObjectsWithTag(string tag)  
	* 添加子对象
		* public static GameObect CreatePrimitive(PrimitiveTypetype) 
	* 遍历对象树
		* foreach (Transform child in transform) {  
	         Debug.Log(child.gameObject.name);  
	        }
  	* 清除所有子对象
	  	* foreach (Transform child in transform) {  
		         Destroy(child.gameObject);  
		  }  
######资源预设（Prefabs）与 对象克隆 (clone)
	* 预设（Prefabs）有什么好处？
		* 预设类似于一个模板，通过预设可以创建相同属性的对象，这些对象和预设关联。一旦预设发生改变，所有通过预设实例化的对象都会产生相应的变化，通过预设，可以避免修改多个对象属性
	* 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？
		* 对象克隆不受克隆本体的影响，因此A对象克隆的对象B不会因为A的改变而相应改变。
	* 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象
```c#
using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
  
public class shilihua : MonoBehaviour {  
    public GameObject MyTable;   
  
    // Use this for initialization  
    void Start () {  
        GameObject instance = (GameObject)Instantiate(MyTable, transform.position, transform.rotation);  
    }  
  
    // Update is called once per frame  
    void Update () {  
  
    }  
}  
```
###### 尝试解释组合模式（Composite Pattern / 一种设计模式）。使用 BroadcastMessage() 方法 向子对象发送消息
	* 组合模式允许用户将对象组合成树形结构来表现”部分-整体“的层次结构，使得客户以一致的方式处理单个对象以及对象的组合。组合模式实现的最关键的地方是——简单对象和复合对象必须实现相同的接口。这就是组合模式能够将组合对象和简单对象进行一致处理的原因。
	*  子类对象方法
		 void recallMessage() {  
	         print("Hello!");  
		}  
	* 父类对象方法
		 void Start () {  
	         this.BroadcastMessage("recallMessage");  
	 }  

##### 编程实战井字棋小游戏
井字棋小游戏其实只是一个2D游戏，具体实现过程并不是很难，主要是要掌握GUI的一些API的使用
具体代码实现如下：
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game : MonoBehaviour {
    private int turn = 1;

    private int[,] state = new int[3, 3];

	// Use this for initialization
	void Start () {
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
       
        //是否重新开始游戏
        if (GUI.Button(new Rect(180,50,100,50),"Reset")) {
            Reset();
        }
        int result = check();
        if (result == 1)
        {
            GUI.Label(new Rect(25, 200, 100, 50), "O WIN");
        }
        else if (result == 2)
        {
            GUI.Label(new Rect(25, 200, 100, 50), "X WIN");
        }
        else if (result == 0)
        {
            GUI.Label(new Rect(25, 200, 100, 50), "TIED");
        }
        Debug.Log(result);
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (state[i, j] == 1)
                {
                    GUI.Button(new Rect(i * 50, j * 50, 50, 50), "O");
                }
                else if (state[i, j] == 2)
                {
                    GUI.Button(new Rect(i * 50, j * 50, 50, 50), "X");
                }
                if (GUI.Button(new Rect(i * 50, j * 50, 50,50), ""))
                {
                    if (result == 0)
                    {
                        if (turn == 1)
                        {
                            state[i, j] = 1;
                        }
                        else
                        {
                            state[i, j] = 2;
                        }
                        turn = -turn;
                    }
                }
               
            }
        }
    }

    private void Reset()
    {
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                state[i, j] = 0;
            }
        }
    }
	//判断游戏是否结束
    int check()
    {
        //横向
        for (int i = 0; i < 3; ++i)
        {
            if (state[i, 0] != 0 && state[i, 0] == state[i, 1] && state[i, 1] == state[i, 2])
                return state[i, 0];
        }
        //纵向
        for (int j = 0; j < 3; ++j)
        {
            if (state[0, j] != 0 && state[0,j] == state[1,j] && state[1,j] == state[2,j])
            {
                return state[0, j];
            }
        }
        //斜线
        if (state[1, 1] != 0 && 
            (state[1, 1] == state[2, 2] && state[1, 1] == state[0, 0] || 
             state[1, 1] == state[0, 2] && state[1, 1] == state[2, 0]))
            return state[1, 1];

        return 0;
    }
}

```