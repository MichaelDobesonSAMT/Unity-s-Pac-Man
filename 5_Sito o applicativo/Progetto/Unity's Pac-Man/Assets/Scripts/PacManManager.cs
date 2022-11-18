using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PacManManager : MonoBehaviour
{
    // Private variables
    private GameObject pacman;
    private int x = 0;
    private int y = 0;
    private int col;
    private int row;
    private int WALL;
    private float timeLeft = 0;
    private float pacManSpeed;
    private int pacManLives;

    // Public variables
    public Sprite PacManSprite;
    public Sprite PacManSprite2;
    public Button ResumeButton;
    public Button MenuButton;
    public Canvas menu;
    public float[,] Grid;

    // Imports necessary variables from grid manager
    public void getGridVariables()
    {
        var g = GetComponent<GridManager>();
        col = g.Columns;
        row = g.Rows;
        Grid = g.Grid;
        pacManSpeed = g.PacManSpeed;
        WALL = GridManager.WALL;
        pacManLives = g.Lives;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        menu.enabled = false;
        Button btn1 = ResumeButton.GetComponent<Button>();
        Button btn2 = MenuButton.GetComponent<Button>();
        btn1.onClick.AddListener(ResumeGame);
        btn2.onClick.AddListener(ReturnToMenu);
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
        MovePacMan();
    }

    // Un/Pauses the game by un/freezing all objects
    public void PauseGame()
    {
        Time.timeScale = 0f;
        menu.enabled = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menu.enabled = false;
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Stops the game
    public void GameOver()
    {
        Time.timeScale = 0f;
        GetComponent<GridManager>().gameOver.gameObject.SetActive(false);
    }

    // makes Pac-Man move
    private void MovePacMan()
    {

        pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite2;

        // Makes timer go down and when the wait time is over lets Pac-Man move again
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            // Check what key is pressed and checks if the movement makes Pac-Man go out of the borders
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

            // Reset wait time
            timeLeft = pacManSpeed;

            // Set Pac-Man position
            pacman.transform.position = new Vector3(
                x - (col / 2 - 0.5f),
                (row / 2 - 0.5f) - y);

            // Do all checks
            CheckPacManEatsPill(x - (col / 2 - 0.5f), (row / 2 - 0.5f) - y);
            CheckPacManEncountersBlinky(x - (col / 2 - 0.5f), (row / 2 - 0.5f) - y);

            // Update the Grid (for the AI)
            Grid = GetComponent<GridManager>().SetGrid(x, y);
        }
        // Once half of the wait time is over the sprite is changed
        if (timeLeft < pacManSpeed / 2)
        {
            pacman.GetComponent<SpriteRenderer>().sprite = PacManSprite;
        }
    }

    // Check if the next space on the map is a wall
    public bool CheckIfNotWall(int gridX, int gridY)
    {
        return (Grid[gridX, gridY] != WALL);
    }

    // Creates the Pac-Man and places him on the grid
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

    // Checks if Pac-Man is on a pill, deletes said pill and increases points
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

    // Checks if Pac-Man is on the same position as Blinky
    public void CheckPacManEncountersBlinky(float pacX, float pacY)
    {
        var blinky = GameObject.FindGameObjectWithTag("Blinky");
        float blinkyX = blinky.transform.position.x;
        float blinkyY = blinky.transform.position.y;
        if(pacX == blinkyX && pacY == blinkyY)
        {
            // If Pac-Man still has live reset position otherwise Game Over
            if (pacManLives > 0)
            {
                pacManLives--;
                x = 0;
                y = 0;
                GetComponent<EnemyManager>().ResetBlinkyPosition();
                GetComponent<GridManager>().Lives = pacManLives;
            }
            else {
                GameOver();
            }
        }
    }
}