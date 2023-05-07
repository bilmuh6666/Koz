using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayControl : MonoBehaviour
{
    private RaycastHit hit;
    public LayerMask objectLayer;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            KozEventServices.GameAction.RayTrigger?.Invoke(hit.transform.tag);
        }
        else
        {
            KozEventServices.GameAction.CloseDoorText?.Invoke();
            KozEventServices.GameAction.CloseAllText?.Invoke();
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit h;
        if (Physics.Raycast(ray, out h, 2.5f, objectLayer))
        {
            KozEventServices.GameAction.RaySelected?.Invoke(h.transform.gameObject.GetComponent<KozObject>(), ray);
            KozEventServices.GameAction.OpenTakeText?.Invoke();
        }
        else
            KozEventServices.GameAction.RaySelected?.Invoke(null, ray);

        Debug.DrawRay(ray.origin, ray.direction * 2.5f, Color.green);
    }
}