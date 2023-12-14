![header](https://capsule-render.vercel.app/api?type=cylinder&color=ffd3d3&height=150&section=header&text=Popup%20with%20UIManager&fontSize=60&fontColor=ECFBFF&animation=fadeIn)

<br>

![UIManager](https://github.com/TodangTodang/TodangTodangPublic/assets/62470991/8d075435-eee0-40c3-a1d2-43f4c9472c87)

<br>

### ⚠️ 문제
- 기존 방식은 모든 팝업의 매개변수를 UIManager의 ShowPopup 메서드에 전달하던 방식
  ```cs
  public T ShowPopup<T>(string content = "", Callback confirmAction = null, Callback cancelAction = null
            , Action<int> valueConfirmAction = null, int sliderMaxValue = -1) where T : UI_Popup
  {
      ...

      if (valueConfirmAction == null || sliderMaxValue == -1)
      {
          UI_DefaultPopup uiPopup = uiPopupDic[key] as UI_DefaultPopup;
          uiPopup.ShowPopup(confirmAction, cancelAction, content);
      }
      else
      {
          UI_SliderPopup uiSliderPopup = uiPopupDic[key] as UI_SliderPopup;
          uiSliderPopup.ShowPopup(confirmAction, cancelAction, valueConfirmAction, sliderMaxValue, content);
      }

      return _uiPopupDic[key] as T;
  }
  ```
  - Popup의 종류가 증가할수록 ShowPopup 메서드의 매개변수가 늘어나는 단점
  - Popup의 종류가 증가하면 예외 처리가 복잡해지고, 오류 발생 가능성이 높아짐<br>
    -> 확장성 저하
    
<br>

### 🛠️ 시도
- ShowPopup 메서드에는 모든 Popup들의 공통 Parameter만 전달하고, Popup을 반환 받아서 추가 Parameter를 Set 하는 방식
  ```cs
  UIManager.Instance.ShowPopup<UI_SliderPopup>()
      ?.SetPopupValue(valueConfirmAction: valueConfirmAction, sliderMaxValue: quantity);
  ```
  - Unity 개발을 할 때 ? 이나 ?? 같은 Null 체크 연산자를 사용하는 것은 고쳐야 하는 코딩 습관이라는 것을 알게 됨
- 임시 변수에 반환 받은 Popup을 할당한 뒤, Null 체크 후 Set 하는 방식<br>
  -> Popup을 Open 하는 동작과 필요한 Parameter를 할당하는 동작을 ShowPopup 메서드를 호출하는 부분에서 끝낼 수는 없을까?

<br>

### 💡 선택
- Popup에 필요한 Parameter를 담는 PopupParameter 클래스 생성
- 각각 세분화 되는 Popup들은 PopupParameter 클래스를 상속 받아서 필요한 Parameter를 선언하여 전달
  ```cs
  public class SliderPopupParameter : PopupParameter
  {
      private int _sliderMaxValue;
      private Action<int> _valueConfirmAction; 
  
      public SliderPopupParameter(int sliderMaxValue, Action<int> valueConfirmAction, 
          string content = "", Callback confirmCallback = null, Callback cancelCallback = null)
      : base(content, confirmCallback, cancelCallback)
      {
          _sliderMaxValue = sliderMaxValue;
          _valueConfirmAction = valueConfirmAction;
      }
  
      public int GetSliderMaxValue()
      {
          return _sliderMaxValue;
      }
  
      public Action<int> GetValueConfirmAction()
      {
          return _valueConfirmAction;
      }
  }
  ```
- 팝업 내부에서 필요한 Parameter 클래스 타입으로 형 변환하여 사용
  ```cs
  public class UI_SliderPopup : UI_Popup
  {
      ...
  
      public override void ShowPopup(PopupParameter popupParameter)
      {
          base.ShowPopup(popupParameter);
  
          SliderPopupParameter parameter = popupParameter as SliderPopupParameter;
  
          _max = parameter.GetSliderMaxValue();
          _value = (int)(_max * 0.5);
          _slider.maxValue = _max;
          _slider.value = _value;
          _quantityText.text = $"{_value}";
  
          OnValueCallback = null;
          OnValueCallback += parameter.GetValueConfirmAction();
      }

      ...
  }
  ```

<br><br>


#### [🐰 상세코드 보기 🐰](https://github.com/j-miiin/TodangTodangCodes/tree/main/UIManager%EB%A5%BC%20%ED%86%B5%ED%95%9C%20UI%20%EA%B4%80%EB%A6%AC)

#### [🌙 Main README로 돌아가기 🌙](/README.md)
