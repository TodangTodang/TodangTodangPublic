![header](https://capsule-render.vercel.app/api?type=cylinder&color=A1B6FF&height=150&section=header&text=Inventory%20with%20MVC&fontSize=60&fontColor=ECFBFF&animation=fadeIn)

<br>


## :crescent_moon: 목차

| [🐰 개요 🐰](#rabbit-개요) |
| :---: |
| [🐇 기술 도입 배경 🐇](#rabbit2-기술-도입-배경) |
| [🍡 주요 메서드 🍡](#dango-주요-메서드) |
| [🥕 트러블 슈팅 🥕](#carrot-트러블-슈팅) |

<br>

* * *

<br>

## :rabbit: 개요  
- View - Controller - Data 클래스의 역할을 분리한다.
- 데이터와 View의 의존성을 줄인다.

<br>

* * *

<br>

## :rabbit2: 기술 도입 배경
> 문제점<br>
> 인벤토리 안에서 다루는 Data는 3가지 종류로 분류되며, View에 보여지기 전 각각 다른 처리가 필요하다.<br>
> Data를 처리하는 로직을 UI 클래스 내에서 다루면 클래스 하나가 여러 역할을 수행하게 되고,<br>
> 이는 코드 수정으로 인한 사이드 이펙트가 커지는 문제점으로 이어질 수 있다.<br>
- View와 Data를 처리하는 Controller를 나누어 처리함으로써 클래스의 역할을 분리한다.

<br>

* * *

<br>

## :dango: 주요 메서드

### InventoryHandler
- InventoryController들을 관리한다.

|메서드|기능|
|:---:|:---:|
|[Init](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryHandler.cs#L19-L25)|Controller들의 Data를 초기화하며, 현재 Controller를 첫 번째 Tab의 Controller로 설정한다.|
|[CallOnOpenInventory](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryHandler.cs#L27-L30)|인벤토리 UI가 Open 되었을 때, Init 메서드를 실행한다.|
|[CallOnChangeTab](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryHandler.cs#L32-L37)|인벤토리의 탭이 변경되면 현재 Controller를 변경한 뒤 RefreshTab을 요청한다.|
|[CallOnRefreshDetail](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryHandler.cs#L39-L42)|인벤토리 상세 정보 창 상태가 변경되면 Controller에게 RefreshDetail을 요청한다.|

<br>

### InventoryController
- 각 Tab을 다루는 Controller들의 부모 클래스이다.

|메서드|기능|
|:---:|:---:|
|[InitDatas](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryController.cs#L25-L44)|Controller에서 공통으로 사용하는 Manager와 데이터 클래스를 캐싱한다.<br>각 Tab에 대응하는 Controller들은 해당 메서드를 오버라이드하며, 추가적으로 필요한 Data들을 설정한다.|
|[RefreshPlayerMoney](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryController.cs#L46-L49)|UI_Inventory에게 Player의 재화 정보 Update를 요청한다.|
|[RefreshTab](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryController.cs#L51-L54)|각 Tab에 대응하는 Controller들은 해당 메서드를 오버라이드하여,<br>현재 자신이 다루는 List로 UI_Inventory의 ScrollView를 Update한다.|
|[RefreshDetail](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/InventoryController.cs#L56-L59)|각 Tab에 대응하는 Controller들은 해당 메서드를 오버라이드하여,<br>현재 자신이 다루는 Data의 상세 정보 UI를 Update한다.|

<br>

### UI_Inventory
- Inventory에서 View를 담당하는 클래스이다.

|메서드|기능|
|:---:|:---:|
|[RefreshScrollView](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L77-L96)|ScrollView를 갱신한 뒤, 첫 번째 슬롯이 선택된 상태로 변경한다.|
|[UpdatePlayerMoneyUI](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L98-L114)|Player의 재화 정보 UI를 업데이트한다.|
|[InitTab](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L116-L125)|Tab 리스트 초기 설정을 담당한다.<br>각 Tab 버튼에 클릭 Listener를 연결한 뒤, 첫 번째 탭이 선택된 상태로 설정한다.|
|[ChangeTab](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L127-L136)|Tab 버튼의 클릭 Listener로 연결되는 메서드이다.<br>ScrollView를 맨 위로 이동한 뒤, 선택된 탭을 변경한다.|
|[InitSlots](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L138-L169)|받아온 데이터 개수만큼 오브젝트 풀링을 이용하여 ScrollView의 슬롯 오브젝트를 생성한다.|
|[OnSelectedSlotChanged](https://github.com/j-miiin/TodangTodangCodes/blob/9d523e24056454e40ffc5d78ad6103da6c516c28/MVC%20%EA%B5%AC%EC%A1%B0%EB%A5%BC%20%ED%99%9C%EC%9A%A9%ED%95%9C%20Inventory/UI_Inventory.cs#L171-L178)|ScrollView 슬롯 오브젝트의 클릭 Listener로 연결되는 메서드이다.<br>선택된 슬롯을 변경한 뒤, 슬롯에 대한 상세 정보 Update 이벤트를 실행한다.|

<br>

[🌙 목차로 돌아가기](#crescent_moon-목차)

<br><br>


#### [🐰 상세코드 보기 🐰]()

#### [🌙 Main README로 돌아가기 🌙](/README.md)