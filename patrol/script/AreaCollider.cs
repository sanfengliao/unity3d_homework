using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollider : MonoBehaviour {

    public int area;
    private FirstSceneController sceneController;
    void Start()
    {
        sceneController = Director.getInstance().currentSceneController as FirstSceneController;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sceneController.area = this.area;

        }
    }
}
