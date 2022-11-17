using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public const float PAC_AURA = -0.5f;
    public const int INVALID = -1;
    public const int WALL = -2;
    public const int BLINKY = -3;

    public Sprite sprite;
    public TextMesh gameOver;
    public Font retro;
    public float[,] Grid;
    public bool[,] GridWalls;
    public int PacManPoints = 0;
    public int HighScore = 100;

    private TextMesh score;
    private TextMesh lives;
    private TextMesh highScore;
    [HideInInspector]
    public int Vertical, Horizontal, Columns, Rows;

    // Game Setting Variables
    public float WallPercent = 0.3f;
    public float SuperPillPercent = 0.25f;
    public int BlinkyFromPac = 10;
    public int PacManLives = 1;
    public float BlinkySpeed = 0.5f;
    public float PacManSpeed = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        // Set Sizes
        Horizontal = Vertical = 10;
        Columns = Rows = Horizontal * 2;
        Grid = new float[Columns, Rows];
        GridWalls = new bool[Columns, Rows];

        PlaceWalls();

        PlaceGrid();
        
        PlaceText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlaceText()
    {
        lives.text = PacManLives.ToString();
        score.text = "0";
        highScore.text = "High Score: " + HighScore.ToString();
        highScore.text = "Game Over";
        //lives.font = retro;
        //score.font = retro;
        //highScore.font = retro;

        lives.transform.position = new Vector3(-10, 12);
        score.transform.position = new Vector3(7, 12);
        highScore.transform.position = new Vector3(-4, 12);
        gameOver.transform.position = new Vector3(0, 0);
        gameOver.gameObject.SetActive(false);
    }

    // Place Walls in Grid
    private void PlaceWalls()
    {
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                var rnd = Random.Range(0.0f, 1.0f);
                if (rnd < WallPercent)
                {
                    GridWalls[i, j] = true;
                    SetGrid(0,0);
                    for (int k = 0; k < Columns; k++)
                    {
                        for (int l = 0; l < Rows; l++)
                        {
                            if (Grid[k, l] == INVALID)
                            {
                                GridWalls[i, j] = false;
                            }
                        }
                    }
                }
                else
                {
                    GridWalls[i, j] = false;
                }
            }
        }
    }

    // Set Grid Values
    public float[,] SetGrid(int x, int y)
    {
        Grid = new float[Columns, Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (GridWalls[i, j])
                {
                    Grid[i, j] = WALL;
                }
                else
                {
                    Grid[i, j] = INVALID;
                }
            }
        }
        Visit(Grid, x, y, 0);
        return Grid;
    }

    // Manhattan Distance Algorithm
    public void Visit(float[,] tempGrid, int col, int row, int dist)
    {
        if (row < Rows && row >= 0 && col < Columns && col >= 0)
        {
            if (
                (tempGrid[col, row] > dist && tempGrid[col, row] != WALL) ||
                tempGrid[col, row] == INVALID
            )
            {
                tempGrid[col, row] = dist;
                Visit(tempGrid, col, row - 1, dist + 1);
                Visit(tempGrid, col - 1, row, dist + 1);
                Visit(tempGrid, col, row + 1, dist + 1);
                Visit(tempGrid, col + 1, row, dist + 1);
            }
        }
    }

    // Spawns an Element of the Map
    private void SpawnTile(int x, int y, float value, Transform parent)
    {
        GameObject g = new GameObject("Tile - x: " + x + ",y: " + y);
        g.transform.parent = parent;
        g.transform.position = new Vector3(x - (Horizontal - 0.5f), (Vertical - 0.5f) - y);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        if (Grid[x, y] == WALL)
        {
            s.color = new Color(0, 0, 255);
        }
        else
        {
            s.color = new Color(0, 0, 0);
        }

        // Adds Text for Debug
        /*
        var myText = CreateText(g.transform);
        myText.text = x + ", " + y + "\n: " + value;
        myText.transform.position = new Vector3(x - (Horizontal), (Vertical) - y, -0.25f);
        myText.transform.localScale = new Vector3(0.25f, 0.25f);
        */
    }

    // Place Grid in the Game
    private void PlaceGrid()
    {
        GameObject parent = new GameObject("Grid");
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                SpawnTile(i, j, Grid[i, j], parent.transform);
            }
        }

        lives = CreateText(parent.transform);
        score = CreateText(parent.transform);
        highScore = CreateText(parent.transform);
    }

    // Adds Text to a Game Object
    private TextMesh CreateText(Transform parent)
    {
        var go = new GameObject("Text");
        go.transform.parent = parent;
        var text = go.AddComponent<TextMesh>();
        return text;
    }
}