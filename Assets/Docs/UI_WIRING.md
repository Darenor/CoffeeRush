# UI Wiring Guide

## Canvas
Use Canvas Scaler → Scale With Screen Size, reference resolution 1920×1080, Match 0.5. For pixel art use Point (no filter) texture import, consistent Pixels Per Unit, and consider Pixel Perfect Camera.

## Main Menu
Create panel with title/logo and buttons:
- Play → `MainMenuView.Play`
- Settings → `MainMenuView.OpenSettings`
- Quit → `MainMenuView.Quit` (hide in WebGL if preferred)

Attach `StatePanel` to panel and set `visibleState = MainMenu`.

## Settings
Slider range 0..1 → `SettingsView.volumeSlider`; Back button → `SettingsView.BackToMenu`. Attach StatePanel (`Settings`). In a pause-origin settings flow, production UI should store the previous state so Back returns to pause; the minimal helper returns to menu.

## Cafe HUD
- Money TMP label, reputation TMP label, timer TMP label, orders TMP label, timer Slider and notification TMP label → `HudView`.
- Pause button → `GameManager.TogglePause`.
- Add `BrewController` to an object and assign GameManager.
- Each ingredient button has `IngredientButton`, choose IngredientType in Inspector, assign BrewController, Button OnClick → `IngredientButton.Select`.
- Create order card prefab with `OrderCardView`. A production `OrderListPresenter` should listen to `GameEvents.OrderAdded`/`OrderRemoved`, instantiate/destroy cards and bind `OrderCardView.Bind(order, config.OrderPatienceSeconds + upgrades.GetExtraPatienceSeconds())`.
- Select an order card and set it on `ServeOrderButton.selectedOrderCard`. Ingredient controls collect selected ingredients; Brew/Serve button → `ServeOrderButton.BrewAndServe`.
- Clear button → `ServeOrderButton.ClearIngredients`; Undo → `ServeOrderButton.RemoveLastIngredient`.

## Pause
Panel with `StatePanel.visibleState = Paused`; Resume → `PauseView.Resume`; Settings → `PauseView.OpenSettings`; Menu → `PauseView.MainMenu`.

## Results
Panel with `DayResultView`, TMP title/summary. StatePanel can be controlled by custom presenter because result panel is shared by DayComplete and GameOver. Buttons: ContinueToUpgrades, BackToMenu, Retry.

## Upgrades
Panel with `StatePanel.visibleState = Upgrades`; `UpgradeShopView` gets catalog and content parent. Create a row prefab with `UpgradeRowView` and assign fields. Continue → `UpgradeShopView.ContinueDay`.

## Notes
`StatePanel` toggles the GameObject off when state doesn't match. Do not put `StatePanel` on the same GameObject as a controller that must stay subscribed while hidden; put it on the visual panel child. Ensure there is only one active `GameManager`.
