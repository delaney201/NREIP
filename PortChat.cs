using UnityEngine;
using System;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using System.Threading;



public class SerialCommunication : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("COM4", 9600);
    string data_str = "";

    //test variable
    public int value = 0;
    public float conversion = 0;

    void Start()
    {
        data_stream.Open();
        //  data_stream.ReadTimeout = 3;  //make shorter than encoder delay
    }
    void Update()
    {
        try
        {
            data_str = data_stream.ReadLine();  //read single encoder measurement 
            //Debug.Log(data_str);
            if (!data_str.Equals(""))
            {
                value = int.Parse(data_str);  //check if data shows up in Unity
                conversion = ((float)value) / 100f;
                transform.localPosition = new Vector3(conversion, 0, 0);
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

    }
}