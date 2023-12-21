using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpikesSpell : BaseSpell
{
    public GameObject projectilePrefab;
    public override void StartSpell()
    {
        Console.instance.ConsoleLog("使用" + spellName + "技能");
        CastSpell();
    }

    public override void CastSpell()
    {
        base.CastSpell();
        StartCoroutine(ShockWave());
        GameManager.instance.PlayerAction();
    }

    Vector2Int[] shockTiles =
    {
        new Vector2Int(1,1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,-1),
        new Vector2Int(1,-1),
    };

    private IEnumerator ShockWave()
    {
        SFX.instance.PlayRock();
        yield return new WaitForSeconds(0.08f);
        List<GameObject> projectiles = new List<GameObject>();
        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Vector2Int position = PlayerController.instance.GetPosition() + shockTiles[j] * i;
                BaseEnemy enemy = LevelData.instance.CheckForEnemyCurrentPosition(position);
                if (enemy != null)
                {
                    enemy.TakeDamage(3);
                }
                else
                {
                    if (LevelData.instance.floor.GetTile((Vector3Int)position) != null)
                    {
                        projectiles.Add(Instantiate(projectilePrefab, (Vector2)position, Quaternion.identity));
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.2f);


        foreach (GameObject projectile in projectiles)
        {
            yield return new WaitForSeconds(0.1f);
            Destroy(projectile);
        }
    }
}
