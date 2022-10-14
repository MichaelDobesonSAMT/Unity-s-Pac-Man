using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private const float PAC_AURA = -0.5f;
    private const int INVALID = -1;
    private const int WALL = -2;
    private const int BLINKY = -3;

    public Sprite sprite;
    public float[,] Grid;
    public bool[,] GridWalls;
    public float WallPercent = 0.3f;
    int Vertical, Horizontal, Columns, Rows;
    
    void Start()
    {
        Horizontal = Vertical = (int)Camera.main.orthographicSize * 2;
        Columns = Rows = Horizontal * 2;
        Grid = new float[Columns, Rows];
        GridWalls = new bool[Columns, Rows];

        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++)
            {/*
                var rnd = Random.Range(0.0f, 1.0f);
                if(rnd < WallPercent){
                    GridWalls[i,j] = true;
                    SetGrid();
                    for (int k = 0; k < Columns; k++) {
                        for (int l = 0; l < Rows; l++) {
                            if(Grid[k,l] == INVALID){
                                GridWalls[i,j] = false;
                            }
                        }
                    }
                }else{
                    GridWalls[i,j] = false;
                }*/
                GridWalls[1, 0] = false;
                GridWalls[1, 5] = false;
                GridWalls[2, 5] = false;
                GridWalls[3, 5] = false;
                GridWalls[3, 6] = false;
                SetGrid();
            }
        }
        Visit(Grid, 0, 0, 0);
        for (int i = 0; i < Columns; i++)
        {
            for (int j = 0; j < Rows; j++){
                SpawnTile(i, j, Grid[i, j]);
                Debug.Log("C: " + i + " ,R: " + j + " ,val: " + Grid[i,j]);
            }
        }
    }

    private void SetGrid(){
        Grid = new float[Columns, Rows];

        for (int i = 0; i < Columns; i++) {
            for (int j = 0; j < Rows; j++) {
                if(GridWalls[i,j]){
                    Grid[i,j] = WALL;
                }else{
                    Grid[i,j] = INVALID;
                }
            }
        }
        Visit(Grid, 0, 0, 0); //2,3 parameters are for pac pos
    }

    //manhattan distance algorithm
    private void Visit(float[,] tempGrid, int col, int row, int dist){
        if(row < Rows && row >= 0 && col < Columns && col >= 0){
            if(
                (tempGrid[col,row] > dist && tempGrid[col,row] != WALL) || 
                tempGrid[col,row] == INVALID
            ){
                tempGrid[col,row] = dist;
                Visit(tempGrid, row, col-1,dist+1);
                Visit(tempGrid, row-1, col,dist+1);
                Visit(tempGrid, row, col+1,dist+1);
                Visit(tempGrid, row+1, col,dist+1);
            }
        }
    }
    
    private void SpawnTile(int x, int y, float value)
    {
        GameObject g = new GameObject("x: " + x + ",y: " + y);
        g.transform.position = new Vector3(x - (Horizontal - 0.5f),  (Vertical - 0.5f) - y);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
        if(Grid[x,y] == WALL){
            s.color = new Color(0, 0, 255);
        }else{
            s.color = new Color(0, 0, 0);
        }
        var myText = CreateText(g.transform);
        myText.text = x + ", " + y + "\n: " + value;
        myText.transform.position = new Vector3(x - (Horizontal), (Vertical) - y, -0.25f);
        myText.transform.localScale = new Vector3(0.25f, 0.25f);
    }

    private TextMesh CreateText(Transform parent)
    {
        var go = new GameObject();
        go.transform.parent = parent;
        var text = go.AddComponent<TextMesh>();
        return text;
    }
}