![header](https://capsule-render.vercel.app/api?type=cylinder&color=A1B6FF&height=150&section=header&text=UIManager&fontSize=60&fontColor=ECFBFF&animation=fadeIn)

<br>


## :crescent_moon: 목차

| [🐰 개요 🐰](#rabbit-개요) |
| :---: |
| [🍡 주요 메서드 🍡](#dango-주요-메서드) |
| [🍵 활용 🍵](#tea-활용) |
| [🥕 트러블 슈팅 🥕](#carrot-트러블-슈팅) |

<br>

* * *

<br>

## :rabbit: 개요  
- UIManager를 통해 UI Component들에 쉽게 접근하고 관리한다.
- UIManager를 통해 Popup들을 편리하게 사용한다.

<br>

* * *

<br>

## :dango: 주요 메서드

### UIManager

|메서드|기능|
|:---:|:---:|
|[GetUIComponent](https://github.com/j-miiin/TodangTodangCodes/blob/bcb8da4e827f8da3155b97215df1687f5cb33bc6/UIManager%EB%A5%BC%20%ED%86%B5%ED%95%9C%20UI%20%EA%B4%80%EB%A6%AC/UIManager.cs#L12-L37)|요청 받은 UI Component가 Dictionary에 있다면 반환하고,<br>없다면 Resources 폴더에서 Load하여 Dictionary에 저장한 뒤 반환한다.|
|[TryGetUIComponent](https://github.com/j-miiin/TodangTodangCodes/blob/bcb8da4e827f8da3155b97215df1687f5cb33bc6/UIManager%EB%A5%BC%20%ED%86%B5%ED%95%9C%20UI%20%EA%B4%80%EB%A6%AC/UIManager.cs#L39-L67)|GetUIComponent를 응용하여 만든 메서드로, 예외 처리를 보다 쉽게 하기 위해서 추가한 메서드이다.<br>TryGetComponent 메서드처럼 out 매개변수를 사용하여 UI Component를 반환한다.<br>요청 받은 UI Component를 가져오는데 성공하면 true, 실패하면 false를 반환한다.|
|[RemoveUIComponent](https://github.com/j-miiin/TodangTodangCodes/blob/bcb8da4e827f8da3155b97215df1687f5cb33bc6/UIManager%EB%A5%BC%20%ED%86%B5%ED%95%9C%20UI%20%EA%B4%80%EB%A6%AC/UIManager.cs#L69-L76)|UI Component를 저장한 Dictionary에서 요청받은 해당 UI Component를 삭제한다.<br>Scene이 변경되었을 때 오브젝트가 파괴되어 더 이상 참조할 수 없는 UI Component에 접근하는 것을 방지하기 위해 사용한다.|
|[RemoveAllUIComponent](https://github.com/j-miiin/TodangTodangCodes/blob/bcb8da4e827f8da3155b97215df1687f5cb33bc6/UIManager%EB%A5%BC%20%ED%86%B5%ED%95%9C%20UI%20%EA%B4%80%EB%A6%AC/UIManager.cs#L78-L81)|UI Component를 저장한 Dictionary에 존재하는 모든 UI Component를 삭제한다.|
|[ShowPopup](https://github.com/j-miiin/TodangTodangCodes/blob/bcb8da4e827f8da3155b97215df1687f5cb33bc6/UIManager%EB%A5%BC%20%ED%86%B5%ED%95%9C%20UI%20%EA%B4%80%EB%A6%AC/UIManager.cs#L83-L112)|요청 받은 UI Popup을 Show 한다.<br>GetUIComponent와 마찬가지로 요청 받은 Popup이 Dictionary에 존재하지 않는다면 Resources 폴더에서 Load하여 저장한 뒤 반환한다.|

<br>

### UI_Popup
|메서드|기능|
|:---:|:---:|
|[ShowPopup](https://github.com/j-miiin/TodangTodangCodes/blob/bcb8da4e827f8da3155b97215df1687f5cb33bc6/UIManager%EB%A5%BC%20%ED%86%B5%ED%95%9C%20UI%20%EA%B4%80%EB%A6%AC/UI_Popup.cs#L37-L44)|PopupParameter를 통해 전달 받은 Popup의 내용과 콜백을 정의한 뒤, 해당 Popup을 Open 한다.|
|[ClosePopup](https://github.com/j-miiin/TodangTodangCodes/blob/bcb8da4e827f8da3155b97215df1687f5cb33bc6/UIManager%EB%A5%BC%20%ED%86%B5%ED%95%9C%20UI%20%EA%B4%80%EB%A6%AC/UI_Popup.cs#L46-L52)|Button의 Type(확인/취소)에 따라 ShowPopup에서 전달 받았던 Callback 메서드를 실행한 뒤, Popup을 Close 한다.|

<br>

[🌙 목차로 돌아가기](#crescent_moon-목차)

<br>

* * *

<br>

## :tea: 활용 

### UIManager를 통해 UI Component에 접근하기

```cs
private void OpenInventory()
{
    if (_uiInventory == null)
    {
        if (!_uiManager.TryGetUIComponent<UI_Inventory>(out _uiInventory))
        {
            Debug.LogError("Null Exception : UI_Inventory");
            return;
        }
    }
    _uiInventory.OpenUI();

    ...
}
```

<br>

### UIManager와 PopupParameter를 활용한 Popup 사용
```cs
UIManager.Instance.ShowPopup<UI_SliderPopup>(
    new SliderPopupParameter(
          sliderMaxValue: quantity
          , valueConfirmAction: valueConfirmAction
        )
    );
```

<br>

[🌙 목차로 돌아가기](#crescent_moon-목차)

<br><br>


#### [🐰 상세코드 보기 🐰]()

#### [🌙 Main README로 돌아가기 🌙](/README.md)