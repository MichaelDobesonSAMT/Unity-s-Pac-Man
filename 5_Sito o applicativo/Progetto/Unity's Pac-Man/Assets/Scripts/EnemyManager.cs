using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
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
    private int horizontal;
    private int vertical;
    private int WALL;
    private float PAC_AURA;
    private float blinkySpeed;

    private bool isFirstTime = true;

    public GameObject blinkyPrefab;
    public Sprite sprite;
    public RuntimeAnimatorController BlinkyController;

    // Start is called before the first frame update
    void Start()
    {
        GetGridVariables();
        ResetBlinkyPosition();
        SpawnBlinky(blinkyX, blinkyY);
        isFirstTime = false;
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
        horizontal = g.Horizontal;
        vertical = g.Vertical;
        WALL = GridManager.WALL;
        PAC_AURA = GridManager.PAC_AURA;
        blinkySpeed = g.BlinkySpeed;
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

        if(!isFirstTime){
            SetPosition(blinkyX, blinkyY);
        }
    }

    // Places Blinky on the Map
    private void SpawnBlinky(int x, int y)
    {
        //blinky = new GameObject("Blinky");
        blinky = Instantiate(blinkyPrefab, new Vector3(x - (horizontal - 0.5f), (vertical - 0.5f) - y), Quaternion.identity);
        blinky.name = "Blinky";
        /*blinky.transform.position = new Vector3(
            x - (horizontal - 0.5f), 
            (vertical - 0.5f) - y
        );*/
        /*
        blinky.tag = "Blinky";

        var s = blinky.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        s.color = Color.red;
        s.sortingOrder = 1;

        var a = blinky.AddComponent<Animator>();
        blinkyAnimator = blinky.GetComponent<Animator>();
        blinkyAnimator.runtimeAnimatorController = BlinkyController;
        */
        blinkyAnimator = blinky.GetComponent<Animator>();
    }

    private void SetPosition(int x, int y)
    {
        blinky.transform.position = new Vector3(
            x - (horizontal - 0.5f),
            (vertical - 0.5f) - y
        );
    }

    // Every x Blinky moves Closer to Pac-Man
    private void MoveBlinky()
    {
        // Get Updated Grid
        grid = GetComponent<PacManManager>().Grid;

        // If x time hasn't passed don't execute the Method
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
        float[,] ghostGrid = grid;

        /*
        if (isScared)
        {
            ghostGrid = inverseGrid;
        }
        */

        // Assign if the positions are in the field
        if (blinkyY > 0)
        {
            north = ghostGrid[blinkyX, blinkyY - 1];
        }

        if (blinkyX > 0)
        {
            west = ghostGrid[blinkyX - 1, blinkyY];
        }

        if (blinkyY < rows - 1)
        {
            south = ghostGrid[blinkyX, blinkyY + 1];
        }

        if (blinkyX < columns - 1)
        {
            east = ghostGrid[blinkyX + 1, blinkyY];
        }

        // Positions which are walls or invalid
        var avoid = columns * 1000;

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

        //Debug.Log(north + "N, " + west + "W, " + south + "S, " + east + "E");
        if (north <= west && north <= east && north <= south)
        {
            /*
            if (isBlinkyEaten)
            {
                blinkyImage = "img/eyesUp.png";
            }
            else
            {
                blinkyImage = "img/blinkyUp.gif";
            }
            */
            blinkyAnimator.SetBool("isLeft", false);
            blinkyAnimator.SetBool("isRight", false);
            blinkyAnimator.SetBool("isDown", false);
            blinkyAnimator.SetBool("isUp", true);
            blinkyY--;
        }
        else if (west <= north && west <= east && west <= south)
        {
            /*
            if (isBlinkyEaten)
            {
                blinkyImage = "img/eyesLeft.png";
            }
            else
            {
                blinkyImage = "img/blinkyLeft.gif";
            }
            */
            blinkyAnimator.SetBool("isUp", false);
            blinkyAnimator.SetBool("isRight", false);
            blinkyAnimator.SetBool("isDown", false);
            blinkyAnimator.SetBool("isLeft", true);
            blinkyX--;
        }
        else if (east <= west && east <= north && east <= south)
        {
            /*
            if (isBlinkyEaten)
            {
                blinkyImage = "img/eyesRight.png";
            }
            else
            {
                blinkyImage = "img/blinkyRight.gif";
            }
            */
            blinkyAnimator.SetBool("isLeft", false);
            blinkyAnimator.SetBool("isUp", false);
            blinkyAnimator.SetBool("isDown", false);
            blinkyAnimator.SetBool("isRight", true);
            blinkyX++;
        }
        else
        {
            /*
            if (isBlinkyEaten)
            {
                blinkyImage = "img/eyesDown.png";
            }
            else
            {
                blinkyImage = "img/blinkyDown.gif";
            }
            */
            blinkyAnimator.SetBool("isUp", false);
            blinkyAnimator.SetBool("isRight", false);
            blinkyAnimator.SetBool("isLeft", false);
            blinkyAnimator.SetBool("isDown", true);
            blinkyY++;
        }

        /*
        if (isScared && !isBlinkyEaten)
        {
            blinkyImage = "img/ghostScared.gif";
        }
        blinky.children[0].src = blinkyImage;
        */

        blinky.transform.position = new Vector3(
            blinkyX - (horizontal - 0.5f),
            (vertical - 0.5f) - blinkyY
        );

        // Pac goes in contact with blinky
        /*
        if (getGridX(pacmanX) == blinkyX && getGridY(pacmanY) == blinkyY && !isScared)
        {
            youLose();
        }
        */
        
    }
}