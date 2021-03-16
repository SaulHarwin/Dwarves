using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{

    public int width;
    public int height;

    public float scale;

    public int octaveCount;

    public Sprite deepWaterSprite;
    public Sprite waterSprite;
    public Sprite sandSprite;
    public Sprite grassSprite;
    public Sprite highGrassSprite;
    public Sprite mountainSprite;

    public TerrainType[] terrainTypes;

    List<GridSpace> gridspaces = new List<GridSpace>();

    public bool useRandomSeed;
    public int seed;

    private void Start()
    { 
        InitializeWorld();
    }
    private void Update()
    {
        
    }

    void InitializeWorld()
    {
        float[] frequencies = new float[octaveCount];
        for (int i = 0; i < frequencies.Length; i++)
        {
            frequencies[i] = Random.Range(0f, 2f);
            Debug.Log(frequencies[i]);
        }

        if (useRandomSeed)
        {
            seed = Random.Range(0, 100000);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridSpace gs = new GridSpace();
                gs.GSGO = new GameObject();
                gs.GSGO.AddComponent<SpriteRenderer>();
                gs.GSGO.transform.position = new Vector2((float)x / 10, (float)y / 10);
                gs.pos = new Vector2(x, y);
                gs.GSGO.transform.localScale = new Vector2(0.1f, 0.1f);
                gs.GSGO.transform.SetParent(GameObject.Find("Grid Manager").transform);

                float[] samples = new float[octaveCount];
                float Sample = 0;
                for (int i = 0; i < frequencies.Length; i++)
                {
                    samples[i] = Mathf.PerlinNoise(seed + (x * scale + 0.1f) * frequencies[i], seed + (y * scale + 0.1f) * frequencies[i]);
                    Sample += samples[i];
                }
                int count = 0;
                foreach(TerrainType tt in terrainTypes)
                {
                    if(Sample <= terrainTypes[count].height)
                    {
                        gs.terrainTypeID = terrainTypes[count].terrainID;
                        break;
                    }
                    count++;
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
            switch (gs.terrainTypeID)
            {
                case 0:
                    gs.GSGO.GetComponent<SpriteRenderer>().sprite = mountainSprite;
                    break;
                case 1:
                    gs.GSGO.GetComponent<SpriteRenderer>().sprite = highGrassSprite;
                    break;
                case 2:
                    gs.GSGO.GetComponent<SpriteRenderer>().sprite = grassSprite;
                    break;
                case 3:
                    gs.GSGO.GetComponent<SpriteRenderer>().sprite = sandSprite;
                    break;
                case 4:
                    gs.GSGO.GetComponent<SpriteRenderer>().sprite = waterSprite;
                    break;
                case 5:
                    gs.GSGO.GetComponent<SpriteRenderer>().sprite = deepWaterSprite;
                    break;
            }
        }
    }


}

public class GridSpace
{
    public GameObject GSGO;
    public Vector2 pos;
    public int terrainTypeID;
}

[System.Serializable]
public struct TerrainType
{
    public string name;

    [Range(0, 1)]
    public float height;

    public int terrainID;
}
