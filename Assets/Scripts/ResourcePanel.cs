using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePanel : MonoBehaviour
{
    public static ResourcePanel instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateMPPanel();
        UpdateHPPanel();
        UpdateCoinPanel();
    }

    public TMP_Text MPTextBox;
    public TMP_Text HPTextBox;
    public TMP_Text coinTextBox;
    public void UpdateMPPanel()
    {
        string str = "";
        str += "魔力" + "【" + PlayerController.instance.MP + "/" + PlayerController.instance.maxMP + "】";
        MPTextBox.text = str;
    }

    public void UpdateHPPanel()
    {
        string str = "";
        str += "生命" + "【" + PlayerController.instance.HP + "/" + PlayerController.instance.maxHP + "】";
        HPTextBox.text = str;
    }

    public void UpdateCoinPanel()
    {
        string str = "";
        str += "金币" + "【" + PlayerController.instance.coin+ "】";
        coinTextBox.text = str;
    }
}
