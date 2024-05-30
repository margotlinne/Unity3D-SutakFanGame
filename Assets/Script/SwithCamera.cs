using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwithCamera : MonoBehaviour
{
    public CinemachineVirtualCamera[] cam;

    CinemachineBrain cinemachineBrain;

    private int activeNum;

    void Awake()
    {
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();   
    }

    void Start()
    {
        setPriority(0);
        activeNum = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            switchCam(KeyCode.E);
        }
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            switchCam(KeyCode.Q);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            switchCam(KeyCode.O);
        }
    }

    void switchCam(KeyCode key)
    {
        ICinemachineCamera activeCam = cinemachineBrain.ActiveVirtualCamera;

        if (key == KeyCode.E)
        {
            if (activeCam.Name == "Virtual Camera")
            {
                setPriority(3);
            }
            else if (activeCam.Name == "Virtual Camera (1)")
            {
                setPriority(0);
            }
            else if (activeCam.Name == "Virtual Camera (2)")
            {
                setPriority(1);
            }
            else if (activeCam.Name == "Virtual Camera (3)")
            {
                setPriority(2);
            }
            else if (activeCam.Name == "Virtual Camera (4)")
            {
                setPriority(activeNum);
            }
        }
        else if (key == KeyCode.Q)
        {
            if (activeCam.Name == "Virtual Camera")
            {
                setPriority(1);
            }
            else if (activeCam.Name == "Virtual Camera (1)")
            {
                setPriority(2);
            }
            else if (activeCam.Name == "Virtual Camera (2)")
            {
                setPriority(3);
            }
            else if (activeCam.Name == "Virtual Camera (3)")
            {
                setPriority(0);
            }
            else if (activeCam.Name == "Virtual Camera (4)")
            {
                setPriority(activeNum);
            }
        }
        else if (key == KeyCode.O)
        {
            if (activeCam.Name != "Virtual Camera (4)")
            {
                setPriority(4);
            }
            else
            {
                setPriority(activeNum);
            }
        }

    }

    void setPriority(int num)
    {
        for (int i = 0; i < cam.Length; i++)
        {
            if (i != num)
            {
                cam[i].Priority = 0;
            }
            else 
            { 
                cam[i].Priority = 10; 
                if (num != 4) { activeNum = i; }                
            }
        }
    }
}
