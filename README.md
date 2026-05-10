## Архітектура системи Магазину та Збереження даних

```mermaid
classDiagram
    class PlayerController {
        +int hp
        +int coins
        +Sprite currentSkin
        +AddCoins(int amount)
        +UpdateHUD()
    }

    class ShopManager {
        -DatabaseService dbService
        -List~SkinData~ availableSkins
        +OpenShop()
        +BuySkin(string skinId)
        +EquipSkin(string skinId)
    }

    class DatabaseService {
        -string dbPath
        +SaveProgress(int coins, List~string~ unlockedSkins)
        +LoadProgress() PlayerData
        -ConnectToDatabase()
    }

    class SkinData {
        +string skinId
        +string skinName
        +int price
        +bool isUnlocked
        +Sprite displayImage
    }

    ShopManager ..> DatabaseService : Використовує (Uses)
    ShopManager --> PlayerController : Оновлює баланс
    ShopManager "1" *-- "*" SkinData : Містить каталог