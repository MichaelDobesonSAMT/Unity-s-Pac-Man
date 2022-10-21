using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PacManManager : MonoBehaviour
{

    private float x = 0 ;
    private float y = 0;

    public Sprite sprite;
    public Sprite sprite2;
    GameObject pacman;
    private int col;
    private int row;
    private float timer = 0;

    public void getGridVariables()
    {
        var g = GetComponent<GridManager>();
        col = g.Columns;
        row = g.Rows;
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

    private void CheckPacManMovement()
    {
        

        pacman.GetComponent<SpriteRenderer>().sprite = sprite2;
        if (Input.GetKeyDown(KeyCode.S) && y < row - 1)
        {
            y++;
        }
        else if (Input.GetKeyDown(KeyCode.D) && x < col - 1)
        {
            x++;
        }
        else if (Input.GetKeyDown(KeyCode.W) && y > 0)
        {
            if (timer < 0.5f)
            {
                timer += Time.deltaTime;
                return;
            }
            else
            {
                timer = 0;
            }
            y--;
        }
        if (Input.GetKeyDown(KeyCode.A) && x > 0)
        {
            x--;
        }

        // pacman.transform.position = Vector3.forward * -90;
        //var s = pacman.AddComponent<SpriteRenderer>();
        //s.sprite = sprite;
        pacman.GetComponent<SpriteRenderer>().sprite = sprite;
        pacman.transform.position = new Vector3(
            x - (col / 2 - 0.5f),
            (row / 2 - 0.5f) - y);

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
    }
}