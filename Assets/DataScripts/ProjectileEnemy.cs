using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : BaseEnemy
{
    public GameObject poisonPrefab;

    public Vector2Int[] patrolPath =
    {

    };

    //vector zero is no shot
    public Vector2Int[] shotPattern =
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

        if (LevelData.instance.walls.GetTile(new Vector3Int(targetPosition.x, targetPosition.y, 0)) == null)
        {
            if (PlayerController.instance.GetPosition() == targetPosition)
            {
                patrolStep--;
                PlayerController.instance.TakeDamage(contactDamage);
            }
            else
            {
                lastPosition = position;
                position = targetPosition;
                transform.position = (Vector2)position;

                if(shotPattern[patrolStep] != Vector2Int.zero)
                {
                    Poison poison = Instantiate(poisonPrefab, transform.position + (Vector3)(Vector2)shotPattern[patrolStep], Quaternion.identity).GetComponent<Poison>();
                    poison.spellDirection = shotPattern[patrolStep];
                }
            }
        }

        patrolStep++;
        if (patrolStep > patrolPath.Length - 1) patrolStep = 0;
    }
}
