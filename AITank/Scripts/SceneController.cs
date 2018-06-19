using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour, IUserAction {
    public GameObject player;
    public float moveSpeed = 10.0f;		//玩家移动速度
    public float rotateSpeed = 60.0f;	//玩家旋转速度
    public int enemyNum = 6;
    public GameObject BustedTank;
    public int Score { get; set; }
    private bool isGameOver = false;
    private Factory f;
    void Awake()
    {
        f = Singleton<Factory>.Instance;
        player = f.GetPlayer();
        Director director = Director.getInstance();
        director.currentSceneController = this;
        Score = 0;
    }
    public void move()
    {
        float h = Input.GetAxisRaw("Horizontal");	//获取玩家水平轴上的输入
        float v = Input.GetAxisRaw("Vertical"); //获取玩家在垂直方向的输入
        player.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime * v);
        //v<0表示获取玩家向后的输入，玩家以moveSpeed的速度向后运动
        player.transform.Rotate(Vector3.up * h * rotateSpeed * Time.deltaTime);
    }

    public void shoot()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject b = f.GetBullet(0);
            b.transform.position = new Vector3(player.transform.position.x, 1.5f, player.transform.position.z) + player.transform.forward * 1.5f;
            b.transform.forward = player.transform.forward;//设置子弹方向
            Rigidbody rb = b.GetComponent<Rigidbody>();
            rb.AddForce(b.transform.forward * 20, ForceMode.Impulse);//发射子弹
        }
    }

    // Use this for initialization
    void Start () {
       for (int i = 0; i < enemyNum; ++i)
        {
            f.GetEnemy();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!isGameOver)
        {
            Camera.main.transform.position = new Vector3(player.transform.position.x, 15, player.transform.position.z);
            move();
            shoot();
            if(Score == enemyNum)
            {
                isGameOver = true;
            }
        }
        
    }
    public void SetGameOver(bool gameover)
    {
        isGameOver = gameover;
        if(gameover)
        {
            GameObject b = Instantiate<GameObject>(BustedTank);
            b.transform.position = player.transform.position;
        }
       
    }
    public bool GetGameOver()
    {
        return isGameOver;
    }
}
