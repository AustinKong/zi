using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitEnemy : BaseEnemy
{
    public override void Start()
    {
        base.Start();
    }

    private int chaseTick = 1;

    public override void ActionTick()
    {
        base.ActionTick();
        List<Vector2Int> path = CalculateLine(new Vector2Int((int)transform.position.x, (int)transform.position.y), PlayerController.instance.GetPosition());
        
        if (!PathObstructed(path))
        {
            Vector2Int targetPosition = path[0];

            if (chaseTick == 0)
            {
                if (PlayerController.instance.GetPosition() == targetPosition)
                {
                    PlayerController.instance.TakeDamage(contactDamage);
                    chaseTick = 1;
                }
                else
                {
                    lastPosition = position;
                    position = targetPosition;
                    transform.position = (Vector2)position;
                    chaseTick = 1;
                }
            }
            else chaseTick--; //allows enemy to be stunned when hit

            
            
        }

    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        chaseTick = 1;
    }

    private bool PathObstructed(List<Vector2Int> path)
    {
        foreach(Vector2Int point in path)
        {
            if(LevelData.instance.walls.GetTile(new Vector3Int(point.x, point.y, 0)) != null)
            {
                return true;
            }
        }
        return false;
    }

    private List<Vector2Int> CalculateLine(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        Vector2 currPoint = start;
        Vector2Int approximatedPoint = start;
        Vector2 delta = ((Vector2)(end - start)).normalized;


        int failSafe = 0;
        while (Vector2.Distance(approximatedPoint,end) > 0.5f)
        {
            if(failSafe > 100)
            {
                Debug.Log("failed");
                break;
            }
            failSafe++;
            currPoint += delta;
            approximatedPoint = new Vector2Int(Mathf.RoundToInt(currPoint.x), Mathf.RoundToInt(currPoint.y));
            points.Add(approximatedPoint);
        }

        points.Add(PlayerController.instance.GetPosition());
        return points;
    }
}
