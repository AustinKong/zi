using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSpell : BaseSpell
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
        Instantiate(projectilePrefab, PlayerController.instance.transform.position + (Vector3)(Vector2)GetArrowKeyDirection(), Quaternion.identity).GetComponent<Fireball>().spellDirection = GetArrowKeyDirection();
    }
    
}
