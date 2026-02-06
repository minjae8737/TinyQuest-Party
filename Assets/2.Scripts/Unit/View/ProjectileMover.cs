using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    private Vector2? targetPos;
    private float speed;

    public void Init()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        targetPos = null;
        this.speed = 0;
    }
    
    public void Init(Vector2 target, float speed)
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        targetPos = target;
        this.speed = speed;
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
        Vector3 myPos = transform.position;
        transform.position = Vector2.MoveTowards(myPos, targetPos.Value, Time.deltaTime * speed);
    }
}