public class UI_RecipeBookPanel : UI_Base
{
    private GameManager _gameManager;
    private PlayerData _playerData;
    public void Awake()
    {
        
    }

    public void Start()
    {
        _gameManager = GameManager.Instance;
        
    }
}
