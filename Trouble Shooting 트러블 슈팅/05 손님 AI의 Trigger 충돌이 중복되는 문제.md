![header](https://capsule-render.vercel.app/api?type=cylinder&color=ffd3d3&height=150&section=header&text=Customer%20Trigger&fontSize=60&fontColor=ECFBFF&animation=fadeIn)

<br>

### ⚠️ 문제
- 충돌체가 두 개 존재하였고, 이로 인해 들어올 때 TriggerEnter 한 번 나갈 때 TriggerEnter 한 번으로, 가판대에 위치했을 때 일어나는 행위가 또 실행되는 문제

<br>

### 🛠️ 시도
- Trigger가 닿지 않도록 trigger의 위치 변경하여 Trigger가 두 번 충돌되지 않도록 방지
     → 수동적인 수정 방식으로 충돌이 발생할 가능성 존재하므로 완벽하지 않음.
- Layer Matrix를 이용한 방법
     → 이중 충돌을 방지는 할 수 있음
     → 하지만 스크립트 자체에 통제되는 영역이 아닌 점
     → 추가적인 변수에 대한 대응을 매번해야 한다는 단점이 있음

<br>

### 💡 선택
- 손님 AI에게 상태머신을 도입하여 손님의 상태에 따라 행위를 통제
    → 현재 손님 상태에 따른 각 행위의 예외 상황을 완전하게 통제 가능하고, 특정 상태에서만 알맞는 Trigger를 처리함으로써 중복된 처리 방지
    

<br><br>


#### [🐰 상세코드 보기 🐰]()

#### [🌙 Main README로 돌아가기 🌙](/README.md)
