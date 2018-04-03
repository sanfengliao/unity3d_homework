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
        if (gameObject.name == "boat")
        {
            action.moveBoat();
        }
        else
        {
            action.characterIsClicked(characterController);
        }
    }
}