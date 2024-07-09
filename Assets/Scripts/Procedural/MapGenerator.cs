using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] roomTiles;
    public int rows;
    public int columns;
    public float roomWidth = 50f;
    public float roomHeight = 50f;
    private Room[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject randomRoomPrefab() {
        return roomTiles[Random.Range(0, roomTiles.Length)];
    }

    public void GenerateMap()
    {
        grid = new Room[columns, rows];

        //ROW
        for(int currRow = 0; currRow < rows; currRow++)
        {
            //COLUMN
            for(int currCol = 0;currCol < columns; currCol++) 
            {
                float xPos = roomWidth * currCol;
                float zPos = roomHeight * currRow;

                Vector3 newPos = new Vector3(xPos, 0.0f, zPos);

                GameObject tempRoomObj = Instantiate(randomRoomPrefab(), newPos, Quaternion.identity) as GameObject;
            
                tempRoomObj.transform.parent = this.transform;
                tempRoomObj.name = "Room" + currCol + ", " + currRow;

                Room tempRoom = tempRoomObj.GetComponent<Room>();

                grid[currCol, currRow] = tempRoom;
            }
        }
    }
}
