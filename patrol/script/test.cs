using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    private float x;
    private float z;
    private Vector3 nextLocation;
    private int dir;
    private Vector3 startLocation;
	// Use this for initialization
	void Start () {
        dir = 1;
        x = 1;
        z = 2;
        //gameObject.GetComponent<Animator>().SetBool("walk", true);
        //startLocation = transform.position;
        nextLocation = new Vector3(3, 0, 5);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, nextLocation, Time.deltaTime);
        //if (Input.GetKey(KeyCode.A))
        //{
        //    gameObject.GetComponent<Animator>().SetBool("walk", true);
        //    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-2, 0, -4), Time.deltaTime * 2);
        //}
        //else
        //{
        //    gameObject.GetComponent<Animator>().SetBool("walk", false);
        //}
       
        /*if (Vector3.Distance(transform.position, nextLocation) < 0.1)
        {
            if (dir == 1)
            {
                nextLocation = new Vector3(x, 0, z);
            } else if (dir == 2)
            {
                nextLocation = new Vector3(startLocation.x, 0, z);
            }else if (dir == 3)
            {
                nextLocation = new Vector3(startLocation.x, 0, startLocation.z);
            }
            else
            {
                nextLocation = new Vector3(1, 0, startLocation.z);
            }
            dir = (dir + 1) % 4;
        }
        */
	}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triger");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
    }
}
