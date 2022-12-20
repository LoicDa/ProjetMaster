using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.XR;
using System.IO;



public class test_controller : MonoBehaviour
{
    public Text affichText;

    /*
    public Text leftText;
    public Text rightText;
    public GameObject leftCylinder;
    public GameObject rightCylinder;
    public GameObject leftSphere;
    public GameObject rightSphere;
    public GameObject XRControllerLeft;
    public GameObject XRControllerRight;
    */

    public GameObject leftCursor;
    public GameObject rightCursor;

    string saisie;
    float countdown;

    public Material normalMat;
    public Material clickedMat;


    char[] ligne1 = new char[4] { 'A', 'B', 'C', 'D' };
    char[] ligne2 = new char[4] { 'E', 'F', 'G', 'H' };
    char[] ligne3 = new char[4] { 'I', 'J', 'K', 'L' };
    char[] ligne4 = new char[4] { 'M', 'N', 'O', 'P' };

    char[] ligne1D = new char[4] { 'Q', 'R', 'S', 'T' };
    char[] ligne2D = new char[4] { 'U', 'V', 'W', 'X' };
    char[] ligne3D = new char[4] { 'Y', 'Z', ' ', ',' };
    char[] ligne4D = new char[4] { '.', '-', '-', '-' };


    float yLeftPast = 0;
    float xLeftPast = 0;

    float yRightPast = 0;
    float xRightPast = 0;


    // ---- variables de test pour stats ---- \\
    public bool trackPreviousPos = false;
    public bool testing = true;
    bool hasStarted = false;
    float tpsTotal = 0;
    int nbClic = 0;
    int nbErreur = 0;
    public string objectif = "BIENVENUE DANS LA VR.";
    string saisieTotale = "";
    // ---- \\

    void Start()
    {
        /*
        leftText = GameObject.Find("leftText").GetComponent<Text>();
        rightText = GameObject.Find("rightText").GetComponent<Text>();

        leftSphere = GameObject.Find("leftCircle");
        rightSphere = GameObject.Find("rightCircle");
        leftCylinder = GameObject.Find("leftCylinder");
        rightCylinder = GameObject.Find("rightCylinder");

        XRControllerLeft = GameObject.Find("XRControllerLeft");
        XRControllerRight = GameObject.Find("XRControllerRight");
        */
        affichText = GameObject.Find("Values").GetComponent<Text>();

        leftCursor = GameObject.Find("leftCursor");
        rightCursor = GameObject.Find("rightCursor");

        normalMat = Resources.Load("transparentBlue", typeof(Material)) as Material;
        clickedMat = Resources.Load("ClickedMaterial", typeof(Material)) as Material;
        

        saisie = "";
        countdown = 0.2f;
    }

