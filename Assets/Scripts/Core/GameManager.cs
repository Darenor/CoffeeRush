using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoffeeRush
{
    public sealed class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Configuration")]
        [SerializeField] private GameConfig config;
        [SerializeField] private List<RecipeData> recipes = new List<RecipeData>();
        [SerializeField] private List<UpgradeData> upgrades = new List<UpgradeData>();

        private SaveService saveService;
        private SaveData saveData;
        private EconomyService economy;
        private ReputationService reputation;
        private UpgradeManager upgradeManager;
        private OrderManager orderManager;
        private DayController dayController;
        private GameStateMachine stateMachine;
        private int currentDay = 1;
        private GameState previousState;

        public GameState State => stateMachine == null ? GameState.MainMenu : stateMachine.Current;
        public EconomyService Economy => economy;
        public ReputationService Reputation => reputation;
        public UpgradeManager Upgrades => upgradeManager;
        public OrderManager Orders => orderManager;
        public DayController Day => dayController;
        public GameConfig Config => config;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (config == null)
            {
                Debug.LogError("GameManager: assign GameConfig in Inspector.");
                enabled = false;
                return;
            }
            if (recipes == null || recipes.Count == 0)
            {
                Debug.LogError("GameManager: assign at least one RecipeData asset.");
                enabled = false;
                return;
            }

            saveService = new SaveService();
            saveData = saveService.Load(config.StartingMoney);
            currentDay = Mathf.Max(1, saveData.currentDay);
            economy = new EconomyService();
            economy.Initialize(saveData);
            reputation = new ReputationService();
            reputation.Initialize(config.StartingReputation, config.MaxReputation);
            upgradeManager = new UpgradeManager(upgrades, economy);
            upgradeManager.Load(saveData);
            orderManager = new OrderManager(recipes, new OrderFactory(), config, economy, upgradeManager);
            dayController = new DayController(config, orderManager, economy, reputation);
            stateMachine = new GameStateMachine();
            stateMachine.ForceSet(GameState.MainMenu);
            AudioListener.volume = saveData.masterVolume;
        }

        private void Update()
        {
            if (stateMachine != null && stateMachine.Current == GameState.Playing)
                dayController.Tick(Time.deltaTime);
        }

        public void StartNewDay()
        {
            if (State != GameState.MainMenu && State != GameState.Upgrades &&
                State != GameState.DayComplete && State != GameState.GameOver &&
                State != GameState.Paused) return;

            reputation.Initialize(config.StartingReputation, config.MaxReputation);
            dayController.StartDay(currentDay);
            stateMachine.ForceSet(GameState.Playing);
            SaveProgress();
        }

        public void StartNewRun()
        {
            currentDay = 1;
            reputation.Initialize(config.StartingReputation, config.MaxReputation);
            StartNewDay();
        }

        public void TogglePause()
        {
            if (State == GameState.Playing) stateMachine.TrySet(GameState.Paused);
            else if (State == GameState.Paused) stateMachine.TrySet(GameState.Playing);
        }

        public void OpenSettings()
        {
            if (State != GameState.MainMenu && State != GameState.Paused)
                return;

            previousState = State;
            stateMachine.TrySet(GameState.Settings);
        }
        
        public void CloseSettings()
        {
            if (State != GameState.Settings)
                return;

            stateMachine.TrySet(previousState);
        }

        public void ReturnToMenu()
        {
            stateMachine.ForceSet(GameState.MainMenu);
            SaveProgress();
        }

        public void OpenUpgrades()
        {
            if (State != GameState.MainMenu &&
                State != GameState.DayComplete)
                return;
            
            if (State == GameState.DayComplete)
                currentDay++;
            stateMachine.ForceSet(GameState.Upgrades);
            SaveProgress();
        }

        public bool PurchaseUpgrade(UpgradeData data)
        {
            bool purchased = upgradeManager.TryPurchase(data);
            if (purchased) SaveProgress();
            return purchased;
        }

        public void ServeOrder(string orderId, IReadOnlyList<IngredientType> ingredients)
        {
            if (State != GameState.Playing) return;
            orderManager.TryServe(orderId, ingredients);
            if (reputation.Current <= 0 && dayController.IsRunning)
                dayController.Tick(0f);
            SaveProgress();
        }

        public float GetPreparationDuration(int ingredientCount)
        {
            float baseDuration = 2f;
            foreach (RecipeData recipe in recipes)
                if (recipe != null && recipe.Ingredients.Length == ingredientCount)
                    baseDuration = Mathf.Min(baseDuration, recipe.PreparationSeconds);
            return Mathf.Max(0.15f, baseDuration * upgradeManager.GetPreparationMultiplier());
        }

        public void SetVolume(float value)
        {
            value = Mathf.Clamp01(value);
            AudioListener.volume = value;
            PlayerPrefs.SetFloat("CoffeeRush_MasterVolume", value);
            PlayerPrefs.Save();
            saveData.masterVolume = value;
            SaveProgress();
        }

        private void OnEnable() => GameEvents.DayFinished += HandleDayFinished;
        private void OnDisable() => GameEvents.DayFinished -= HandleDayFinished;

        private void HandleDayFinished(DaySummary summary)
        {
            if (summary.Reputation <= 0)
                stateMachine.ForceSet(GameState.GameOver);
            else if (summary.GoalReached)
                stateMachine.ForceSet(GameState.DayComplete);
            else
                stateMachine.ForceSet(GameState.DayComplete); // time-up is a completed shift, not necessarily a loss
            SaveProgress();
        }

        public void SaveProgress()
        {
            if (saveService == null || saveData == null || economy == null) return;
            saveData.wallet = economy.Wallet;
            saveData.bestDayRevenue = economy.BestDayRevenue;
            saveData.bestOrdersCompleted = economy.BestOrdersCompleted;
            saveData.currentDay = currentDay;
            saveData.upgrades = upgradeManager.ToSaveEntries();
            saveService.Save(saveData);
        }

        public void QuitToDesktop()
        {
            SaveProgress();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
