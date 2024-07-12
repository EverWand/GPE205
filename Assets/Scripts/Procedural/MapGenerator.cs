using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] roomTiles;
    public int rows;
    public int columns;
    public float roomWidth = 50f;
    public float roomHeight = 50f;
    private Room[,] grid;

    public enum SeedTypes { Preset, Random, Daily, Time}
    public SeedTypes seedMode;
    public int seed;

    public GameObject randomRoomPrefab() {
        return roomTiles[UnityEngine.Random.Range(0, roomTiles.Length)];
    }

    //Get the day and convert it into an integer
    public int DateToInt(DateTime date) {
        return date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond;
    }

    public void GenerateMap()
    {
        //Set the Seed based on given Seed Type:
        switch (seedMode)
        {
            case SeedTypes.Preset:
                setSeed(seed);
                break;
            case SeedTypes.Random:
                setSeed(UnityEngine.Random.Range(0, 9999));
                break;
            case SeedTypes.Daily:
                setSeed(DateToInt(DateTime.Now.Date));
                break;
            case SeedTypes.Time:
                setSeed(DateToInt(DateTime.Now));
                break;
        }

        grid = new Room[columns, rows];

        //ROW
        for(int currRow = 0; currRow < rows; currRow++)
        {
            //COLUMN
            for (int currCol = 0; currCol < columns; currCol++)
            {
                float xPos = roomWidth * currCol;
                float zPos = roomHeight * currRow;

                Vector3 newPos = new Vector3(xPos, 0.0f, zPos);

                GameObject tempRoomObj = Instantiate(randomRoomPrefab(), newPos, Quaternion.identity) as GameObject;

                tempRoomObj.transform.parent = this.transform;
                tempRoomObj.name = "Room" + currCol + ", " + currRow;

                Room tempRoom = tempRoomObj.GetComponent<Room>();

                //OPENING DOORS
                //--Vertical Facing
                if (currRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currRow == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else { 
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }

                //--Horizontal Facing
                if (currCol == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (currCol == columns - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    tempRoom.doorWest.SetActive(false);
                    tempRoom.doorEast.SetActive(false);
                }


                grid[currCol, currRow] = tempRoom;
            }
        }
    }

    private void setSeed(int newSeed)
    {
        UnityEngine.Random.InitState(newSeed);
        seed = newSeed;
    }
}
