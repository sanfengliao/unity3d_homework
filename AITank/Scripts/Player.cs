using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank {

	// Use this for initialization
	void Start () {
        Hp = 10;
	}
	
	// Update is called once per frame
	void Update () {
		if (Hp <= 0)
        {
            ParticleSystem ps = Singleton<Factory>.Instance.GetTankPs();
            ps.transform.position = transform.position;
            ps.Play();
            this.gameObject.SetActive(false);
            Director.getInstance().currentSceneController.SetGameOver(true);
        }
	}
}
