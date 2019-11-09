using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private CanvasGroup TempCanvasGroup;
    // Start is called before the first frame update
    private GameObject BagPanel;
    
    public Text moneyText;
    public Text powerText;
    public Text nameText;
    public Image headIcon;

    private static MainPanel _instance;

    public static MainPanel Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("MainPanel").GetComponent<MainPanel>();
            return _instance;
        }
    }
    
    private void Awake()
    {
        TempCanvasGroup = GetComponent<CanvasGroup>();
//        BagPanel = GameObject.Find("BagPanel");
    }

    public override void OnEnter()
    {
        TempCanvasGroup.alpha = 1;
        TempCanvasGroup.blocksRaycasts = true;
        TempCanvasGroup.interactable = true;
    }

    public override void OnResume()
    {
        TempCanvasGroup.interactable = true;   
        TempCanvasGroup.blocksRaycasts = true;
    }

    // Update is called once per frame
    public override void OnPause()
    {
        TempCanvasGroup.interactable = false;
        TempCanvasGroup.blocksRaycasts = false;
    }

    public void OnButtonClick(string panelTypeString)
    {
        UIPanelType value = (UIPanelType) System.Enum.Parse(typeof(UIPanelType), panelTypeString);
        CanvasManger.Instance.PanelOnEnter(value);
    }

    public void IntailMainPanelMessage(PlayerInfo player)
    {
        moneyText.text = player.money.ToString();
        powerText.text = player.power.ToString();
        nameText.text = player.playername.ToString();
        headIcon.sprite = player.headIcon;
    }
    
}
