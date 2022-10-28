using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacManManager : MonoBehaviour
{

    private int x = 0;
    private int y = 0;

    public Sprite sprite;
    public Sprite sprite2;
    GameObject pacman;
    private int col;
    private int row;
    public float[,] Grid;
    private int WALL;
    private float timeLeft = 0;
    private float PacManSpeed;

    public void getGridVariables()
    {
        var g = GetComponent<GridManager>();
        col = g.Columns;
        row = g.Rows;
        Grid = g.Grid;
        PacManSpeed = g.PacManSpeed;
        WALL = GridManager.WALL;
        print(row);
    }
    void Start()
    {
        getGridVariables();
        spawnPacMan();
    }

    void Update()
    {
        CheckPacManMovement();
    }

    //makes PacMan move
    private void CheckPacManMovement()
    {
        pacman.GetComponent<SpriteRenderer>().sprite = sprite2;
        // makes timer go down and when wait time is over lets PacMan move again
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            // checks what key is pressed and checks if the movement makes PacMan go out of the borders
            if (Input.GetKey(KeyCode.S) && y < row - 1)
            {
                if (checkIfNotWall((int)x, (int)y + 1))
                {
                    y++;
                    pacman.transform.eulerAngles = Vector3.forward * -90;
                    pacman.GetComponent<SpriteRenderer>().sprite = sprite2;
                }
            }
            else if (Input.GetKey(KeyCode.D) && x < col - 1)
            {
                if (checkIfNotWall((int)x + 1, (int)y))
                {
                    x++;
                    pacman.transform.eulerAngles = Vector3.forward;
                    pacman.GetComponent<SpriteRenderer>().sprite = sprite2;
                }
            }
            else if (Input.GetKey(KeyCode.W) && y > 0)
            {
                if (checkIfNotWall((int)x, (int)y - 1))
                {
                    y--;
                    pacman.transform.eulerAngles = Vector3.forward * 90;
                    pacman.GetComponent<SpriteRenderer>().sprite = sprite2;
                }
            }
            else if (Input.GetKey(KeyCode.A) && x > 0)
            {
                if (checkIfNotWall((int)x - 1, (int)y))
                {
                    x--;
                    pacman.transform.eulerAngles = Vector3.forward * 180;
                    pacman.GetComponent<SpriteRenderer>().sprite = sprite2;
                }
            }
            // resets wait time
            timeLeft = PacManSpeed;
            pacman.transform.position = new Vector3(
                x - (col / 2 - 0.5f),
                (row / 2 - 0.5f) - y);
            Grid = GetComponent<GridManager>().SetGrid(x, y);
        }
        // once half of the wait time is over changes sprite
        if (timeLeft < PacManSpeed / 2)
        {
            pacman.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    // checks if the next space on the Map is a wall
    public bool checkIfNotWall(int gridX, int gridY)
    {
        return (Grid[gridX, gridY] != WALL);
    }

    public void spawnPacMan()
    {
        pacman = new GameObject("Pac-Man");
        pacman.transform.position = new Vector3(
            x - (col / 2 - 0.5f),
            (row / 2 - 0.5f) - y);
        var s = pacman.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        s.sortingOrder = 1;
        timeLeft = 1;
    }
}