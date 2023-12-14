public class Enums
{
    public enum PlayerDayCycleState
    {
        StartStore, // 영업 전
        OpenMarket, // 영업 종료 후 재료 주문 전
        DayEnd      // 재료 주문 후 
    }

    public enum PlayerEndingState
    {
        ContinuePlaying,
        BankruptcyEnding,
        GameOverEnding,
        GameClearEnding
    }

    public enum AudioType
    {
        Bgm,
        Effect,
    }

    public enum InventoryType
    {
        Ingredient,
        Recipe,
        Kitchen
    }

    public enum MarketTabType
    {
        Total, Ricecake, Tea
    }

    // HomeScene Character
    public enum NPCBunny1
    {
        WalkingToFridge,
        SearchingFridge,
        TurningFromFridge,
        WalkingToDishes,
        WashingDishes,
        TurningFromDishes
    }

    public enum CookStepInteraction
    {
        None,
        Interaction,
        MultipleInteraction,
        PickUp
    }
    
    public enum FoodType
    {
        Ricecake, Tea
    }

    public enum PopupButtonType
    {
        Confirm,
        Cancel
    }

    public enum TextureQuality
    {
        Low, 
        Medium,
        High
    }
    
    public enum SaveData
    {
        PlayerData=0, MarketData, DecoStoreData, EffectData, NewsData,
    }
    
    public enum FaceType
    {
        Normal,Angry,Happy,

    }

    public class Tutorial
    {
        public enum DayEndState
        {
            OpenMarket,
            Upgrade,
            DayEnd
        }
    }
}
