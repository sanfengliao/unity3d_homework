using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    private FirstSceneController scene;
    public float distanceH = 1f;
    public float distanceV = 1f;
    void Start()
     {
         scene = Director.getInstance().currentSceneController as FirstSceneController;
         player = scene.player.transform;
        
     }

    void LateUpdate()
    {
        Vector3 nextpos = player.forward * -4f + player.up * 3f + player.position;

        this.transform.position = nextpos;

        this.transform.LookAt(player);
    }
}
