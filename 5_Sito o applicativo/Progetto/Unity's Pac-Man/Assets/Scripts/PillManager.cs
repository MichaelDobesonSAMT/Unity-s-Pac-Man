using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillManager : MonoBehaviour
{
    // Public variables
    public Sprite PillSprite;
    public GameObject PillPrefab;
    public GameObject SuperPillPrefab;
    public float PillSize = 0.15f;
    public float SuperPillSize = 0.5f;

    // Private variables
    private bool[,] gridWalls;
    private int columns;
    private int rows;
    private float superPillPercent;

    // Start is called before the first frame update
    void Start()
    {
        GetGridVariables();
        PlacePills();
    }

    // Imports necessary variables from Grid Manager
    public void GetGridVariables()
    {
        var g = GetComponent<GridManager>();
        gridWalls = g.GridWalls;
        columns = g.Columns;
        rows = g.Rows;
        superPillPercent = g.SuperPillPercent;
    }

    // Spawns a Pill (Normal o Super) on the grid
    private void SpawnPill(int x, int y, bool isSuper, Transform parent)
    {
        GameObject g;
        // Spawn Super Pill
        if (isSuper)
        {
            g = Instantiate(SuperPillPrefab, new Vector3(
                x - (columns / 2 - 0.5f), 
                (rows / 2 - 0.5f) - y), Quaternion.identity);
            g.name = "Super Pill - x: " + x + ",y: " + y;
        }
        // Spawn Normal Pill
        else
        {
            g = Instantiate(PillPrefab, new Vector3(
                x - (columns / 2 - 0.5f), 
                (rows / 2 - 0.5f) - y), Quaternion.identity);
            g.name = "Pill - x: " + x + ",y: " + y;
        }
        g.transform.parent = parent;
    }

    // Places Pills in the Game
    public void PlacePills()
    {
        GameObject parent = new GameObject("Pills");
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                //if there isn't a Wall
                if (GetComponent<GridManager>().Grid[i, j] != GridManager.WALL)
                {
                    // if it's corned place a Super Pill sometimes
                    if (
                        !(i == 0 && j == 0) && (
                        (j - 1 < 0 || gridWalls[i,j - 1] == true) &&
                        (i - 1 < 0 || gridWalls[i - 1, j] == true) &&
                        (j + 1 > rows - 1 || gridWalls[i, j + 1] == true) ||
                        (j - 1 < 0 || gridWalls[i, j - 1] == true) &&
                        (i + 1 > columns - 1 || gridWalls[i + 1, j] == true) &&
                        (j + 1 > rows - 1 || gridWalls[i, j + 1] == true) ||
                        (j - 1 < 0 || gridWalls[i, j - 1] == true) &&
                        (i - 1 < 0 || gridWalls[i - 1, j] == true) &&
                        (i + 1 > columns - 1 || gridWalls[i + 1, j] == true) ||
                        (j + 1 > rows - 1 || gridWalls[i, j + 1] == true) &&
                        (i - 1 < 0 || gridWalls[i - 1, j] == true) &&
                        (i + 1 > rows - 1 || gridWalls[i + 1, j] == true)
                        )
                    )
                    {
                        var rnd = Random.Range(0.0f, 1.0f);
                        if (rnd < superPillPercent)
                        {
                            SpawnPill(i, j, true, parent.transform);
                        }
                        else
                        {
                            SpawnPill(i, j, false, parent.transform);
                        }
                    }
                    else
                    {
                        SpawnPill(i, j, false, parent.transform);
                    }
                }
            }
        }
    }
}
