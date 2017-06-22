﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DS = DataSingleton;


//This script is the Generate (GenerateWall) script
//It generates the GenerateWall object.
public class GenerateGenerateWall : MonoBehaviour {
    public GameObject create; //This create is prefab of the create object.
	public Camera cam;	

	private GameObject currCreate; //current generate wall object that exists.

    private AudioClip audioClip;

    public void SetWaveSrc(string file)
    {
       
        string wavSrc = "Audio\\" + file;

        audioClip = Resources.Load(wavSrc) as AudioClip;
    }

    public AudioClip GetWaveSrc()
    {
        return audioClip;
    }

    public Text timer;
    

    public void ResetCreate()
    {
		Vector3 vec = cam.transform.position;

		vec.y = DS.GetData().CharacterData.Height;
		cam.transform.position = vec;
        Destroy(currCreate);
        currCreate = Instantiate(create);
    }


    //This is the delay value between key presses of changing number of walls in seconds.
    private const float delay = 0.1f;
    


    //This is the current running timestamp that is outputted to console.
    private float timestamp;

    

    // Use this for initialization
    void Start () {
		DS.Load ();
        create.transform.position = Vector3.zero;
        timestamp = 0;
        currCreate = Instantiate(create);
    }
	
	private bool begin = false;
	// Update is called once per frame
	void Update () {
        if (Time.time - timestamp >=  delay) //Checking to see if the current time is > than timestamp
        {

			if (DS.GetData().WallData.Sides <= DS.GetData().WallData.MaxNumWalls && begin) {
				

				Application.CaptureScreenshot ("Assets/OutputFiles/" + DS.GetData().OutputFolderName + "/Wall" + DS.GetData().WallData.Sides + ".png");



				DS.GetData().WallData.Sides += DS.GetData().WallData.WallStep;


				ResetCreate ();

			} else {
				begin = false;
			}


			if (Input.GetKey (KeyCode.H)) {
				System.IO.Directory.CreateDirectory ("Assets/OutputFiles/" + DS.GetData().OutputFolderName);

				DS.GetData().WallData.Sides = 4;
				begin = true;
			}


            //This is the input key for decreasing walls. This is button 1
            else if (Input.GetKey(KeyCode.Alpha1))
            {
                if (DS.GetData().WallData.Sides > DS.GetData().WallData.MinNumWalls)
                {
                    DS.GetData().WallData.Sides -= DS.GetData().WallData.WallStep;
                    ResetCreate();
                }
            }

            //This is the input key for increasing walls. This is button 2
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                if (DS.GetData().WallData.Sides < DS.GetData().WallData.MaxNumWalls)
                {
                    DS.GetData().WallData.Sides+= DS.GetData().WallData.WallStep;
                    ResetCreate();

                }
            }

            //This is the input key for increasing walls. This is button 3
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                NewZoneRandom();

            }

            //This is the input key for increasing walls. This is space key
            else if (Input.GetKey(KeyCode.Space))
            {
                ResetCreate();

            }

            //This is the input key for screenshots. For now, I'm going to set it to be the G key
            else if (Input.GetKey(KeyCode.G))
            {
                Application.CaptureScreenshot("Assets/OutputFiles/Screenshot/Screenshot.png");
            }


            //Increment the timestamp
            timestamp = Time.time;
        }
    }
    public void NewZoneRandom()
    {
        System.Random r = new System.Random();
        int diff = DS.GetData().WallData.MaxNumWalls - DS.GetData().WallData.MinNumWalls + 1;
        int val = r.Next(diff) + DS.GetData().WallData.MinNumWalls;
        DS.GetData().WallData.Sides = val;
        ResetCreate();
    }
}
