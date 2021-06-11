using UnityEngine;
using System;
using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Threading;
using Valve.VR;
using static System.IO.StreamReader;
using System.Runtime.Remoting.Channels;
using System.Text;
using UnityEditor.Experimental.GraphView;


public class SerialCommunication : MonoBehaviour
{
    public SerialPort data_stream = new SerialPort("COM5", 9600);


    static byte ubyte = (byte) 'u';


    //test variable
    public int value = 0;
    public float conversion = 0;
    public int debugBtn;
    public static int buttonPressed = 0;
    public static int prevButtonPressed = 0;
    public static int calibratePressed = 0;
    public static int prevCalibratePressed = 0;
    private string[] numbers;
    private String leftover = "";
    
    void Start()
    {
        //data_stream.DataReceived += DataReceivedHandler;
        data_stream.Open();
        ReadFunction();
    }
    
    private void ReadFunction()
    {
        byte[] buffer = new byte[4096];
        Action kickoffRead = null;
        kickoffRead = (Action)(() => data_stream.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate (IAsyncResult ar)
        {
            try
            {
                int count = data_stream.BaseStream.EndRead(ar);
                byte[] dst = new byte[count];
                Buffer.BlockCopy(buffer, 0, dst, 0, count);
                Debug.Log("im working");
                ProcessData(dst);
            }
            catch (Exception exception)
            {
                Console.WriteLine("SerialPort error !");
            }
            kickoffRead();
        }, null)); kickoffRead();
    }

    void ProcessData(byte[] data)
    {
        int nextStart = 0;
        byte nl = (byte)'\n';
        Debug.Log("data to process");
        String str = Encoding.UTF8.GetString(data);
        Debug.Log(str);
        string temp = "";
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == 13)  //newline char found
            {
                temp = leftover + str.Substring(nextStart, i - nextStart);
                nextStart = i + 2;
                leftover = "";
                decodeData(temp);
                //call decode function
            }
            if (i == data.Length - 1)  //last byte
            {
                if (data[i] != 13 && data[i] != 10)
                {
                    leftover = str.Substring(nextStart, i - nextStart + 1);
                }
                else  //ends with new line -> no leftover
                {
                    leftover = "";
                }
            }
        }
        
        //data_str = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
        
    }
    
    void decodeData(string data_str)
    {
        try
        {
            if (!data_str.Equals(""))  //a value has changed
            {
                numbers = data_str.Split(',');
                prevButtonPressed = buttonPressed;
                if (numbers[0] != " ")
                {
                    buttonPressed = int.Parse(numbers[0]); //update button input
                }
                debugBtn = buttonPressed;
                prevCalibratePressed = calibratePressed;
                if (numbers[1] != " ")
                {
                    calibratePressed = int.Parse(numbers[1]); //update calibrate button input
                }
                if (numbers[2] != " ")
                {
                    value = int.Parse(numbers[2]); //check if data shows up in Unity
                    conversion = ((float) value) / 100f;
                    //transform.localPosition = new Vector3(conversion, 0, 0);
                }
            }
            else
            {
                // Debug.Log("no new data");
            }
        }
        catch (TimeoutException)
        {
            //read buffer is temporarily empty (do nothing)
            // Debug.Log("buffer empty");
        }
        catch (FormatException)
        {
            Debug.Log("problemo");
            Debug.Log(numbers[0]);
            Debug.Log(numbers[1]);
        }
        catch (IndexOutOfRangeException)
        {
            // Debug.Log("problem with data");
        }   
    }


}