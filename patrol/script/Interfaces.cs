using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Competeted }
public interface SceneController
{
    void loadResources();
}

public interface IUserAction
{
     void MovePlayer();
}

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, 
        int intParam = 0,  GameObject objectParam = null);
}