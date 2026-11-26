namespace CoffeeRush
{
    public sealed class ReputationService
    {
        public static ReputationService Instance { get; private set; }
        public int Current { get; private set; }
        public int Max { get; private set; }

        public ReputationService()
        {
            Instance = this;
        }

        public void Initialize(int current, int max)
        {
            Max = max;
            Current = current;
            Publish();
        }

        public void ApplyPenalty(int amount)
        {
            Current = System.Math.Max(0, Current - System.Math.Max(0, amount));
            Publish();
        }

        public void ApplyBonus(int amount)
        {
            Current = System.Math.Min(Max, Current + System.Math.Max(0, amount));
            Publish();
        }

        private void Publish() => GameEvents.RaiseReputationChanged(Current, Max);
    }
}
