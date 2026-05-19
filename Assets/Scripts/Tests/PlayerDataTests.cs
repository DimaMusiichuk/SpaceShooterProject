using NUnit.Framework;

public class PlayerDataTests
{
    [Test]
    public void PlayerData_NewInstance_HasDefaultValues()
    {
        PlayerData data = new PlayerData();

        Assert.AreEqual(0, data.coins, "Новий гравець повинен мати 0 монет");
        Assert.AreEqual(0, data.extraHealth, "У нового гравця не має бути купленого додаткового ХП");
        Assert.IsFalse(data.hasDoubleShot, "Подвійний постріл за замовчуванням має бути вимкнений");
        Assert.IsFalse(data.hasPremiumSkin, "Преміум скін за замовчуванням має бути вимкнений");
    }
}