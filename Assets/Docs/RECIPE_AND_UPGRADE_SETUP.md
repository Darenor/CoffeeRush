# ScriptableObject setup

## Recipes
Create four assets via Assets → Create → Coffee Rush → Recipe:
1. `Recipe_Americano`: id `americano`, name `Американо`, ingredients Coffee + Water, reward 20, prep 2.0.
2. `Recipe_Cappuccino`: id `cappuccino`, name `Капучино`, ingredients Coffee + Milk, reward 25, prep 2.5.
3. `Recipe_Latte`: id `latte`, name `Латте`, ingredients Coffee + Milk + MilkFoam, reward 30, prep 3.0.
4. `Recipe_LemonTea`: id `lemon_tea`, name `Чай с лимоном`, ingredients Tea + Water + Lemon, reward 22, prep 2.0.

## Upgrades
Create assets via Assets → Create → Coffee Rush → Upgrade. Suggested starting catalog:
- `Upgrade_FastHands`: FasterPreparation, effectValue 0.10, cost 150, maxLevel 3.
- `Upgrade_BonusTips`: BonusRewardPercent, effectValue 10, cost 200, maxLevel 3.
- `Upgrade_ExtraQueue`: ExtraConcurrentOrder, effectValue 1, cost 250, maxLevel 1.
- `Upgrade_SecondChance`: ErrorShield, effectValue 1, cost 300, maxLevel 1.
- `Upgrade_PatientGuests`: LongerOrderPatience, effectValue 5 seconds, cost 150, maxLevel 3.

`effectValue` means:
- FasterPreparation: fractional reduction per level (0.10 = 10%).
- BonusRewardPercent: percentage points per level.
- ExtraConcurrentOrder: extra simultaneous order slots per level.
- ErrorShield: number of mistakes ignored per day per level.
- LongerOrderPatience: additional seconds per level.

## GameConfig
Suggested defaults: 180 second day, target 180 earned coins, reputation 5/5, starting wallet 120, base concurrent orders 2, patience 45 sec, spawn interval 8 sec, wrong-order penalty 1, expired-order penalty 1, correct bonus 0.
