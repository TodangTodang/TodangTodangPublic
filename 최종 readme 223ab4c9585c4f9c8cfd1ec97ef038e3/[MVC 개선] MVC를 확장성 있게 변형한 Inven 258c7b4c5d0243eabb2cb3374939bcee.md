# [MVC 개선] MVC를 확장성 있게 변형한 Inventory 구현

## 🐰 개요

- View - Controller - Data 클래스의 역할을 분리한다.
- 데이터와 View의 의존성을 줄인다.

---

## 🐇 기술 도입 배경

> 문제점
> 
> 
> 인벤토리 안에서 다루는 Data는 3가지 종류로 분류되며, View에 보여지기 전 각각 다른 처리가 필요하다.
> 
> Data를 처리하는 로직을 UI 클래스 내에서 다루면 클래스 하나가 여러 역할을 수행하게 되고,
> 
> 이는 코드 수정으로 인한 사이드 이펙트가 커지는 문제점으로 이어질 수 있다.
> 
- View와 Data를 처리하는 Controller를 나누어 처리함으로써 클래스의 역할을 분리한다.

---

## 🍡 주요 메서드

### InventoryHandler

- InventoryController들을 관리한다.

| 메서드 | 기능 |
| --- | --- |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryHandler.cs#L19-L25 | Controller들의 Data를 초기화하며, 현재 Controller를 첫 번째 Tab의 Controller로 설정한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryHandler.cs#L27-L30 | 인벤토리 UI가 Open 되었을 때, Init 메서드를 실행한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryHandler.cs#L32-L37 | 인벤토리의 탭이 변경되면 현재 Controller를 변경한 뒤 RefreshTab을 요청한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryHandler.cs#L39-L42 | 인벤토리 상세 정보 창 상태가 변경되면 Controller에게 RefreshDetail을 요청한다. |

### InventoryController

- 각 Tab을 다루는 Controller들의 부모 클래스이다.

| 메서드 | 기능 |
| --- | --- |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryController.cs#L25-L44 | Controller에서 공통으로 사용하는 Manager와 데이터 클래스를 캐싱한다.각 Tab에 대응하는 Controller들은 해당 메서드를 오버라이드하며, 추가적으로 필요한 Data들을 설정한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryController.cs#L46-L49 | UI_Inventory에게 Player의 재화 정보 Update를 요청한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryController.cs#L51-L54 | 각 Tab에 대응하는 Controller들은 해당 메서드를 오버라이드하여,현재 자신이 다루는 List로 UI_Inventory의 ScrollView를 Update한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryController.cs#L56-L59 | 각 Tab에 대응하는 Controller들은 해당 메서드를 오버라이드하여,현재 자신이 다루는 Data의 상세 정보 UI를 Update한다. |

### UI_Inventory

- Inventory에서 View를 담당하는 클래스이다.

| 메서드 | 기능 |
| --- | --- |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L77-L96 | ScrollView를 갱신한 뒤, 첫 번째 슬롯이 선택된 상태로 변경한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L98-L114 | Player의 재화 정보 UI를 업데이트한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L116-L125 | Tab 리스트 초기 설정을 담당한다.각 Tab 버튼에 클릭 Listener를 연결한 뒤, 첫 번째 탭이 선택된 상태로 설정한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L127-L136 | Tab 버튼의 클릭 Listener로 연결되는 메서드이다.ScrollView를 맨 위로 이동한 뒤, 선택된 탭을 변경한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L138-L169 | 받아온 데이터 개수만큼 오브젝트 풀링을 이용하여 ScrollView의 슬롯 오브젝트를 생성한다. |
| https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L171-L178 | ScrollView 슬롯 오브젝트의 클릭 Listener로 연결되는 메서드이다.선택된 슬롯을 변경한 뒤, 슬롯에 대한 상세 정보 Update 이벤트를 실행한다. |

