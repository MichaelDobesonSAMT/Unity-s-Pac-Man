using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public Sprite sprite;

    void Start()
    {
        SpawnBlinky(0, 0);
    }

    void Update()
    {
        
    }

    private void SpawnBlinky(int x, int y)
    {
        GameObject blinky = new GameObject("Blinky");
        blinky.transform.position = new Vector3(
            x - (GridManager.Horizontal - 0.5f), 
            (GridManager.Vertical - 0.5f) - y
        );
        var s = blinky.AddComponent<SpriteRenderer>();
        s.sprite = sprite;
    }
}