    void Update()
    {

        countdown -= Time.deltaTime;
        if (countdown < 0f && countdown > -0.5f)
        {
            countdown = -1f;
            leftCursor.GetComponent<Renderer>().material = normalMat;
            rightCursor.GetComponent<Renderer>().material = normalMat;
        }

        if (hasStarted)
        {
            tpsTotal += Time.deltaTime;
        }

        //affichText.text = "H : " + Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal") + " \nV : " + Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");

        float yLeft = 0;
        float xLeft = 0;
        if (trackPreviousPos)
        {
            yLeft = (Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal") + yLeftPast*2) / 3;
            xLeft = (Input.GetAxis("XRI_Left_Primary2DAxis_Vertical") + xLeftPast*2) / 3;


            yLeftPast = yLeft;
            xLeftPast = xLeft;
        }
        else
        {
            yLeft = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal");
            xLeft = Input.GetAxis("XRI_Left_Primary2DAxis_Vertical");
        }


        //leftSphere.transform.position = leftCylinder.transform.position + new Vector3(-0f + xLeft * 0.33f, -0f - yLeft * 0.33f, 0f);
        //leftCursor.transform.position = new Vector3(-4f + yLeft, 2f - xLeft, 6.5f); //nul
        float leftCursorX = -100;
        float leftCursorY = -100;
        //leftCylinder.transform.rotation = leftSphere.transform.rotation;
        char lettreChoisie = '-';
        if ((yLeft == 0) && (xLeft == 0))
        {
            affichText.text = "Aucune lettre sélectionnée";
        }
        else
        {
            char[] ligneChoisie = new char[4];
            if (xLeft > 0.5f)
            {
                leftCursorX = 0.85f-2f;
                ligneChoisie = ligne4;
            }
            else if (xLeft > 0f)
            {
                leftCursorX = 1.6f - 2f;
                ligneChoisie = ligne3;
            }
            else if (xLeft > -0.5f)
            {
                leftCursorX = 2.5f - 2f;
                ligneChoisie = ligne2;
            }
            else if (xLeft >= -1f)
            {
                leftCursorX = 3.5f - 2f;
                ligneChoisie = ligne1;
            }

            if (yLeft > 0.5f)
            {
                leftCursorY = -2.9f+4f;
                lettreChoisie = ligneChoisie[3];
            }
            else if (yLeft > 0f)
            {
                leftCursorY = -3.6f + 4f;
                lettreChoisie = ligneChoisie[2];
            }
            else if (yLeft > -0.5f)
            {
                leftCursorY = -4.5f + 4f;
                lettreChoisie = ligneChoisie[1];
            }
            else if (yLeft >= -1f)
            {
                leftCursorY = -5.2f + 4f;
                lettreChoisie = ligneChoisie[0];
            }

            leftCursor.transform.position = new Vector3(-3 + leftCursorY, 3 + leftCursorX, 9f);
        }
        
        

        //Debug.Log("Click " + Input.GetAxis("XRI_Left_Primary2DAxisClick"));



        /*
        if (Input.GetAxis("XRI_Left_Grip") == 1)
        {
            //texte += "\nGrip";
        }
        if (Input.GetAxis("XRI_Left_SecondaryButton") == 1)
        {
            //texte += "\nJoystick";
        }
        if (Input.GetAxis("XRI_Left_MenuButton") == 1)
        {
            //texte += "\nMenu";
        }
        //leftText.text = texte;
        */



        //rightSphere.transform.position = rightCylinder.transform.position + new Vector3(0f + Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal")*0.33f, 0f - Input.GetAxis("XRI_Right_Primary2DAxis_Vertical")*0.33f, 0f);

        //rightCylinder.transform.rotation = rightSphere.transform.rotation;

        float yLeftD = 0;
        float xLeftD = 0;

        if (trackPreviousPos)
        {
            yLeftD = (Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal") + yRightPast*2) / 3;
            xLeftD = (Input.GetAxis("XRI_Right_Primary2DAxis_Vertical") + xRightPast*2) / 3;

            yRightPast = yLeftD;
            xRightPast = xLeftD;
        }
        else
        {
            yLeftD = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal");
            xLeftD = Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");
        }
        


        //rightSphere.transform.position = rightCylinder.transform.position + new Vector3(xLeftD, yLeftD, 0f);
        //rightCursor.transform.position = new Vector3(4f + xLeftD, 2f - yLeftD, 6.5f);

        float rightCursorX = -100;
        float rightCursorY = -100;


        char lettreChoisieD = '-';
        if ((yLeftD == 0) && (xLeftD == 0))
        {
            affichText.text = "Aucune lettre sélectionnée";
        }
        else
        {
            char[] ligneChoisie = new char[4];
            if (xLeftD > 0.5f)
            {
                rightCursorX = 0.85f-2f;
                ligneChoisie = ligne4D;
            }
            else if (xLeftD > 0f)
            {
                rightCursorX = 1.6f-2f;
                ligneChoisie = ligne3D;
            }
            else if (xLeftD > -0.5f)
            {
                rightCursorX = 2.5f-2f;
                ligneChoisie = ligne2D;
            }
            else if (xLeftD >= -1f)
            {
                rightCursorX = 3.5f - 2f;
                ligneChoisie = ligne1D;
            }

            if (yLeftD > 0.5f)
            {
                rightCursorY = 5.2f-4f;
                lettreChoisieD = ligneChoisie[3];
            }
            else if (yLeftD > 0f)
            {
                rightCursorY = 4.5f - 4f;
                lettreChoisieD = ligneChoisie[2];
            }
            else if (yLeftD > -0.5f)
            {
                rightCursorY = 3.6f - 4f;
                lettreChoisieD = ligneChoisie[1];
            }
            else if (yLeftD >= -1f)
            {
                rightCursorY = 2.9f - 4f;
                lettreChoisieD = ligneChoisie[0];
            }

            rightCursor.transform.position = new Vector3(3+rightCursorY, 3+rightCursorX, 9f);
        }




        //Debug.Log("Click " + Input.GetAxis("XRI_Left_Primary2DAxisClick"));

        if (countdown < 0f) //si le décompte est écoulé
        {
            if (Input.GetAxis("XRI_Left_Trigger") == 1)
            {
                //texte += "Trigger";
                if (saisie.Length >= 1)
                    saisie = saisie.Substring(0, saisie.Length - 1);
                countdown = 0.5f;
                nbErreur++;
                saisieTotale += '<';
            }
            if (Input.GetAxis("XRI_Left_Primary2DAxisClick") != 0)
            {
                //texte += "\nTouchpad";
                saisieTotale += lettreChoisie;
                saisie += lettreChoisie;
                countdown = 0.5f;
                leftCursor.GetComponent<Renderer>().material = clickedMat;
                hasStarted = true;
                nbClic++;
            }
        }


        if (countdown < 0f) //si le décompte est écoulé
        {
            if (Input.GetAxis("XRI_Right_Trigger") == 1)
            {
                //texte += "Trigger";
                if (saisie.Length >= 1)
                    saisie = saisie.Substring(0, saisie.Length - 1);
                countdown = 0.5f;
                nbErreur++;
                saisieTotale += '<';
            }
            if (Input.GetAxis("XRI_Right_Primary2DAxisClick") != 0)
            {
                //texte += "\nTouchpad";
                saisieTotale += lettreChoisieD;
                saisie += lettreChoisieD;
                countdown = 0.5f;
                rightCursor.GetComponent<Renderer>().material = clickedMat;
                hasStarted = true;
                nbClic++;
                if (lettreChoisieD == '.' && testing)
                {
                    string resultat = "Temps total : " + tpsTotal + "\nNb clics : " + nbClic + "\nTemps par clics : " + tpsTotal / nbClic + "\nNb erreurs : " + nbErreur + "\nObjectif : " + objectif + "\nRésultat : " + saisie + "\nSaisie totale : " + saisieTotale + "\nTracking pos précédente : " + trackPreviousPos + "\n\n\n";     
                    File.AppendAllText(@"c:\Users\loicd\Desktop\tests Grille carrée.txt", resultat);
                    
                }
            }
        }
        /*
        texte = "";
        if (Input.GetAxis("XRI_Right_Trigger") == 1)
        {
            texte += "Trigger";
        }
        if (Input.GetAxis("XRI_Right_Grip") == 1)
        {
            texte += "\nGrip";
        }
        if (Input.GetAxis("XRI_Right_Primary2DAxisClick") != 0)
        {
            texte += "\nTouchpad";
        }
        if (Input.GetAxis("XRI_Right_SecondaryButton") == 1)
        {
            texte += "\nJoystick";
        }
        if (Input.GetAxis("XRI_Right_MenuButton") == 1)
        {
            texte += "\nMenu";
        }
        rightText.text = texte;
        */
        
        
        
        //leftText.text = lettreChoisie.ToString();
        //rightText.text = lettreChoisieD.ToString();
        //affichText.text = "Lettre G : " + lettreChoisie.ToString() + "\n Lettre D : " + lettreChoisieD.ToString() + "\n Saisie: " + saisie;
        affichText.text = "Essayez d'écrire : \n" + objectif + "\n\n Saisie: \n" + saisie;

    }
}