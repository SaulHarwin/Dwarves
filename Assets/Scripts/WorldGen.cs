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

    public TerrainType[] terrainTypes;

    private GameObject worldDisplay;

    List<GridSpace> gridspaces = new List<GridSpace>();

    public bool useRandomSeed;
    public int seed;

    Texture2D worldTexture;

    private void Start()
    { 
        InitializeObjects();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            updateWorld();
        }
    }

    void InitializeObjects()
    {
        gridspaces.Clear();
        worldTexture = new Texture2D(width, height);
        worldTexture.filterMode = FilterMode.Point;
        worldDisplay = GameObject.CreatePrimitive(PrimitiveType.Quad);
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
                        gs.terrainType = terrainTypes[count];
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
        gridspaces.Clear();
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
                foreach (TerrainType tt in terrainTypes)
                {
                    if (sample <= terrainTypes[count].height)
                    {
                        gs.terrainType = terrainTypes[count];
                        break;
                    }
                    count++;
                }
                
                gridspaces.Add(gs);
            }
        }
        

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridSpace gs = returnGS(x, y);

                worldTexture.SetPixel(x, y, gs.terrainType.terrainColor);
            }

            worldTexture.Apply();
        }
    }

    float DistFromCentre(GridSpace gs)
    {
        float centreX = Mathf.Ceil(width / 2);
        float centreY = Mathf.Ceil(height / 2);
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
    public TerrainType terrainType;
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
    public Color terrainColor;

    [Range(0, 1)]
    public float height;

    public int terrainID;
}
