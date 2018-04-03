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

    public interface UserAction
    {
        void moveBoat();
        void characterIsClicked(MyCharactorController characterCtrl);
        void restart();
    }

    public class Moveable:MonoBehaviour
    {
        float speed = 20f;
        Vector3 dest;
        int moving_status;
        private void Update()
        {
            if (moving_status == 1)
            {
                if (transform.position == dest)
                {
                    moving_status = 0;
                    return;
                }
                    transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);
                
               

            }
        }
        public void setDestionation(Vector3 _dest)
        {
            dest = _dest;
            moving_status = 1;
        }
        public void reset()
        {
            moving_status = 0;
        }
    }
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

        public void GetOnBoat(BoatController boatController)
        {
            //下岸
            coast.removeCharacter(character.name);
            coast = null;

            moveToPosition(boatController.getEmptyPosition());
            character.transform.parent = boatController.getGameobj().transform;
            isOnBoat = true;
            boatController.addCharacterCtrl(this);
            
        }
        public bool getIsOnBoat()
        {
            return isOnBoat;
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
        public void addCharacter(MyCharactorController characterCtrl)
        {
            int index = getEmptyIndex();
            passengerPlaner[index] = characterCtrl;
        }
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
            boat.AddComponent(typeof(ClickGUI));
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

        public void addCharacterCtrl(MyCharactorController characterCtrl)
        {
            int index = getEmptyIndex();
            passenger[index] = characterCtrl;
        }
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
}
