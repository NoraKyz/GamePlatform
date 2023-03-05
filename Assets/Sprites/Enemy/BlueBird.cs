using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBird : Enemy
{
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        Move();
    }

    public void Move()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 playerPosition = player.transform.position;
            if (transform.position.x - playerPosition.x < 0)
            {
                Vector3 tmp = transform.localScale;
                tmp.x = -1;
                transform.localScale = tmp;
            }
            else
            {
                Vector3 tmp = transform.localScale;
                tmp.x = 1;
                transform.localScale = tmp;
            }
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, moveSpeed * Time.deltaTime);
        }
        
    }

}
