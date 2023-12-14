# [New InputSystem] 크로스 플랫폼 대응

## 개요

- 프로젝트의 효율을 위해 NewInputSystem을 활용하여 여러 디바이스에서 대응이 가능하도록 함
- NewInputSystem을 활용하는 과정에서 UI와 관련된 문제점이 여러 부분에서 발생

---

## New Input System 활용

⚠️ **기술 도입 배경**

- 본래 모바일에서만 개발하려던 프로젝트를 PC 플랫폼에서도 가능하도록 설계하기로 결정하면서, 멀티 플랫폼 개발에 용이한 New InputSystem을 이용

**💡 기술 도입으로 얻은 이점**

- 물리적 입력을 추상적인 입력으로 변환하여 사용하면서, 같은 행동에 대해서 입력 추가를 통해 추가적인 작업 없이 같은 행동에 대해 다른 입력을 적용 가능

💫 **Input System 구성**

![](Image/InputAction.png)

---

## New Input System의 활용에 따른 **문제점 발생**

### **마우스 좌클릭과 UI 버튼 입력의 겹침 오류 현상 -**

![](Image/Describe.png)

### **⚠️ 문제**

- 상호작용 키를 마우스 좌클릭으로 변경하면서 Input 입력과 버튼 클릭이 겹치게 됨
    
    → 재료상자나 찻주전자 앞에서 UI버튼을 누를 경우, 입력이 우선적으로 처리되어 전용 UI가 뜨기 때문에 버튼의 기능을 수행할 수 없는 상황이 됨
    

### **🛠️ 과정**

- 마우스 커서를 숨기고, alt키를 눌렀을 때만 커서가 보이도록, 이 때는 상호작용 입력 경로로 사용되지 않도록 설정하는 방법 고민. 하지만 기획적인 측면에서 좋지 못하다고 판단
- UI 버튼을 On-ScreenButton으로 변경하는 것을 고려했으나, 마우스 클릭보다 늦게 인식되는 것이 마찬가지였고, 인풋 액션의 우선순위를 설정하지 못한다는 문제가 여전히 존재
- 버튼 위에 마우스가 있을 때 인풋 자체를 막는 방법에 대해 생각했으나, 순간적으로 인풋을 막는 행위가 반복적으로 일어나는 것은 비효율적이고 이미 인풋을 활성/비활성화하는 부분이 있기 때문에 문제 발생 우려

### **💡 선택** 및 결과

- 마우스 좌클릭 이벤트에 연결 된 콜백 메서드에서 현재 마우스가 버튼 위에 있는지 확인한 후 메서드를 실행하도록 함
- 관련 코드
    
    ```
    public bool IsMouseOverUIButton()
    {
        rayResults.Clear();
        eventData.position = Input.mousePosition;
    
        EventSystem.current.RaycastAll(eventData, rayResults);
    
        return rayResults.Count > 0;
    }
    ```
    
    - 마우스가 버튼 요소 위에 있는지 확인하는 메서드
    - UI 요소들을 대상으로 RaycastAll을 사용해서 마우스 커서가 UI요소 위에 있는지 판단
 
  <br>
    
    ```
    public bool IsInteractable()
    {
        if (Input.IsMouseOverUIButton()) return false;
        if (InteractionAction != null) return true;
        return false;
    }
    ```
    
    - 플레이어가 상호작용 가능한 상태인지 확인하는 메서드

### 활성화 중인 UI에 따라 Input을 제어해야 하는 문제 

### **⚠️ 문제**

- Player오브젝트가 컴포넌트로 가지고 있는 PlayerInput을 UI가 참조하여 제어하는 것이 부자연스러운 구조
    - 프로젝트 전반에서 Scene이 로드 되면, 내부적으로 Player를 생성하고, UI가 필요한 경우에 한해서 UI매니저를 통해서 생성 및 호출되도록 구조가 짜여져 있음.
    
    → 따라서 미리 Inspector 창에서 참조하도록 연결시켜둘 수 없는 상황
    

### **🛠️ 과정**

- FindObjectWithTag()메서드를 활용해서 태그를 통해 플레이어를 검색한 후 PlayerInput 컴포넌트를 찾아서 **UI에서 직접 Input을 제어하는 방식**
    - 여러 UI 스크립트에서 PlayerInput 컴포넌트를 직접 찾아서 제어하는 것이 UI 스크립트의 역할에서 벗어난다고 판단
    - UI 스크립트가 UI와 관련된 역할만 수행할 수 있도록 하지 못한다는 문제
- UI들은 UIManager를 통해서 참조 가능한 상황이므로, UI들이 열리거나 닫힐 때 이벤트를 발생시키도록 하고, 해당 이벤트에 콜백 메서드를 PlayerInput에서 연결하는 방법
    - UIManager에서 모든 UI가 활성화될 때 Dictionary에 UI들을 저장하도록 되어 있고, UIManager의 메서드를 통해서 활성화된 UI에 접근이 가능함
        
        → UI가 활성 / 비활성화 될 때 UI를 체크하는 이벤트를 발생시켜서 인풋 제어 메서드가 실행될 수 있도록 하는 방법 생각
        
    - 하지만 PlayerInput 클래스는 유저 입력과 관련된 처리를 담당하는 클래스인데, UI를 같이 처리하는 것은 코드 결합도를 높이는 나쁜 요소라고 판단.

