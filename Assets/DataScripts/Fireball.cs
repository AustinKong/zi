using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.projectileTick += SpellTick;


        //DESTROY when insta collide with enemy
        BaseEnemy enemy = LevelData.instance.CheckForEnemy(new Vector2Int((int)transform.position.x, (int)transform.position.y));
        if (enemy != null)
        {
            enemy.TakeDamage(1);
            DestroySpell();
        }
        else if (LevelData.instance.walls.GetTile(new Vector3Int((int)transform.position.x, (int)transform.position.y,0)) != null) DestroySpell();
    }

    public Vector2Int spellDirection = Vector2Int.zero;

    public void SpellTick()
    {
        Vector3Int targetPosition = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0) + (Vector3Int)spellDirection;

        BaseEnemy enemy = LevelData.instance.CheckForEnemy((Vector2Int)targetPosition);
        if(enemy != null)
        {
            enemy.TakeDamage(1);
            DestroySpell();
        }
        else if (LevelData.instance.walls.GetTile(targetPosition) != null) DestroySpell();

        transform.position = targetPosition;
    }

    public void DestroySpell()
    {
        GameManager.instance.projectileTick -= SpellTick;
        Destroy(gameObject);
    }
}
