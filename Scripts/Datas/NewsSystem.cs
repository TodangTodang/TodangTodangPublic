using System;
using System.Collections.Generic;
using UnityEngine;

public class NewsSystem : Savable
{
    private EffectManager _effectManager;
    private GameManager _gameManager;
    private DataManager _dataManager;
    private ResourceManager _resourceManager;
    private UIManager _uIManager;
    
    private NewsSO[] _dayNews;
    private AchievementNewsSO[] _achievementNews;
    private List<bool> _completeAchievements;
    private Queue<UI_News> newsQueue;

    private void Init()
    {
        _effectManager = EffectManager.Instance;
        _gameManager = GameManager.Instance;
        _resourceManager = ResourceManager.Instance;
        _uIManager = UIManager.Instance;
        newsQueue = new Queue<UI_News>();
        
        Debug.Assert(_effectManager,$"_effectManager {Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_gameManager,$"_gameManager {Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_resourceManager,$"_resourceManager {Strings.DebugLog.INIT_PROBLEM}");
        Debug.Assert(_uIManager, $"_resourceManager {Strings.DebugLog.INIT_PROBLEM}");
    }
    public NewsSystem() : base()
    {
        Init();
        DataManager dataManager = DataManager.Instance;
        Debug.Assert(dataManager,"ddd");
        
        _dayNews = dataManager.GetDefaultDataArray<NewsSO>();
        _achievementNews = dataManager.GetDefaultDataArray<AchievementNewsSO>();
        _completeAchievements = new List<bool>(_achievementNews.Length);
        
        foreach (var achievementNews in _achievementNews)
        {
            _completeAchievements.Add(false);
        }
    }

    public void GetCompleteAchievements(out List<bool> copiedAchievemeents)
    {
        copiedAchievemeents = new List<bool>(_completeAchievements.Count);
        copiedAchievemeents.AddRange(_completeAchievements);
    }

    public void CheckTodaysNews(int day, Action onClosed = null)
    {
        NewsSO news = null;
        foreach (NewsSO _news in _dayNews)
        {
            if (_news.Day == day)
            {
                news = _news;
            }
        }

        if (news != null)
        {
            GameObject UI_NewsObj = _resourceManager.Instantiate(Strings.Prefabs.UI_NEWS);
            UI_News _newsUI;

            if (UI_NewsObj.TryGetComponent(out _newsUI))
            {
                EnqueueNews(_newsUI, news);
                _newsUI.gameObject.SetActive(false);
                if(onClosed != null)
                    _newsUI.OnClosed += onClosed;
            }
        }
    }
    
    public void CheckAchieveMent(int star)
    {
        for (int i = 0; i < _achievementNews.Length; ++i)
        {
            if (_achievementNews[i].StarAchievement <= star && !_completeAchievements[i])
            {
                GameObject UI_NewsObj = _resourceManager.Instantiate(Strings.Prefabs.UI_NEWS);
                UI_News _newsUI;

                if (UI_NewsObj.TryGetComponent(out _newsUI))
                {
                    _completeAchievements[i] = true;
                    _newsUI.gameObject.SetActive(false);
                    EnqueueNews(_newsUI,_achievementNews[i]);
                }    
            }
        }
        
    }
    
    private void EnqueueNews(UI_News _newsUI,NewsSO news)
    {
        if (news != null)
        {
            int currentDay = news.Day;

            if (currentDay == 0)
            {
                PlayerData playerData = _gameManager.GetPlayerData();
                Debug.Assert(playerData!= null,$"PlayerData ${Strings.DebugLog.INIT_PROBLEM}");
                currentDay = playerData.Date;
            }
                
            _newsUI.ShowNews(news,currentDay);
            
            if (news.Effects != null && news.Effects.Count > 0)
            {
                List<Sprite> sprites;
                ApplyNewsEffect(news.Effects, news.EffectDuration, out sprites);
                
                if (sprites.Count > 0)
                {
                    _newsUI.OnClosed += () =>
                    {
                        UI_ImagePopup popup = _uIManager.ShowPopup<UI_ImagePopup>(
                            new ImagePopupParameter(
                                content: Strings.HomeSceneMenu.GAIN_ITEM,
                                spriteList: sprites.ToArray() , 
                                confirmCallback: ViewNews
                            )
                            );
                        popup.OpenUI(false);
                    };
                    
                }
                else
                {
                    _newsUI.OnClosed += ViewNews;
                }
            }
            else
            {
                _newsUI.OnClosed += ViewNews;
            }

            newsQueue.Enqueue(_newsUI);
        }
    }
     
    private void ApplyNewsEffect(List<EffectSO> effects, List<int> duration,out List<Sprite> getList)
    {
        getList = new List<Sprite>();
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] is EffectCustomerSO)
            {
                _effectManager.AddEffectInQueue(effects[i], duration[i]);
            }
            else if (effects[i] is EffectIngredientSO)
            {
                _effectManager.AddEffectInQueue(effects[i], duration[i]);
            }else if (effects[i] is EffectCustomerUtensilSO)
            {
                EffectCustomerUtensilSO dataSO = effects[i] as EffectCustomerUtensilSO;
                _effectManager.UnlockUtensil(dataSO);
                getList.Add(dataSO.KitchenUtensilInfoSos.IconSprite);
            }else if (effects[i] is EffectCustomerAddIngredientSO)
            {
                EffectCustomerAddIngredientSO dataSO = effects[i] as EffectCustomerAddIngredientSO;
                _effectManager.AddIngredient(dataSO, duration[i]);
                getList.Add(dataSO.ingredientSos.IconSprite);
            }else if (effects[i] is EffectCustomerAddRecipeSO)
            {
                EffectCustomerAddRecipeSO dataSO = effects[i] as EffectCustomerAddRecipeSO;
                _effectManager.UnlockRecipe(dataSO);
                getList.Add(dataSO.RecipeSos.IconSprite);
            }
        }
    }

    public void ViewNews()
    {
        if (newsQueue.Count == 0)
            return;

        UI_News news = newsQueue.Dequeue();
        news.OpenUI();
    }
    
    public override void Init(string json, Param saveParam = null)
    {
        NewsData data = JsonUtility.FromJson<NewsData>(json);
        DataManager dataManager = DataManager.Instance;
        Debug.Assert(dataManager,$"dataManager {Strings.DebugLog.INIT_PROBLEM}");

        Init();
        
        _dayNews = dataManager.GetDefaultDataArray<NewsSO>();
        _achievementNews = dataManager.GetDefaultDataArray<AchievementNewsSO>();
        
        if (data.completeAchievements != null)
        {
            _completeAchievements = data.completeAchievements;
            if (_achievementNews.Length > _completeAchievements.Count)
            {
                for (int i=_completeAchievements.Count; i<_achievementNews.Length;++i)
                {
                    _completeAchievements.Add(false);
                }
            }            
        }
    }

    public override string GetJsonData()
    {
        NewsData newsData = new NewsData();
        
        GetCompleteAchievements(out newsData.completeAchievements);
        
        string newsDataString = JsonUtility.ToJson(newsData);
        return newsDataString;
    }
}
