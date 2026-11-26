using UnityEngine;

namespace CoffeeRush
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Coffee Rush/Game Config")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("Day")]
        [SerializeField, Min(10f)] private float dayDurationSeconds = 180f;
        [SerializeField, Min(1)] private int targetMoneyPerDay = 180;
        [SerializeField, Min(1)] private int startingReputation = 5;
        [SerializeField, Min(1)] private int maxReputation = 5;
        [SerializeField, Min(1)] private int startingMoney = 120;

        [Header("Orders")]
        [SerializeField, Min(1)] private int baseMaxConcurrentOrders = 2;
        [SerializeField, Min(5f)] private float orderPatienceSeconds = 45f;
        [SerializeField, Min(1f)] private float spawnIntervalSeconds = 8f;
        [SerializeField, Min(0)] private int wrongOrderReputationPenalty = 1;
        [SerializeField, Min(0)] private int expiredOrderReputationPenalty = 1;
        [SerializeField, Min(0)] private int correctOrderReputationBonus = 0;

        public float DayDurationSeconds => dayDurationSeconds;
        public int TargetMoneyPerDay => targetMoneyPerDay;
        public int StartingReputation => startingReputation;
        public int MaxReputation => maxReputation;
        public int StartingMoney => startingMoney;
        public int BaseMaxConcurrentOrders => baseMaxConcurrentOrders;
        public float OrderPatienceSeconds => orderPatienceSeconds;
        public float SpawnIntervalSeconds => spawnIntervalSeconds;
        public int WrongOrderReputationPenalty => wrongOrderReputationPenalty;
        public int ExpiredOrderReputationPenalty => expiredOrderReputationPenalty;
        public int CorrectOrderReputationBonus => correctOrderReputationBonus;
    }
}
