using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class TableControl : MonoBehaviour
{
    public List<GameObject> kozObjects;
    public List<GameObject> kozInstanceObjects;
    public List<Transform> randomPoss;
    public List<GameObject> tableOne;
    public List<GameObject> tableTwo;
    public KozObject lastSelected;

    private Bounds _tableOneBounds;
    private Bounds _tableTwoBounds;

    public Renderer tableOneRenderer;
    public Renderer tableTwoRenderer;
    private KozObject skoreObject;

    private void Start()
    {
        KozEventServices.GameAction.SelectedObject += SelectedObject;
        RandomArray();
        StartCoroutine(ObjectCreate());

        _tableOneBounds = tableOneRenderer.bounds;
        _tableTwoBounds = tableTwoRenderer.bounds;
    }

    private void OnDestroy()
    {
        KozEventServices.GameAction.SelectedObject -= SelectedObject;
    }

    public IEnumerator ObjectCreate()
    {
        foreach (var item in kozObjects)
        {
            yield return new WaitForSeconds(0.1f);
            int i = Random.Range(0, 2);
            var position = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)) +
                           randomPoss[i].transform.position;
            GameObject obj = Instantiate(item, position, Quaternion.identity);
            kozInstanceObjects.Add(obj);
            if (i == 0)
            {
                obj.GetComponent<KozObject>().tableType = TableType.TableOne;
                tableOne.Add(obj);
            }
            else
            {
                obj.GetComponent<KozObject>().tableType = TableType.TableTwo;
                tableTwo.Add(obj);
            }
        }

        foreach (var item in kozInstanceObjects)
        {
            item.GetComponent<KozObject>().changeObject = kozInstanceObjects.FirstOrDefault(x =>
                $"{item.GetComponent<KozObject>().objectEnum}(Clone)" == $"{x.name}");
        }
    }


    public void SelectedObject(KozObject result)
    {
        if (result == null)
        {
            return;
        }

        skoreObject = result;
        lastSelected = result;
        KozEventServices.GameAction.OpenTakeText?.Invoke();
    }

    private void RandomArray()
    {
        for (int i = 0; i < kozObjects.Count; i++)
        {
            GameObject t = kozObjects[i];
            int r = RandomNumber(kozObjects.Count - i);
            kozObjects[i] = kozObjects[i + r];
            kozObjects[i + r] = t;
        }
    }

    private int RandomNumber(int max)
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] data = new byte[4];
            rng.GetBytes(data);
            int value = Math.Abs(BitConverter.ToInt32(data, 0)) % max;
            return value;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
            StartCoroutine(BoundsControl());
    }

    private IEnumerator BoundsControl()
    {
        yield return new WaitForSeconds(0.1f);


        tableOne.Clear();
        tableTwo.Clear();


        foreach (var item in kozInstanceObjects)
        {
            Vector3 m_Point = item.transform.position;

            if (_tableOneBounds.Contains(m_Point))
            {
                tableOne.Add(item);
                if (item.GetComponent<KozObject>().Equals(lastSelected))
                {
                    item.GetComponent<KozObject>().changeObject.transform.position = randomPoss[1].transform.position;
                    lastSelected = null;
                }

                item.GetComponent<KozObject>().tableType = TableType.TableOne;
            }
            else if (_tableTwoBounds.Contains(m_Point))
            {
                tableTwo.Add(item);
                if (item.GetComponent<KozObject>().Equals(lastSelected))
                {
                    item.GetComponent<KozObject>().changeObject.transform.position = randomPoss[0].transform.position;
                    lastSelected = null;
                }

                item.GetComponent<KozObject>().tableType = TableType.TableTwo;
            }
            else
            {
                item.GetComponent<KozObject>().tableType = TableType.None;
                Debug.Log("Obje farklı biryerde");
            }
        }


        if (skoreObject != null)
        {
            if (skoreObject.type == KozEnum.LeftItem1)
            {
                if (tableOne.Any(item => ReferenceEquals(item.gameObject, skoreObject.gameObject)))
                {
                    KozEventServices.GameAction.SetSkore?.Invoke(5);
                }
                else
                {
                    KozEventServices.GameAction.SetSkore?.Invoke(-5);
                }
            }
            else
            {
                if (tableTwo.Any(item => ReferenceEquals(item.gameObject, skoreObject.gameObject)))
                {
                    KozEventServices.GameAction.SetSkore?.Invoke(5);
                }
                else
                {
                    KozEventServices.GameAction.SetSkore?.Invoke(-5);
                }
            }
        }

        skoreObject = null;
        FinishControl();
    }


    public void FinishControl()
    {
        if (tableOne.Count == 4)
        {
            if (tableOne.TrueForAll(i => i.GetComponent<KozObject>().type == KozEnum.LeftItem1))
            {
                foreach (var VARIABLE in kozInstanceObjects)
                {
                    VARIABLE.GetComponent<Rigidbody>().isKinematic = true;
                    VARIABLE.GetComponent<MeshCollider>().enabled = false;
                }
                KozEventServices.GameAction.OpenClosePanel?.Invoke();
            }
        }
    }
}