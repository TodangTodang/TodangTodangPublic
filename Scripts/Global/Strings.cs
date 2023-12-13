public class Strings
{
    public class Prefs
    {
        // 0 : 튜토리얼 처음 X, 1 : 튜토리얼 처음 O
        public const string IS_FIRST_TUTORIAL = "IsFirstTutorial";
        // 0 : 튜토리얼 스킵 X , 1 : 튜토리얼 스킵 O
        public const string IS_TUTORIAL_SKIP = "IsTutorialSkip";

        // GameSettings
        public const string BGMVOLUME_KEY = "BGMVolumeKey";
        public const string EFFECTVOLUME_KEY = "EffectVolumeKey";
        public const string TEXTUREQUALITY_KEY = "TextureQualityKey";
        public const string FRAMERATE_KEY = "FramerateKey";
    }

    public class Prefabs
    {
        public const string UI_MARKETITEMSLOT = "UI/UI_MarketItemSlot";
        public const string UI_CARTSLOT = "UI/UI_CartSlot";
        public const string UI_EMOTION_TEST = "UI/UI_EmotionText";
        public const string UI_GOLD_TEXT = "UI/UI_GoldText";

        public const string TUTORIAL_DAY1_START = "Tutorials/TutorialDay1Start";
        public const string TUTORIAL_DAY1_END = "Tutorials/TutorialDay1End";
        
        
        public const string INGREDIENT_DIALOG  = "HelpEvent/IngredientHelpDialog";

        public const string UI_INVENTORY_SLOT = "UI/UI_InventorySlot";
        public const string UI_DECOSTORE_SLOT = "UI/UI_DecoStoreSlot";

        public const string UI_NEWS = "UI/UI_News";

        public const string UI_BASE_INGREDIENT = "UI/CookBook/UI_BaseIngredient";
        public const string UI_COOKSTEP_SLOT = "UI/CookBook/UI_CookStepSlot";
        public const string UI_INGREDIENT = "UI/CookBook/UI_Ingredient";
        public const string UI_INTERACTION = "UI/CookBook/UI_Interaction";
        public const string UI_MULTIPLE_INTERACTION = "UI/CookBook/UI_MultipleInteraction";
        public const string UI_PICK_UP = "UI/CookBook/UI_PickUp";
        public const string UI_RECIPE_SLOT = "UI/CookBook/UI_RecipeSlot";

        public const string UI_PREFAB_PATH = "Prefabs/UI/";
        public const string UI_POPUP_PREFAB_PATH = "Prefabs/UI/Popup/";
    }

    public class Tags
    {
        public const string PLAYER_TAG = "Player"; 
    }
    
    public class Sounds
    {
        public const string BGM_STARTSCENE = "BGM/StartScene";
        public const string BGM_HOMESCENE = "BGM/HomeScene";
        public const string BGM_GAMESCENE = "BGM/GameScene";
        public const string BGM_GAMEOVERSCENE = "BGM/GameOverScene";
        public const string BGM_GAMECLEARSCENE = "BGM/GameClearScene";
        public const string BGM_TESTVERSIONCOMPLETESCENE= "BGM/TestVersionCompleteScene"; 
        
        public const string UI_BUTTON = "UI/Pressed";
        public const string UI_BUYANDSELL = "UI/BuyAndSell";
        public const string UI_TIME = "UI/Time";
        public const string UI_GAME_END = "UI/GameEnd";
        public const string UI_DIALOG = "UI/Dialog"; 

        public const string CUSTOMER_ORDER = "Customer/Order_Yo";
        public const string CUSTOMER_SUCCESSE = "Customer/Successe";
        public const string CUSTOMER_FAIL = "Customer/Fail";

        public const string PLAYER_FOOTSTEP = "Player/FootStep";

        public const string DAY_END = "UI/Test/Fade/DayEnd";
        public static string DAY_START = "UI/Test/Fade/DayStart";
        public static string PLAYER_FALLDOWN ="UI/Test/Fall Down/FallDown";

        public const string GAMEOVER_SFX_CRICKETS = "GameOverSceneSFX/Crickets";
        public const string GAMEOVER_SFX_MURMUR = "GameOverSceneSFX/Murmur";
        public const string GAMEOVER_SFX_NEWSPAPER = "GameOverSceneSFX/Newspaper";

        public const string KITCHEN_PICK_UP = "Player/PickUpAndPutDown1";
        public const string KITCHEN_PUT_DOWN = "Player/PickUpAndPutDown2";
        public const string KITCHEN_BASIC_SUCCESS = "KitchenUtensils/FoodSuccess2";
        public const string KITCHEN_ICE_SUCCESS = "KitchenUtensils/Freezer2";
        public const string KITCHEN_WAIT_SUCCESS = "KitchenUtensils/WaitSuccess";
        public const string KITCHEN_WATER = "KitchenUtensils/TeaPotAndWaterPurifier";
        public const string KITCHEN_TRASH = "KitchenUtensils/CreateTrash2";
        public const string KITCHEN_COUNTERTOP = "KitchenUtensils/CounterTop2";
        public const string KITCHEN_MORTAR = "KitchenUtensils/Mortar2";
        public const string KITCHEN_INGREDIENTBOX = "KitchenUtensils/Close";
        public const string KITCHEN_WARNING = "KitchenUtensils/Warning";
        public const string KITCHEN_WARNING_FAST = "KitchenUtensils/Warning2";
    }

    public class Sprites
    {
        // Inventory
        public const string INVENTORY_MONEY_ICON = "InventoryIcons/money_icon";
        public const string INVENTORY_TIME_ICON = "InventoryIcons/time_icon";
        public const string INVENTORY_RICECAKE_TYPE_ICON = "InventoryIcons/ricecake_type_icon";
        public const string INVENTORY_TEA_TYPE_ICON = "InventoryIcons/tea_type_icon";
        public const string INVENTORY_KITCHEN_TYPE_ICON = "InventoryIcons/kitchen_type_icon";

        // DecoStore
        public const string DECO_STORE_BUY_BTN = "DecoStoreIcons/buy_btn";
        public const string DECO_STORE_CANT_BUY_BTN = "DecoStoreIcons/buy_btn_disabled";
        public const string DECO_STORE_SOLD_OUT_BTN = "DecoStoreIcons/sold_out_btn";

        // Market
        public const string MARKET_RICECAKE_TYPE_ICON = "MarketIcons/ricecake_type2";
        public const string MARKET_TEA_TYPE_ICON = "MarketIcons/tea_type2";
    }

    public class Inventory
    {
        public const string LOCKED_LEVEL_PRICE = "LOCK";
        public const string MAX_LEVEL_PRICE = "MAX";

        public const string UNLOCK_RECIPE1 = "새로운 레시피를 배우셨군요!";
        public const string UNLOCK_RECIPE2 = " 재료를 선물로 드릴게요!";

        public const string SELL_BUTTON_TEXT = "판매";
        public const string UNLOCK_BUTTON_TEXT = "잠금 해제";
        public const string UPGRADE_SKILL_BUTTON_TEXT = "숙련도 증가";
        public const string UPGRADE_BUTTON_TEXT = "업그레이드";

        public const string EXPIRATION_DATE_TITLE = "남은 유통기한";
        public const string SKILL_UPGRADE_TITLE = "가격";
        public const string SPEED_UPGRADE_TITLE = "속도";
        public const string QUANTITY_UPGRADE_TITLE = "개수";
    }

    public class KitchenUtensils
    {
        public const string INGREDIENT_BOX = " 재료상자";
        public const string TEA_POT = "찻주전자";
        public const string STEAMER = "찜기";
        public const string POT = "냄비";
        public const string COUNTERTOP = "조리대";
        public const string MORTAR = "절구";
        public const string ICE_MAKER = "얼음기계";
        public const string WATER_DRINKER = "정수기";
        public const string TRASH_BIN = "쓰레기통";
    }
    
    public class Market
    {
        public const string ORDERCONFIRM = "주문하시겠습니까?";
        public const string EXITCONFIRM = "주문 창을 닫으면 다음 영업일에 다시 열 수 있습니다.\n 그래도 닫으시겠습니까?";
        public const string ASCENDINGSORT_TEXT = "가격순 ▲";
        public const string DESCENDINGSORT_TEXT = "가격순 ▼"; 
    }

    public class GameSettings
    {
        public const string CLOSEDBUTTON = "변경 내용을 저장하시겠습니까?";
        public const string EXITBUTTON = "게임을 종료하시겠습니까?";
        public const string DATARESETBUTTON = "데이터를 초기화 하시겠습니까?";

        public const string LOW = "낮음";
        public const string MEDIUM = "중간";
        public const string HIGH = "높음"; 
    }

    public class PausePanel
    {
        public const string HOMEBUTTON = "홈으로 이동하시겠습니까?";
    }

    public class PracticeModeScene
    {
        public const string EXIT_POPUP_CONTENT = "연습 모드를 종료하시겠습니까?";
    }

    public class HomeSceneMenu
    {
        public const string LOAD_PRACTICE_MODE_POPUP = "연습 모드를 진행하시겠습니까?";
        public const string FINANTIAL_HELP = "달조각이 부족해요!\n주변에 도움을 요청할까요?";
        public const string OTHER_HELP = "주변 가게 사장님들께 도움을 받았지만 토슐랭 점수는 떨어졌어요!";
        public const string GAIN_ITEM = "새로운 아이템을 얻었습니다!";
    }

    public class GameSceneMenu
    {
        public const string EARLY_EXIT_WARNING = "영업을 종료하시겠습니까?\n(남은 손님들이 불만족하니\n주의하세요!)";
    }

    public class Tutorial
    {
        public const string TUTORIAL_SKIP_POPUP = "튜토리얼을 스킵하시겠습니까?\n끝까지 진행하면\n깜짝 선물이 있을지도 몰라요!";
    }

    public class Help
    {
        public const string FINANCIAL_HELP_POPUP = "재료를 획득했어요!";
    }

    public class SaveData
    {
        public const string SAVE_VERSION = "Version0.1";
        public const string IS_FIRST_PLAY = "IsFirstPlay";
    }

    public class AnimationData
    {
        public const string MOVE_PARAMETER = "Move";
        public const string WAIT_PARAMETER = "Wait";
    }

    public class DefaultDataName
    {
        public const string CUSTOMER_DATA_SO = "BaseCustomerData";
    }

    public class DebugLog
    {
        public const string INIT_PROBLEM = " Null Exception";
        public const string NOT_ALLOCATE_IN_INSPECTOR = "NOT_ALLOCATE_IN_INSPECTOR";
    }
}
