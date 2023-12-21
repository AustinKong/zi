using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public int coin = 0;

    public int HP= 3;
    public int MP = 3;

    public int maxHP = 3;
    public int maxMP = 3;

    public Transform FOVOverlay;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.actionTick += ArrowKeyMovement;
        transform.position = Vector3.zero;
        GameManager.instance.actionTick += ResourceRegen;
        Application.quitting += ClearPlayerPrefs;
        ReadPlayerPrefs();
    }

    //private int HPRegenTimer = 4;
    private int MPRegenTimer = 3;

    private void ResourceRegen()
    {
        if (MP < maxMP)
        {
            MPRegenTimer--;
            if(MPRegenTimer == 0)
            {
                MP++;
                ResourcePanel.instance.UpdateMPPanel();
                MPRegenTimer = 3;
            }
        }
        /*
        if (HP < maxHP)
        {
            HPRegenTimer--;
            if (HPRegenTimer == 0)
            {
                HP++;
                ResourcePanel.instance.UpdateHPPanel();
                HPRegenTimer = 4;
            }
        }
        */
        

    }

    private void Update()
    {
        if (!awaitingInput)
        {
            ArrowKeyInput();
            SpellcastingInput();
        }
        if (HP <= 0)
        {
            SFX.instance.PlayDeath();
            GameManager.instance.PlayerDeath();
            GameManager.instance.actionTick -= ArrowKeyMovement;
            GameManager.instance.actionTick -= ResourceRegen;
            gameObject.SetActive(false);
        }
    }

    #region Movement

    private Vector2Int movementDirection = Vector2Int.zero;
    private void ArrowKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) movementDirection = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.DownArrow)) movementDirection = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) movementDirection = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.RightArrow)) movementDirection = Vector2Int.right;
        if (Input.GetKeyDown(KeyCode.Space)) GameManager.instance.PlayerAction(); //frame skipping
        if (movementDirection == Vector2Int.zero) return;

        GameManager.instance.PlayerAction();
    }

    private void ArrowKeyMovement()
    {
        Vector2Int targetPosition = GetPosition() + movementDirection;
        if (LevelData.instance.walls.GetTile(new Vector3Int(targetPosition.x, targetPosition.y, 0)) == null)
        {
            transform.position = (Vector2)targetPosition;
            FOVOverlay.position = transform.position + new Vector3(0.5f, 0.5f, 0);
            LevelData.instance.CheckUniqueInteractions();

            if(LevelData.instance.CheckForEnemyCurrentPosition(new Vector2Int(targetPosition.x, targetPosition.y)) != null)
            {
                TakeDamage(LevelData.instance.CheckForEnemyCurrentPosition(new Vector2Int(targetPosition.x, targetPosition.y)).contactDamage);
            }
        }
        else Console.instance.ConsoleLog("无法通过");
        movementDirection = Vector2Int.zero;
    }

    #endregion

    public BaseSpell[] equippedSpells = new BaseSpell[5];

    public static bool awaitingInput = false;

    #region Spellcasting
    private void SpellcastingInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && equippedSpells[0] != null)
        {
            if (equippedSpells[0].MPCost <= MP) equippedSpells[0].StartSpell();
            else Console.instance.ConsoleLog("魔力不足");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && equippedSpells[1] != null)
        {
            if (equippedSpells[1].MPCost <= MP) equippedSpells[1].StartSpell();
            else Console.instance.ConsoleLog("魔力不足");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && equippedSpells[2] != null)
        {
            if (equippedSpells[2].MPCost <= MP) equippedSpells[2].StartSpell();
            else Console.instance.ConsoleLog("魔力不足");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && equippedSpells[3] != null)
        {
            if (equippedSpells[3].MPCost <= MP) equippedSpells[3].StartSpell();
            else Console.instance.ConsoleLog("魔力不足");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && equippedSpells[4] != null)
        {
            if (equippedSpells[4].MPCost <= MP) equippedSpells[4].StartSpell();
            else Console.instance.ConsoleLog("魔力不足");
        }
    }
    #endregion

    public void TakeDamage(int amount)
    {
        Console.instance.ConsoleLog("我受" + amount + "点伤害");
        HP -= amount;
        ResourcePanel.instance.UpdateHPPanel();
        SFX.instance.PlayHitHurt();
        StartCoroutine(Hurt(0.1f, 0.3f));
    }

    public Vector2Int GetPosition() => new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));

    public IEnumerator Hurt(float duration, float magnitude)
    {
        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        SR.color = Color.red;

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
        SR.color = Color.white;
    }

    public GameObject[] allSpellPrefabs;

    public void ClearPlayerPrefs()
    {
        for(int i = 0; i < 5; i++) { PlayerPrefs.SetInt("spell" + i.ToString(), -1); }

        //devs are op

        PlayerPrefs.SetInt("level", 1);

        bool opDev = false;
        if (opDev)
        {
            PlayerPrefs.SetInt("maxHP", 99);
            PlayerPrefs.SetInt("maxMP", 99);
            PlayerPrefs.SetInt("spell0", 0);
            PlayerPrefs.SetInt("spell1", 1);
            PlayerPrefs.SetInt("coin", 0);
        }
        else
        {
            PlayerPrefs.SetInt("maxHP", 3);
            PlayerPrefs.SetInt("maxMP", 3);
            PlayerPrefs.SetInt("spell0", 0);
            PlayerPrefs.SetInt("coin", 0);
        }
        
    }

    private void ReadPlayerPrefs()
    {
        maxHP = PlayerPrefs.GetInt("maxHP");
        HP = maxHP;
        maxMP = PlayerPrefs.GetInt("maxMP");
        MP = maxMP;
        coin = PlayerPrefs.GetInt("coin");
        for(int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.GetInt("spell" + i.ToString()) != -1)
            {
                equippedSpells[i] =Instantiate(allSpellPrefabs[PlayerPrefs.GetInt("spell" + i.ToString())]).GetComponent<BaseSpell>();
            }
            else equippedSpells[i] = null;
        }
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("maxHP", maxHP);
        PlayerPrefs.SetInt("maxMP", maxMP);
        PlayerPrefs.SetInt("coin", coin);
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(GetSpellIndex(equippedSpells[i]));
            PlayerPrefs.SetInt("spell" + i.ToString(), GetSpellIndex(equippedSpells[i]));
        }
    }

    private int GetSpellIndex(BaseSpell spell)
    {
        for(int i = 0; i < allSpellPrefabs.Length; i++)
        {
            if (spell != null)
            {
                if (allSpellPrefabs[i].GetComponent<BaseSpell>().GetType() == spell.GetType())
                {
                    return i;
                }
            }
        }
        return -1;
    }
}
