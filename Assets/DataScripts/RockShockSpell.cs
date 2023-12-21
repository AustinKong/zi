using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockShockSpell : BaseSpell
{
    public GameObject projectilePrefab;
    public override void StartSpell()
    {
        Console.instance.ConsoleLog("使用" + spellName + "技能，请指定方向");
        StartCoroutine(AwaitingDirectionalInput());
    }

    private IEnumerator AwaitingDirectionalInput()
    {
        PlayerController.awaitingInput = true;
        while (true)
        {
            yield return null;

            if (AnyArrowKeyPressed())
            {
                GameManager.instance.PlayerAction();
                CastSpell();
                break;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Console.instance.ConsoleLog("取消使用技能");
                break;
            }
        }
        PlayerController.awaitingInput = false;
    }

    public override void CastSpell()
    {
        //SFX.instance.PlayFire();
        base.CastSpell();
        StartCoroutine(ShockWave());
        
    }

    private IEnumerator ShockWave()
    {
        SFX.instance.PlayRock();
        Vector2Int arrowDir = GetArrowKeyDirection();
        yield return new WaitForSeconds(0.08f);
        List<GameObject> projectiles = new List<GameObject>();
        for(int i = 1; i < 12; i++)
        {
            Vector2Int position = PlayerController.instance.GetPosition() + arrowDir * i;
            BaseEnemy enemy = LevelData.instance.CheckForEnemyCurrentPosition(position);
            if (enemy != null)
            {
                enemy.TakeDamage(2);
            }
            else
            {
                if (LevelData.instance.walls.GetTile((Vector3Int)position) == null)
                {
                    projectiles.Add(Instantiate(projectilePrefab, (Vector2)position, Quaternion.identity));
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        foreach(GameObject projectile in projectiles)
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(projectile);
        }
    }
}
