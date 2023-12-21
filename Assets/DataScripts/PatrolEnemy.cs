using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : BaseEnemy
{
    public Vector2Int[] patrolPath =
    {
        
    };

    private int patrolStep;

    public override void Start()
    {
        base.Start();
        patrolStep = Random.Range(0, patrolPath.Length);
    }

    public override void ActionTick()
    {
        base.ActionTick();

        Vector2Int targetPosition = position + patrolPath[patrolStep];

        if (LevelData.instance.walls.GetTile(new Vector3Int(targetPosition.x, targetPosition.y,0)) == null)
        {
            if(PlayerController.instance.GetPosition() == targetPosition)
            {
                patrolStep--;
                PlayerController.instance.TakeDamage(contactDamage);
            }
            else
            {
                lastPosition = position;
                position = targetPosition;
                transform.position = (Vector2)position;
            }
        }

        patrolStep++;
        if (patrolStep > patrolPath.Length - 1) patrolStep = 0;
    }
}
