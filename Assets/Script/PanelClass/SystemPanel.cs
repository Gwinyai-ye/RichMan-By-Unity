using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemPanel :BasePanel
{
    private CanvasGroup TempCanvasGroup;
    // Start is called before the first frame update

    private void Start()
    {
        TempCanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void OnEnter()
    {
        if (TempCanvasGroup==null)
        {
            TempCanvasGroup = GetComponent<CanvasGroup>();
        }

        TempCanvasGroup.interactable = true;
        TempCanvasGroup.blocksRaycasts = true;
        TempCanvasGroup.alpha = 1;
    }

    public override void OnExit()
    {
        TempCanvasGroup.alpha = 0;
        CanvasManger.Instance.PanelOnClose();
    }

    public override void OnPause()
    {
        TempCanvasGroup.interactable = false;
        TempCanvasGroup.blocksRaycasts = false;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
