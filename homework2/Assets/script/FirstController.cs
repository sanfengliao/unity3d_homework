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
    public void moveBoat()
    {
        if (boat.isEmpty())
        {
            return;
        }
        boat.move();
        userGUI.status = check_game_over();
    }

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
