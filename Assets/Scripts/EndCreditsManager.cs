using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCreditsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Process());
        ClearPlayerPrefs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private IEnumerator Process()
    {
        yield return new WaitForSeconds(2f);
        Console.instance.ConsoleLog("恭喜你成功闯关");
        yield return new WaitForSeconds(1f);
        Console.instance.ConsoleLog("请点击【Esc】以回到标题页面");
        yield return new WaitForSeconds(3f);
        Console.instance.ConsoleLog("Made by Austin Kong");
        yield return new WaitForSeconds(2f);
        Console.instance.ConsoleLog("Dec 2022 - Jan 2023");
        yield return new WaitForSeconds(2f);
        Console.instance.ConsoleLog("An homage to all the roguelikes I never finished making");
        yield return new WaitForSeconds(2f);
        Console.instance.ConsoleLog("An homage to Rogue - the original");
        yield return new WaitForSeconds(3f);
        Console.instance.ConsoleLog("Thanks for playing!");
    }

    public void ClearPlayerPrefs()
    {
        for (int i = 0; i < 5; i++) { PlayerPrefs.SetInt("spell" + i.ToString(), -1); }


        PlayerPrefs.SetInt("maxHP", 3);
        PlayerPrefs.SetInt("maxMP", 3);
        PlayerPrefs.SetInt("spell0", 0);
        PlayerPrefs.SetInt("coin", 0);

    }
}
