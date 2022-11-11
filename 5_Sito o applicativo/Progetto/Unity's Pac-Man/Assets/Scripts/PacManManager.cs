using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacManManager : MonoBehaviour
{
    private GameObject pacman;
    private int x = 0;
    private int y = 0;
    private int col;
    private int row;
    private int WALL;
    private float timeLeft = 0;
    private float pacManSpeed;
    private int pacManLives;

    public Sprite PacManSprite;
    public Sprite PacManSprite2;
    public float[,] Grid;

    public void getGridVariables()
    {
        var g = GetComponent<GridManager>();
        col = g.Columns;
        row = g.Rows;
        Grid = g.Grid;
        pacManSpeed = g.PacManSpeed;
        WALL = GridManager.WALL;
        pacManLives = g.PacManLives;
    }
    void Start()
    {
        getGridVariables();
        SpawnPacMan();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && x > 0)
        {
            PauseGame();
        }
        CheckPacManMovement();
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        GetComponent<GridManager>().gameOver.gameObject.SetActive(false);
    }

    //makes PacMan move
    private void CheckPacManMovement()
    {
        pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;

        // Makes Timer go Down and when Wait Time is Over lets Pac-Man move Again
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            // Check what Key is pressed and Check if the Movement makes Pac-Man go Out of the Borders
            if (Input.GetKey(KeyCode.S) && y < row - 1)
            {
                if (CheckIfNotWall((int)x, (int)y + 1))
                {
                    y++;
                    pacman.transform.eulerAngles = Vector3.forward * -90;
                    pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;
                }
            }
            else if (Input.GetKey(KeyCode.D) && x < col - 1)
            {
                if (CheckIfNotWall((int)x + 1, (int)y))
                {
                    x++;
                    pacman.transform.eulerAngles = Vector3.forward;
                    pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;
                }
            }
            else if (Input.GetKey(KeyCode.W) && y > 0)
            {
                if (CheckIfNotWall((int)x, (int)y - 1))
                {
                    y--;
                    pacman.transform.eulerAngles = Vector3.forward * 90;
                    pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;
                }
            }
            else if (Input.GetKey(KeyCode.A) && x > 0)
            {
                if (CheckIfNotWall((int)x - 1, (int)y))
                {
                    x--;
                    pacman.transform.eulerAngles = Vector3.forward * 180;
                    pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;
                }
            }

            // Reset Eait Time
            timeLeft = pacManSpeed;

            // Set Pac-Man Position
            pacman.transform.position = new Vector3(
                x - (col / 2 - 0.5f),
                (row / 2 - 0.5f) - y);

            // Do All Checks
            CheckPacManEatsPill(x - (col / 2 - 0.5f), (row / 2 - 0.5f) - y);
            CheckPacManEncountersBlinky(x - (col / 2 - 0.5f), (row / 2 - 0.5f) - y);

            // Update the Grid (for the AI)
            Grid = GetComponent<GridManager>().SetGrid(x, y);
        }
        // Once Half of the Wait Time is Over Changes Sprite
        if (timeLeft < pacManSpeed / 2)
        {
            pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite;
        }
    }

    // Check if the Next Space on the Map is a Wall
    public bool CheckIfNotWall(int gridX, int gridY)
    {
        return (Grid[gridX, gridY] != WALL);
    }

    public void SpawnPacMan()
    {
        pacman = new GameObject("Pac-Man");
        pacman.transform.position = new Vector3(
            x - (col / 2 - 0.5f),
            (row / 2 - 0.5f) - y);
        var s = pacman.AddComponent<SpriteRenderer>();
        s.sprite = PacManSprite;
        s.sortingOrder = 1;
        timeLeft = 1;
    }

    // Check if Pac-Man is on a Pill and Delete said Pill
    public void CheckPacManEatsPill(float pacX, float pacY)
    {
        var pills = GameObject.FindGameObjectsWithTag("Pill");
        foreach(var obj in pills)
        {
            float pillX = obj.transform.position.x;
            float pillY = obj.transform.position.y;
            if(pacX == pillX && pacY == pillY)
            {
                GetComponent<GridManager>().PacManPoints++;
                Destroy(obj);
            }
        }
    }

    public void CheckPacManEncountersBlinky(float pacX, float pacY)
    {
        var blinky = GameObject.FindGameObjectWithTag("Blinky");
        float blinkyX = blinky.transform.position.x;
        float blinkyY = blinky.transform.position.y;
        if(pacX == blinkyX && pacY == blinkyY)
        {
            if (pacManLives > 0)
            {
                pacManLives--;
                x = 0;
                y = 0;
                GetComponent<EnemyManager>().BlinkyResetPosition();
                GetComponent<GridManager>().PacManLives = pacManLives;
            }
            else {
                GameOver();
            }
        }
    }
}