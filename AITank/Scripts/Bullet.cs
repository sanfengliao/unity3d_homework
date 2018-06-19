using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int type; // 0 表示player的子弹， 1 表示enemy的子弹
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision collision)
    {
        ParticleSystem ps = Singleton<Factory>.Instance.GetBulletPs();
        ps.transform.position = transform.position;
        if (collision.gameObject.tag == "tank" && this.type == 0)
        {
            collision.gameObject.GetComponent<Tank>().Hp -= 3;
        } else if (collision.gameObject.tag == "Player" && this.type == 1)
        {
            collision.gameObject.GetComponent<Tank>().Hp -= 2;
        }
        ps.Play();
        if (this.gameObject.activeSelf)
        {
            Singleton<Factory>.Instance.RecycleBullet(this.gameObject);
        }
    }
   
}