### **💡 선택** 및 결과

- GameSceneUIInputController라는 클래스 생성
    
    → UI에 Input을 제어하는 메서드를 연결시키도록 역할을 분리
    
- 관련 코드
    
    ```
    private void AddCallbacks()
        {
            _pausePanel.OnUIOpen += (() => PauseCallback(true));
            _pausePanel.OnUIClose += (() => PauseCallback(false));
    
            _cookBook.OnUIOpen += (() => CookBookCallback(true));
            _cookBook.OnUIClose += (() => CookBookCallback(false));
    
            _resultPanel.OnUIOpen += (() => _input.EnableInput(false));
    
            _ingredientBox.OnUIOpen += (() => _input.EnableInput(false));
            _ingredientBox.OnUIClose += (() => _input.EnableInput(true));
    
            _input.GameActions.Pause.performed += TogglePausePanel;
            _input.GameActions.CookBook.performed += ToggleCookBook;
    
            #region Ensure Deactivating UI
            _pausePanel.gameObject.SetActive(false);
            _cookBook.gameObject.SetActive(false);
            _resultPanel.gameObject.SetActive(false);
            _ingredientBox.gameObject.SetActive(false);
            #endregion
        }
    ```
    
    - GameSceneUIInputController에서 이벤트에 Callback을 연결하는 메서드
 
  <br>
    
    ```
    private void Init()
    {
        _uiManager = UIManager.Instance;
        _gameManager = GameManager.Instance;
    
        Debug.Assert(_uiManager != null, "Null Exception : UIManager");
        Debug.Assert(_gameManager != null, "Null Exception : GameManager");
    
        _gameSceneUIInputController = new GameSceneUIInputController(this);
    
        EnableInput(true);
        _gameManager.ActivateEscapeInput(false);
    }
    ```
    
    - PlayerInput에서 GameSceneUIInputController 객체 생성 부분

---

## 개선점 생각하기

### **아쉬운 점**

- 처음 설계할 때는 Input이 플레이어가 요리를 하거나 움직일 때만 쓰이는 것으로 기획되었기 때문에 Player에게 종속되도록 만들어짐. 하지만 프로젝트 **후반부에 Input과 관련된 다양한 기능이 추가되면서 Input 스크립트를 참조하는 것이 어려워짐**
- 처음에 Input을 활성화/비활성화 할 때 InputAction을 직접 활성화, 비활성화 하다 보니, Input이 비활성화 되어야 할 상황임에도 불구하고, 현재 상태와 상관 없이 Input이 활성화되는 경우도 발생하였음.
    - InputSystem을 제어하는 메서드를 통일시키고, 외부에서 제어 중인 InputAction을 HashSet에 저장하여 전체적인 InputSystem이 활성화/비활성화 될 때 제어 중인 InputAction에 영향이 가지 않도록 구조를 변경함
    - 하지만 Input의 통제를 더 큰 틀에서, 일관성 있게 Input 통제 상태를 관리할 필요성을 느낌

<br>

### **개선 방안**

- Input System이 여러 개 존재하고, 여러 씬에서 접근해서 통제해야 하는 상황이 되었을 때는 **Input을 전체적으로 관리하는 매니저 클래스를 만드는 것**도 좋은 방안이었을 것 같다고 생각.

---

## 주요 메서드

### PlayerInput

| 메서드 | 설명 |
| --- | --- |
| EnableInput | InputSystem asset전체를 활성/비활성화 하는 기능 |
| ControlInput | InputSystem내에서 특정한 InpuyAction만을 활성/비활성화 하는 기능 |
| IsMouseOverUIButton | 마우스 커서의 위치에 따라 플레이어의 인풋을 제어할 수 있도록 bool값을 반환하는 기능 |

<br>

### GameSceneUIInputController

| 메서드 | 설명 |
| --- | --- |
| AddCallbacks | UI 및 InputAction에 콜백 메서드를 연결하는 기능 |
| TogglePausePanel | ESC 키의 입력이 들어왔을 때, 상황에 따라 일시정지 UI를 띄워주거나 닫아주는 기능 |
| ToggleCookBook | TAB 키의 입력이 들어왔을 때, 상황에 따라 쿡북(조리법) UI를 띄워주거나 닫아주는 기능 |
| PauseCallback | 일시정지 UI가 열리거나 닫힐 때 발생하는 이벤트에 따라 Input이 제어될 수 있도록 해주는 Callback 메서드 |
| CookBookCallback | 쿡북(조리법) UI가 열리거나 닫힐 때 발생하는 이벤트에 따라 Input이 제어될 수 있도록 해주는 Callback 메서드 |


## [돌아가기](/)
