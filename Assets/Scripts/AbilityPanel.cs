using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityPanel : MonoBehaviour
{
    public static AbilityPanel instance;
    public TMP_Text textBox;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateAbilityPanel();
    }

    public void UpdateAbilityPanel()
    {
        string str = "";
        if (PlayerController.instance.equippedSpells[0] == null) str += "【1】\n";
        else str += ("【1】" + PlayerController.instance.equippedSpells[0].spellName + " （" + PlayerController.instance.equippedSpells[0].MPCost + "魔力）" + "\n");

        if (PlayerController.instance.equippedSpells[1] == null) str += "【2】\n";
        else str += ("【2】" + PlayerController.instance.equippedSpells[1].spellName + " （" + PlayerController.instance.equippedSpells[1].MPCost + "魔力）" + "\n");

        if (PlayerController.instance.equippedSpells[2] == null) str += "【3】\n";
        else str += ("【3】" + PlayerController.instance.equippedSpells[2].spellName + " （" + PlayerController.instance.equippedSpells[2].MPCost + "魔力）" + "\n");

        if (PlayerController.instance.equippedSpells[3] == null) str += "【4】\n";
        else str += ("【4】" + PlayerController.instance.equippedSpells[3].spellName + " （" + PlayerController.instance.equippedSpells[3].MPCost + "魔力）" + "\n");

        if (PlayerController.instance.equippedSpells[4] == null) str += "【5】\n";
        else str += ("【5】" + PlayerController.instance.equippedSpells[4].spellName + " （" + PlayerController.instance.equippedSpells[4].MPCost + "魔力）" + "\n");
        textBox.text = str;
    }
}
