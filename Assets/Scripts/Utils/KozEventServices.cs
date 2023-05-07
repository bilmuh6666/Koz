using System;
using UnityEngine;

public class KozEventServices : MonoBehaviour
{
    public static KozEventServices Instance;

    private void Awake()
    {
        Instance ??= this;
    }


    [HideInInspector]
    public class GameAction
    {
        public static Action<string> RayTrigger;
        public static Action OpenDoorText;
        public static Action OpenTakeText;
        public static Action CloseTakeText;
        public static Action CloseAllText;
        public static Action<int> SetSkore;
        public static Action OpenClosePanel;
        public static Action<KozObject,Ray> RaySelected;
        public static Action<KozObject> SelectedObject;
        public static Action CloseDoorText;
        public static Action PlayerInHouse;
        public static Action DontOpenDoorText;
        public static Action FinishGame;
    }

    public class ButtonAction
    {
        public static Action EButtonClick;
    }

    public class SoundEffect
    {
        public static Action PlaySound;
    }
}