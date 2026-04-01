using System;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private Vector2? targetPos;
    private float speed;

    private Action OnArrived;

    private void OnDisable()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        targetPos = null;
        this.speed = 0;
    }

    public void Init(Vector2 startPos, Vector2 targetPos, float speed, Action action = null)
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.position = startPos;
        this.targetPos = targetPos;
        this.speed = speed;
        OnArrived = action;
        Rotate();
    }

    private void Rotate()
    {
        Vector2 myPos = transform.position;
        Vector2 dir = targetPos.Value - myPos;
        float angle = Vector2.SignedAngle(Vector2.right, dir);

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        if (targetPos.HasValue)
        {
            DoMove();
        }
    }

    private void DoMove()
    {
        Vector2 myPos = transform.position;
        Vector2 nextPos = Vector2.MoveTowards(myPos, targetPos.Value, Time.deltaTime * speed);

        transform.position = nextPos;
        
        // targetPos에 도착
        if ((nextPos - targetPos.Value).sqrMagnitude <  0.001f)
        {
            OnArrived?.Invoke();
            targetPos = null;
            OnArrived = null;
        }
    }

    public float GetArrivedTime()
    {
        if (!targetPos.HasValue) return 0f;
        float distance = Vector2.Distance(transform.position, targetPos.Value);

        return distance / speed; 
    }
}