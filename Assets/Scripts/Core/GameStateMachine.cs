namespace CoffeeRush
{
    /// <summary>Small explicit State pattern; transitions are centralized and observable.</summary>
    public sealed class GameStateMachine
    {
        public GameState Current { get; private set; } = GameState.MainMenu;

        private bool CanTransition(GameState next)
        {
            if (Current == next) return true;
            switch (Current)
            {
                case GameState.MainMenu:
                    return next == GameState.Settings || next == GameState.Playing;
                case GameState.Settings:
                    return next == GameState.MainMenu || next == GameState.Paused;
                case GameState.Playing:
                    return next == GameState.Paused || next == GameState.DayComplete ||
                           next == GameState.GameOver || next == GameState.Upgrades;
                case GameState.Paused:
                    return next == GameState.Playing || next == GameState.Settings ||
                           next == GameState.MainMenu;
                case GameState.DayComplete:
                    return next == GameState.Upgrades || next == GameState.MainMenu ||
                           next == GameState.Playing;
                case GameState.Upgrades:
                    return next == GameState.Playing || next == GameState.MainMenu;
                case GameState.GameOver:
                    return next == GameState.MainMenu || next == GameState.Playing;
                default:
                    return false;
            }
        }

        public bool TrySet(GameState next)
        {
            if (!CanTransition(next)) return false;
            Current = next;
            GameEvents.RaiseStateChanged(Current);
            return true;
        }

        public void ForceSet(GameState next)
        {
            Current = next;
            GameEvents.RaiseStateChanged(Current);
        }
    }
}
