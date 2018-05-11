using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneController : MonoBehaviour, SceneController,IUserAction {
    private PatrolActionManager actionManger;
    public bool game_over;
    private ScoreRecorder recorder;
    private UserGUI userGUI;
    public EventPublisher publisher;
    public GameObject player;
    public GameObject map;
    private List<GameObject> patrolsList;
    public int area;
    public void loadResources()
    {
        map = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/map"));
        player = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Player"));
        player.AddComponent<PlayerData>();
        actionManger = gameObject.AddComponent<PatrolActionManager>();
        recorder = gameObject.AddComponent<ScoreRecorder>();
        publisher = gameObject.AddComponent<EventPublisher>();
        userGUI = gameObject.AddComponent<UserGUI>();
        Director.getInstance().currentSceneController = this;
        PatrolFactory patrols = gameObject.AddComponent<PatrolFactory>();
        patrolsList = patrols.GetPatrols();
       
      
        
    }
    private void Awake()
    {
        loadResources();
    }
    public void StartGame()
    {
        for (int i = 0; i < patrolsList.Count; ++i)
        {
            actionManger.StartGoAction(patrolsList[i]);
        }
    }
    void Start () {
       
	}
	
	void Update () {
        player.GetComponent<PlayerData>().area = area;
	}
    private void OnEnable()
    {
        EventPublisher.ScoreAddEvent += AddScore;
        EventPublisher.GameOverEvent += GameOver;
    }
    public void MovePlayer()
    {
        if(!game_over)
        {
            float horizontal = Input.GetAxis("Horizontal"); //A D 左右
            float vertical = Input.GetAxis("Vertical"); //W S 上 下
            player.transform.Translate(Vector3.forward * vertical * 3 * Time.deltaTime);//W S 上 下
            if (vertical != 0)
            {
                player.GetComponent<Animator>().SetBool("run", true);
            }
            else
            {
                player.GetComponent<Animator>().SetBool("run", false);
            }

            player.transform.Rotate(0, horizontal * 50 * Time.deltaTime, 0);
        }
       
    }

    void AddScore()
    {
        recorder.addRecord();
    }

    void GameOver()
    {
        game_over = true;
        actionManger.DestroyAllAction();
    }

    public int GetScore()
    {
        return recorder.score;
    }
}
