using UnityEngine;
using System;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Valve.VR;


public class SerialCommunication : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("COM5", 9600);
    string data_str = "";

    //test variable
    public int value = 0;
    public float conversion = 0;
    public int debugBtn;
    public static int buttonPressed = 0;
    public static int prevButtonPressed = 0;
    private string[] numbers;
   

   
    void Start()
    {
        data_stream.Open();
      //  data_stream.ReadTimeout = 3;  //make shorter than encoder delay
    }
    void Update()
    {
        try
        {
            data_str = data_stream.ReadLine(); //read single encoder measurement 
            //Debug.Log(data_str);
            if (!data_str.Equals(""))
            {
                numbers = data_str.Split(',');
                prevButtonPressed = buttonPressed;
                buttonPressed = int.Parse(numbers[0]); //update button input
                if (buttonPressed == 1)
                {
                    Debug.Log("pressedddd");
                }
                debugBtn = buttonPressed;
                try
                {
                    value = int.Parse(numbers[1]); //check if data shows up in Unity
                    conversion = ((float) value) / 100f;
                    transform.localPosition = new Vector3(conversion, 0, 0);

                }
                catch (IndexOutOfRangeException)
                {
                    // Debug.Log("problem with data");
                }

            }
            else
            {
                //Debug.Log("invalid data");
            }
        }
        catch (TimeoutException)
        {
            //read buffer is temporarily empty (do nothing)
            // Debug.Log("buffer empty");
        }
        catch (FormatException)
        {
            Debug.Log(numbers[0]);
            Debug.Log(numbers[1]);
        }
        
    }
}