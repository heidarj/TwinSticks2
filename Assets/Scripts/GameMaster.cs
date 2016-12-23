using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public GameObject TilesToSpawn;
    public int amountOfRooms = 4;
    public int AmountOfTiles = 25;
    public GameObject Wall;

    List<GameObject> Roots;
    List<Vector3> TileLocations;
    List<GameObject> AllTiles;
    Vector3[] Directions = new Vector3[8];

    // Use this for initialization
    void Start () {
        for (int i = 0; i < 8; i++)
        {
            Directions[0] = new Vector3(0, 0, 1);
            Directions[1] = new Vector3(0, 0, -1);
            Directions[2] = new Vector3(1, 0, 0);
            Directions[3] = new Vector3(-1, 0, 0);
            Directions[4] = new Vector3(1, 0, 1);
            Directions[5] = new Vector3(-1, 0, 1);
            Directions[6] = new Vector3(1, 0, -1);
            Directions[7] = new Vector3(-1, 0, -1);
        }
        TileLocations = new List<Vector3>();
        AllTiles = new List<GameObject>();
        Roots = new List<GameObject>();
        for(int i = 0; i < amountOfRooms; i++)
        {
            Vector3 rootPos = new Vector3(Random.Range(-40, 40), 0, Random.Range(-40, 40));
            GameObject root = Instantiate(TilesToSpawn, rootPos, Quaternion.identity);
            Roots.Add(root);
        }
        foreach(GameObject root in Roots)
        {
            TileLocations.Add(root.transform.position);
            SpawnFloor(root);
            CleanupFloor();
            SpawnWalls();
        }

        ConnectRoots();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SpawnFloor(GameObject root)
    {
        GameObject[] tiles = new GameObject[AmountOfTiles];
        GameObject workingtile = root;
        for (int i = 0; i < AmountOfTiles; i++ )
        {
            GameObject NewTile = SpawnTile(root);
            tiles[i] = NewTile;
            workingtile = NewTile;
        }

        foreach(GameObject tile in tiles)
        {
            SpawnTile(root);
        }

    }

    void CleanupFloor()
    {
        foreach (GameObject tile in AllTiles)
        {
            Vector3 TileLocation = tile.transform.position;
            if (!TileLocations.Contains(TileLocation + Directions[0]) && !TileLocations.Contains(TileLocation + Directions[1]) && !TileLocations.Contains(TileLocation + Directions[2]) && !TileLocations.Contains(TileLocation + Directions[3]))
            {
                Destroy(tile);
                TileLocations.Remove(TileLocation);
            }
        }
    }

    GameObject SpawnTile(GameObject workingtile)
    {
        Vector3 NewTileLocation;
        NewTileLocation = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2)) + workingtile.transform.position;
        do { NewTileLocation = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2)) + NewTileLocation; }
        while (TileLocations.Contains(NewTileLocation));
        GameObject NewTile = Instantiate(TilesToSpawn, NewTileLocation, workingtile.transform.rotation);
        AllTiles.Add(NewTile);
        TileLocations.Add(NewTileLocation);
        return NewTile;
    }

    void SpawnWalls()
    {
        foreach(Vector3 TileLocation in TileLocations)
        {
            foreach (Vector3 dir in Directions)
            {
                if (!TileLocations.Contains(TileLocation + dir))
                {
                    Instantiate(Wall, TileLocation + dir, Quaternion.identity);
                }
            }
        }
    }

    void ConnectRoots()
    {
        OnDrawGizmos();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Roots[0].transform.position, Roots[1].transform.position);
        Gizmos.DrawLine(Roots[1].transform.position, Roots[2].transform.position);
        Gizmos.DrawLine(Roots[2].transform.position, Roots[3].transform.position);
        Gizmos.DrawLine(Roots[3].transform.position, Roots[4].transform.position);
        Gizmos.DrawLine(Roots[4].transform.position, Roots[5].transform.position);
    }

    bool isOccupied(Vector3 location)
    {
        Vector3 boxSize = new Vector3((float)0.5, (float)0.5, (float)0.5);
        var hitColliders = Physics.OverlapBox(location, boxSize);
        if (hitColliders.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
