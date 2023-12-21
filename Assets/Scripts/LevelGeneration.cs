using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGeneration : MonoBehaviour
{
    private void Start()
    {
        GenerateLevel(); 
    }

    private const int corridorLength = 10;
    private const int corridorLengthVariance = 4;
    private const int roomLength = 5;
    private const int roomLengthVariance = 2;

    public Tile wallTile;
    public Tile floorTile;
    public Tile stairs;
    public GameObject spellbookPrefab;

    private List<Vector2Int> openPositions = new List<Vector2Int>();

    private void GenerateLevel()
    {
        GenerateRoom(Vector2Int.zero);
        
        for (int r = 0; r < 1; r++)
        {
            RecursivelyGenerateWing(Vector2Int.zero, 2, 2);
        }
        Vector2Int stairPosition = StandardGenerateWing(Vector2Int.zero, 3);

        LevelData.instance.floor.SetTile(new Vector3Int(stairPosition.x, stairPosition.y, 0), stairs);
        openPositions.Remove(new Vector2Int(stairPosition.x, stairPosition.y));

        LevelData.instance.stairPosition = stairPosition;
        GenerateWalls();
        GenerateSpellbook();
        AltarGeneration();
        ChestGeneration();
        PopulateEnemies();
        GenerateCoin();
    }

    private void GenerateSpellbook()
    {
        int newSpellIndex = LevelData.instance.thisLevel.newSpellIndex;

        if (newSpellIndex == -1) return;

        int index = Random.Range(0, openPositions.Count);
        GameObject GO = Instantiate(spellbookPrefab, (Vector2)openPositions[index], Quaternion.identity);
        LevelData.instance.spellbooks.Add(openPositions[index], GO);
        GO.GetComponent<Spellbook>().spellIndex = newSpellIndex;
        openPositions.RemoveAt(index);
    }

    private void GenerateCoin()
    {
        int x = openPositions.Count;
        for (int i = 0; i < x; i++)
        {
            int index = Random.Range(0, openPositions.Count);
            LevelData.instance.CreateCoin(openPositions[index], Random.Range(1, 4));
            openPositions.RemoveAt(index);
        }
    }

    private void PopulateEnemies()
    {
        List<GameObject> enemyList = LevelData.instance.thisLevel.enemyPool;

        foreach(GameObject enemy in enemyList)
        {
            int index = Random.Range(0, openPositions.Count);
            LevelData.instance.enemies.Add((Instantiate(enemy, (Vector2)openPositions[index], Quaternion.identity)).GetComponent<BaseEnemy>());
            openPositions.RemoveAt(index);
        }
    }

    public GameObject altarPrefab;
    public GameObject chestPrefab;

    private void AltarGeneration()
    {
        for(int i = 0; i < LevelData.instance.thisLevel.altarCount; i++)
        {
            int index = Random.Range(0, openPositions.Count);
            LevelData.instance.altars.Add(openPositions[index], Instantiate(altarPrefab, (Vector2)openPositions[index], Quaternion.identity));
            openPositions.RemoveAt(index);
        }
    }

    private void ChestGeneration()
    {
        for (int i = 0; i < LevelData.instance.thisLevel.chestCount; i++)
        {
            int index = Random.Range(0, openPositions.Count);
            LevelData.instance.chests.Add(openPositions[index], Instantiate(chestPrefab, (Vector2)openPositions[index], Quaternion.identity));
            openPositions.RemoveAt(index);
        }
    }

    private void RecursivelyGenerateWing(Vector2Int agentPos, int wingComplexity, int newWingAmount)
    {
        if (wingComplexity <= 0) return;
        for(int i = 0; i < newWingAmount; i++)
        {
            Vector2Int agentPosition = StandardGenerateWing(agentPos, 2);
            RecursivelyGenerateWing(agentPosition, wingComplexity - 1, newWingAmount);
        }
        
    }

    private Vector2Int StandardGenerateWing(Vector2Int agentPos, int iterations)
    {
        Vector2Int agentPosition = agentPos;
        for (int i = 0; i < iterations; i++)
        {
            Vector2Int to = agentPosition + RandomCardinalDirection() * (corridorLength + (int)(corridorLengthVariance * Random.Range(-1f, 1f)));
            bool hasRoom = LevelData.instance.floor.GetTile(new Vector3Int(to.x, to.y, 0)) != null;
            GenerateCell(agentPosition, to);
            agentPosition = to;
            agentPosition += RandomCardinalDirection();

            if (!hasRoom)
            {
                GenerateRoom(agentPosition);
                openPositions.Add(agentPosition);

                /*
                Vector2Int nearbyPosition = agentPosition + Vector2Int.right * (Random.Range(0, 1f) > 0.5f ? -1 : 1) + Vector2Int.up * (Random.Range(0, 1f) > 0.5f ? -1 : 1);
                if (LevelData.instance.floor.GetTile((Vector3Int)nearbyPosition) != null)
                {
                    openPositions.Add(nearbyPosition);
                }
                */
            }
            else i--;
        }
        return agentPosition;
    }

    private void GenerateWalls()
    {
        for (int x = (int)LevelData.instance.floor.localBounds.min.x; x < (int)LevelData.instance.floor.localBounds.max.x; x++)
        {
            for (int y = (int)LevelData.instance.floor.localBounds.min.y; y < (int)LevelData.instance.floor.localBounds.max.y; y++)
            {
                if(LevelData.instance.floor.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        for (int k = -1; k < 2; k++)
                        {
                            if (LevelData.instance.floor.GetTile(new Vector3Int(x + j, y + k, 0)) == null)
                            {
                                LevelData.instance.walls.SetTile(new Vector3Int(x + j, y + k, 0),wallTile);
                            }
                        }
                    }
                }
            }
        }
        /*
                for (int x = 1; x < level.GetLength(0) - 1; x++)
        {
            for (int y = 1; y < level.GetLength(1) - 1; y++)
            {
                if (level[x, y].Equals(TileDefinitionFile.floor))
                {
                    for(int j = -1; j < 2; j++)
                    {
                        for (int k = -1; k < 2; k++)
                        {
                            if(level[x + j, y + k].Equals(TileDefinitionFile.blank)){
                                level[x + j, y + k] = TileDefinitionFile.wall;
                            }
                        }
                    }
                }
            }
        }
        */
    }

    private void GenerateRoom(Vector2Int roomCenter)
    {
        Vector2Int from = roomCenter - new Vector2Int(roomLength / 2 + (int)(roomLengthVariance * Random.Range(-1f, 1f)), roomLength / 2 + (int)(roomLengthVariance * Random.Range(-1f, 1f)));
        Vector2Int to = roomCenter + new Vector2Int(roomLength / 2 + (int)(roomLengthVariance * Random.Range(-1f, 1f)), roomLength / 2 + (int)(roomLengthVariance * Random.Range(-1f, 1f)));

        GenerateCell(from, to);
    }

    private void GenerateCell(Vector2Int from, Vector2Int to)
    {
        
        int xIter = Mathf.Abs(to.x - from.x) + 1;
        int yIter = Mathf.Abs(to.y - from.y) + 1;

        int xMult = (to.x - from.x) > 0 ? 1 : -1;
        int yMult = (to.y - from.y) > 0 ? 1 : -1;

        for (int x = 0; x < xIter; x++)
        {
            for (int y = 0; y < yIter; y++)
            {
                LevelData.instance.floor.SetTile(new Vector3Int(from.x + x * xMult, from.y + y * yMult, 0), floorTile);
            }
        }
        
    }

    private Vector2Int RandomCardinalDirection()
    {
        int i = Random.Range(0, 4);
        switch (i)
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.down;
            case 2:
                return Vector2Int.left;
            case 3:
                return Vector2Int.right;
            default:
                return Vector2Int.zero;
        }
    }
}