[🌙 목차로 돌아가기](https://github.com/j-miiin/TodangTodangCodes/tree/main/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory#crescent_moon-%EB%AA%A9%EC%B0%A8)

---

## 🥕 트러블 슈팅

### ⚠️ 문제 1

- 초기에는 MVC 패턴처럼 View에서 Data를 구독하여, Data가 갱신되면 View도 갱신되는 방식 사용
- 하지만 Scene이 변경된 후 View를 참조할 수 없는 상태에서 Data가 갱신되는 문제 발생

### 🛠️ 시도

- UI 오브젝트가 Destroy 될 때, 구독한 Data 이벤트를 해제하는 방식> 일일이 구독을 해제해야 되는 불편함> View와 Data의 의존성을 줄일 수 없을까?

### 💡 선택

- View에서 전달 받은 입력으로 Data를 갱신하는 동작과 Data의 갱신으로 인한 View를 Update하는 동작을 모두 Controller에서 수행하도록 변경> View와 Data의 의존성을 줄이는 MVP 구조를 응용하여 문제를 해결

### ⚠️ 문제 2

- Inventory에서 다루는 모든 Data에 대한 처리를 InventoryController 내부에서 수행
- 하나의 Controller에서 모든 Data에 대한 UI 갱신을 처리하면 코드가 매우 길고 복잡해질 것이라고 판단하여,UI 클래스에서 Controller를 참조하여 필요한 Data를 가져오도록 구현
- 하지만 이는 결과적으로 Controller와 View가 상호 참조 관계를 가지며, 의존성이 상당히 높아지게 됨> Tab의 종류가 늘어나면 확장성이 떨어지며 예외 처리 증가로 인한 오류 발생 가능성이 높아질 수 있음

### 🛠️ 시도

- InventoryController -> UI_Inventory의 단방향 방식으로 변경> InventoryController 내부에서 다루는 Data 종류가 다양하며, 이를 switch문이나 if문을 사용하여 처리하는 것은 확장성이 떨어진다고 판단> Tab의 종류가 증가해도 쉽게 확장할 수 있는 구조를 만들 수는 없을까?

### 💡 선택

- 각 Tab에 대한 Data를 처리하는 Controller를 따로 생성 -> 해당 Controller들은 기존의 InventoryController를 상속
- Controller들을 관리하는 InventoryHandler를 추가 -> Tab이 변경되면 InventoryHandler는 현재 Tab에 대응하는 Controller로 바꾸어 동작 실행
    
    ![https://private-user-images.githubusercontent.com/62470991/289359793-44b4d882-98ed-4c9c-9aea-922d169abf39.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTEiLCJleHAiOjE3MDI0NzA5MjQsIm5iZiI6MTcwMjQ3MDYyNCwicGF0aCI6Ii82MjQ3MDk5MS8yODkzNTk3OTMtNDRiNGQ4ODItOThlZC00YzljLTlhZWEtOTIyZDE2OWFiZjM5LnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFJV05KWUFYNENTVkVINTNBJTJGMjAyMzEyMTMlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjMxMjEzVDEyMzAyNFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTgxNTFmMTc0YjBkYjY1ODk5M2FhZTJjMjllNWI5MzJiYTBiODAyYzk0NDQ5YWRiNzEzMjljNmJlNTllMmZjZDYmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.xYNP5pF53LNQJ6vt1y4a-dk4sx12OiYB9SDEcLKO_0A](https://private-user-images.githubusercontent.com/62470991/289359793-44b4d882-98ed-4c9c-9aea-922d169abf39.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTEiLCJleHAiOjE3MDI0NzA5MjQsIm5iZiI6MTcwMjQ3MDYyNCwicGF0aCI6Ii82MjQ3MDk5MS8yODkzNTk3OTMtNDRiNGQ4ODItOThlZC00YzljLTlhZWEtOTIyZDE2OWFiZjM5LnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFJV05KWUFYNENTVkVINTNBJTJGMjAyMzEyMTMlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjMxMjEzVDEyMzAyNFomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPTgxNTFmMTc0YjBkYjY1ODk5M2FhZTJjMjllNWI5MzJiYTBiODAyYzk0NDQ5YWRiNzEzMjljNmJlNTllMmZjZDYmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.xYNP5pF53LNQJ6vt1y4a-dk4sx12OiYB9SDEcLKO_0A)
    

```
public class InventoryHandler : MonoBehaviour
{
  ...

  private void CallOnChangeTab(Enums.InventoryType inventoryType)
  {
      _curSelectedInventoryType = inventoryType;
      _curController = _inventoryControllers[(int)_curSelectedInventoryType];
      _curController.RefreshTab();
  }

  ...
}
```

[🌙 목차로 돌아가기](https://github.com/j-miiin/TodangTodangCodes/tree/main/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory#crescent_moon-%EB%AA%A9%EC%B0%A8)