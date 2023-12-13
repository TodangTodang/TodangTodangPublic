using UnityEngine;
using UnityEngine.UI;

public class IceMaker : KitchenInteraction
{
    [SerializeField] private float _maxCoolTime;
    [SerializeField] private Image _progressIcon;
    private float _currentCoolTime;
    private bool _isAvailable;
    private Color _iconColor;

    protected override void Initialize()
    {
        base.Initialize();
 
        _isAvailable = true;
        CanInteractWithPlayer = true;

        interactionSound = "";
        successSound = Strings.Sounds.KITCHEN_ICE_SUCCESS;

        _currentCoolTime = _maxCoolTime;
        progressBar.SetActive(true);
        progressFill.fillAmount = 1;
        ChangeIconColor(true);
    }

    private void Update()
    {
        if (!_isAvailable)
        {
            ChangeIconColor(false);
 
            _currentCoolTime -= Time.deltaTime;
            progressFill.fillAmount = (float)(_maxCoolTime - _currentCoolTime) / _maxCoolTime;

            if (_currentCoolTime < 0)
            {
                _currentCoolTime = _maxCoolTime;
                _isAvailable = true;
                ChangeIconColor(true);
            }
        }
    }

    public override void Interaction()
    {
        if (!_isAvailable) return;
        if (player.Ingredient == null) return;
        if (player.Ingredient.tag == "Trash") return;

        if (player.Ingredient != null)
        {
            ingredients.Add(player.Ingredient);
        }
        player.Ingredient = null;
        interactionPos = player.foodPos;

        base.Interaction();

        _currentCoolTime = _maxCoolTime;
        _isAvailable = false;
    }


    protected override void MakeResult(GameObject result)
    {
        base.MakeResult(result);
        currentProgress = 0;
    }

    private void ChangeIconColor(bool isVisible)
    {
        _iconColor = Color.white;
        if (!isVisible )
        {
            _iconColor.a = 0f;
        }
        _progressIcon.color = _iconColor;
    }
}
