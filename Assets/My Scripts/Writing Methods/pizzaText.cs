using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using System.IO;

using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Events;

public class pizzaText : MonoBehaviour
{
    //PUBLIC VARIABLES

    public PlayerController_writing playerController;

    public GameObject leftCercle;
    public GameObject rightCercle;

    public Renderer rightCercleMaterial;

    public GameObject leftCursor;
    public GameObject rightCursor;


    Material normalMat;
    Material clickedMat;

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



    char[] slice_wuxv = new char[4] { 'W', 'U', 'X', 'V' };
    char[] slice_yz = new char[4] { ' ', 'Y', '<', 'Z' };
    char[] slice_cadb = new char[4] { 'C', 'A', 'D', 'B' };
    char[] slice_gehf = new char[4] { 'G', 'E', 'H', 'F' };
    char[] slice_kilj = new char[4] { 'K', 'I', 'L', 'J' };
    char[] slice_ompn = new char[4] { 'O', 'M', 'P', 'N' };
    char[] slice_sqtr = new char[4] { 'S', 'Q', 'T', 'R' };

    Texture[] textureArray = new Texture[7];

    float[] listeAngleSlice = new float[4];

    List<char[]> pizza;

    float[] listeAnglePizza = new float[7];

    //int currentSliceIndex;
    //int currentLetterIndex;

    float countdown;


    float[] calculAngle(int arrayLength, float slice)
    {
        float[] resultat = new float[arrayLength];

        for (int i_i = 0; i_i < arrayLength; i_i++)
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


    // Start is called before the first frame update
    void Start()
    {
        normalMat = Resources.Load("transparentBlue", typeof(Material)) as Material;
        clickedMat = Resources.Load("ClickedMaterial", typeof(Material)) as Material;

        textureArray[0] = Resources.Load("PizzaText_UVWX", typeof(Texture)) as Texture;
        textureArray[1] = Resources.Load("PizzaText_YZ", typeof(Texture)) as Texture;
        textureArray[2] = Resources.Load("PizzaText_ABCD", typeof(Texture)) as Texture;
        textureArray[3] = Resources.Load("PizzaText_EFGH", typeof(Texture)) as Texture;
        textureArray[4] = Resources.Load("PizzaText_IJKL", typeof(Texture)) as Texture;
        textureArray[5] = Resources.Load("PizzaText_MNOP", typeof(Texture)) as Texture;
        textureArray[6] = Resources.Load("PizzaText_QRST", typeof(Texture)) as Texture;

        pizza = new List<char[]>() { slice_wuxv , slice_yz , slice_cadb , slice_gehf , slice_kilj , slice_ompn , slice_sqtr };

        float anglePizza = 360.0f / 7f;
        listeAnglePizza = calculAngle(pizza.Count, anglePizza);

        float angleSlice = 360f / 4f;
        listeAngleSlice = calculAngle(4, angleSlice);
        listeAngleSlice[0] -= 45f;
        listeAngleSlice[1] -= 45f;
        listeAngleSlice[2] -= 45f;
        listeAngleSlice[3] -= 45f;
    }

    // Update is called once per frame
    void Update()
    {
        //      TIMERS
        countdown -= Time.deltaTime;
        if (countdown < 0f && countdown > -0.5f)
        {
            countdown = -1f;
            rightCursor.GetComponent<Renderer>().material = normalMat;
        }


        //  Controller gauche
        Vector2 input_gauche;
        input_gauche.x = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal");
        input_gauche.y = Input.GetAxis("XRI_Left_Primary2DAxis_Vertical");

        leftCursor.transform.localPosition = new Vector3(
                input_gauche.x * -5f,
                0.1f,
                input_gauche.y * 5f
            );

        //  Controller droit
        Vector2 input_droit;
        input_droit.x = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal");
        input_droit.y = Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");

        rightCursor.transform.localPosition = new Vector3(
                input_droit.x * -5f,
                0.1f,
                input_droit.y * 5f
            );

        input_gauche = cartToPolar(input_gauche);

        input_droit = cartToPolar(input_droit);

        
        char input = '¤';

        if (input_gauche.x != 0f)
        {
            for (int i_i = 0; i_i < pizza.Count; i_i++) //letter group loop
            {
                if (input_gauche.y < listeAnglePizza[i_i])
                {
                    rightCercle.GetComponent<Renderer>().material.mainTexture = textureArray[i_i];
                    //Debug.Log("slice : " + pizza[i_i][0] + pizza[i_i][1] + pizza[i_i][2] + pizza[i_i][3]);
                    if (input_droit.x != 0f)
                    {
                        bool aTrouveMatch = false;
                        for (int j_j = 0; j_j < 4; j_j++) //letter loop
                        {
                            if (input_droit.y < listeAngleSlice[j_j])
                            {
                                aTrouveMatch = true;
                                input = pizza[i_i][j_j];
                                Debug.Log("Lettre : " + input);
                                break;
                            }
                        }
                        if (!aTrouveMatch)
                        {
                            //got out of the loop meaning that it is smaller and should be pizza[i_i][j_j]
                            input = pizza[i_i][0];
                            Debug.Log("Lettre : " + input);
                        }
                    }
                    break;
                }
            }
        }

        if ((Input.GetAxis("XRI_Right_Primary2DAxisClick") != 0f) && (input != '¤') && (countdown <= 0f))
        {
            playerController.AddChar(input);
            countdown = 0.3f;
            rightCursor.GetComponent<Renderer>().material = clickedMat;
        }
    }

}
