using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Boss : BaseEnemy
{
    public GameObject poisonPrefab;

    public Vector2Int[] patrolPath =
    {

    };

    //vector zero is no shot
    //vector -1 -1 is circle
    //vector 1 1 is RandomWaves
    public Vector2Int[] shotPattern =
    {

    };

    private int patrolStep;

    public override void Start()
    {
        Console.instance.ConsoleLog("打败霸王");
        base.Start();
        patrolStep = 0;
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

                if (shotPattern[patrolStep] != Vector2Int.zero)
                {
                    if(shotPattern[patrolStep] == new Vector2Int(-1, -1))
                    {
                        CircleAttack();
                    }
                    if(shotPattern[patrolStep] == new Vector2Int(1, 1))
                    {
                        RandomWaves();
                    }
                    
                }
            }
        }

        patrolStep++;
        if (patrolStep > patrolPath.Length - 1) patrolStep = 0;
    }

    public Tile stairs;

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (hitPoints <= 0)
        {
            Vector2Int stairPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y);
            LevelData.instance.floor.SetTile(new Vector3Int(stairPosition.x, stairPosition.y, 0), stairs);
            LevelData.instance.stairPosition = stairPosition;
        }
    }

    private void CircleAttack()
    {
        SFX.instance.PlayPoison();
        Vector2Int[] pattern =
        {
            new Vector2Int(1,1),
            new Vector2Int(-1,1),
            new Vector2Int(-1,-1),
            new Vector2Int(1,-1),
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,-1),
            new Vector2Int(0,1),
        };

        foreach(Vector2Int dir in pattern)
        {
            Poison poison = Instantiate(poisonPrefab, transform.position + (Vector3)(Vector2)dir, Quaternion.identity).GetComponent<Poison>();
            poison.spellDirection = dir;
        }
    }

    private void RandomWaves()
    {
        SFX.instance.PlayPoison();
        Vector2Int dir = Vector2Int.zero;

        int randint = Random.Range(0, 4);

        switch (randint)
        {
            case 0:
                dir = Vector2Int.right;
                break;
            case 1:
                dir = Vector2Int.up;
                break;
            case 2:
                dir = Vector2Int.left;
                break;
            case 3:
                dir = Vector2Int.down;
                break;
        }


        Vector3 position = transform.position + (Vector3)(Vector2)dir;
        Instantiate(poisonPrefab, position, Quaternion.identity).GetComponent<Poison>().spellDirection = dir;

        if (dir == Vector2Int.right || dir == Vector2Int.left)
        {
            Instantiate(poisonPrefab, position + Vector3.up, Quaternion.identity).GetComponent<Poison>().spellDirection = dir;
            Instantiate(poisonPrefab, position + Vector3.down, Quaternion.identity).GetComponent<Poison>().spellDirection = dir;
        }
        else
        {
            Instantiate(poisonPrefab, position + Vector3.left, Quaternion.identity).GetComponent<Poison>().spellDirection = dir;
            Instantiate(poisonPrefab, position + Vector3.right, Quaternion.identity).GetComponent<Poison>().spellDirection = dir;
        }
    }
}
