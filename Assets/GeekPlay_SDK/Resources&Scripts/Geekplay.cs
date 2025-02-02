using CrazyGames;
using GamePix;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GeekplaySchool
{
    [System.Serializable]
    public class Rewards
    {
        public string rewardName;
        public UnityEvent rewardEvent;
    }

    [System.Serializable]
    public class Purchases
    {
        public string itemName;
        public UnityEvent purchaseEvent;
    }

    public enum Platform
    {
        Editor,
        Yandex,
        VK,
        GameArter,
        CrazyGames,
        VKPlay,
        Kongregate,
        GameDistribution,
        GamePix,
        OK
    }
    public class Geekplay : MonoBehaviour
    {
        public string language; //язык
        public bool mobile; //Устройство игрока мобильное?
        public bool SoundOn = true; //Звук включен?
        public PlayerData PlayerData; //сохраняемые данные
        [SerializeField] private Rewards[] rewardsList; //список ревардов
        [SerializeField] private Purchases[] purchasesList; //список покупок

        [Space(50)]
        public Platform Platform; //Платформа
        [SerializeField] private GameObject gameArterPrefab; //Префаб площадки GameArter
        public GameObject leaderboardBtn; //КНОПКА, ОТКРЫВАЮЩАЯ ЛИДЕРБОРД
        public static Geekplay Instance;
        private bool canShowAd = true; //Можно ли проигрывать рекламу на вк?
        private string developerNameYandex = "GeeKid%20-%20школа%20программирования";

        private IEnumerator cor;
        private string rewardTag; //Тэг награды
        public bool adOpen; //Реклама открыта?
        private string purchasedTag; //Тэг покупки
        private bool wasLoad; //Игра загружалась?
        private bool canAd;
        string colorDebug = "yellow"; //Цвет Дебага

        public int MultipleSpeedBuff = 1;

        public bool BuffIncreaseHP { get; internal set; }
        public bool BuffAcceleration { get; internal set; }
        public bool BuffDoubleSlap { get; internal set; }
        public bool BuffIncreasePower { get; internal set; }

        public float TimeToShowReward;
        public float TimePasedFromLastReward;
        public bool CanShowReward => TimePasedFromLastReward > TimeToShowReward && !BuffAcceleration && !BuffDoubleSlap && !BuffIncreasePower && !BuffIncreaseHP;
        public Action OnShowBuffs;

        public bool IsTournamentStart;
        #region Pause
        public GameObject pausePopup;
        public bool canPause = true;
        public bool isOnPause = false;
        public bool isOnPromocodeZone = false;
        public void ChangeCanPause(bool canPause)
        {
            this.canPause = canPause;
        }
        #endregion

        #region Leaderboard

        public List<string> lS = new List<string>();
        public List<string> lN = new List<string>();
        public int leaderNumber;
        public int leaderNumberN;
        //public LeaderboardInGame leaderboardInGame;
        public float remainingTimeUntilUpdateLeaderboard;
        public float timeToUpdateLeaderboard = 60;
        public string lastLeaderText;

        #endregion


        public GameObject reviewPanel;
        private bool openReview;

        [Header("Interstitial")]
        [SerializeField] private TextMeshProUGUI interstitialRemainingText;
        [SerializeField] private float interstitialTime = 75;
        [SerializeField] private float pastedTimeFromLastInterstitial;

        //private GameObject curtain;
        //private Image curtainLoadVisual;

        //private SceneLoader sceneLoader;
        public LeaderBoard leaderboard;

        public void Awake()
        {
            GameReady();
            AudioListener.volume = PlayerData.IsVolumeOn ? 1 : 0;

           // sceneLoader = new SceneLoader();

            isOnPause = false;

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            if (Platform == Platform.GameDistribution || Platform == Platform.CrazyGames || Platform == Platform.Editor)
            {
                AfterPlatformChange();
            }
        }

        private void Start()
        {
            pastedTimeFromLastInterstitial = interstitialTime;


            //if (Platform == Platform.Yandex)
            //{
            //    Utils.CheckPlayGame(284619);
            //    Utils.CheckPlayGame(295391);
            //    Utils.CheckPlayGame(289951);
            //    Utils.CheckPlayGame(227711);
            //}
        }
        private void Update()
        {
            pastedTimeFromLastInterstitial -= Time.deltaTime;
            TimePasedFromLastReward += Time.deltaTime;

            remainingTimeUntilUpdateLeaderboard -= Time.deltaTime;
        }

        //public void LoadScene(int sceneIndex)
        //{
        //    //sceneLoader.LoadScene(sceneIndex);
        //}
        //public void EnablePlayedGameToggle(int id)
        //{
        //    ourGame.EnabledGameToggle(id);
        //}

        //public void DisablePlayedGameToggle(int id)
        //{
        //    ourGame.DisableGameToggle(id);
        //}

        public void SubscribeOnPurshace(string tag, UnityAction action)
        {
            for (int i = 0; i < purchasesList.Length; i++)
            {
                if (purchasesList[i].itemName == tag)
                {
                    purchasesList[i].purchaseEvent.AddListener(action);
                }
            }
        }
        public void UnsubscribeOnPurshace(string tag, UnityAction action)
        {
            for (int i = 0; i < purchasesList.Length; i++)
            {
                if (purchasesList[i].itemName == tag)
                {
                    purchasesList[i].purchaseEvent.RemoveListener(action);
                }
            }
        }


        public void SubscribeOnReward(string tag, UnityAction action)
        {
            for (int i = 0; i < rewardsList.Length; i++)
            {
                if (rewardsList[i].rewardName == tag)
                {
                    rewardsList[i].rewardEvent.AddListener(action);
                }
            }
        }
        public void UnsubscribeOnReward(string tag, UnityAction action)
        {
            for (int i = 0; i < rewardsList.Length; i++)
            {
                if (rewardsList[i].rewardName == tag)
                {
                    rewardsList[i].rewardEvent.RemoveListener(action);
                }
            }
        }

        //РЕКЛАМА
        public void OnRewarded() //ВОЗНАГРАЖДЕНИЕ ПОСЛЕ ПРОСМОТРА РЕКЛАМЫ
        {
            if (Platform == Platform.CrazyGames || Platform == Platform.GamePix)
                ResumeMusAndGame();

            for (int i = 0; i < rewardsList.Length; i++)
            {
                if (rewardTag == rewardsList[i].rewardName)
                {
                    rewardsList[i].rewardEvent.Invoke();
                    Save();
                }
            }
            if (cor != null)
                StopCoroutine(cor);
            cor = AdOff();
            StartCoroutine(cor);
        }
        public void SetPurchasedItem() //начислить уже купленные предметы на старте
        {
            for (int i = 0; i < purchasesList.Length; i++)
            {
                if (PlayerData.LastBuy == purchasesList[i].itemName)
                {
                    purchasesList[i].purchaseEvent.Invoke();
                    PlayerData.LastBuy = "";
                    Save();
                }
            }
        }
        IEnumerator AdOff() //ТАЙМЕР С ВЫКЛЮЧЕНИЕМ РЕКЛАМЫ
        {
            canAd = false;
            yield return new WaitForSeconds(180);
            canAd = true;
        }

        IEnumerator AdOn() //ТАЙМЕР БЕЗ ВЫКЛЮЧЕНИЯ РЕКЛАМЫ
        {
            yield return new WaitForSeconds(180);
            canAd = true;
        }

        IEnumerator CanAdShow()
        {
            yield return new WaitForSeconds(60);
            canShowAd = true;
        }

        public void ShowInterstitialAd() //МЕЖСТРАНИЧНАЯ РЕКЛАМА - ПОКАЗАТЬ
        {
            canPause = false;
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>INTERSTITIAL SHOW</color>");
                    break;
                case Platform.Yandex:
                    Utils.AdInterstitial();
                    break;
                case Platform.VK:
                    if (canShowAd)
                    {
                        canShowAd = false;
                        StartCoroutine(CanAdShow());
                        Utils.VK_Interstitial();
                    }
                    break;
                case Platform.CrazyGames:
                    if (canAd)
                    {
                        CrazyAds.Instance.beginAdBreak(ResumeMusAndGame, ResumeMusAndGame);
                        StopMusAndGame();
                    }
                    break;
                case Platform.GameDistribution:
                    GameDistribution.Instance.ShowAd();
                    break;
                case Platform.GamePix:
                    StopMusAndGame();
                    Gpx.Ads.InterstitialAd(ResumeMusAndGame);
                    break;
                case Platform.OK:
                    Utils.OK_Interstitial();
                    break;
            }
        }

        public void ShowRewardedAd(string idOrTag) //РЕКЛАМА С ВОЗНАГРАЖДЕНИЕМ - ПОКАЗАТЬ
        {
            canPause = false;
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>REWARD SHOW</color>");
                    rewardTag = idOrTag;
                    OnRewarded();
                    break;
                case Platform.Yandex:
                    rewardTag = idOrTag;
                    Utils.AdReward();
                    break;
                case Platform.VK:
                    canShowAd = false;
                    StartCoroutine(CanAdShow());
                    rewardTag = idOrTag;
                    Utils.VK_Rewarded();
                    break;
                case Platform.CrazyGames:
                    rewardTag = idOrTag;
                    CrazyAds.Instance.beginAdBreakRewarded(OnRewarded, ResumeMusAndGame);
                    StopMusAndGame();
                    break;
                case Platform.GameDistribution:
                    rewardTag = idOrTag;
                    GameDistribution.Instance.ShowRewardedAd();
                    break;
                case Platform.GamePix:
                    rewardTag = idOrTag;
                    StopMusAndGame();
                    Gpx.Ads.RewardAd(OnRewarded, OnRewarded);
                    break;
                case Platform.OK:
                    rewardTag = idOrTag;
                    Utils.OK_ShowRewardedAd();
                    break;
            }
        }

        public void ShowBannerAd()
        {
            switch (Platform)
            {
                case Platform.Editor:
#if INIT_DEBUG
                Debug.Log($"<color={colorDebug}>BANNER SHOW</color>");
#endif
                    break;
                case Platform.VK:
                    Utils.VK_Banner();
                    break;
                case Platform.OK:
                    Utils.OK_ShowBannerAds();
                    break;
            }
        }

        //СОЦИАЛЬНЫЕ ФУНКЦИИ
        public void OpenOtherGames() //ССЫЛКА НА ДРУГИЕ ИГРЫ
        {
            switch (Platform)
            {
                case Platform.Editor:
#if INIT_DEBUG
                Debug.Log($"<color={colorDebug}>OPEN OTHER GAMES</color>");
#endif
                    break;
                case Platform.Yandex:
                    var domain = Utils.GetDomain();
                    Application.OpenURL($"https://yandex.{domain}/games/developer?name=" + developerNameYandex);
                    break;
                case Platform.VK:
                    rewardTag = "Group";
                    Utils.VK_ToGroup();
                    break;
            }
        }
        public void StartRatingSystem()
        {
            if (!openReview && !reviewPanel.activeSelf && Platform == Platform.Yandex)
            {
                if (PlayerData.Review == false)
                {
                    //Utils.OpenReviewInGame();
                    OpenReviewPanel();
                    PlayerData.Review = true;
                    Save();
                }
            }
        }

        public void RateGameFunc() //ПРОСЬБА ОЦЕНИТЬ ИГРУ ПРИ 4 И 5 ЗВЕЗДАХ
        {
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>REWIEV GAME</color>");
                    break;
                case Platform.Yandex:
                    Utils.RateGame();
                    break;
                case Platform.OK:
                    Utils.OK_ShowRating();
                    break;
            }
        }

        public void OpenReviewPanel()
        {
            reviewPanel.SetActive(true);
            openReview = true;
        }

        public void SetLeaderboard(string leaderboardName, int value) //ЗАНЕСТИ В ЛИДЕРБОРД
        {
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>SET LEADERBOARD:</color> {value}");
                    break;
                case Platform.Yandex:
                    Utils.SetToLeaderboard(value, leaderboardName);
                    break;
                case Platform.VK:
                    if (mobile)
                        Utils.VK_OpenLeaderboard(value);
                    break;
            }
        }

        public void ToStarGame() //ДОБАВИТЬ В ИЗБРАННОЕ (ВК)
        {
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>GAME TO STAR</color>");
                    break;
                case Platform.Yandex:
                    break;
                case Platform.VK:
                    Utils.VK_Star();
                    break;
            }
        }

        public void ShareGame() //ПОДЕЛИТЬСЯ ИГРОЙ (ВК)
        {
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>SHARE</color>");
                    break;
                case Platform.Yandex:
                    break;
                case Platform.VK:
                    Utils.VK_Share();
                    break;
            }
        }

        public void InvitePlayers() //ПРИГЛАСИТЬ ИГРОКОВ (ВК)
        {
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>INVITE</color>");
                    break;
                case Platform.Yandex:
                    break;
                case Platform.VK:
                    Utils.VK_Invite();
                    break;
            }
        }

        public void LeaderboardBtn(int value) //Для кнопки лидерборда в VK
        {
            //value = playerData.Level;
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>SET LEADERBOARD:</color> {value}");
                    break;
                case Platform.Yandex:
                    break;
                case Platform.VK:
                    Utils.VK_OpenLeaderboard(value);
                    break;
            }
        }

        public void GetLeadersScore(string valueAndName)
        {
            string[] parts = valueAndName.Split(',');
            string value = parts[0];
            string leaderboardName = parts[1];

            lS.Add(value);

            if (leaderNumber < 6)
            {
                leaderNumber += 1;
                Utils.GetLeaderboard("score", leaderNumber, leaderboardName);
            }
            else if(leaderNumber == 6)
            {
                EndGetLeaderboardsValue();
            }
        }

        public void GetLeadersName(string valueAndName)
        {
            string[] parts = valueAndName.Split(',');
            string value = parts[0];
            string leaderboardName = parts[1];

            lN.Add(value);

            if (leaderNumberN < 6)
            {
                leaderNumberN += 1;
                Utils.GetLeaderboard("name", leaderNumberN, leaderboardName);
            }
        }

        public void EndGetLeaderboardsValue()
        {
            leaderNumber = 0;
            leaderNumberN = 0;

            if (leaderboard == null) Debug.Log("Leaderboard is null");
            
            leaderboard.SetLeadersView(lN.ToArray(), lS.ToArray(), lS.Count);

            lN.Clear();
            lS.Clear();
        }

        //СОХАРЕНИЕ И ЗАГРУЗКА
        public void Save() //СОХРАНЕНИЕ
        {
            string jsonString = "";

            switch (Platform)
            {
                case Platform.Editor:
                    jsonString = JsonUtility.ToJson(PlayerData);
                    PlayerPrefs.SetString("PlayerData", jsonString);
                    Debug.Log("SAVE: " + jsonString);
                    break;
                case Platform.Yandex:
                    jsonString = JsonUtility.ToJson(PlayerData);
                    Utils.SaveExtern(jsonString);
                    break;
                case Platform.VK:
                    if (wasLoad == false)
                    {
                        break;
                    }
                    jsonString = JsonUtility.ToJson(PlayerData);
                    Utils.VK_Save(jsonString);
                    break;
                case Platform.CrazyGames:
                    jsonString = JsonUtility.ToJson(PlayerData);
                    PlayerPrefs.SetString("PlayerData", jsonString);
                    break;
                case Platform.Kongregate:
                    jsonString = JsonUtility.ToJson(PlayerData);
                    PlayerPrefs.SetString("PlayerData", jsonString);
                    break;
            }
        }

        public void SetPlayerData(string value) //ЗАГРУЗКА
        {
            PlayerData = JsonUtility.FromJson<PlayerData>(value);
            Debug.Log("LOAD " + JsonUtility.ToJson(PlayerData));
            
            SetPurchasedItem();
        }


        //АВТОМАТИЧЕСКАЯ СМЕНА ПЛАТФОРМЫ
        public void ChangePlatform(string dom)
        {
            if (dom.Contains("yandex"))
            {
                Platform = Platform.Yandex;
                AfterPlatformChange();
                return;
            }
            else if (dom.Contains("vk") && dom.Contains("play"))
            {
                Platform = Platform.VKPlay;
                AfterPlatformChange();
                return;
            }
            else if (dom.Contains("vk"))
            {
                Platform = Platform.VK;
                AfterPlatformChange();
                return;
            }
            else if (dom.Contains("kongregate"))
            {
                Platform = Platform.Kongregate;
                AfterPlatformChange();
                return;
            }
            else if (dom.Contains("gamepix"))
            {
                Platform = Platform.GamePix;
                AfterPlatformChange();
                return;
            }

            Debug.Log("PLATFORM CHANGE: " + Platform);
            Debug.Log("PLATFORM DOMAIN: " + dom);
            AfterPlatformChange();
        }

        void AfterPlatformChange()
        {
#if UNITY_EDITOR
            Platform = Platform.Editor;
#endif
            canAd = false;
            StartCoroutine(AdOn());
            //Старт площадок
            switch (Platform)
            {
                case Platform.Editor:
                    if (PlayerPrefs.HasKey("PlayerData"))
                    {
                        string jsonString = PlayerPrefs.GetString("PlayerData");
                        PlayerData = JsonUtility.FromJson<PlayerData>(jsonString);
                    }
                    else
                    {
                        PlayerData = new PlayerData();
                    }
                    language = "ru"; //ВЫБРАТЬ ЯЗЫК ДЛЯ ТЕСТОВ. ru/en/tr/
                    Localization();
                    break;
                case Platform.Yandex:
                    language = Utils.GetLang();
                    Localization();
                    break;
                case Platform.VK:
                    language = "ru";
                    Localization();
                    StartCoroutine(BannerVK());
                    StartCoroutine(RewardLoad());
                    StartCoroutine(InterLoad());
                    if (wasLoad)
                        Utils.VK_Load();
                    break;
                case Platform.CrazyGames:
                    language = "en";
                    Localization();
                    if (PlayerPrefs.HasKey("PlayerData"))
                    {
                        string jsonString = PlayerPrefs.GetString("PlayerData");
                        PlayerData = JsonUtility.FromJson<PlayerData>(jsonString);
                    }
                    else
                    {
                        PlayerData = new PlayerData();
                    }
                    break;
                case Platform.Kongregate:
                    PlayerData = new PlayerData();
                    language = "en";
                    Localization();
                    if (PlayerPrefs.HasKey("PlayerData"))
                    {
                        string jsonString = PlayerPrefs.GetString("PlayerData");
                        PlayerData = JsonUtility.FromJson<PlayerData>(jsonString);
                    }
                    else
                    {
                        PlayerData = new PlayerData();
                    }
                    break;
                case Platform.GameDistribution:
                    PlayerData = new PlayerData();
                    language = "en";
                    Localization();
                    if (PlayerPrefs.HasKey("PlayerData"))
                    {
                        string jsonString = PlayerPrefs.GetString("PlayerData");
                        PlayerData = JsonUtility.FromJson<PlayerData>(jsonString);
                    }
                    else
                    {
                        PlayerData = new PlayerData();
                    }
                    GameDistribution.OnResumeGame += ResumeMusAndGame;
                    GameDistribution.OnPauseGame += StopMusAndGame;
                    GameDistribution.OnPreloadRewardedVideo += OnPreloadRewardedVideo;
                    GameDistribution.OnRewardedVideoSuccess += OnRewardedVideoSuccess;
                    GameDistribution.OnRewardedVideoFailure += OnRewardedVideoFailure;
                    GameDistribution.OnRewardGame += OnRewarded;
                    break;
                case Platform.GamePix:
                    PlayerData = new PlayerData();
                    language = "en";
                    Localization();
                    if (PlayerPrefs.HasKey("PlayerData"))
                    {
                        string jsonString = PlayerPrefs.GetString("PlayerData");
                        PlayerData = JsonUtility.FromJson<PlayerData>(jsonString);
                    }
                    else
                    {
                        PlayerData = new PlayerData();
                    }
                    break;
            }
        }

        //ЗАГРУЗКА РЕКЛАМЫ
        void LoadBanner()
        {
            if (Platform == Platform.OK)
            {
                Utils.OK_RequestBannerAds();
            }
        }
        IEnumerator RewardLoad()
        {
            yield return new WaitForSeconds(15);
            switch (Platform)
            {
                case Platform.Editor:
                    Debug.Log($"<color={colorDebug}>REWARD LOAD</color>");
                    break;
                case Platform.VK:
                    Utils.VK_AdRewardCheck();
                    break;
                case Platform.OK:
                    Utils.OK_LoadRewardedAd();
                    break;
            }
        }

        IEnumerator InterLoad()
        {
            while (true)
            {
                yield return new WaitForSeconds(15);
                switch (Platform)
                {
                    case Platform.Editor:
                        Debug.Log($"<color={colorDebug}>INTERSTITIAL LOAD</color>");
                        break;
                    case Platform.VK:
                        Utils.VK_AdInterCheck();
                        break;
                }
            }
        }

        IEnumerator BannerVK()
        {
            yield return new WaitForSeconds(5);
            ShowBannerAd();
        }

        //ПЕРЕВОД
        public void Localization()
        {

        }

        //ВНУТРИИГРОВЫЕ ПОКУПКИ
        public void RealBuyItem(string idOrTag) //открыть окно покупки
        {
            switch (Platform)
            {
                case Platform.Editor:
                    purchasedTag = idOrTag;
                    OnPurchasedItem();
                    Debug.Log($"<color={colorDebug}>PURCHASE: </color> {purchasedTag}");
                    break;
                case Platform.Yandex:
                    PlayerData.LastBuy = idOrTag;
                    purchasedTag = idOrTag;
                    string jsonString = "";
                    jsonString = JsonUtility.ToJson(PlayerData);
                    Utils.BuyItem(idOrTag, jsonString);
                    break;
                case Platform.VK:
                    purchasedTag = idOrTag;
                    Debug.Log($"<color={colorDebug}>PURCHASE VK</color>");
                    break;
                case Platform.Kongregate:
                    purchasedTag = idOrTag;
                    Utils.Kongregate_InApp(idOrTag);
                    break;
            }
        }

        private void OnPurchasedItem() //начислить покупку (при удачной оплате)
        {
            PlayerData.LastBuy = "";
            for (int i = 0; i < purchasesList.Length; i++)
            {
                if (purchasedTag == purchasesList[i].itemName)
                {
                    purchasesList[i].purchaseEvent.Invoke();
                    Save();
                }
            }
        }

        public void CheckBuysOnStart(string idOrTag) //проверить покупки на старте
        {
            Utils.CheckBuyItem(idOrTag);
        }



        //СОБЫТИЯ ДЛЯ GAMEDISTRIBUTION
        public void OnPreloadRewardedVideo(int loaded)
        {
            // Feedback about preloading ad after called GameDistribution.Instance.PreloadRewardedAd
            // 0: SDK couldn't preload ad
            // 1: SDK preloaded ad
        }
        public void OnRewardedVideoSuccess()
        {
            // Rewarded video succeeded/completed.;
        }

        public void OnRewardedVideoFailure()
        {
            // Rewarded video failed.;
        }




        //ПАУЗА И ПРОДОЛЖЕНИЕ ИГРЫ
        public void StopMusAndGame()
        {
            adOpen = true;
            canShowAd = false;
            canPause = false;
            StartCoroutine(CanAdShow());
            AudioListener.volume = 0;
            AudioListener.pause = true;
            Time.timeScale = 0;
        }

        public void ResumeMusAndGame()
        {
            pastedTimeFromLastInterstitial = interstitialTime;

            adOpen = false;
            AudioListener.volume = PlayerData.IsVolumeOn ? 1 : 0;
            AudioListener.pause = false;
            Time.timeScale = 1;
        }

        //ФОКУС И ЗВУК
        void OnApplicationFocus(bool hasFocus)
        {
            //Silence(!hasFocus);
        }

        void OnApplicationPause(bool isPaused)
        {
            Silence(isPaused);
        }

        private void Silence(bool silence)
        {
            AudioListener.volume = PlayerData.IsVolumeOn ? 1 : 0;
            Time.timeScale = isOnPause ? 0 : 1;

            if (adOpen)
            {
                Time.timeScale = 0;
                AudioListener.volume = 0;
            }
            if (!SoundOn)
            {
                AudioListener.volume = 0;
            }
        }

        public void ItIsMobile()
        {
            mobile = true;

            //вставить включение мобильного управления
        }

        public void GameReady()
        {
            if (Platform == Platform.Yandex)
                Utils.GameReady();
        }

    }
}