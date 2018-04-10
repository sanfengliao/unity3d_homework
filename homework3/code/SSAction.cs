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
        public bool enable = true;
        public bool distroy = false;
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

    }

    public class CCMoveAction : SSAction
    {
        public Vector3 target;
        public float speed;
        public static CCMoveAction GetCCMoveAction(Vector3 target, float speed)
        {
            CCMoveAction action = ScriptableObject.CreateInstance<CCMoveAction>();
            action.target = target;
            action.speed = speed;
            return action;
        }
        public override void Start()
        {

        }

        public override void Update()
        {

            this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
            if (this.transform.position == target)
            {
                this.distroy = true;
                this.callback.SSActionEvent(this);
            }
        }
    }

    public class CCSequenceAction : SSAction, ISSActionCallback
    {

        public List<SSAction> sequence;
        public int repeat = -1;
        public int start = 0;


        public static CCSequenceAction GetSSAction(int repeat, int start, List<SSAction> sequence)
        {
            CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
            action.repeat = repeat;
            action.sequence = sequence;
            action.start = start;
            return action;
        }

        public override void Update()
        {
            if (sequence.Count == 0) return;
            if (start < sequence.Count)
            {
                sequence[start].Update();
            }
        }

        public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted, int Param = 0, string strParam = null, Object objectParam = null)
        {
            source.distroy = false;
            this.start++;
            if (this.start >= sequence.Count)
            {
                this.start = 0;
                if (repeat > 0) repeat--;
                if (repeat == 0)
                {
                    this.distroy = true;
                    this.callback.SSActionEvent(this);
                }
            }
        }
        // Use this for initialization
        public override void Start()
        {
            foreach (SSAction action in sequence)
            {
                action.gameObject = this.gameObject;
                action.transform = this.transform;
                action.callback = this;
                action.Start();
            }
        }

        void OnDestory() { }
    }

}