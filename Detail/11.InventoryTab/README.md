# 인벤토리 탭 UI의 구현 방식

![인벤토리탭.png](/Detail//11.InventoryTab//인벤토리%20탭%20UI의%20구현%20방식/11.InventoryTab.png)
🗨️ **고민** 

인벤토리 UI를 열면 세 가지의 탭으로 나누어져 있고, 레이아웃이 같아서 이를 효과적으로 재사용하는 방법에 대한 고민 

**🛠️ 시도**

- 각 탭마다 3가지의 UI를 미리 만들어서 탭을 누를 때 Sort Order를 조절하여 해당하는 탭 UI가 나타나도록 구현
    
    → 매번 보이지 않는 UI까지 모두 생성해야 하는 문제
    
    → 해당 탭을 클릭하지 않는다면 불필요한 생성을 하게 되는 것이므로 효율성 저하
    

**💡 선택**

- 1개의 UI를 재사용하는 방식
    - 각 탭을 클릭하면 탭에 맞는 데이터들을 보여주도록 구현
    - ScrollView의 슬롯 오브젝트들은 오브젝트 풀을 활용
        - 탭을 누를 때마다 ScrollView가 갱신되어야 하는데, 이때마다 슬롯 오브젝트들을 생성하고 파괴하는 것은 비효율적인 동작
        - 탭을 누르는 것은 자주 일어날 수 있는 동작이므로 매번 오브젝트를 생성/파괴하면 CPU 성능 저하
        - 오브젝트 풀을 활용하여 미리 처음 인벤토리를 열었을 때 슬롯 오브젝트들을 생성한 뒤, 탭이 변경될 때마다 해당 탭의 데이터 개수만큼 슬롯 오브젝트들을 풀에서 가져와 사용
        
        → 반복적인 생성/ 파괴를 최소화하여 CPU 성능 향상 
        
        → 오브젝트 풀을 통해 미리 생성된 오브젝트를 재활용하여 효율적인 동작 가능
        

### 상세코드 보기

### [돌아가기](/README.md)