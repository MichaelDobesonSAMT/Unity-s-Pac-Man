using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    // Public constant variables
    public const float PAC_AURA = -0.5f;
    public const int INVALID = -1;
    public const int WALL = -2;
    public const int BLINKY = -3;
    public const int PAC_AURA_DIST = 2;

    // Public variables
    public GameObject LivesPrefab;
    public GameObject HighScorePrefab;
    public GameObject PointsPrefab;
    public Sprite sprite;
    public float[,] Grid;
    public float[,] InverseGrid;
    public bool[,] GridWalls;
    public int Points = 0;
    public int HighScore;
    [HideInInspector]
    public int Columns, Rows;

    // Game Setting Variables
    public float WallPercent = 0.3f;
    public float SuperPillPercent = 0.25f;
    public float SuperPillEffectTime = 5f;
    public int BlinkyFromPac = 10;
    public int Lives = 1;
    public float BlinkySpeed = 0.5f;
    public float PacManSpeed = 0.25f;

    // Private variables
    private GameObject lives;
    private GameObject highScore;
    private GameObject points;
    private TextMesh livesText;
    private TextMesh highScoreText;
    private TextMesh pointsText;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set sizes
        Columns = Rows = 20;
        Grid = new float[Columns, Rows];
        GridWalls = new bool[Columns, Rows];

        PlaceWalls();

        PlaceGrid();
        
        PlaceTexts();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    //Places texts on the scene
    private void PlaceTexts()
    {
        GameObject parent = new GameObject("Texts");

        lives = Instantiate(LivesPrefab, new Vector3(-10, 11.5f), Quaternion.identity);
        lives.name = "Lives";
        lives.transform.parent = parent.transform;
        livesText = lives.GetComponent<TextMesh>();

        highScore = Instantiate(HighScorePrefab, new Vector3(-4, 11.5f), Quaternion.identity);
        highScore.name = "High Score";
        highScore.transform.parent = parent.transform;
        highScoreText = highScore.GetComponent<TextMesh>();

        points = Instantiate(PointsPrefab, new Vector3(5, 11.5f), Quaternion.identity);
        points.name = "Points";
        points.transform.parent = parent.transform;
        pointsText = points.GetComponent<TextMesh>();
    }

    // Update texts
    private void UpdateText()
    {
        lives.text = "Lives: " + Lives.ToString();
        points.text = "Score: " + Points.ToString();
        highScore.text = "High Score: " + HighScore.ToString();
    }

    // Place walls in Grid
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

    // Set Grid values
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

    // Set Inverse Grid values
    public float[,] SetInverseGrid(int x, int y)
    {
        InverseGrid = new float[Columns, Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (GridWalls[i, j])
                {
                    InverseGrid[i, j] = WALL;
                }
                else
                {
                    InverseGrid[i, j] = INVALID;
                }
            }
        }
        var max = Grid.Cast<float>().Max();
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                if (Grid[i, j] == max)
                {
                    Visit(InverseGrid, i, j, 0);
                }
            }
        }
        PlaceAura(InverseGrid, x, y);
        return InverseGrid;
    }

    // Places an aura arround Pac-Man to scare ghost away
    private void PlaceAura(float[,] tempGrid, int row, int col)
    {
        for (int i = row - PAC_AURA_DIST; i < row + PAC_AURA_DIST + 1; i++)
        {
            for (int j = col - PAC_AURA_DIST; j < col + PAC_AURA_DIST + 1; j++)
            {
                if (i < Columns && i >= 0 && j < Rows && j >= 0 && tempGrid[i,j] != WALL)
                {
                    tempGrid[i,j] = PAC_AURA;
                }
            }
        }
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

    // Spawns an element of the map
    private void SpawnTile(int x, int y, Transform parent)
    {
        GameObject g = new GameObject("Tile - x: " + x + ",y: " + y);
        g.transform.parent = parent;
        g.transform.position = new Vector3(x - (Columns / 2 - 0.5f), (Rows / 2 - 0.5f) - y);
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
    }

    // Place Grid in the Game
    private void PlaceGrid()
    {
        GameObject parent = new GameObject("Grid");
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                SpawnTile(i, j, parent.transform);
            }
        }
    }
}