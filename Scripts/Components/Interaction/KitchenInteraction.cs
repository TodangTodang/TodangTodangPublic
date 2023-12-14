using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KitchenInteraction : MonoBehaviour, IInteractable
{
    [Header("Cook")]
    [SerializeField] protected List<CookStepSO> availableRecipe;
    [SerializeField] protected List<GameObject> ingredients;
    [SerializeField] protected ParticleSystem successParticle;
    protected KitchenUtensilInfoData data;
    protected GameObject trashObj;

    [Header("Interaction")]
    [SerializeField] protected ParticleSystem workingParticle;
    public ISelection[] Selections { get; set; }
    protected Transform interactionPos;
    public bool CanInteractWithPlayer { get; set; }

    [Header("Progress")]
    [SerializeField] protected GameObject progressBar;
    [SerializeField] protected Image progressFill;
    [SerializeField] protected Warning warning;
    protected float maxProgress;
    protected float currentProgress;

    [Header("PickUp")]
    [SerializeField] protected Transform[] foodPos;
    public bool IsPlaceable { get; set; }

    protected string interactionSound;
    protected string successSound;
    
    protected Player tryGetPlayer;
    protected Player player;

    #region Managers
    protected DataManager dataManager;
    protected SoundManager soundManager;
    protected GameManager gameManager;
    protected ResourceManager resourceManager;
    #endregion

    protected void Start()
    {
        Selections = GetComponents<ISelection>();
        soundManager = SoundManager.Instance;
        SetManagers();
        Initialize();
    }

    private void SetManagers()
    {
        dataManager = DataManager.Instance;
        soundManager = SoundManager.Instance;
        gameManager = GameManager.Instance;
        resourceManager = ResourceManager.Instance;

#if UNITY_EDITOR
        Debug.Assert(dataManager != null, "Null : DataManager");
        Debug.Assert(soundManager != null, "Null : SoundManager");
        Debug.Assert(gameManager != null, "Null : GameManager");
        Debug.Assert(resourceManager != null, "Null : ResourceManager");
#endif
    }


    protected virtual void Initialize()
    {
        trashObj = dataManager.GetDefaultData<IntermediateResultsInfoSO>("Trash").Prefab;
        
        if (ingredients == null) ingredients = new List<GameObject>();
        
        currentProgress = 0;

        if (data != null && data.Level > 0)
        {
            maxProgress = (data == null) ? 1 : data.DefaultData.SpeedUpgradeInfo[data.Level - 1];
        }
        else
        {
            maxProgress = 1;
        }

        UpdateProgressBar();
        successSound = Strings.Sounds.KITCHEN_BASIC_SUCCESS;
    }

    protected void GetUtensilData()
    {
        PlayerData playerData = gameManager.GetPlayerData();
        
        List<KitchenUtensilInfoData> utensilList = playerData.GetInventory<KitchenUtensilInfoData>();

        foreach (KitchenUtensilInfoData utensilData in utensilList)
        {
            if (utensilData.DefaultData.name == GetType().Name)
                data = utensilData;
        }
    }


    public void SetPlayer(Player player)
    {
        this.player = player;
    }


    public virtual void Interaction()
    {
        if (ingredients.Count <= 0) return;
        if (player.Ingredient != null) return;
        if (currentProgress >= maxProgress) return;
        if (ingredients[0].tag == "Trash") return;

        ++ currentProgress;

        if (interactionSound != "") soundManager.Play(interactionSound);
        WorkingParticle();
        UpdateProgressBar();

        if (currentProgress == maxProgress)
        {
            CheckValidity();
            UpdateProgressBar();
        }
    }


    public virtual void PickUp()
    {
        if (player.Ingredient != null) return;
        if (ingredients.Count <= 0) return;

        currentProgress = 0;
        UpdateProgressBar();

        soundManager.Play(Strings.Sounds.KITCHEN_PICK_UP);
        SuccessParticle(false);
        UpdateWarning(0);

        SetObejctsParent(ingredients[ingredients.Count - 1], player.foodPos);
        player.Ingredient = ingredients[ingredients.Count - 1];
        ingredients.RemoveAt(ingredients.Count - 1);
    }


    public virtual void PutDown()
    {
        if (player.Ingredient == null) return;
        if (ingredients.Count == foodPos.Length) return;

        if (currentProgress != 0)
        {
            currentProgress = 0;
            UpdateProgressBar();
        }

        soundManager.Play(Strings.Sounds.KITCHEN_PUT_DOWN);

        ingredients.Add(player.Ingredient);
        SetObejctsParent(ingredients[ingredients.Count - 1], foodPos[ingredients.Count - 1]);
        player.Ingredient = null;
    }


    protected virtual void CheckValidity()
    {
        List<IngredientInfoSO> ingredientStack = new List<IngredientInfoSO>();
        foreach(var ingredient in ingredients)
        {
            if (ingredient == null) continue;
            ingredientStack.Add(GetIngredient(ingredient));
        }

        foreach (var cookStep in availableRecipe)
        {
            if (AreEquivalent(cookStep.Ingredients, ingredientStack))
            {
                MakeResult(cookStep.result);
                
                if (successSound != "" || successSound != null) soundManager.Play(successSound);
                SuccessParticle(true);

                return;
            } 
        }
        MakeResult();
        SuccessParticle(false);
        soundManager.Play(Strings.Sounds.KITCHEN_TRASH);
        currentProgress = 0;
    }


    protected virtual void MakeResult(GameObject  result = null)
    {
        if (ingredients.Count <= 0) return;
        if (result == null) result = trashObj;

        foreach (var obj in ingredients)
        {
            if (obj == null) continue;
            resourceManager.Destroy(obj); 
        }
        ingredients.Clear();

        GameObject newIngredient = resourceManager.Instantiate($"Foods/{result.name}");

#if UNITY_EDITOR
        if (newIngredient == null)
        {
            Debug.LogError("Result Prefab does not exist.");
        }
#endif

        if (CanInteractWithPlayer && !IsPlaceable)  // direct interaction without placing the result at any foodPos
        {
            player.Ingredient = newIngredient;
        }
        else
        {
            ingredients.Add(newIngredient);
        }

        SetObejctsParent(newIngredient, interactionPos);
    }

    protected bool AreEquivalent(List<IngredientInfoSO> cookStepIngredients, List<IngredientInfoSO> submittedIngredients)
    {
        if (cookStepIngredients.Count != submittedIngredients.Count)
        {
            return false;
        }

        foreach (IngredientInfoSO ingredient in cookStepIngredients)
        {
            if (!submittedIngredients.Contains(ingredient))
            {
                return false;
            }
        }

        return true;
    }


    protected IngredientInfoSO GetIngredient(GameObject obj)
    {
        IngredientObject ingredient;
        obj.TryGetComponent<IngredientObject>(out ingredient);
        if (ingredient != null)
        {
            return ingredient.GetIngredientObjectSO();
        }
        else
        {
            Debug.LogError("The gameObject has no IngredientObject component");
        }
        return null;
    }

    protected void SetObejctsParent(GameObject obj, Transform parent)
    {
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }


    public void SelectObject(bool isSelected)
    {
        foreach (ISelection selected in Selections)
        {
            selected.SelectObject(isSelected);
        }
    }

    protected void SuccessParticle(bool isPlaying)
    {
        if (successParticle == null) return;
        if (!IsPlaceable) return;

        if (isPlaying) successParticle.Play();
        else successParticle.Stop();
    }

    protected void WorkingParticle()
    {
        if (workingParticle == null) return;
        if (!IsPlaceable) return;
        if (!CanInteractWithPlayer) return;

        workingParticle.Play();
    }

    protected void UpdateProgressBar()
    {
        if (progressBar == null) return;
        if (!IsPlaceable) return;

        if (currentProgress == 0 || currentProgress == maxProgress)
        {
            progressBar.SetActive(false);
        }
        if (currentProgress == 1 && currentProgress != maxProgress)
        {
            progressBar.SetActive(true);
        }

        progressFill.fillAmount = (float)currentProgress / maxProgress;
    }

    protected void UpdateWarning(int stage)
    {
        if (warning == null) return;
        
        if (stage == 0)
        {
            warning.gameObject.SetActive(false);
            return;
        }

        if (!warning.gameObject.activeInHierarchy)
        {
            warning.gameObject.SetActive(true);
            SuccessParticle(false);
        }

        warning.SetBlinkTerm(stage);
    }
}
