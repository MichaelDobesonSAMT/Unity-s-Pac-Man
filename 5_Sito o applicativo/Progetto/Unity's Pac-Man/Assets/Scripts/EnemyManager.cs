using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    // Public variables
    public GameObject blinkyPrefab;
    public Sprite sprite;
    public Sprite eyesUp;
    public Sprite eyesDown;
    public Sprite eyesLeft;
    public Sprite eyesRight;
    public RuntimeAnimatorController BlinkyController;
    [HideInInspector]
    public bool isScared = false;
    [HideInInspector]
    public bool isEaten = false;

    // Private variables
    private GameObject blinky;
    private Animator blinkyAnimator;
    private int blinkyX;
    private int blinkyY;
    private float timer;
    private bool[,] gridWalls;
    private float[,] grid;
    private int blinkyFromPac;
    private int columns;
    private int rows;
    private int WALL;
    private float PAC_AURA;
    private float blinkySpeed;
    private float superPillEffect;
    private float timeLeft = 0;
    private bool isFirstTime = true;

    // Start is called before the first frame update
    void Start()
    {
        GetGridVariables();
        ResetBlinkyPosition();
        SpawnBlinky(blinkyX, blinkyY);
        isFirstTime = false;
        timeLeft = superPillEffect;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBlinky();
    }

    // Imports necessary Variables from Grid Manager
    public void GetGridVariables()
    {
        var g = GetComponent<GridManager>();
        gridWalls = g.GridWalls;
        grid = g.Grid;
        blinkyFromPac = g.BlinkyFromPac;
        columns = g.Columns;
        rows = g.Rows;
        WALL = GridManager.WALL;
        PAC_AURA = GridManager.PAC_AURA;
        blinkySpeed = g.BlinkySpeed;
        superPillEffect = g.SuperPillEffectTime;
    }

    // Finds Random Position for Blinky to Spawn
    public void ResetBlinkyPosition()
    {
        // Goes through all the Positions (from Preset Distance) to find one without a Wall
        do
        {
            blinkyX = UnityEngine.Random.Range(blinkyFromPac, columns - 1);
            blinkyY = UnityEngine.Random.Range(blinkyFromPac, rows - 1);
        } while (gridWalls[blinkyX, blinkyY]);

        if (!isFirstTime){
            SetBlinkyPosition(blinkyX, blinkyY);
        }
    }

    // Places Blinky on the Grid
    private void SpawnBlinky(int x, int y)
    {
        blinky = Instantiate(blinkyPrefab, new Vector3(
            x - (columns / 2 - 0.5f), 
            (rows / 2 - 0.5f) - y), Quaternion.identity);
        blinky.name = "Blinky";
        blinkyAnimator = blinky.GetComponent<Animator>();
    }

    // Sets the position of Blinky
    private void SetBlinkyPosition(int x, int y)
    {
        blinky.transform.position = new Vector3(
            x - (columns / 2 - 0.5f),
            (rows / 2 - 0.5f) - y
        );
    }

    // Every x Blinky moves closer to Pac-Man
    private void MoveBlinky()
    {
        // Timer for how long the ghost should be scared for
        if(isScared && !isEaten)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                isScared = false;
                timeLeft = superPillEffect;
            }
        }

        // Get updated Grid
        grid = GetComponent<PacManManager>().Grid;
        if (isScared)
        {
            grid = GetComponent<PacManManager>().InverseGrid;
        }

        // If x time hasn't passed don't execute the method
        if (timer < blinkySpeed)
        {
            timer += Time.deltaTime;
            return;
        }
        else
        {
            timer = 0;
        }

        float north = WALL;
        float west = WALL;
        float south = WALL;
        float east = WALL;

        // Assign if the positions are in the field
        if (blinkyY > 0)
        {
            north = grid[blinkyX, blinkyY - 1];
        }

        if (blinkyX > 0)
        {
            west = grid[blinkyX - 1, blinkyY];
        }

        if (blinkyY < rows - 1)
        {
            south = grid[blinkyX, blinkyY + 1];
        }

        if (blinkyX < columns - 1)
        {
            east = grid[blinkyX + 1, blinkyY];
        }

        // Positions which are walls or invalid
        float avoid = columns * 1000;

        if (north == WALL || north == PAC_AURA)
        {
            north = Math.Abs(north * avoid);
        }

        if (west == WALL || west == PAC_AURA)
        {
            west = Math.Abs(west * avoid);
        }

        if (south == WALL || south == PAC_AURA)
        {
            south = Math.Abs(south * avoid);
        }

        if (east == WALL || east == PAC_AURA)
        {
            east = Math.Abs(east * avoid);
        }

        blinkyAnimator.SetBool("isUp", false);
        blinkyAnimator.SetBool("isRight", false);
        blinkyAnimator.SetBool("isLeft", false);
        blinkyAnimator.SetBool("isDown", false);
        blinkyAnimator.SetBool("isScared", false);

        // Blinky going up
        if (north <= west && north <= east && north <= south)
        {
            if (isScared && !isEaten)
            {
                blinkyAnimator.SetBool("isScared", true);
            }
            else
            {
                if (isEaten)
                {
                    blinky.GetComponent<SpriteRenderer>().sprite = eyesUp;
                }
                else
                {
                    blinkyAnimator.SetBool("isUp", true);
                }
            }
            
            blinkyY--;
        }
        // Blinky going left
        else if (west <= north && west <= east && west <= south)
        {
            if (isScared && !isEaten)
            {
                blinkyAnimator.SetBool("isScared", true);
            }
            else
            {
                if (isEaten)
                {
                    blinky.GetComponent<SpriteRenderer>().sprite = eyesLeft;
                }
                else
                {
                    blinkyAnimator.SetBool("isLeft", true);
                }
            }
            blinkyX--;
        }
        // Blinky going right
        else if (east <= west && east <= north && east <= south)
        {
            if (isScared && !isEaten)
            {
                blinkyAnimator.SetBool("isScared", true);
            }
            else
            {
                if (isEaten)
                {
                    blinky.GetComponent<SpriteRenderer>().sprite = eyesRight;
                }
                else
                {
                    blinkyAnimator.SetBool("isRight", true);
                }
            }
            blinkyX++;
        }
        // Blinky going down
        else
        {
            if (isScared && !isEaten)
            {
                blinkyAnimator.SetBool("isScared", true);
            }
            else
            {
                if (isEaten)
                {
                    blinky.GetComponent<SpriteRenderer>().sprite = eyesDown;
                }
                else
                {
                    blinkyAnimator.SetBool("isDown", true);
                }
            }
            blinkyY++;
        }

        blinky.transform.position = new Vector3(
            blinkyX - (columns / 2 - 0.5f),
            (rows / 2 - 0.5f) - blinkyY
        );

        // Pac goes in contact with blinky
        var pac = GetComponent<PacManManager>();
        if (!isScared)
        {
            pac.CheckPacManEncountersBlinky(pac.x, pac.y);
        }
        
    }
}