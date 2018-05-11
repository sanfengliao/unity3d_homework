using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolCollideAndTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<PatrolData>().player = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<PatrolData>().player = null;
            EventPublisher publisher = Singleton<EventPublisher>.Instance;
            publisher.ScoreAdd();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
          
            EventPublisher publisher = Singleton<EventPublisher>.Instance;
            publisher.GameOver();
        }
    }
}
