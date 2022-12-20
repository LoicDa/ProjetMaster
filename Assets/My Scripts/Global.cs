using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Global : MonoBehaviour{

    public static Global Instance { get; set; }

    public GameObject Writing_playerController;
    public GameObject Wandering_playerController;

    public GameObject LeftController;
    public GameObject RightController;

    public WritingTarget currentTarget;

    private bool hasSelectedObj = false;

    private bool LeftHovering = false;
    private bool RightHovering = false;



    //VARIABLES FOR TESTING
    public bool usingPizzaText;
    public int nbTester = 0;

    private int fileCounter = 0;
    private List<float> timestampArray;
    private string inputArray;
    private List<float> timestampFnArray;
    private List<FnBall_enum> FnArray;
    private float TestStartTime;
    private string testObjective;
    private GameObject currentTBox;

    //BLANK, FOR READNG, THEN FOR WRITING
    public enum FnBall_enum { Null, ChangeTitle, ChangeText, Lock, Delete, Anchor, ExitRead, Copy, Cursors, LayoutChange, Save, ExitWrite };
    public enum WritingTarget { Title, Text };
    //public string paperclip;

    private void Awake()
    {
        timestampArray = new List<float>();
        timestampFnArray = new List<float>();
        FnArray = new List<FnBall_enum>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool isHovering()
    {
        if (!LeftHovering && !RightHovering)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void objectSelected(SelectEnterEventArgs args)
    {
        if (!hasSelectedObj)
        {
            hasSelectedObj = true;

            var selectedObj = args.interactableObject.transform.gameObject;
            string objName = selectedObj.name;
            //GET WHICH MODE IS ACTIVE TO KNOW WHICH PLAYER_CONTROLLER IS ACTIVE
            if(objName.Contains("FnBall"))
            {
                GameObject textBox = selectedObj.GetComponent<FnBall_Script>().ParentTextBox;

                TextBoxMode mode = textBox.GetComponent<TextBoxController>().GetMode();

                selectedObj.gameObject.GetComponent<FnBall_Script>().hasCollided(); //change FnBall color

                //IF WRITE MODE -> SPEAK TO PC_WRITING
                if (mode == TextBoxMode.Write)
                {
                    Writing_playerController.GetComponent<PlayerController_writing>().collisionDetected(selectedObj.gameObject.GetComponent<FnBall_Script>().function, selectedObj.GetComponent<FnBall_Script>().ParentTextBox); //tell writing controller what Fn is the ball
                    saveFnInput(selectedObj.gameObject.GetComponent<FnBall_Script>().function);
                }

                //IF READ MODE -> SPEAK TO PC_WANDERING
                if(mode == TextBoxMode.Read)
                {
                    Wandering_playerController.GetComponent<PlayerController_wandering>().FnBall_touched(selectedObj.gameObject.GetComponent<FnBall_Script>().function, selectedObj.GetComponent<FnBall_Script>().ParentTextBox); //tell wandering controller what Fn is the ball
                }
            }


        }
    }

    public void objectExited(SelectExitEventArgs args)
    {
        hasSelectedObj = false;
    }

    //NEED TO BLOCK OTHER INPUTS WHEN HOVERING ON A UI ELEMENT (SLIDER, FNBALL) TO AVOID MISTAKES (ADD TEXTZONE)

    public void hoverEntered(HoverEnterEventArgs args)
    {
        if (args.interactorObject.transform.gameObject.Equals(LeftController))
        {
            LeftHovering = true;
        }
        if (args.interactorObject.transform.gameObject.Equals(RightController))
        {
            RightHovering = true;
        }
        
        
    }

    public void hoverExited(HoverExitEventArgs args)
    {
        if (args.interactorObject.transform.gameObject.Equals(LeftController))
        {
            LeftHovering = false;
        }
        if (args.interactorObject.transform.gameObject.Equals(RightController))
        {
            RightHovering = false;
        }
        
        
    }




    //METHOD TO SAVE INPUTS + TIMES IN FILES
    public void StartExercise(string objective, GameObject TBox)
    {
        currentTBox = TBox;
        testObjective = objective;

        timestampArray.Clear();
        inputArray = "";

        //timestampFnArray.Clear();
        FnArray.Clear();

        TestStartTime = Time.fixedTime;//time in seconds since the launch of the program
        //PUT FIRST CHAR TO VISUALISE START TIME
        timestampArray.Add(0f);
        inputArray = "¤";
    }

    public void saveInput(char newInput)
    {
        //GET INPUT AND TIMESTAMP IN AN ARRAY
        timestampArray.Add(Time.fixedTime-TestStartTime); //time since that test started
        inputArray += newInput;
    }

    public void saveFnInput(FnBall_enum fn) {
        timestampFnArray.Add(Time.fixedTime - TestStartTime);
        FnArray.Add(fn);
    }

    public void save(string finalText, string finalState)
    {

        //PUT LAST CHAR TO VISUALISE END TIME
        timestampArray.Add(Time.fixedTime - TestStartTime);
        inputArray += "¤";

        float tpsTotal = timestampArray[timestampArray.Count-1];

        int nbClic = (inputArray.Length - 2); //NB OF LETTERS ENTERED

        int nbError = inputArray.Split('<').Length - 1; // < signify a char was deleted, 

        int nbFnUse = FnArray.Count;


        //file name is date_USERnbTester_TextBoxNumber_fileCounter.csv
        string fileName = DateTime.Today.ToString("dd")+ DateTime.Today.ToString("MM") + "_USER" + nbTester + "_"+ currentTBox.name + "_" + fileCounter;

        string resultatTxt = "Total Test Duration: " + tpsTotal + "\nNb clics: " + nbClic + "\nAverage Speed (Letter/sec): " + tpsTotal / nbClic + "\nNb errors: " + nbError + "\nNb function used: " + nbFnUse + "\nObjective: " + testObjective + "\nResult: " + finalText + "\nComplete input: " + inputArray.Trim('¤') + "\nFinal State: "+ finalState + "\n\n\n";
        
        string resultatCSV = "";
        Debug.Log(inputArray);
        for (int i_i = 0; i_i < inputArray.Length; i_i++)
        {
            resultatCSV += timestampArray[i_i] + ";";
            resultatCSV += inputArray[i_i] + "\n";
        }
        resultatCSV += "\n\n";
        for (int i_i = 0; i_i < FnArray.Count-1; i_i++)
        {
            resultatCSV += timestampFnArray[i_i] + ";";
            resultatCSV += FnArray[i_i] + "\n";
        }

        File.AppendAllText(@"c:\Users\loicd\Desktop\" + fileName + ".txt", resultatTxt);
        File.AppendAllText(@"c:\Users\loicd\Desktop\" + fileName + ".csv", resultatCSV);


        fileCounter ++;
    }
}
