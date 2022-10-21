using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public Sprite sprite;
    private GameObject blinky;
    private int BlinkyX;
    private int BlinkyY;
    private float timer;

    private bool[,] GridWalls;
    private float[,] Grid;
    private int BlinkyFromPac;
    private int Columns;
    private int Rows;
    private int Horizontal;
    private int Vertical;
    private int WALL;
    private float PAC_AURA;
    private float BlinkySpeed;

    void Start()
    {
        GetGridVariables();
        SetBlinkyPosition();
    }

    void Update()
    {
        MoveBlinky();
    }

    private void SetBlinkyPosition()
    {
        do
        {
            BlinkyX = UnityEngine.Random.Range(BlinkyFromPac, Columns - 1);
            BlinkyY = UnityEngine.Random.Range(BlinkyFromPac, Rows - 1);
        } while (GridWalls[BlinkyX, BlinkyY]);
        SpawnBlinky(BlinkyX, BlinkyY);
    }

    private void SpawnBlinky(int x, int y)
    {
        blinky = new GameObject("Blinky");
        blinky.transform.position = new Vector3(
            x - (Horizontal - 0.5f), 
            (Vertical - 0.5f) - y
        );
        var s = blinky.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        s.color = Color.red;
        s.sortingOrder = 1;
    }

    private void MoveBlinky()
    {
        if(timer < BlinkySpeed)
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
        float[,] ghostGrid = Grid;

        /*
        if (isScared)
        {
            ghostGrid = inverseGrid;
        }
        */

        // Assign if the positions are in the field
        if (BlinkyY > 0)
        {
            north = ghostGrid[BlinkyX, BlinkyY - 1];
        }

        if (BlinkyX > 0)
        {
            west = ghostGrid[BlinkyX - 1, BlinkyY];
        }

        if (BlinkyY < Rows - 1)
        {
            south = ghostGrid[BlinkyX, BlinkyY + 1];
        }

        if (BlinkyX < Columns - 1)
        {
            east = ghostGrid[BlinkyX + 1, BlinkyY];
        }

        // Positions which are walls or invalid
        var avoid = Columns * 1000;

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
            BlinkyY--;
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
            BlinkyX--;
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
            BlinkyX++;
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
            BlinkyY++;
        }

        /*
        if (isScared && !isBlinkyEaten)
        {
            blinkyImage = "img/ghostScared.gif";
        }
        blinky.children[0].src = blinkyImage;
        */

        blinky.transform.position = new Vector3(
            BlinkyX - (Horizontal - 0.5f),
            (Vertical - 0.5f) - BlinkyY
        );

        // Pac goes in contact with blinky
        /*
        if (getGridX(pacmanX) == blinkyX && getGridY(pacmanY) == blinkyY && !isScared)
        {
            youLose();
        }
        */
        
    }

    public void GetGridVariables()
    {
        var g = GetComponent<GridManager>();
        GridWalls = g.GridWalls;
        Grid = g.Grid;
        BlinkyFromPac = g.BlinkyFromPac;
        Columns = g.Columns;
        Rows = g.Rows;
        Horizontal = g.Horizontal;
        Vertical = g.Vertical;
        WALL = GridManager.WALL;
        PAC_AURA = GridManager.PAC_AURA;
        BlinkySpeed = g.BlinkySpeed;
    }
}
