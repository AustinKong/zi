using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/NewLevel")]
public class BaseLevel : ScriptableObject
{
    public List<GameObject> enemyPool;
    public int newSpellIndex = -1;
    public int altarCount = 2;
    public int chestCount = 2;
}
