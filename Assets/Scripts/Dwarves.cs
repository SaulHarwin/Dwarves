using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dwarves : MonoBehaviour
{

    public int DwarfCount;

    void Start()
    {
        for (int d = 0; d < DwarfCount; d++)
        {
            Dwarf dwarf = new Dwarf();

            dwarf.ID = d;

            dwarf.GO = new GameObject();
            dwarf.GO.AddComponent<SpriteRenderer>();
            dwarf.GO.transform.localScale = new Vector2(3.5f, 3.5f);
            
        }
    }

    void Update()
    {
        
    }
}

public class Dwarf
{
    public int ID;
    public GameObject GO;
}
