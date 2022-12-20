using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class RoundBoard : MonoBehaviour
{
    // PUBLIC VARIABLES
    public PlayerController_writing playerController;

    public GameObject leftCercle;
    public GameObject rightCercle;

    public GameObject leftCursor;
    public GameObject rightCursor;

    // PRIVATE VARIABLES

    Material normalMat;
    Material clickedMat;

    //  FOR INPUT METHOD
    float countdown;
    float thetaMax0 = 0.26f;
    float thetaMax1 = 0.6f;
    public float trackMultiplier = 0.05f;



    //      LEFT
    char[] cercle2_gauche = new char[10] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
    char[] cercle1_gauche = new char[6] { 'K', 'L', 'M', 'N', 'O', 'P' };
    char[] cercle0_gauche = new char[1] { 'Q' };

    float cercle2_gauche_slice;
    float[] cercle2_gauche_angle;
    float cercle1_gauche_slice;
    float[] cercle1_gauche_angle;
    float cercle0_gauche_slice;
    float[] cercle0_gauche_angle;

    float CG_Precedent;
    float TGMax_Precedent;
    float TGMin_Precedent;
    float IG_Precedent;
    float IGS_Precedent;
    char LG_Precedent;


    //      RIGHT
    char[] cercle2_droit = new char[10] { 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '.' };
    char[] cercle1_droit = new char[6] { '<', ' ', '\'', '-', ',', ' ' };
    char[] cercle0_droit = new char[1] { '>' };

    float cercle2_droit_slice;
    float[] cercle2_droit_angle;
    float cercle1_droit_slice;
    float[] cercle1_droit_angle;
    float cercle0_droit_slice;
    float[] cercle0_droit_angle;

    float CD_Precedent;
    float TDMax_Precedent;
    float TDMin_Precedent;
    float ID_Precedent;
    float IDS_Precedent;
    char LD_Precedent;

    //FUNCTIONS TO CALCULATE INPUT
    float[] calculAngle(char[] cercle, float slice)
    {
        float[] resultat = new float[cercle.Length];

        for (int i_i = 0; i_i < cercle.Length; i_i++)
        {
            resultat[i_i] = -180.0f + slice * (i_i + 1);
        }
        return resultat;
    }
    Vector2 cartToPolar(Vector2 cartCoord)
    {
        float r = (float)Math.Sqrt(cartCoord.y * cartCoord.y + cartCoord.x * cartCoord.x);
        float theta = (float)Math.Atan2(cartCoord.y, cartCoord.x) * 180.0f / 3.14f;
        return new Vector2(r, theta);
    }
    Vector2 polarToCart(Vector2 polarCoord)
    {
        float x = polarCoord.x * (float)Math.Cos(polarCoord.y);
        float y = polarCoord.x * (float)Math.Sin(polarCoord.y);
        return new Vector2(x, y);
    }

    //PUBLIC METHODS

    public void Activate()
    {
        gameObject.SetActive(true);
        leftCercle.SetActive(true);
        rightCercle.SetActive(true);
    }

    public void Deactivate()
    {
        leftCercle.SetActive(false);
        rightCercle.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ResetCountdown()
    {
        countdown = 0.3f;
    }

    // Start is called before the first frame update
    void Start()
    {
        //GET MATERIALS
        normalMat = Resources.Load("transparentBlue", typeof(Material)) as Material;
        clickedMat = Resources.Load("ClickedMaterial", typeof(Material)) as Material;

        //CALCULATE EACH CIRCLE ANGLE INTERVAL
        cercle2_gauche_slice = 360.0f / cercle2_gauche.Length;
        cercle2_gauche_angle = calculAngle(cercle2_gauche, cercle2_gauche_slice);
        cercle1_gauche_slice = 360.0f / cercle1_gauche.Length;
        cercle1_gauche_angle = calculAngle(cercle1_gauche, cercle1_gauche_slice);
        cercle0_gauche_slice = 360.0f / cercle0_gauche.Length;
        cercle0_gauche_angle = calculAngle(cercle0_gauche, cercle0_gauche_slice);


        cercle2_droit_slice = 360.0f / cercle2_droit.Length;
        cercle2_droit_angle = calculAngle(cercle2_droit, cercle2_droit_slice);
        cercle1_droit_slice = 360.0f / cercle1_droit.Length;
        cercle1_droit_angle = calculAngle(cercle1_droit, cercle1_droit_slice);
        cercle0_droit_slice = 360.0f / cercle0_droit.Length;
        cercle0_droit_angle = calculAngle(cercle0_droit, cercle0_droit_slice);



        LG_Precedent = ' ';
        LD_Precedent = ' ';
    }

    // Update is called once per frame
    void Update()
    {
        //      TIMERS
        countdown -= Time.deltaTime;
        if (countdown < 0f && countdown > -0.5f)
        {
            countdown = -1f;
            leftCursor.GetComponent<Renderer>().material = normalMat;
            rightCursor.GetComponent<Renderer>().material = normalMat;
        }


        //      INPUTS

        //      LEFT CONTROLLER VISUAL MOVEMENT
        Vector2 input_gauche;
        input_gauche.x = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal");
        input_gauche.y = Input.GetAxis("XRI_Left_Primary2DAxis_Vertical");

        Vector2 leftCursorMovement = new Vector2(input_gauche.x, input_gauche.y);
        leftCursor.transform.localPosition = new Vector3(
                leftCursorMovement.x * -5f,
                0.1f,
                leftCursorMovement.y * 5f
            );

        //LEFT CONTROLLER INPUT CALCULATION
        input_gauche = cartToPolar(input_gauche);

        char lettreChoisie_G = '¤';
        bool memeCase_G = false;
        if (LG_Precedent != ' ')
        {

            if (input_gauche.x <= TGMax_Precedent + CG_Precedent && input_gauche.x >= TGMin_Precedent - CG_Precedent) //compare si est proche de theta precedent
            {
                if ((input_gauche.y > (IG_Precedent - IGS_Precedent - trackMultiplier * 50)) && (input_gauche.y < IG_Precedent + trackMultiplier * 50))
                {
                    memeCase_G = true;
                    lettreChoisie_G = LG_Precedent;
                }
            }
        }

        //  
        if (!memeCase_G)
        {
            LG_Precedent = ' ';
            if (input_gauche.x <= thetaMax0)
            {
                TGMax_Precedent = thetaMax0;
                TGMin_Precedent = 0f;
                CG_Precedent = trackMultiplier;
                IGS_Precedent = cercle0_gauche_slice;
                for (int i_i = 0; i_i < cercle0_gauche.Length; i_i++)
                    if (input_gauche.y < cercle0_gauche_angle[i_i])
                    {
                        IG_Precedent = cercle0_gauche_angle[i_i];
                        lettreChoisie_G = cercle0_gauche[i_i];
                        break;
                    }
            }
            else if (input_gauche.x < thetaMax1)
            {
                TGMax_Precedent = thetaMax1;
                TGMin_Precedent = thetaMax0;
                CG_Precedent = -1 * trackMultiplier;
                IGS_Precedent = cercle1_gauche_slice;
                for (int i_i = 0; i_i < cercle1_gauche.Length; i_i++)
                    if (input_gauche.y < cercle1_gauche_angle[i_i])
                    {
                        IG_Precedent = cercle1_gauche_angle[i_i];
                        lettreChoisie_G = cercle1_gauche[i_i];
                        break;
                    }
            }
            else
            {
                TGMax_Precedent = 1f;
                TGMin_Precedent = thetaMax1;
                CG_Precedent = trackMultiplier;
                IGS_Precedent = cercle2_gauche_slice;
                for (int i_i = 0; i_i < cercle2_gauche.Length; i_i++)
                    if (input_gauche.y < cercle2_gauche_angle[i_i])
                    {
                        IG_Precedent = cercle2_gauche_angle[i_i];
                        lettreChoisie_G = cercle2_gauche[i_i];
                        break;
                    }
            }
        }


        //      RIGHT CONTROLLER VISUAL MOVEMENT
        Vector2 input_droit;
        input_droit.x = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal");
        input_droit.y = Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");

        Vector2 rightCursorMovement = new Vector2(input_droit.x, input_droit.y);
        rightCursor.transform.localPosition = new Vector3(
                rightCursorMovement.x * -5f,
                0.1f,
                rightCursorMovement.y * 5f
            );

        //LEFT CONTROLLER INPUT CALCULATION
        input_droit = cartToPolar(input_droit);
        char lettreChoisie_D = '¤';

        bool memeCase_D = false;
        if (LD_Precedent != ' ')
        {

            if (input_droit.x <= TDMax_Precedent + CD_Precedent && input_droit.x >= TDMin_Precedent - CD_Precedent) //compare si est proche de theta precedent
            {

                if ((input_droit.y > (ID_Precedent - IDS_Precedent - trackMultiplier * 50)) && (input_droit.y < ID_Precedent + trackMultiplier * 50))
                {
                    memeCase_D = true;
                    lettreChoisie_D = LD_Precedent;
                }
            }
        }



        if (!memeCase_D)
        {
            LD_Precedent = ' ';
            if (input_droit.x <= thetaMax0)
            {
                TDMax_Precedent = thetaMax0;
                TDMin_Precedent = 0f;
                CD_Precedent = trackMultiplier;
                IDS_Precedent = cercle0_droit_slice;
                for (int i_i = 0; i_i < cercle0_droit.Length; i_i++)
                    if (input_droit.y < cercle0_droit_angle[i_i])
                    {
                        ID_Precedent = cercle0_droit_angle[i_i];
                        lettreChoisie_D = cercle0_droit[i_i];
                        break;
                    }
            }


            else if (input_droit.x < thetaMax1)
            {
                TGMax_Precedent = thetaMax1;
                TGMin_Precedent = thetaMax0;
                CD_Precedent = -1 * trackMultiplier;
                IDS_Precedent = cercle1_droit_slice;
                for (int i_i = 0; i_i < cercle1_droit.Length; i_i++)
                    if (input_droit.y < cercle1_droit_angle[i_i])
                    {
                        ID_Precedent = cercle1_droit_angle[i_i];
                        lettreChoisie_D = cercle1_droit[i_i];
                        break;
                    }
            }
            else
            {
                TDMax_Precedent = 1f;
                TDMin_Precedent = thetaMax1;
                CD_Precedent = trackMultiplier;
                IDS_Precedent = cercle2_droit_slice;
                for (int i_i = 0; i_i < cercle2_droit.Length; i_i++)
                    if (input_droit.y < cercle2_droit_angle[i_i])
                    {
                        ID_Precedent = cercle2_droit_angle[i_i];
                        lettreChoisie_D = cercle2_droit[i_i];
                        break;
                    }
            }
        }







        
        if(countdown <= 0f)
        {
            // IF CLICKED -> SEND INPUT

            if ((Input.GetAxis("XRI_Left_Primary2DAxisClick") != 0)&&(lettreChoisie_G != '¤')) //it's possible to push the button without having chosen a char, so block default choice
            {
                playerController.AddChar(lettreChoisie_G);
                LG_Precedent = lettreChoisie_G;
                countdown = 0.3f;
                leftCursor.GetComponent<Renderer>().material = clickedMat;

            }


            if ((Input.GetAxis("XRI_Right_Primary2DAxisClick") != 0) && (lettreChoisie_D != '¤'))
            {
                playerController.AddChar(lettreChoisie_D);
                LD_Precedent = lettreChoisie_D;
                countdown = 0.3f;
                rightCursor.GetComponent<Renderer>().material = clickedMat;

            }
        }
        

    }
}
