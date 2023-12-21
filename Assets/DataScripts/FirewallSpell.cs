using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallSpell : BaseSpell
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
        SFX.instance.PlayFire();
        base.CastSpell();
        //setting direction immediately after
        Vector3 position = PlayerController.instance.transform.position + (Vector3)(Vector2)GetArrowKeyDirection();
        Instantiate(projectilePrefab, position , Quaternion.identity).GetComponent<Fireball>().spellDirection = GetArrowKeyDirection();
        
        if(GetArrowKeyDirection()==Vector2Int.right || GetArrowKeyDirection() == Vector2Int.left)
        {
            Instantiate(projectilePrefab, position + Vector3.up, Quaternion.identity).GetComponent<Fireball>().spellDirection = GetArrowKeyDirection();
            Instantiate(projectilePrefab, position + Vector3.down, Quaternion.identity).GetComponent<Fireball>().spellDirection = GetArrowKeyDirection();
        }
        else
        {
            Instantiate(projectilePrefab, position + Vector3.left, Quaternion.identity).GetComponent<Fireball>().spellDirection = GetArrowKeyDirection();
            Instantiate(projectilePrefab, position + Vector3.right, Quaternion.identity).GetComponent<Fireball>().spellDirection = GetArrowKeyDirection();
        }
    }
}
