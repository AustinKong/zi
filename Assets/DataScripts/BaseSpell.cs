using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpell : MonoBehaviour
{
    public int MPCost;
    public string spellName;
    public virtual void StartSpell() { }
    
    public virtual void CastSpell()
    {
        PlayerController.instance.MP -= MPCost;
        ResourcePanel.instance.UpdateMPPanel();
    }

    public virtual void SpellTick() { }

    public Vector2Int GetArrowKeyDirection()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) return Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) return Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) return Vector2Int.right;
        else return Vector2Int.zero;
    }

    public bool AnyArrowKeyPressed()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return true;
        else if (Input.GetKeyDown(KeyCode.DownArrow)) return true;
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) return true;
        else if (Input.GetKeyDown(KeyCode.RightArrow)) return true;
        else return false;
    }
}
