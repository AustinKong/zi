using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public string enemyName;
    public int hitPoints;
    public int coinOnDeath;
    public int contactDamage;
    [HideInInspector]
    public Vector2Int position;
    [HideInInspector]
    public Vector2Int lastPosition;
    public virtual void Start()
    {
        GameManager.instance.actionTick += ActionTick;
        lastPosition = position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public virtual void ActionTick() { }

    public virtual void TakeDamage(int amount)
    {
        hitPoints -= amount;
        Console.instance.ConsoleLog(enemyName + "受" + amount.ToString() + "点伤害");
        if (hitPoints <= 0) StartCoroutine(Death());
        else StartCoroutine(Hurt(0.1f, 0.2f));
    }

    public IEnumerator Death()
    {
        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        SR.color = Color.white;
        SFX.instance.PlayDeath();
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < 0.2f)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * 0.2f;
            float yOffset = Random.Range(-0.5f, 0.5f) * 0.2f;
            transform.position = originalPos + new Vector3(xOffset, yOffset, 0);
            elapsedTime += Time.deltaTime; //wait one frame
            yield return null;
        }
        GameManager.instance.actionTick -= ActionTick;
        Console.instance.ConsoleLog(enemyName + "死亡");
        LevelData.instance.CreateCoin(new Vector2Int(Mathf.RoundToInt(originalPos.x), Mathf.RoundToInt(originalPos.y)), coinOnDeath);
        Destroy(gameObject);
    }

    public IEnumerator Hurt(float duration, float magnitude)
    {
        SFX.instance.PlayHitHurt();
        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        SR.color = Color.white;

        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            transform.position = originalPos + new Vector3(xOffset, yOffset, 0);
            elapsedTime += Time.deltaTime; //wait one frame
            yield return null;
        }
        transform.position = originalPos;
        SR.color = new Color(1f, 0, 0);
    }
}
