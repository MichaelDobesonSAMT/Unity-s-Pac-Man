using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillManager : MonoBehaviour
{
    private bool[,] gridWalls;
    private int horizontal;
    private int vertical;
    private int columns;
    private int rows;
    private float superPillPercent;

    public Sprite PillSprite;
    public float PillSize = 0.15f;
    public float SuperPillSize = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        GetGridVariables();
        PlacePills();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Spawns a Pill (Normal o Super) on the Map
    private void SpawnPill(int x, int y, bool isSuper, Transform parent)
    {
        // Spawn Super Pill
        if (isSuper)
        {
            GameObject g = new GameObject("Super Pill - x: " + x + ",y: " + y);

            g.transform.parent = parent;
            g.transform.position = new Vector3(x - (horizontal - 0.5f), (vertical - 0.5f) - y);
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = PillSprite;
            s.color = new Color(255, 255, 0);
            s.sortingOrder = 1;

            g.tag = "SuperPill";
            g.transform.localScale = new Vector3(SuperPillSize, SuperPillSize);
        }
        // Spawn Normal Pill
        else
        {
            GameObject g = new GameObject("Pill - x: " + x + ",y: " + y);

            g.transform.parent = parent;
            g.transform.position = new Vector3(x - (horizontal - 0.5f), (vertical - 0.5f) - y);
            var s = g.AddComponent<SpriteRenderer>();
            s.sprite = PillSprite;
            s.color = new Color(255, 255, 0);
            s.sortingOrder = 1;

            g.tag = "Pill";
            g.transform.localScale = new Vector3(PillSize, PillSize);
        }
    }

    // Places Pills in the Game
    private void PlacePills()
    {
        GameObject parent = new GameObject("Pills");
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (!gridWalls[i, j])
                {
                    //if it's Corned place a Super Pill
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

    // Imports necessary Variables from Grid Manager
    public void GetGridVariables()
    {
        var g = GetComponent<GridManager>();
        gridWalls = g.GridWalls;
        columns = g.Columns;
        rows = g.Rows;
        horizontal = g.Horizontal;
        vertical = g.Vertical;
        superPillPercent = g.SuperPillPercent;
    }
}
