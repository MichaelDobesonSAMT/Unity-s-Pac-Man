using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillManager : MonoBehaviour
{
    public Sprite sprite;

    // Variables imported from GridManager
    private bool[,] GridWalls;
    private int Horizontal;
    private int Vertical;
    private int Columns;
    private int Rows;
    private float SuperPillPercent;

    public float pillSize = 0.15f;
    public float superPillSize = 0.5f;

    void Start()
    {
        GetGridVariables();
        PlacePills();
    }

    
    void Update()
    {
        
    }

    // Spawns a Pill (Normal o Super) on the Map
    private void SpawnPill(int x, int y, bool isSuper, Transform parent)
    {
        if (isSuper)
        {
            GameObject g = new GameObject("Super Pill - x: " + x + ",y: " + y);
            g.transform.parent = parent;
            g.transform.position = new Vector3(x - (Horizontal - 0.5f), (Vertical - 0.5f) - y);
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = sprite;
            s.color = new Color(255, 255, 0);
            g.transform.localScale = new Vector3(superPillSize, superPillSize);
        }
        else
        {
            GameObject g = new GameObject("Pill - x: " + x + ",y: " + y);
            g.transform.parent = parent;
            g.transform.position = new Vector3(x - (Horizontal - 0.5f), (Vertical - 0.5f) - y);
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = sprite;
            s.color = new Color(255, 255, 0);
            g.transform.localScale = new Vector3(pillSize, pillSize);
        }
    }

    // Places Pills in the Game
    private void PlacePills()
    {
        GameObject parent = new GameObject("Pills");
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (!GridWalls[i, j])
                {
                    //if it's Corned place a Super Pill
                    if (
                        !(i == 0 && j == 0) && (
                        (j - 1 < 0 || GridWalls[i,j - 1] == true) &&
                        (i - 1 < 0 || GridWalls[i - 1, j] == true) &&
                        (j + 1 > Rows - 1 || GridWalls[i, j + 1] == true) ||
                        (j - 1 < 0 || GridWalls[i, j - 1] == true) &&
                        (i + 1 > Columns - 1 || GridWalls[i + 1, j] == true) &&
                        (j + 1 > Rows - 1 || GridWalls[i, j + 1] == true) ||
                        (j - 1 < 0 || GridWalls[i, j - 1] == true) &&
                        (i - 1 < 0 || GridWalls[i - 1, j] == true) &&
                        (i + 1 > Columns - 1 || GridWalls[i + 1, j] == true) ||
                        (j + 1 > Rows - 1 || GridWalls[i, j + 1] == true) &&
                        (i - 1 < 0 || GridWalls[i - 1, j] == true) &&
                        (i + 1 > Rows - 1 || GridWalls[i + 1, j] == true)
                        )
                    )
                    {
                        var rnd = Random.Range(0.0f, 1.0f);
                        if (rnd < SuperPillPercent)
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

    // Imports necessary Variables from Grid Manager
    public void GetGridVariables()
    {
        var g = GetComponent<GridManager>();
        GridWalls = g.GridWalls;
        Columns = g.Columns;
        Rows = g.Rows;
        Horizontal = g.Horizontal;
        Vertical = g.Vertical;
        SuperPillPercent = g.SuperPillPercent;
    }
}
