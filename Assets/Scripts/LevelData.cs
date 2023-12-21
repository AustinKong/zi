using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelData : MonoBehaviour
{

    public static LevelData instance;
    private void Awake()
    {
        instance = this;
    }

    public List<BaseEnemy> enemies = new List<BaseEnemy>();
    public Tilemap walls;
    public Tilemap floor;
    public GameObject coinPrefab;

    public BaseLevel thisLevel;
    public Dictionary<Vector2Int, GameObject> altars = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector2Int, GameObject> coins = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector2Int, GameObject> chests = new Dictionary<Vector2Int, GameObject>();
    public Vector2Int stairPosition;
    public Dictionary<Vector2Int, GameObject> spellbooks = new Dictionary<Vector2Int, GameObject>();

    private void Start()
    {
        enemies.AddRange(FindObjectsOfType<BaseEnemy>());
    }

    public void CreateCoin(Vector2Int position, int amount)
    {
        if (amount == 0) return;

        if (coins.ContainsKey(position))
        {
            coins[position].name = (int.Parse(coins[position].name) + amount).ToString();
            return;
        }
        GameObject coin = Instantiate(coinPrefab, (Vector2)position, Quaternion.identity);
        coin.name = amount.ToString();
        coins.Add(position, coin);
    }

    public void CheckUniqueInteractions()
    {
        Vector2Int playerPos = PlayerController.instance.GetPosition();

        if (playerPos == stairPosition) GameManager.instance.LoadNextLevel();

        if (altars.ContainsKey(playerPos))
        {
            if (PlayerController.instance.coin >= 5)
            {
                //altar interaction
                SFX.instance.PlayAltarUse();
                Console.instance.ConsoleLog("我走向祭台，奉出金币");
                PlayerController.instance.coin -= 5;
                switch (Random.Range(0, 4))
                {
                    case 0:
                        Console.instance.ConsoleLog("生命+2");
                        PlayerController.instance.maxHP+=2;
                        PlayerController.instance.HP+=2;
                        ResourcePanel.instance.UpdateHPPanel();
                        break;
                    case 1:
                        Console.instance.ConsoleLog("魔力+1");
                        PlayerController.instance.maxMP++;
                        ResourcePanel.instance.UpdateMPPanel();
                        break;
                    case 2:
                        Console.instance.ConsoleLog("恢复生命");
                        PlayerController.instance.HP = PlayerController.instance.maxHP;
                        ResourcePanel.instance.UpdateHPPanel();
                        break;
                    case 3:
                        Console.instance.ConsoleLog("生命-1");
                        SFX.instance.PlayHitHurt();
                        if (PlayerController.instance.HP == PlayerController.instance.maxHP) PlayerController.instance.HP--;
                        PlayerController.instance.maxHP--;
                        PlayerController.instance.StartCoroutine(PlayerController.instance.Hurt(0.1f, 0.1f));
                        ResourcePanel.instance.UpdateHPPanel();
                        break;
                }
                ResourcePanel.instance.UpdateCoinPanel();
                Destroy(altars[playerPos]);
                altars.Remove(playerPos);
            }
            else
            {
                Console.instance.ConsoleLog("请奉出金币");
            }

        }

        if (coins.ContainsKey(playerPos))
        {
            GameObject coin = coins[playerPos];
            SFX.instance.PlayPickup();
            PlayerController.instance.coin += int.Parse(coin.name);
            Console.instance.ConsoleLog("金币+" + coin.name);
            ResourcePanel.instance.UpdateCoinPanel();
            coins.Remove(playerPos);
            Destroy(coin);
        }

        if (chests.ContainsKey(playerPos))
        {
            SFX.instance.PlayPickup();
            Console.instance.ConsoleLog("我走向箱子，打开它");
            switch (Random.Range(0, 3))
            {
                case 0:
                    Console.instance.ConsoleLog("金币+1");
                    PlayerController.instance.coin++;
                    ResourcePanel.instance.UpdateCoinPanel();
                    break;
                case 1:
                    Console.instance.ConsoleLog("金币+2");
                    PlayerController.instance.coin += 2;
                    ResourcePanel.instance.UpdateCoinPanel();
                    break;
                case 2:
                    Console.instance.ConsoleLog("恢复1生命");
                    if (PlayerController.instance.HP < PlayerController.instance.maxHP) PlayerController.instance.HP++;
                    ResourcePanel.instance.UpdateHPPanel();
                    break;
            }
            Destroy(chests[playerPos]);
            chests.Remove(playerPos);
        }

        if (spellbooks.ContainsKey(playerPos))
        {
            StartCoroutine(equipSpell(spellbooks[playerPos].GetComponent<Spellbook>().spellIndex));
            SFX.instance.PlayPickup();
            Destroy(spellbooks[playerPos]);
            spellbooks.Remove(playerPos);   
        }
    }

    private IEnumerator equipSpell(int spellIndex)
    {
        PlayerController.awaitingInput = true;
        int input = -1;
        Console.instance.ConsoleLog("学会了" + PlayerController.instance.allSpellPrefabs[spellIndex].GetComponent<BaseSpell>().spellName + "技能，请点击选择置放位置");
        while (true)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Alpha1)) input = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) input = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3)) input = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4)) input = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5)) input = 4;
            if (Input.GetKeyDown(KeyCode.Space)) input = -2;

            if (input != -1) break;
        }
        if (input != -2)
        {
            PlayerController.instance.equippedSpells[input] = Instantiate(PlayerController.instance.allSpellPrefabs[spellIndex]).GetComponent<BaseSpell>();
            AbilityPanel.instance.UpdateAbilityPanel();
        }
        else
        {
            Console.instance.ConsoleLog("忘记" + PlayerController.instance.allSpellPrefabs[spellIndex].GetComponent<BaseSpell>().spellName + "技能");
        }
        PlayerController.awaitingInput = false;
    }

    public BaseEnemy CheckForEnemy(Vector2Int position)
    {
        foreach(BaseEnemy enemy in enemies)
        {
            if (enemy.position == position || enemy.lastPosition == position) return enemy;
        }
        return null;
    }

    public BaseEnemy CheckForEnemyCurrentPosition(Vector2Int position)
    {
        foreach (BaseEnemy enemy in enemies)
        {
            if (enemy.position == position) return enemy;
        }
        return null;
    }
}
