using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class KozObject : MonoBehaviour
{
    public GameObject obj;
    public GameObject changeObject;
    public TableType tableType;
    public KozObjectEnum objectEnum;
    public KozEnum type;
    public bool RayWork;
    public bool mouseWork;
    public Ray ray;
    private KozObject selected;

    private void Start()
    {
        KozEventServices.GameAction.RaySelected += RayControl;
    }

    private void OnMouseDown()
    {
        //  Debug.Log("down");
        if (RayWork)
        {
            if (selected != null)
                KozEventServices.GameAction.SelectedObject?.Invoke(selected);
            else 
                KozEventServices.GameAction.SelectedObject?.Invoke(null);
            mouseWork = true;
        }
    }

    private void OnMouseUp()
    {
        //  Debug.Log("up");
        mouseWork = false;
        if(mouseWork)
           KozEventServices.SoundEffect.PlaySound?.Invoke();
    }


    private void OnDestroy()
    {
        KozEventServices.GameAction.RaySelected -= RayControl;
    }

    public void RayControl(KozObject tKozObject, Ray ray)
    {
        this.ray = ray;
        selected = tKozObject;
        RayWork = tKozObject != null;
    }


    private void LateUpdate()
    {
        if (mouseWork)
        {
            obj.transform.position = new Vector3(ray.GetPoint(0.75f).x, ray.GetPoint(0.75f).y, ray.GetPoint(0.75f).z);
        }
    }
}