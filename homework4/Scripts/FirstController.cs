using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class FirstController : MonoBehaviour, SceneController, UserAction
{

    public CCActionManager actionManger { get; set; }
    public Queue<GameObject> disks = new Queue<GameObject>();
    public ScoreRecorder scoreRecorder { get; set; }
    public int diskNum;
    private int currentRound = -1;
    private int round = 3;
    private float time;
    private GameState state = GameState.START;
    UserGUI userGUI;

    void Awake()
    {
        Director dir = Director.getInstance();
        dir.currentSceneController = this;
        this.gameObject.AddComponent<ScoreRecorder>();
        this.gameObject.AddComponent<DiskFactory>();
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        Debug.Log(scoreRecorder == null);
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
    public void NextRound()
    {
        DiskFactory df = Singleton<DiskFactory>.Instance;
        for (int i = 0; i < diskNum; ++i)
        {
            disks.Enqueue(df.GetDisk(currentRound));
        }
        actionManger.StartThrow(disks);
    }

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

    public void GameOver()
    {
        //throw new System.NotImplementedException();
    }

    public int getScore()
    {
        //throw new System.NotImplementedException();
        return scoreRecorder.score;
    }
  

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
