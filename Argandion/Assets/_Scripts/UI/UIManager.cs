using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    // Singleton Patturn
    public static UIManager _uimanagerInstance;
    private void Awake()
    {
        _uimanagerInstance = this;
        firstClick = new int[4];
    }

    // Panel var
    [SerializeField] public GameObject _baseuipanel;
    [SerializeField] private GameObject _mapuipanel;
    [SerializeField] private MainPagePanel _mainpage;
    [SerializeField] private OptionPanel _optionpanel;
    [SerializeField] private OptionPanel _optionfrommain;
    [SerializeField] private CreateCharacter _createcharacter;
    [SerializeField] private ConversationPanel _conversationpanel;
    [SerializeField] private CraftingPanel _craftingpanel;
    [SerializeField] private CookingPanel _cookingpanel;
    [SerializeField] private BuildEventPanel _buildeventpanel;
    [SerializeField] private TransactionAnimalPanel _transactionanimalpanel;
    [SerializeField] private TransactionPanel _transactionpanel;
    [SerializeField] private InventoryPanel _inventorypanel;
    [SerializeField] private GameObject _storagepanel;
    [SerializeField] private TradeModal _trademodal;
    [SerializeField] private GameObject _inventory;
    [SerializeField] private TextMeshProUGUI _invenMoney;
    [SerializeField] private GameObject _notificationpanel;
    [SerializeField] private ResultNotificationPanel _resultnotificationpanel;
    [SerializeField] private TransactionDoubleCheck _transactiondoublecheck;

    public GameObject _eventAnnounce;
    public TextMeshProUGUI _announceTitle;
    public TextMeshProUGUI _announceText;
    private GameObject _nowequip;

    [SerializeField] private SystemManager _systemmanager;
    [SerializeField] private PlayerSystem _playersystem;
    [SerializeField] private Item _itemmanager;
    [SerializeField] private WorldTree _worldtree;
    [SerializeField] private TeleportAltar _alterdown;
    [SerializeField] private TeleportAltar _alterup;
    [SerializeField] private Altar _alter;

    private Slider _healthbar;
    private Slider _energybar;
    public RectTransform _timer;
    public GameObject _daytime;
    public GameObject _noontime;

    // 상태 저장 데이터
    public Quaternion rotateZero = Quaternion.Euler(new Vector3(0, 0, 0));     // 회전값 기본값 세팅

    public int conversationNPC;
    private int selectCharacter;
    [SerializeField] public bool isPressESC;
    private bool isMyHome;

    // 패널 오픈 여부 변수
    [SerializeField] private bool isTransactionOpen;
    [SerializeField] private bool isInventoryOpen;
    [SerializeField] private bool isInvenRightModal;
    [SerializeField] private bool isStorageOpen;
    private bool isCraftOpen;
    private bool isCookOpen;
    private bool isBuildEventOpen;
    private bool isMapOpen;
    private bool isConversationOpen;

    private bool isInvenLeftClick;
    private int[] firstClick;

    public GameObject _eventpanel;
    public FoodManager _foodmanager;

    [SerializeField] private GameObject _teleport;

    // Sprite 이미지 저장 Map
    private Dictionary<int, Sprite> Dic = new Dictionary<int, Sprite>();
    private Dictionary<int, Sprite> storageDic = new Dictionary<int, Sprite>();

    // Sprite 탐색 해서 저장하는 함수
    public Sprite getItemIcon(int key)
    {
        if (Dic.ContainsKey(key))
        {
            return Dic[key];
        }
        Sprite icon = Resources.Load<Sprite>("Sprites/" + key);
        Dic.Add(key, icon);
        return icon;
    }

    public Sprite getItemIconInStorage(int key) {
        if(storageDic.ContainsKey(key)) {
            return storageDic[key];
        }
        Sprite icon = Resources.Load<Sprite>("StorageCloth/" + key);
        storageDic.Add(key, icon);
        return icon;
    }

    public void Start()
    {
        conversationNPC = 0;
        selectCharacter = -1;
        isPressESC = false;
        isMyHome = false;
        isTransactionOpen = false;
        isInventoryOpen = false;
        isStorageOpen = false;
        isCraftOpen = false;
        isCookOpen = false;
        isBuildEventOpen = false;
        isMapOpen = false;
        isConversationOpen = false;

        _systemmanager = GameObject.Find("SystemManager").GetComponent<SystemManager>();
        _playersystem = GameObject.Find("PlayerObject").GetComponent<PlayerSystem>();
        _itemmanager = GameObject.Find("ItemManager").GetComponent<Item>();
        _foodmanager = GameObject.Find("FoodManager").GetComponent<FoodManager>();
        _worldtree = GameObject.Find("WorldTree").GetComponent<WorldTree>();

        _teleport = GameObject.Find("Teleport");
        _alterdown = GameObject.Find("teleportDown").GetComponent<TeleportAltar>();
        GameObject.Find("Down").SetActive(false);
        _alterup = GameObject.Find("teleportUp").GetComponent<TeleportAltar>();
        GameObject.Find("Up").SetActive(false);
        _alter = GameObject.Find("Altar").GetComponent<Altar>();

        _baseuipanel = gameObject.transform.Find("BaseUIPanel").gameObject;
        _healthbar = _baseuipanel.transform.GetChild(0).GetComponent<Slider>();
        _energybar = _baseuipanel.transform.GetChild(1).GetComponent<Slider>();
        _eventpanel = _baseuipanel.transform.GetChild(4).gameObject;
        _eventpanel.GetComponent<EventPanel>().setting();
        _daytime = _baseuipanel.transform.GetChild(2).GetChild(2).gameObject;
        _noontime = _baseuipanel.transform.GetChild(2).GetChild(3).gameObject;
        _foodmanager._eventPanel = _eventpanel.GetComponent<EventPanel>();

        _nowequip = GameObject.Find("NowEquip").gameObject;

        _mapuipanel = GameObject.Find("MapUIPanel");
        _mainpage = gameObject.transform.Find("MainPagePanel").GetComponent<MainPagePanel>();
        _optionpanel = gameObject.transform.Find("OptionPanel").GetComponent<OptionPanel>();
        _optionfrommain = gameObject.transform.Find("OptionPanelFromMainPage").GetComponent<OptionPanel>();
        _createcharacter = gameObject.transform.Find("CreateCharacter").GetComponent<CreateCharacter>();
        _conversationpanel = gameObject.transform.Find("ConversationPanel").GetComponent<ConversationPanel>();
        _cookingpanel = gameObject.transform.Find("CookingPanel").GetComponent<CookingPanel>();
        _craftingpanel = gameObject.transform.Find("CraftingPanel").GetComponent<CraftingPanel>();
        _buildeventpanel = gameObject.transform.Find("BuildEventPanel").GetComponent<BuildEventPanel>();
        _transactionanimalpanel = gameObject.transform.Find("TransactionAnimalPanel").GetComponent<TransactionAnimalPanel>();
        _transactionpanel = gameObject.transform.Find("TransactionPanel").GetComponent<TransactionPanel>();
        _inventorypanel = gameObject.transform.Find("InventoryPanel").GetComponent<InventoryPanel>();
        _storagepanel = gameObject.transform.Find("StoragePanel").gameObject;
        _trademodal = gameObject.transform.Find("TradeModal").GetComponent<TradeModal>();
        _inventory = gameObject.transform.Find("Inventory").gameObject;
        _invenMoney = _inventory.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        _invenMoney.text = _systemmanager.getPlayerGold().ToString();
        _resultnotificationpanel = gameObject.transform.Find("ResultNotificationPanel").GetComponent<ResultNotificationPanel>();
        _notificationpanel = GameObject.Find("NotificationPanel");
        _transactiondoublecheck = gameObject.transform.Find("TransactionDoubleCheckModal").GetComponent<TransactionDoubleCheck>();
        _eventAnnounce = GameObject.Find("EventUIAnnounce");
        _announceTitle = _eventAnnounce.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        _announceText = _eventAnnounce.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();

        _transactiondoublecheck.gameObject.SetActive(false);
        _resultnotificationpanel.gameObject.SetActive(false);
        _notificationpanel.SetActive(false);
        _baseuipanel.SetActive(false);
        _mapuipanel.SetActive(false);
        _optionpanel.gameObject.SetActive(false);
        _optionfrommain.gameObject.SetActive(false);
        _createcharacter.gameObject.SetActive(false);
        _conversationpanel.gameObject.SetActive(false);
        _cookingpanel.gameObject.SetActive(false);
        _craftingpanel.gameObject.SetActive(false);
        _buildeventpanel.gameObject.SetActive(false);
        _transactionanimalpanel.gameObject.SetActive(false);
        _transactionpanel.gameObject.SetActive(false);
        _inventorypanel.gameObject.SetActive(false);
        _storagepanel.SetActive(false);
        _trademodal.gameObject.SetActive(false);
        _inventory.gameObject.SetActive(false);
        _eventAnnounce.SetActive(false);

        isInvenLeftClick = false;
    }

    public void startInvenSet() {
        ItemObject item1 = findItem(300);
        acquireItem(item1, 1);
        ItemObject item2 = findItem(301);
        acquireItem(item2, 1);
        ItemObject item3 = findItem(302);
        acquireItem(item3, 1);
        ItemObject item4 = findItem(303);
        acquireItem(item4, 1);
        ItemObject item5 = findItem(304);
        acquireItem(item5, 1);
        ItemObject item10 = findItem(320);
        acquireItem(item10, 1);

        _systemmanager.setPlayerGold(1000);
    }

    void Update()
    {
        setTimer();

        if (Input.GetButtonDown("optionKey"))
        {
            if((!isConversationOpen && isPanelOpen()) || _mapuipanel.activeSelf) {
                closeAllPanel();
            } else {
                pressedESC();
            }
        }

        // 다른 키 다 X, inventory key만 동작
        if ((!isPanelOpen() && Input.GetButtonDown("InventoryKey")) || (isInventoryOpen && Input.GetButtonDown("InventoryKey")))
        {
            if (getGameState())
            {
                OnInventoryPanel();
            }
        }

        // 다른 키 다 X, map key만 동작
        if ((!isPanelOpen() && Input.GetButtonDown("mapKey")) || (isMapOpen && Input.GetButtonDown("mapKey")))
        {
            if (getGameState() && !isMyHome)
            {
                OnMapUIPanel();
            }
        }

        if (Input.GetButtonDown("interactionKey") && isInvenRightModal)
        {
            closeInvenRightClickModal();
        }

        if (Input.GetButtonDown("interactionKey"))
        {
            if (isConversationOpen)
            {
                int conversationCnt = _conversationpanel.GetComponent<ConversationPanel>().getConversationCnt();
                if (_conversationpanel.GetComponent<ConversationPanel>().getIsConversation())
                {
                    _conversationpanel.GetComponent<ConversationPanel>().conversation();
                }
                else if (_conversationpanel.GetComponent<ConversationPanel>().getIsInformation())
                {
                    _conversationpanel.GetComponent<ConversationPanel>().information();
                }
                else
                {
                    switch (conversationCnt)
                    {
                        case 0:
                            if (conversationNPC == 9)
                            {
                                break;
                            }
                            _conversationpanel.GetComponent<ConversationPanel>().secondConversation();
                            break;
                        case 1:
                            _conversationpanel.GetComponent<ConversationPanel>().thirdConversation();
                            break;
                    }
                }
            }
        }
    }

    // UI 전체 닫기
    public void closeAllPanel() {
        _transactiondoublecheck.gameObject.SetActive(false);
        _resultnotificationpanel.gameObject.SetActive(false);
        _notificationpanel.SetActive(false);
        _mapuipanel.SetActive(false);
        _optionpanel.gameObject.SetActive(false);
        _optionfrommain.gameObject.SetActive(false);
        _createcharacter.gameObject.SetActive(false);
        _conversationpanel.gameObject.SetActive(false);
        _cookingpanel.gameObject.SetActive(false);
        _craftingpanel.gameObject.SetActive(false);
        _buildeventpanel.gameObject.SetActive(false);
        _transactionanimalpanel.gameObject.SetActive(false);
        _transactionpanel.closeTransaction();
        _inventorypanel.gameObject.SetActive(false);
        _storagepanel.SetActive(false);
        _trademodal.gameObject.SetActive(false);
        _inventory.gameObject.SetActive(false);
        _eventAnnounce.SetActive(false);
        
        isTransactionOpen = false;
        isInventoryOpen = false;
        isStorageOpen = false;
        isCraftOpen = false;
        isCookOpen = false;
        isBuildEventOpen = false;
        isMapOpen = false;
        isConversationOpen = false;

        runControllKeys();
    }

    public void OnBaseUIPanel()
    {
        _baseuipanel.SetActive(true);
    }

    public void OnTransactionPanel()
    {
        _conversationpanel.GetComponent<ConversationPanel>().resetConversationPanel();
        _transactionpanel.GetComponent<TransactionPanel>().handelPanel(conversationNPC);
    }

    public void OnTransactionAnimalPanel()
    {
        _conversationpanel.GetComponent<ConversationPanel>().resetConversationPanel();
        _transactionanimalpanel.GetComponent<TransactionAnimalPanel>().handelPanel();
    }

    public void OnCraftingPanel(int value)
    {
        _craftingpanel.GetComponent<CraftingPanel>().handelPanel(value);
    }

    public void OnCookingPanel()
    {
        _cookingpanel.GetComponent<CookingPanel>().handelPanel();
    }

    public void OnBuildEventPanel(int value)
    {
        _buildeventpanel.GetComponent<BuildEventPanel>().handelPanel(value);
    }

    public void OnStoragePanel()
    {
        _storagepanel.GetComponent<StoragePanel>().handlePanel();
    }

    public void OnInventoryPanel()
    {
        _inventorypanel.GetComponent<InventoryPanel>().handlePanel();

        if (_inventorypanel.gameObject.activeSelf)
        {
            stopControllKeys();
        }
        else
        {
            runControllKeys();
        }
    }

    public void OnConversationPanel(int value)
    {
        _conversationpanel.GetComponent<ConversationPanel>().setConversationNPC(value);
        stopControllKeys();
    }

    public void OnCreateCharacter()
    {
        _createcharacter.gameObject.SetActive(true);
    }

    public void OnMainPagePanel()
    {
        _mainpage.gameObject.SetActive(true);
        setGameState(false);
    }

    public void OnMapUIPanel()
    {
        if (getGameState())
        {
            _mapuipanel.SetActive(!_mapuipanel.activeSelf);
        }
    }

    public void OnNotificationPanel()
    {
        _notificationpanel.GetComponent<NotificationPanel>().handleNoti();
    }

    public void OnResultNotificationPanel(string text)
    {
        _resultnotificationpanel.GetComponent<ResultNotificationPanel>().handelNoti(text);
    }

    public void OnTransactionDoubleCheckPanel(string name, int store, int itemIdx, int itemCode)
    {
        if (conversationNPC != 5 && !checkInventory(findItem(itemCode), 1))
        {
            OnResultNotificationPanel("구매가 불가능 합니다. \n인벤토리를 확인 해 주세요!!");
            return;
        }
        _transactiondoublecheck.setData(name, store, itemIdx, itemCode);
        _transactiondoublecheck.handleModal();
    }

    public void OnTradeModal(string name, string iconName, int maxCnt, int cost, int checkMod, int storeIdx, int itemIdx)
    {
        if ((checkMod == 1 || checkMod == 4) && !checkInventory(findItem(Int32.Parse(iconName)), 1))
        {
            OnResultNotificationPanel("인벤토리에 빈 공간이 없습니다. \n인벤토리를 확인 해 주세요!!");
            return;
        }

        // sell 요청 시, storeIdx는 인벤토리/퀵슬롯 구분자
        // 1 : 인벤 // 2 : 퀵슬롯
        int _storeKey = storeIdx;
        if (_storeKey == -1)
        {
            _storeKey = conversationNPC;
        }
        _trademodal.GetComponent<TradeModal>().setModal(name, iconName, maxCnt, cost, checkMod, _storeKey, itemIdx);
    }

    public void closeTradeModal()
    {
        _trademodal.GetComponent<TradeModal>().closeModal();
    }

    public void OnInventory(int value)
    {
        switch (value)
        {
            case 1:
                _inventory.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(110, -30, 0), rotateZero);
                break;
            case 2:
                _inventory.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(110, -15.06f, 0), rotateZero);
                break;
            case 3:
                _inventory.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(-211.66f, -5.58f, 0), rotateZero);
                _inventory.transform.GetChild(1).GetComponent<Image>().color = new Color(225, 225, 225, 0);
                _inventory.transform.GetChild(1).GetChild(1).GetComponent<Image>().color = new Color(225, 225, 225, 0);
                _inventory.transform.GetChild(1).GetChild(2).GetComponent<Image>().color = new Color(225, 225, 225, 0);
                break;
        }
        _inventory.gameObject.SetActive(!_inventory.gameObject.activeSelf);
        getPlayerGold();
    }

    // 인벤 좌클릭 관련 함수
    public bool getIsLeftClickInInven() {
        return isInvenLeftClick;
    }

    // key 1 = 인벤  2 = 퀵슬롯
    // idx : 슬롯 idx
    // code : 아이템 코드
    public void setFirstClick(int key, int idx, int code, int count) {
        firstClick[0] = key;
        firstClick[1] = idx;
        firstClick[2] = code;
        firstClick[3] = count;
        isInvenLeftClick = true;
    }
    public void setSecondClick(int key, int idx, int code, int count) {
        Slot[] invenSlot = _inventory.transform.GetChild(1).GetComponent<Inventory>().getInventorySlots();
        Slot[] quickSlot = _inventory.transform.GetChild(0).GetComponent<Quickslot>().getInventorySlots();

        if(key != firstClick[0] && ((code >= 502 && code <= 504) || firstClick[2] >= 502 && firstClick[2] <= 504)) {
            isInvenLeftClick = false;
            return;
        }

        string firstCate = findItem(firstClick[2]).Category;
        string secondCate = findItem(code).Category;

        if(key != firstClick[0] && 
            !(
             (firstCate == "장비" || firstCate == "꽃" || firstCate == "씨앗" || firstCate == "식량") ||
             (secondCate == "장비" || secondCate == "꽃" || secondCate == "씨앗" || secondCate == "식량")
            )
          ) 
        {
            isInvenLeftClick = false;
            return;
        }

        if(key == 1) {
            if(firstClick[2] == 0) {
                invenSlot[idx].ClearSlot();
            } else {
                invenSlot[idx].AddItem(findItem(firstClick[2]), firstClick[3]);
            }
        } else {
            if(firstClick[2] == 0) {
                quickSlot[idx].ClearSlot();
                setPlayerQuickSlot(idx, 0, 0);
            } else {
                quickSlot[idx].AddItem(findItem(firstClick[2]), firstClick[3]);
                setPlayerQuickSlot(idx, firstClick[2], firstClick[3]);
            }
        }

        if(firstClick[0] == 1) {
            if(code == 0) {
                invenSlot[firstClick[1]].ClearSlot();
            } else {
                invenSlot[firstClick[1]].AddItem(findItem(code), count);
            }
        } else {
            if(code == 0) {
                quickSlot[firstClick[1]].ClearSlot();
                setPlayerQuickSlot(firstClick[1], 0, 0);
            } else {
                quickSlot[firstClick[1]].AddItem(findItem(code), count);
                setPlayerQuickSlot(firstClick[1], code, count);
            }
        }

        syncQuickSlot();
        isInvenLeftClick = false;
    }

    // 패널 오픈 여부 함수
    public bool getIsOpenTransaction()
    {
        return isTransactionOpen;
    }

    public void setIsOpenTransaction(bool value)
    {
        isTransactionOpen = value;
    }

    public bool getIsOpenInventory()
    {
        return isInventoryOpen;
    }

    public void setIsOpenInventory(bool value)
    {
        isInventoryOpen = value;
    }

    public bool getIsOpenStorage()
    {
        return isStorageOpen;
    }

    public void setIsOpenStorage(bool value)
    {
        isStorageOpen = value;
    }

    public bool getIsOpenConversation()
    {
        return isConversationOpen;
    }

    public void setIsOpenConversation(bool value)
    {
        isConversationOpen = value;
    }

    public bool getIsOpenCraft()
    {
        return isCraftOpen;
    }

    public void setIsOpenCraft(bool value)
    {
        isCraftOpen = value;
    }

    public bool getIsOpenCook()
    {
        return isCookOpen;
    }

    public void setIsOpenCook(bool value)
    {
        isCookOpen = value;
    }

    public bool getIsOpenBuildEvent()
    {
        return isBuildEventOpen;
    }

    public void setIsOpenBuildEvent(bool value)
    {
        isBuildEventOpen = value;
    }

    // 캐릭터 선택 관련 함수
    public void setCharacterValue(int value)
    {
        selectCharacter = value;

    }
    public int getCharacterValue()
    {
        return selectCharacter;
    }

    public void setHealthBar(float value,float maxValue)
    {
        _healthbar.value = value/maxValue;
        _healthbar.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = (value).ToString();
    }

    public void setEnergyBar(float value,float maxValue)
    {
        _energybar.value = value/maxValue;
        _energybar.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = (value).ToString();
    }

    public void setTimer()
    {
        float angle = (_systemmanager._hour_display - 6) * 15 + (_systemmanager._minute_display / 4);
        _timer.rotation = Quaternion.Euler(180, 0, angle);
        string timeText = _systemmanager._month + "월 " + _systemmanager._day + "일   " + _systemmanager._hour_display + "시 " + _systemmanager._minute_display + "분";
        _baseuipanel.transform.GetChild(2).GetChild(6).GetComponent<TextMeshProUGUI>().text = timeText;
    }

    // 동물 수 동기화 함수
    public void syncAnimalPanel(int capacity, int sheepCnt, int chickenCnt, int cowCnt)
    {
        _transactionanimalpanel.syncRanchData(capacity, sheepCnt, chickenCnt, cowCnt);
    }

    // ESC 클릭 시 동작
    public void pressedESC()
    {
        isPressESC = !isPressESC;

        if (isPressESC)
        {
            if (getGameState())
            {
                _optionpanel.handelPanel();
                stopControllKeys();
            }
            else
            {
                _optionfrommain.handelPanel();
                handleMainBtnInteractable(false);
            }
        }
        else
        {
            if (getGameState())
            {
                _optionpanel.handelPanel();
                runControllKeys();
            }
            else
            {
                _optionfrommain.handelPanel();
                handleMainBtnInteractable(true);
            }
        }
    }

    public void handleMainBtnInteractable(bool value)
    {
        Button[] mainBtns = _mainpage.GetComponentsInChildren<Button>();
        foreach (Button btn in mainBtns)
        {
            btn.interactable = value;
        }
    }

    // inventory 접근 함수
    public bool checkInventory(ItemObject _item, int _count)
    {
        Inventory inven = _inventory.transform.GetChild(1).GetComponent<Inventory>();
        return inven.CheckInven(_item, _count);
    }

    public void acquireItem(ItemObject _item, int _count)
    {
        _inventory.transform.GetChild(1).GetComponent<Inventory>().AcquireItem(_item, _count);
    }

    public void reductItem(ItemObject _item, int _count)
    {
        _inventory.transform.GetChild(1).GetComponent<Inventory>().ReductItem(_item, _count);
    }

    public void sellItem(int slotIdx, int _count, int _key)
    {
        if (_key == 1)
        {
            _inventory.transform.GetChild(1).GetComponent<Inventory>().SellInventoryItem(slotIdx, _count);
        }
        else if (_key == 2)
        {
            _inventory.transform.GetChild(0).GetComponent<Quickslot>().SellQuickslotItem(slotIdx, _count);
        }
        else if (_key == 3)
        {
            switch (slotIdx)
            {
                case 0:
                    _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Slot>().SetSlotCount(-1);
                    break;
                case 1:
                    _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Slot>().SetSlotCount(-1);
                    break;
                case 2:
                    _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Slot>().SetSlotCount(-1);
                    break;
                case 3:
                    _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(4).GetComponent<Slot>().SetSlotCount(-1);
                    break;
            }
        }
    }

    public void onSlotOverModal(string _text, Vector3 _position)
    {
        _inventory.transform.GetChild(3).gameObject.SetActive(true);
        _inventory.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>().text = _text;
        _inventory.transform.GetChild(3).transform.position = _position;
    }

    public void offSlotOverModal()
    {
        _inventory.transform.GetChild(3).gameObject.SetActive(false);
    }

    // 인벤 마우스 우클릭
    public void clickRightSlotModal(int _key, Vector3 _position, ItemObject _item, int _count, int _index)
    {
        isInvenRightModal = true;
        _inventory.transform.GetChild(4).gameObject.SetActive(true);
        _inventory.transform.GetChild(4).transform.position = _position;
        switch (_key)
        {
            case 1:
                _inventory.transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
                _inventory.transform.GetChild(4).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                _inventory.transform.GetChild(4).GetChild(0).GetComponent<Button>().onClick.AddListener(() => rightEquip(_item, _count, _index));
                break;
            case 2:
                _inventory.transform.GetChild(4).GetChild(2).gameObject.SetActive(true);
                _inventory.transform.GetChild(4).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                _inventory.transform.GetChild(4).GetChild(2).GetComponent<Button>().onClick.AddListener(() => rightQuick(_item, _count, _index));
                break;
            case 3:
                _inventory.transform.GetChild(4).GetChild(1).gameObject.SetActive(true);
                _inventory.transform.GetChild(4).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                _inventory.transform.GetChild(4).GetChild(1).GetComponent<Button>().onClick.AddListener(() => rightUse(_item, _count, _index));
                break;
            case 4:
                _inventory.transform.GetChild(4).GetChild(3).gameObject.SetActive(true);
                _inventory.transform.GetChild(4).GetChild(3).GetComponent<Button>().onClick.RemoveAllListeners();
                _inventory.transform.GetChild(4).GetChild(3).GetComponent<Button>().onClick.AddListener(() => rightDismiss(_item, _count, _index));
                break;

        }
    }

    // 인벤 우클릭 모달 close
    public void closeInvenRightClickModal()
    {
        isInvenRightModal = false;
        _inventory.transform.GetChild(4).gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            _inventory.transform.GetChild(4).GetChild(i).gameObject.SetActive(false);
        }
    }

    // 인벤토리 아이템 처리 - 우클릭
    public void rightEquip(ItemObject _item, int _count, int invenIdx)
    {
        ItemObject itemObj = null;
        int equiptCnt = -1;
        switch (_item.ItemCode)
        {
            case 400:
                equiptCnt = _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Slot>().getSlotItemCount();
                if (equiptCnt > 0)
                {
                    itemObj = _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Slot>().getSlotItemData();
                }

                _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Slot>().AddItem(_item);
                sellItem(invenIdx, _count, 1);
                setPlayerEquipSlot(_item.ItemCode, 0);
                break;
            case 401:
                equiptCnt = _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Slot>().getSlotItemCount();
                if (equiptCnt > 0)
                {
                    itemObj = _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Slot>().getSlotItemData();
                }

                _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(2).GetComponent<Slot>().AddItem(_item);
                sellItem(invenIdx, _count, 1);
                setPlayerEquipSlot(_item.ItemCode, 1);
                break;
            case 402:
                equiptCnt = _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Slot>().getSlotItemCount();
                if (equiptCnt > 0)
                {
                    itemObj = _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Slot>().getSlotItemData();
                }

                _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(3).GetComponent<Slot>().AddItem(_item);
                sellItem(invenIdx, _count, 1);
                setPlayerEquipSlot(_item.ItemCode, 2);
                break;
            case 403:
            case 404:
                equiptCnt = _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(4).GetComponent<Slot>().getSlotItemCount();
                if (equiptCnt > 0)
                {
                    itemObj = _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(4).GetComponent<Slot>().getSlotItemData();
                }

                _inventorypanel.transform.GetChild(0).GetChild(2).GetChild(4).GetComponent<Slot>().AddItem(_item);
                sellItem(invenIdx, _count, 1);
                setPlayerEquipSlot(_item.ItemCode, 3);
                break;
            case 502:
            case 503:
            case 504:
                int _cnt = _inventory.transform.GetChild(1).GetComponent<Inventory>().getLessBaitCount(_item.ItemCode);
                if(_cnt > _count) {
                    _cnt = _count;
                }
                _baseuipanel.transform.GetChild(3).GetChild(1).GetComponentInChildren<Slot>().AddItem(_item, _cnt);
                setPlayerQuickSlot(7, _item.ItemCode, _cnt);
                break;
            default:
                break;
        }

        if (itemObj != null)
        {
            acquireItem(itemObj, 1);
        }

        closeInvenRightClickModal();
    }

    public void clearBaitSlot() {
        _baseuipanel.transform.GetChild(3).GetChild(1).GetComponentInChildren<Slot>().ClearSlot();
        setPlayerQuickSlot(7, 0, 0);
    }

    public ItemObject getBaitSlotData() {
        return _baseuipanel.transform.GetChild(3).GetChild(1).GetComponentInChildren<Slot>().item;
    }

    public void rightQuick(ItemObject _item, int _count, int _index)
    {
        if (_inventory.transform.GetChild(0).GetComponent<Quickslot>().CheckInven(_item, _count))
        {
            _inventory.transform.GetChild(0).GetComponent<Quickslot>().AcquireItem(_item, _count);
            sellItem(_index, _count, 1);
        }
        else
        {
            OnResultNotificationPanel("퀵슬롯에 빈 공간이 없습니다.");
        }
        closeInvenRightClickModal();
    }

    public void rightDismiss(ItemObject _item, int _count, int _index)
    {
        if (checkInventory(_item, _count))
        {
            if (_item.Category == "옷")
            {
                sellItem(_index, _count, 3);
            }
            else
            {
                sellItem(_index, _count, 2);
            }
            acquireItem(_item, _count);
        }
        closeInvenRightClickModal();
    }

    public void rightUse(ItemObject _item, int _count, int _index)
    {
        _foodmanager.UseFood(_item.ItemCode);
        sellItem(_index, 1, 1);
        closeInvenRightClickModal();
    }

    public void quickUse(int _itemCode, int _count, int _slotIdx)
    {
        if (findItem(_itemCode).Category == "식량")
        {
            _foodmanager.UseFood(_itemCode);
        }

        if (_slotIdx == 7)
        {
            reductItem(findItem(_itemCode), -1 * _count);
        }
        else
        {
            sellItem(_slotIdx, _count, 2);
        }
        syncQuickSlot();
    }

    public Slot[] getInventorySlots()
    {
        return _inventory.transform.GetChild(1).GetComponent<Inventory>().getInventorySlots();
    }

    public Slot[] getQuickSlots() {
        return _inventory.transform.GetChild(0).GetComponent<Quickslot>().getInventorySlots();
    }

    public Slot[] getStorageSlots() {
        return _storagepanel.transform.GetChild(1).GetChild(4).GetComponent<Storage>().getInventorySlots();
    }

    public Slot[] getEquipSlots() {
        return _inventorypanel.transform.GetChild(0).GetChild(2).GetComponentsInChildren<Slot>();
    }

    // 창고 관련
    public void addToStorage(ItemObject _item, int _count, int slotIdx)
    {
        int value = _storagepanel.transform.GetChild(1).GetChild(4).GetComponent<Storage>().checkStorage(_item, _count);
        if (value <= 0)
        {
            _storagepanel.transform.GetChild(1).GetChild(4).GetComponent<Storage>().AcquireItem(_item, _count);
            _inventory.transform.GetChild(1).GetComponent<Inventory>().SellInventoryItem(slotIdx, _count);
        }
        else if (value == _count)
        {
            OnResultNotificationPanel("창고에 " + _item.Name + "이(가) 가득 찼습니다.");
        }
        else
        {
            _storagepanel.transform.GetChild(1).GetChild(4).GetComponent<Storage>().AcquireItem(_item, _count - value);
            _inventory.transform.GetChild(1).GetComponent<Inventory>().SellInventoryItem(slotIdx, _count - value);
        }
    }

    public void removeToStorage(ItemObject _item, int _count, int slotIdx)
    {
        int nowCnt = _count;
        while (nowCnt > 99)
        {
            if (checkInventory(_item, 99))
            {
                acquireItem(_item, 99);
                _storagepanel.transform.GetChild(1).GetChild(4).GetComponent<Storage>().AcquireItem(_item, -99);
                nowCnt -= 99;
            }
        }
        if (checkInventory(_item, nowCnt))
        {
            acquireItem(_item, nowCnt);
            _storagepanel.transform.GetChild(1).GetChild(4).GetComponent<Storage>().AcquireItem(_item, -1 * nowCnt);
        }

        if(_item.ItemCode == getBaitSlotData().ItemCode) {
            int _cnt = _inventory.transform.GetChild(1).GetComponent<Inventory>().getLessBaitCount(_item.ItemCode);
            rightEquip(_item, _cnt, 0);
        }
    }

    // 제작관련 함수
    public CraftingPanel getCraftPanel()
    {
        return _craftingpanel;
    }

    // 플레이어 함수 관련
    public void stopControllPlayer()
    {
        _playersystem._canMove = false;
    }

    public void runControllPlayer()
    {
        _playersystem._canMove = true;
    }

    public void runCookingAnimation()
    {
        _playersystem.setAnimator(6, 5.0f);
    }

    // input 관련 함수
    public void toggleCanInteract()
    {
        _playersystem.toggleCanInteract();
    }

    public void toggleCanAction()
    {
        _playersystem.toggleCanAction();
    }

    public void setCanInteract(bool value)
    {
        _playersystem.setCanInteract(value);
    }

    public void setCanAction(bool value)
    {
        _playersystem.setCanAction(value);
    }

    public void delayStopControllKeys()
    {
        Invoke("stopControllKeys", 0.25f);
    }

    public void stopControllKeys()
    {
        stopControllPlayer();
        setCanAction(false);
        setCanInteract(false);
    }

    public void delayRunControllKeys()
    {
        Invoke("runControllKeys", 0.2f);
    }

    public void runControllKeys()
    {
        runControllPlayer();
        setCanAction(true);
        setCanInteract(true);
    }

    public bool isPanelOpen()
    {
        return isInventoryOpen || isMapOpen || isStorageOpen || isTransactionOpen ||
                    isCraftOpen || isCookOpen || isBuildEventOpen || isConversationOpen;
    }

    // item 관련 함수
    public ItemObject findItem(int value)
    {
        return _itemmanager.FindItem(value);
    }

    // 퀵슬롯 변경 함수
    public void setEquipPointer(int num)
    {
        _nowequip.transform.SetLocalPositionAndRotation(new Vector3((num - 1) * 31 + 2, 0, 0), rotateZero);
    }

    // 퀵슬롯 동기 함수
    public void syncQuickSlot()
    {
        Slot[] baseuiQuick = _baseuipanel.transform.GetChild(3).GetChild(0).GetChild(0).GetComponentsInChildren<Slot>();
        Slot[] quickSlotData = _inventory.transform.GetChild(0).GetComponent<Quickslot>().getInventorySlots();

        for (int i = 0; i < 7; i++)
        {
            if (quickSlotData[i].itemCount <= 0)
            {
                baseuiQuick[i].SetSlotCount(-1 * baseuiQuick[i].itemCount);
            }
            else
            {
                baseuiQuick[i].AddItem(quickSlotData[i].item, quickSlotData[i].itemCount);
            }
        }
    }

    // 집인지 확인하는 코드
    public void setIsHome(bool value)
    {
        isMyHome = value;
        _baseuipanel.transform.GetChild(2).gameObject.SetActive(!value);
    }

    // 소지금 관련
    public int getPlayerGold()
    {
        int gold = _systemmanager.getPlayerGold();
        _invenMoney.text = gold.ToString();
        return gold;
    }

    public void addPlayerGold(int value)
    {
        _systemmanager.addPlayerGold(value);
        _invenMoney.text = getPlayerGold().ToString();
    }

    // 플레이어 선택
    public void selectPlayer()
    {
        _playersystem.ChangePlayerCharacter(selectCharacter);
    }

    public void setPlayerName(string _name)
    {
        _playersystem.setPlayerName(_name);
    }

    public string getPlayerName()
    {
        return _playersystem.getPlayerName();
    }


    // 플레이어 체력 / 기력
    public void healPlayer()
    {
        _playersystem.changeHealth(-10000);
        _playersystem.changeEnergy(-10000);
        _conversationpanel.selectHeal();
    }

    // 플레이어 장비 관련
    public void setPlayerQuickSlot(int index, int itemCode, int count)
    {
        _playersystem.setQuickItem(index, itemCode, count);
    }

    public void setPlayerEquipSlot(int itemCode, int idx)
    {
        _playersystem.setEquipItem(itemCode, idx);
    }

    // 사운드 관련
    public void playRandomBGM()
    {
        _systemmanager.setBackground(1);
    }

    public void BGMChanger()
    {
        _conversationpanel.selectMusic();
    }

    // 정령 버프 및 제단 관련 코드
    public void prayToSpirit(int _flowerCode)
    {
        if (_systemmanager._buffManager._isFlowerBuffActived || _systemmanager._buffManager._isPrayBuffActived)
        {
            _conversationpanel.conversationWhenAlterBuff(0);
        }
        else
        {
            _conversationpanel.conversationWhenAlterBuff(_flowerCode);
            quickUse(_flowerCode, 1, _playersystem._equipItem);
        }
    }

    public void prayToAltar(int _nowFlowerCode, int _newFlowerCode, int quickIdx)
    {
        int nowCode = _nowFlowerCode;
        if (_systemmanager._buffManager._isPrayBuffActived && _nowFlowerCode != _newFlowerCode)
        {
            nowCode = -1;
        }
        _conversationpanel.selectWhenAlterPray(nowCode, _newFlowerCode, quickIdx);
    }

    public void callSpiritBuff(int _flower)
    {
        _systemmanager._SpiritBuff.Spirit(findItem(_flower));
    }

    public void callPrayBuff(int _flower)
    {
        _alter.goPray(_flower);
    }

    public void resetPrayBuffFx()
    {
        _alter.buffEnd();
    }

    // 제사 며칠 째인지 얻어오는 함수
    public int getNowPrayDate()
    {
        return _systemmanager._PrayBuff.prayDay;
    }

    // 세계수 텔포
    public void doTeleport(int value)
    {
        _worldtree.doTeleport(value);
        _conversationpanel.resetConversationPanel();
    }

    // 제단 텔포
    public void upTeleport()
    {
        _alterdown.goUp();
        _conversationpanel.resetConversationPanel();
    }

    public void downTeleport()
    {
        _alterup.goDown();
        _conversationpanel.resetConversationPanel();
    }

    public void startTime(){
        _systemmanager.setTimeSystem(false);
    }

    // 하루 종료 함수 call
    public void sleep() {
        _systemmanager.playerDeath();
    }
    
    // 게임 시작 종료
    public void setGameState(bool value)
    {
        _systemmanager.setGameState(value);
    }

    public bool getGameState()
    {
        return _systemmanager.getGameState();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    // 아이템 정보 Load
    // 0 : 인벤 || 1 : 퀵슬롯 || 2. 장비 || 3. 창고
    public void loadItemData(int[,] data, int _key)
    {
        Slot[] slots = null;
        switch (_key)
        {
            case 0:
                slots = getInventorySlots();
                break;
            case 1:
                slots = getQuickSlots();
                break;
            case 2:
                slots = getEquipSlots();
                break;
            case 3:
                slots = getStorageSlots();
                break;
        }

        for (int i = 0; i < data.Length; i+=2)
        {
            if (data[i/2, 0] == 0)
            {
                continue;
            }

            if (_key == 1 && i/2 == 7)
            {
                _baseuipanel.transform.GetChild(3).GetChild(1).GetComponentInChildren<Slot>().AddItem(findItem(data[i/2, 0]), data[i/2, 1]);
                continue;
            }

            if(_key == 3) {
                slots[i/2].transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255);
            }
            slots[i/2].AddItem(findItem(data[i/2, 0]), data[i/2, 1]);
        }

        if (_key == 1)
        {
            syncQuickSlot();
        }
    }

    public void DayStart()
    {
        Transform trans = _daytime.GetComponent<RectTransform>().transform;
        Image img = _noontime.GetComponent<Image>();
        if (_systemmanager._buffManager.whitePray || _systemmanager._buffManager.whiteSpirit)
        {
            trans.SetLocalPositionAndRotation(trans.localPosition, Quaternion.Euler(180, 180, 15));
            img.fillAmount = 0.505f;
        }
        else
        {
            trans.SetLocalPositionAndRotation(trans.localPosition, Quaternion.Euler(180, 180, 0));
            img.fillAmount = 0.38f;
        }
    }
}
