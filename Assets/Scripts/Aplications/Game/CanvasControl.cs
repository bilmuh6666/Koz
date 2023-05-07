using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasControl : MonoBehaviour
{
    public GameObject doorOpentext;
    public GameObject takeOpentext;
    public TMP_Text skoreText;
    public TMP_Text dontOpenDoorText;
    public TMP_Text missionDoneText;
    private int skore;
    public TMP_Text skoreDeclerationText;
    public GameObject closePanel;
    public TMP_Text FinishSkore;

    private void Start()
    {
        KozEventServices.GameAction.OpenDoorText += OpenDoorText;
        KozEventServices.GameAction.CloseDoorText += CloseDoorText;
        KozEventServices.GameAction.OpenTakeText += OpenTakeText;
        KozEventServices.GameAction.CloseAllText += CloseAllText;
        KozEventServices.GameAction.CloseTakeText += CloseTakeText;
        KozEventServices.GameAction.SetSkore += SetSkore;
        KozEventServices.GameAction.OpenClosePanel += OpenClosePanel;
        KozEventServices.GameAction.DontOpenDoorText += DontOpenText;
        KozEventServices.GameAction.FinishGame += FinishGame;
    }

    private void OnDestroy()
    {
        KozEventServices.GameAction.OpenDoorText -= OpenDoorText;
        KozEventServices.GameAction.CloseDoorText -= CloseDoorText;
        KozEventServices.GameAction.OpenTakeText -= OpenTakeText;
        KozEventServices.GameAction.CloseAllText -= CloseAllText;
        KozEventServices.GameAction.CloseTakeText -= CloseTakeText;
        KozEventServices.GameAction.SetSkore -= SetSkore;
        KozEventServices.GameAction.OpenClosePanel -= OpenClosePanel;
        KozEventServices.GameAction.DontOpenDoorText -= DontOpenText;
        KozEventServices.GameAction.FinishGame -= FinishGame;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && doorOpentext.activeSelf)
        {
            KozEventServices.ButtonAction.EButtonClick?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (MouseOverUI() == false)
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private bool MouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OpenDoorText()
    {
        doorOpentext.gameObject.SetActive(true);
    }

    public void CloseDoorText()
    {
        doorOpentext.gameObject.SetActive(false);
    }

    public void OpenTakeText()
    {
        takeOpentext.gameObject.SetActive(true);
    }

    public void CloseTakeText()
    {
        takeOpentext.gameObject.SetActive(false);
    }

    public void SetSkore(int number)
    {
        skore += number;
        ScoreAnim(number);
        skoreText.text = $"Skore:{skore}";
    }

    public void LoadScene()
    {
        Debug.Log("sahne yükle");
        SceneManager.LoadScene(0);
    }

    public void DontOpenText()
    {
        dontOpenDoorText.gameObject.SetActive(true);
    }

    public void ScoreAnim(int number)
    {
        skoreDeclerationText.text = $"{number}";
        skoreDeclerationText.gameObject.SetActive(true);
        if (number > 0)
        {
            skoreDeclerationText.color = Color.green;
            skoreDeclerationText.GetComponent<RectTransform>().DOAnchorPosY(200, 1f).OnComplete((() =>
            {
                skoreDeclerationText.gameObject.SetActive(false);
                skoreDeclerationText.GetComponent<RectTransform>().DOAnchorPosY(100, 0);
            }));
        }
        else
        {
            skoreDeclerationText.color = Color.red;
            skoreDeclerationText.GetComponent<RectTransform>().DOAnchorPosY(200, 1f).OnComplete((() =>
            {
                skoreDeclerationText.gameObject.SetActive(false);
                skoreDeclerationText.GetComponent<RectTransform>().DOAnchorPosY(100, 0);
            }));
        }
    }

    public void OpenClosePanel()
    {
        missionDoneText.gameObject.SetActive(true);
        StartCoroutine(CloseText());
    }

    public void FinishGame()
    {
        closePanel.SetActive(true);
        FinishSkore.text = $"{skore}";
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public IEnumerator CloseText()
    {
        yield return new WaitForSeconds(2f);
        missionDoneText.gameObject.SetActive(false);
    }

    public void CloseAllText()
    {
        doorOpentext.gameObject.SetActive(false);
        takeOpentext.gameObject.SetActive(false);
        dontOpenDoorText.gameObject.SetActive(false);
    }
}