using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{

    public int width;
    public int height;

    public float scale;
    public float amplitude;
    public float percistance;
    public float lacunarity;

    public float numOctaves;


    public Color deepWaterColor;
    public Color waterColor;
    public Color sandColor;
    public Color grassColor;
    public Color highGrassColor;
    public Color mountainColor;

    public TerrainType[] terrainTypes;

    List<GridSpace> gridspaces = new List<GridSpace>();

    public bool useRandomSeed;
    public int seed;

    Texture2D worldTexture;

    private void Start()
    { 
        InitializeWorld();
    }
    private void Update()
    {
        
    }

    void InitializeWorld()
    {
        worldTexture = new Texture2D(width, height);
        worldTexture.filterMode = FilterMode.Point;
        GameObject worldDisplay = GameObject.CreatePrimitive(PrimitiveType.Quad);
        MeshRenderer mr = worldDisplay.GetComponent<MeshRenderer>();
        mr.material.mainTexture = worldTexture;
        mr.material.SetFloat("_Glossiness", 0f);
        worldDisplay.transform.localScale = new Vector3(width, height, 1);
        worldDisplay.transform.SetParent(GameObject.Find("Grid Manager").transform);
        if (useRandomSeed)
        {
            seed = Random.Range(0, 100000);
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridSpace gs = new GridSpace();

                gs.pos = new Vector2(x, y);

                float sample = Mathf.PerlinNoise((seed + x) * scale, (seed + y) * scale);
                sample *= amplitude;

                float newAmplitude = amplitude * percistance;
                float newScale = scale * lacunarity;

                for (int o = 0; o < numOctaves; o++, newScale *= lacunarity, newAmplitude *= percistance)
                {
                    float newSample = Mathf.PerlinNoise((seed + x) * newScale, (seed + y) * newScale);
                    newSample *= newAmplitude;
                    sample += newSample;
                }
                sample /= DistFromCentre(gs);

                int count = 0;
                foreach(TerrainType tt in terrainTypes)
                {
                    if(sample <= terrainTypes[count].height)
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
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridSpace gs = returnGS(x, y);
                switch (gs.terrainTypeID)
                {
                    case 0:
                        worldTexture.SetPixel(x, y, mountainColor);
                        break;
                    case 1:
                        worldTexture.SetPixel(x, y, highGrassColor);
                        break;
                    case 2:
                        worldTexture.SetPixel(x, y, grassColor);
                        break;
                    case 3:
                        worldTexture.SetPixel(x, y, sandColor);
                        break;
                    case 4:
                        worldTexture.SetPixel(x, y, waterColor);
                        break;
                    case 5:
                        worldTexture.SetPixel(x, y, deepWaterColor);
                        break;
                }
            }
            worldTexture.Apply();
        }
    }

    float DistFromCentre(GridSpace gs)
    {
        float centreX = Mathf.Ceil(width / 2);
        float centreY = Mathf.Ceil(height / 2);
        float widthHeight = centreX / centreY;
        float distance = Vector2.Distance(new Vector2(centreX, centreY), gs.pos) / 5;
        return distance;
    }

    public GridSpace returnGS(int x, int y)
    {
        foreach(GridSpace gs in gridspaces)
        {
            if (gs.pos == new Vector2(x, y))
            {
                return gs;
            }
        }
        return null;
    }
}

public class GridSpace
{
    public GameObject GSGO;
    public Vector2 pos;
    public int terrainTypeID;
}

public class Chunk
{
    public List<GridSpace> GridSpacesInChunk;
    public Vector2 pos;
}

[System.Serializable]
public struct TerrainType
{
    public string name;

    [Range(0, 1)]
    public float height;

    public int terrainID;
}
