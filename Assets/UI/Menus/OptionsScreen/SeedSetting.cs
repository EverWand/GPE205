using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class SeedSetting : MonoBehaviour
{
    //get the components needed to get seed settings
    public TMP_Dropdown seedOption;
    public TMP_InputField seedInput;

    private void Start()
    {

        
        if (seedInput != null)
        {
            seedInput.text = "0";
        }
    }

    public void SubmitSeedSetting() {

        //Get reference to Game Managers' Map Generator
        MapGenerator map = GameManager.instance.mapGenerator;
        MapGenerator.SeedTypes seedType = MapGenerator.SeedTypes.Preset;
        
        //Prolly bad practice but at least this works : Set the Seed type based on Dropdown Value
        switch (seedOption.value) {
            
            case 0:     //Preset
                seedType = MapGenerator.SeedTypes.Preset;

                break;
            case 1:     //Random
                seedType = MapGenerator.SeedTypes.Random;

                break;
            case 2:     //Daily
                seedType = MapGenerator.SeedTypes.Daily;

                break;
            case 3:     //Time
                seedType = MapGenerator.SeedTypes.Time;

                break;
            default:
                seedType = MapGenerator.SeedTypes.Preset;
                break;
        }

        
        map.seed = int.Parse(seedInput.text);   //Set the map generator's seed to the int value inside the text box | ( will auto change to other value if not set to preset )
        map.seedMode = seedType;                //Set the SeedType of the Map Generator
    }
}
