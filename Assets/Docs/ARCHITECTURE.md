# Architecture notes and known integration points

## Patterns
- State: `GameStateMachine` centralizes legal transitions and publishes `GameEvents.StateChanged`.
- Observer: `GameEvents` decouples services from presentation.
- Factory: `OrderFactory` creates random orders from the configured recipe catalog.
- Strategy-like validation: `RecipeValidator` encapsulates recipe matching; reward modifiers are encapsulated by `UpgradeManager`.

## Extension
To add a recipe: create a `RecipeData` asset, assign ingredients/reward/preparation time, add it to `GameManager.recipes`. No edits to OrderFactory, RecipeValidator, or UI logic are required.
To add a passive upgrade: add an `UpgradeData` asset and a new `UpgradeEffect` case, then implement its aggregation in `UpgradeManager`.

## Important production hardening
- For a polished submission, add automated EditMode/PlayMode tests for recipe validation, order expiry, purchase affordability, state transitions and save round-trip.
- Add a small `OrderCardView` selection button to call `OrderListPresenter.SelectOrder(this)`.
- Keep `GameManager` in one bootstrap scene or use a singleton guard (already present) and make sure scene-specific UI survives transitions as intended.
- Use a single UI result panel with separate state visibility for DayComplete/GameOver; a custom presenter is preferable to a single-state `StatePanel`.
- Decide game design for time-up: this starter treats time-up as end-of-shift and allows upgrades/next day. Goal reached is success; reputation zero is game over.
- `ErrorShield` currently resets each day, which matches a per-day extra attempt upgrade.
- Consider changing `OrderManager.RegisterFailure` to inject `ReputationService` instead of the static `Instance` if using dependency injection in a larger project.
