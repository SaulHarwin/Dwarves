using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{

    public int width;
    public int height;

    public float scale;

    public Sprite grassSprite;
    public Sprite waterSprite;

    List<GridSpace> gridspaces = new List<GridSpace>();

    private void Start()
    {
        InitializeWorld();
    }
    private void Update()
    {
        
    }

    void InitializeWorld()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridSpace gs = new GridSpace();
                gs.GSGO = new GameObject();
                gs.GSGO.AddComponent<SpriteRenderer>();
                gs.GSGO.transform.position = new Vector2((float)x / 5, (float)y / 5);
                gs.GSGO.transform.localScale = new Vector2(0.2f, 0.2f);
                /*
                if(Mathf.PerlinNoise((x * scale) + 0.1f, (y * scale) + 0.1f) > 0.5)
                {
                    gs.gsType = GridSpace.type.GRASS;
                }
                else
                {
                    gs.gsType = GridSpace.type.WATER;
                }
                */
                if(Mathf.PerlinNoise((x * scale) + 0.1f, (y * scale) + 0.1f) > 0.3 + 0.4 * distanceFromEdge(x, y))
                {
                    gs.gsType = GridSpace.type.GRASS;
                }
                else
                {
                    gs.gsType = GridSpace.type.WATER;
                }

                gridspaces.Add(gs);
            }
        }
        updateWorld();
    }

    void updateWorld()
    {
        foreach(GridSpace gs in gridspaces)
        {
            switch (gs.gsType)
            {
                case GridSpace.type.GRASS:
                    gs.GSGO.GetComponent<SpriteRenderer>().sprite = grassSprite;
                    break;
                case GridSpace.type.WATER:
                    gs.GSGO.GetComponent<SpriteRenderer>().sprite = waterSprite;
                    break;
            }
        }
    }

    public int distanceFromEdge(int x, int y)
    {
        int closeness;
        int centerX = width / 2;
        int centerY = height / 2;
        //return closeness
    }
}

public class GridSpace
{
    public GameObject GSGO;
    public enum type { WATER, SAND, GRASS, MOUNTAIN};
    public type gsType;

}
