using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinancialHelpController
{
    private UIManager _uiManager;
    private GameManager _gameManager;
    private PlayerData _playerData;
    private SceneManagerEx _sceneManagerEx;
    private DataManager _datamanager;

    private HashSet<IngredientInfoSO> _ingredients;
    private List<Sprite> _iconList;

    private Button _financialHelpButton;

    public FinancialHelpController(Button financialHelpButton)
    {
        _uiManager = UIManager.Instance;
        _gameManager = GameManager.Instance;
        _sceneManagerEx = SceneManagerEx.Instance;
        _datamanager = DataManager.Instance;
        
        Debug.Assert(_uiManager,$"_uiManager{Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_gameManager,$"_gameManager{Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_sceneManagerEx,$"_sceneManagerEx{Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_datamanager,$"_datamanager{Strings.DebugLog.INIT_PROBLEM}");

        _playerData = _gameManager.GetPlayerData();
        _financialHelpButton = financialHelpButton;
        
        Debug.Assert(_uiManager,$"_uiManager{Strings.DebugLog.INIT_PROBLEM}");
    }
    
    public void ShowRequestHelp()
    {
        _uiManager.ShowPopup<UI_DefaultPopup>(new PopupParameter(
            content: Strings.HomeSceneMenu.FINANTIAL_HELP,
            confirmCallback: RequestHelp
        ));
    }

    private void RequestHelp()
    {
        // - 평점 1 이상일 때 : 재료 빌리기 버튼 에서는 500원 이하일 때 평점 1 깎고 재료 받기
        //     - 주변 가게 사장님들께 도움을 받았지만 토슐랭 점수는 떨어졌어요!
        //     - 쌀가루 10개
        //     - 나머지 재료는 유통기한만큼 개수
        //     - 평점 1 이하일 때 : 파산씬으로 이동 후 부모님 통해 도움 (평점 안 깎이고 돈 받음)
        if (_playerData.Star > 10)
        {       
            _playerData.UpdateStar(-10);
            GetIngredients();
            _uiManager.ShowPopup<UI_ImagePopup>(
                    new ImagePopupParameter(
                        content: Strings.Help.FINANCIAL_HELP_POPUP
                        , spriteList: _iconList.ToArray()
                        , confirmCallback : OtherHelp
                        )
                    );
        }
        else
        {
            _playerData.OnBankruptcy();
            _sceneManagerEx.LoadScene(Scenes.LoadingScene, Scenes.BankruptcyScene);
        }
        
    }

    private void OtherHelp()
    {
        ResourceManager resourceManager = ResourceManager.Instance;
        Debug.Assert(resourceManager,$"resourceManager{Strings.DebugLog.INIT_PROBLEM}");

        GameObject obj = resourceManager.Instantiate(Strings.Prefabs.INGREDIENT_DIALOG);
        obj.gameObject.SetActive(true);
        _playerData.UpdateNeedHelp(false);
        _financialHelpButton.gameObject.SetActive(false);
        _datamanager.SaveAllData();
    }

    private void GetIngredients()
    {
        if (_ingredients == null) _ingredients = new HashSet<IngredientInfoSO>();
        else _ingredients.Clear();

        if (_iconList == null) _iconList = new List<Sprite> ();
        else _iconList.Clear();

        List<RecipeInfoData> allRecipes = _playerData.GetInventory<RecipeInfoData>();

        foreach (var recipe in allRecipes)
        {
            if (recipe.Level > 0)
            {
                IngredientInfoSO[] ingredientInfoSOs = recipe.DefaultData.IngredientInfoSO;
                foreach (var ingredient in  ingredientInfoSOs)
                {
                    _ingredients.Add(ingredient);
                }
            }
        }

        foreach (var ingredient in _ingredients)
        {
            int quantity = ingredient.BaseExpirationDate;
            IngredientInfoData data = new IngredientInfoData(ingredient, quantity);
            _playerData.AddIngredient(data, 0);
            _iconList.Add(ingredient.IconSprite);
        }
 
    }
}
