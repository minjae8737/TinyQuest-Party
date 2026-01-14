using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public CharacterController controller;
    public float curTime;

    void Update()
    {
        // 이동
        Vector2 moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        controller.DoMove((Vector2)transform.position + moveDir);
        curTime = Time.time;

        // 공격
        if (Input.GetKeyDown(KeyCode.Q))
        {
            controller.DoAttack(0, transform.position, curTime);
        }

        // 스킬 1
        if (Input.GetKeyDown(KeyCode.E))
        {
            controller.DoAttack(1, transform.position, curTime);
        }

        //스킬 2
        if (Input.GetKeyDown(KeyCode.R))
        {
            controller.DoAttack(2, transform.position, curTime);
        }
    }
}