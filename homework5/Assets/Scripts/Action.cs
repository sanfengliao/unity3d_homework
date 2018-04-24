using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Action
{
    public enum SSActionEventType : int { Started, Competeted }

    public interface ISSActionCallback
    {
        void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
            int intParam = 0, string strParam = null, Object objectParam = null);
    }
    public class SSAction : ScriptableObject
    {
        public bool hasRigiBody = false;
        public bool enable = false;
        public bool distroy = true;
        // Use this for initialization
        public GameObject gameObject { get; set; }
        public Transform transform { get; set; }
        public ISSActionCallback callback { get; set; }

        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        // Update is called once per frame
        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }

        public virtual void FixedUpdate()
        {
            throw new System.NotImplementedException();
        }
        public void reset()
        {
            enable = false;
            distroy = true;
            gameObject = null;
            transform = null;
            callback = null;
        }

    }
    public class CCFlyAction: SSAction
    {
        private float gravityAcceleration = 9.8f;
        private float verticalSpeed;
        private int diraction;
        private float speed;
        private float flyTime;
        public override void Start()
        {
            
            diraction = gameObject.GetComponent<DiskData>().flyDiraction;
            speed = gameObject.GetComponent<DiskData>().speed;
            enable = true;
            distroy = false;
            flyTime = 0;
            verticalSpeed = 5;
            Rigidbody rigidbody= gameObject.GetComponent<Rigidbody>();
            //如果添加了rigidbody ,添加一个初速度
            if (rigidbody)
            {
                Debug.Log("=====");
                rigidbody.velocity = new Vector3(speed, UnityEngine.Random.Range(1, 3));
            }
        }
        public override void Update()
        {
           
            if (gameObject.activeSelf)
            {
                flyTime += Time.deltaTime;
                this.transform.Translate(new Vector3(diraction * Time.deltaTime * speed,  Time.deltaTime * verticalSpeed, 0));
                if (this.transform.position.y < -4)
                {
                    this.enable = false;
                    this.distroy = true;
                    this.callback.SSActionEvent(this);
                }
                verticalSpeed = verticalSpeed - gravityAcceleration * Time.deltaTime;
            }
            
        }
        public override void FixedUpdate()
        {
            if (gameObject.activeSelf)
            {
                if (this.transform.position.y < -4)
                {
                    this.enable = false;
                    this.distroy = true;
                    this.callback.SSActionEvent(this);
                }
            }
            
        }
        public static CCFlyAction GetAction()
        {
            CCFlyAction action = ScriptableObject.CreateInstance<CCFlyAction>();
            return action;
        }
    }

}