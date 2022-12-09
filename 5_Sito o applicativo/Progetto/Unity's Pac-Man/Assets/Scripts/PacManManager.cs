using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PacManManager : MonoBehaviour
{
    // Public variables
    public GameObject PacManPrefab;
    public GameObject PauseCanvasPrefab;
    public GameObject GameOverCanvasPrefab;
    public Sprite PacManSprite;
    public Sprite PacManSprite2;
    public float[,] Grid;
    public float[,] InverseGrid;
    [HideInInspector]
    public int x = 0;
    [HideInInspector]
    public int y = 0;

    // Private variables
    private GameObject pacman;
    private GameObject Menu;
    private GameObject GOMenu;
    private Canvas MenuCanvas;
    private Canvas GameOverCanvas;
    private int col;
    private int row;
    private int WALL;
    private float timeLeft = 0;
    private float pacManSpeed;
    private int Lives;  

    // Imports necessary variables from grid manager
    public void getGridVariables()
    {
        var g = GetComponent<GridManager>();
        col = g.Columns;
        row = g.Rows;
        Grid = g.Grid;
        InverseGrid = g.InverseGrid;
        pacManSpeed = g.PacManSpeed;
        WALL = GridManager.WALL;
        Lives = g.Lives;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        GetPauseCanvas();
        GetGameOverCanvas();
        getGridVariables();
        SpawnPacMan();
    }

    // Update is called once per frame
    void Update()
    {
        // If the ESC key is pressed pause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GameWin();
        }

        MovePacMan();
    }

    private void GetPauseCanvas()
    {
        Menu = Instantiate(PauseCanvasPrefab,
            new Vector3(550, 259.5f, 10),
            Quaternion.identity);
        Menu.name = "MenuCanvas";

        GameObject.Find("ResumeButton").GetComponent<Button>().onClick.AddListener(ResumeGame);
        GameObject.Find("MenuButton").GetComponent<Button>().onClick.AddListener(ReturnToMenu);

        MenuCanvas = Menu.GetComponent<Canvas>();
        MenuCanvas.enabled = false;
    }

    private void GetGameOverCanvas()
    {
        GOMenu = Instantiate(GameOverCanvasPrefab,
            new Vector3(550, 259.5f, 20),
            Quaternion.identity);
        GOMenu.name = "GameOverCanvas";

        GameObject.Find("GOMenuButton").GetComponent<Button>().onClick.AddListener(ReturnToMenu);

        GameOverCanvas = GOMenu.GetComponent<Canvas>();
        GameOverCanvas.enabled = false;
    }

    // Pauses the game by freezing all objects
    public void PauseGame()
    {
        if(Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            MenuCanvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1f;
            MenuCanvas.enabled = false;
        }
    }

    // Unpauses the game by unfreezing all objects
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        MenuCanvas.enabled = false;
    }

    // Changes scene to main menu
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Stops the game
    public void GameOver()
    {
        Time.timeScale = 0f;
        GameObject.Find("GOScore").GetComponent<TextMeshProUGUI>().text = "Score: " + GetComponent<GridManager>().Points.ToString();
        GameObject.Find("GOHighScore").GetComponent<TextMeshProUGUI>().text = "HighScore: " + GetComponent<GridManager>().HighScore.ToString();
        GameOverCanvas.enabled = true;
    }

    // Resets Grid and position to continue playing
    public void GameWin()
    {
        Destroy(GameObject.Find("Grid"));
        GetComponent<GridManager>().PlaceWalls();
        GetComponent<GridManager>().PlaceGrid();

        Destroy(GameObject.Find("Pills"));
        GetComponent<PillManager>().PlacePills();

        x = 0;
        y = 0;
        GetComponent<EnemyManager>().ResetBlinkyPosition();
    }

    // makes Pac-Man move
    private void MovePacMan()
    {
        // Do all checks
        CheckPacManEatsPill(x - (col / 2 - 0.5f), (row / 2 - 0.5f) - y);
        CheckPacManEatsSuperPill(x - (col / 2 - 0.5f), (row / 2 - 0.5f) - y);
        CheckPacManEncountersBlinky(x - (col / 2 - 0.5f), (row / 2 - 0.5f) - y);

        pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;

        // Makes timer go down and when the wait time is over lets Pac-Man move again
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            // Check what key is pressed and checks if the movement makes Pac-Man go out of the borders
            if (Input.GetKey(KeyCode.S) && y < row - 1)
            {
                if (CheckIfNotWall(x, y + 1))
                {
                    y++;
                    pacman.transform.eulerAngles = Vector3.forward * -90;
                    pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;
                }
            }
            else if (Input.GetKey(KeyCode.D) && x < col - 1)
            {
                if (CheckIfNotWall(x + 1, y))
                {
                    x++;
                    pacman.transform.eulerAngles = Vector3.forward;
                    pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;
                }
            }
            else if (Input.GetKey(KeyCode.W) && y > 0)
            {
                if (CheckIfNotWall(x, y - 1))
                {
                    y--;
                    pacman.transform.eulerAngles = Vector3.forward * 90;
                    pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;
                }
            }
            else if (Input.GetKey(KeyCode.A) && x > 0)
            {
                if (CheckIfNotWall(x - 1, y))
                {
                    x--;
                    pacman.transform.eulerAngles = Vector3.forward * 180;
                    pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;
                }
            }

            // Reset wait time
            timeLeft = pacManSpeed;

            // Set Pac-Man position
            pacman.transform.position = new Vector3(
                x - (col / 2 - 0.5f),
                (row / 2 - 0.5f) - y);

            // Update the Grid (for the AI)
            Grid = GetComponent<GridManager>().SetGrid(x, y);
            InverseGrid = GetComponent<GridManager>().SetInverseGrid(x, y);
        }
        // Once half of the wait time is over the sprite is changed
        if (timeLeft < pacManSpeed / 2)
        {
            pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite;
        }

        // If all pills are eaten
        var pills = GameObject.FindGameObjectsWithTag("Pill");
        var sPills = GameObject.FindGameObjectsWithTag("SuperPill");
        if(pills.Length == 0 && sPills.Length == 0)
        {
            GameWin();
        }
    }

    // Check if the next space on the map is a wall
    public bool CheckIfNotWall(int x, int y)
    {
        return (Grid[x, y] != WALL);
    }

    // Creates the Pac-Man and places him on the grid
    public void SpawnPacMan()
    {
        pacman = Instantiate(PacManPrefab, new Vector3(
            x - (col / 2 - 0.5f),
            (row / 2 - 0.5f) - y), Quaternion.identity);
        pacman.name = "Pac-Man";
        timeLeft = 1;
    }

    // Checks if Pac-Man is on a Pill, deletes said Pill and increases points
    public void CheckPacManEatsPill(float pacX, float pacY)
    {
        var pills = GameObject.FindGameObjectsWithTag("Pill");
        foreach(var obj in pills)
        {
            float pillX = obj.transform.position.x;
            float pillY = obj.transform.position.y;
            if(pacX == pillX && pacY == pillY)
            {
                GetComponent<GridManager>().Points++;
                Destroy(obj);
            }
        }
    }

    // Checks if Pac-Man is on a Super Pill, deletes said Pill, increases points
    // and allows Pac-Man to eat the ghost for x time while the ghost runs away
    public void CheckPacManEatsSuperPill(float pacX, float pacY)
    {
        var pills = GameObject.FindGameObjectsWithTag("SuperPill");
        foreach (var obj in pills)
        {
            float pillX = obj.transform.position.x;
            float pillY = obj.transform.position.y;
            if (pacX == pillX && pacY == pillY)
            {
                GetComponent<GridManager>().Points++;
                Destroy(obj);
                var enemy = GetComponent<EnemyManager>();
                if(!enemy.isScared && !enemy.isEaten)
                {
                    enemy.isScared = true;
                }
            }
        }
    }

    // Checks if Pac-Man is on the same position as Blinky
    public void CheckPacManEncountersBlinky(float pacX, float pacY)
    {
        var blinky = GameObject.FindGameObjectWithTag("Blinky");
        float blinkyX = blinky.transform.position.x;
        float blinkyY = blinky.transform.position.y;
        if(pacX == blinkyX && pacY == blinkyY)
        {
            // If Blinky is scared
            if (GetComponent<EnemyManager>().isScared)
            {
                GetComponent<GridManager>().Points += 5;
                GetComponent<EnemyManager>().isEaten = true;
                GetComponent<EnemyManager>().timeLeft = 5;
            }
            else
            {
                // If Pac-Man still has live reset position otherwise Game Over
                if (Lives > 0)
                {
                    Lives--;
                    x = 0;
                    y = 0;
                    GetComponent<EnemyManager>().ResetBlinkyPosition();
                    GetComponent<GridManager>().Lives = Lives;
                }
                else
                {
                    GameOver();
                }
            }
        }
    }
}