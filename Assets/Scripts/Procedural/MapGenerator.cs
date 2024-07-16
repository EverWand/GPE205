using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //MAP VARIABLES
    //---Map Size
    public int rows;
    public int columns;
    //---Room Size
    public float roomWidth = 50f;
    public float roomHeight = 50f;

    //SEED
    public enum SeedTypes { Preset, Random, Daily, Time }
    public SeedTypes seedMode;
    public int seed;

    //MAP OBJECTS
    //---Map Grid
    public GameObject[] roomTiles;  //The types of Map Tiles the Map can generate
    private Room[,] grid;           //Matrix of the rooms

    //---Map Content
    //------AI
    public float enemyDensity = .2f;    //The Percentage of how many spawners produce AI
    public List<AIController> listOfAIBehaviors = new();  //The Types of Behavior the Map can spawn
    public List<AIController> RequiredAI = new();        //The type of Behaviors the Map Has to spawn
    //------Pickups
    public float pickupDensity = .2f;

    //SCHEDULES
    private void Awake()
    {
        enemyDensity = Mathf.Clamp01(enemyDensity);     //keep the AI density between 0% - 100%
        pickupDensity = Mathf.Clamp01(pickupDensity);   //keep the Pickup density between 0% - 100%
    }

    //Returns a random room tile
    public GameObject RandomRoomPrefab()
    {
        return roomTiles[UnityEngine.Random.Range(0, roomTiles.Length)];
    }

    //Get the day and convert it into an integer
    public int DateToInt(DateTime date)
    {
        return date.Year + date.Month + date.Day + date.Hour + date.Minute + date.Second + date.Millisecond;
    }

    //Generates the Map
    public void GenerateMap()
    {
        //Set the Seed based on given Seed Type:
        switch (seedMode)
        {
            //PRESET SEED
            case SeedTypes.Preset:
                SetSeed(seed);
                break;
            //RANDOM SEED
            case SeedTypes.Random:
                SetSeed(UnityEngine.Random.Range(0, 9999));
                break;
            //THE DAILY SEED
            case SeedTypes.Daily:
                SetSeed(DateToInt(DateTime.Now.Date));
                break;
            //SEED BASED ON THE TIME
            case SeedTypes.Time:
                SetSeed(DateToInt(DateTime.Now));
                break;
        }

        grid = new Room[columns, rows];

        //ROW
        for (int currRow = 0; currRow < rows; currRow++)
        {
            //COLUMN
            for (int currCol = 0; currCol < columns; currCol++)
            {
                //Sets the offsets of the rooms for placement
                float xPos = roomWidth * currCol;
                float zPos = roomHeight * currRow;

                //Gets the position based on the offset
                Vector3 newPos = new(xPos, 0.0f, zPos);

                //Spawn a Random Room Prefab
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPos, Quaternion.identity) as GameObject;

                //
                tempRoomObj.transform.parent = this.transform;
                tempRoomObj.name = "Room" + currCol + ", " + currRow;

                //Get the Room Component
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
                else
                {
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


                grid[currCol, currRow] = tempRoom; // add the Room Prefab to the map grid
            }
        }
    }

    //sets the seed for the Map generator
    private void SetSeed(int newSeed)
    {
        UnityEngine.Random.InitState(newSeed);  //Form the seed based on the ID given
        seed = newSeed;                         //set that as the seed
    }
}
