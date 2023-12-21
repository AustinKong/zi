using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.projectileTick += SpellTick;

        
        //DESTROY when insta collide with player
        if (PlayerController.instance.GetPosition() == new Vector2Int((int)transform.position.x, (int)transform.position.y))
        {
            PlayerController.instance.TakeDamage(1);
            DestroySpell();
        }
        else if (LevelData.instance.walls.GetTile(new Vector3Int((int)transform.position.x, (int)transform.position.y, 0)) != null) DestroySpell();
        
    }

    public Vector2Int spellDirection = Vector2Int.zero;

    public void SpellTick()
    {
        Vector2Int targetPosition = new Vector2Int((int)transform.position.x, (int)transform.position.y) + spellDirection;

        if (PlayerController.instance.GetPosition() == targetPosition)
        {
            PlayerController.instance.TakeDamage(1);
            DestroySpell();
        }
        else if (LevelData.instance.walls.GetTile((Vector3Int)targetPosition) != null) DestroySpell();

        transform.position = (Vector2)targetPosition;
    }

    public void DestroySpell()
    {
        GameManager.instance.projectileTick -= SpellTick;
        Destroy(gameObject);
    }
}
