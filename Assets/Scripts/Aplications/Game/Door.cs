using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject door;
    public bool isOpen;
    private Tweener _tweener;
    public bool isInHouse;
    public bool missionCompleted;

    void Start()
    {
        KozEventServices.GameAction.RayTrigger += DoorTrigger;
        KozEventServices.ButtonAction.EButtonClick += OpenCloseDoor;
        KozEventServices.GameAction.PlayerInHouse += SetInHouse;
        KozEventServices.GameAction.OpenClosePanel += MissionCompleted;
    }

    private void OnDestroy()
    {
        KozEventServices.GameAction.RayTrigger -= DoorTrigger;
        KozEventServices.ButtonAction.EButtonClick -= OpenCloseDoor;
        KozEventServices.GameAction.PlayerInHouse -= SetInHouse;
        KozEventServices.GameAction.OpenClosePanel -= MissionCompleted;
    }

    public void DoorTrigger(string result)
    {
        if (result == "Door" && isInHouse == false)
            KozEventServices.GameAction.OpenDoorText?.Invoke();

        if (result == "Door" && isInHouse && missionCompleted)
            KozEventServices.GameAction.OpenDoorText?.Invoke();
        else if (result == "Door" && isInHouse)
            KozEventServices.GameAction.DontOpenDoorText?.Invoke();
    }

    public void MissionCompleted()
    {
        missionCompleted = true;
    }

    public void OpenCloseDoor()
    {
        if (isInHouse && missionCompleted)
        {
            KozEventServices.GameAction.FinishGame?.Invoke();
            return;
        }

        if (_tweener != null)
        {
            _tweener.Kill();
            _tweener = null;
        }

        _tweener = isOpen
            ? door.transform.DOLocalRotate(new Vector3(0, 0, 0), 1).OnComplete((() => { isOpen = false; }))
            : door.transform.DOLocalRotate(new Vector3(0, -70, 0), 1).OnComplete((() => { isOpen = true; }));
    }

    public void SetInHouse()
    {
        isInHouse = true;
        _tweener = door.transform.DOLocalRotate(new Vector3(0, 0, 0), 1).OnComplete((() => { isOpen = false; }));
    }
}