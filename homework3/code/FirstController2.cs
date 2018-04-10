using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.MyGame;

public class FirstController2 : MonoBehaviour, SceneController, UserAction
{

    public CCActionManager2 actionManger { get; set; }
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
