# 최종 readme

![AppIconwide.png](%E1%84%8E%E1%85%AC%E1%84%8C%E1%85%A9%E1%86%BC%20readme%20223ab4c9585c4f9c8cfd1ec97ef038e3/AppIconwide.png)

## 목차

[📅 프로젝트 미리 보기 📅](about:blank#%ED%94%84%EB%A1%9C%EC%A0%9D%ED%8A%B8-%EB%AF%B8%EB%A6%AC-%EB%B3%B4%EA%B8%B0)

---

[💼 개요 💼](about:blank#%EA%B0%9C%EC%9A%94)

---

[📜 사용 기술 스택 📜](about:blank#%EA%B5%AC%ED%98%84-%EB%82%B4%EC%9A%A9)

---

[🛠 클래스 구성 🛠](about:blank#%ED%81%B4%EB%9E%98%EC%8A%A4-%EA%B5%AC%EC%84%B1)

---

[⚔️ 트러블 슈팅, 기술적 토론 ⚔️](about:blank#%ED%8A%B8%EB%9F%AC%EB%B8%94-%EC%8A%88%ED%8C%85-%EA%B8%B0%EC%88%A0%EC%A0%81-%ED%86%A0%EB%A1%A0)

---

[💡 만든 사람들 💡](about:blank#%EB%A7%8C%EB%93%A0-%EC%82%AC%EB%9E%8C%EB%93%A4)

---

---

# 프로젝트 미리 보기

### [📹 ⭐ 시연 영상 보기 ⭐](https://www.youtube.com/watch?v=e9uwlPD9TDc)

### [🌈 팀 노션 🌈](https://www.notion.so/zl-4c1a665e36aa4a47b40d4a064666cc6d?pvs=21)

### 게임 미리보기

![게임 미리보기.png](%E1%84%8E%E1%85%AC%E1%84%8C%E1%85%A9%E1%86%BC%20readme%20223ab4c9585c4f9c8cfd1ec97ef038e3/%25EA%25B2%258C%25EC%259E%2584_%25EB%25AF%25B8%25EB%25A6%25AC%25EB%25B3%25B4%25EA%25B8%25B0.png)

- 게임 내용
    - 떡과 차를 만들어서 판매하는 3D 타이쿤 게임 입니다.
    - 재고관리와 업그레이드 시스템을 통해 최종적으로 평점 5점을 달성하여 떡집을 물려받는 스토리의 게임입니다.

### [📌 목차로 돌아가기 📌](about:blank#%EB%AA%A9%EC%B0%A8)

---

# 개요

| 게임명 | 토당토당 찰떡쿵 |
| --- | --- |
| 장르 | 음식 판매 및 경영 타이쿤 |
| 개발 환경 | Unity 2022.3.2f1 |
| 타겟 플랫폼 | Android / PC / Web |
| 개발 기간 | 2023.10.23 ~ 2023.12.15 |

### [📌 목차로 돌아가기 📌](about:blank#%EB%AA%A9%EC%B0%A8)

---

# 사용 기술 스택

[[FSM] 플레이어의 상태관리를 위한 상태머신](https://www.notion.so/FSM-d6b630ee758e456eaa40d1a816ad1e0d?pvs=21)

[[ObjectPool] 객체 재활용을 통한 생성 오버헤드를 줄이자](https://www.notion.so/ObjectPool-55a46ec8479b4841853029ac736bbd6f?pvs=21)

[[Singleton&Generic] UI를 편리하게 관리하기 위한 UIManager](https://www.notion.so/Singleton-Generic-UI-UIManager-b0ddef3ed78a4971afc3fbd2d1fc4802?pvs=21)

[[MVC 개선] MVC를 확장성 있게 변형한 Inventory 구현](https://www.notion.so/MVC-MVC-Inventory-258c7b4c5d0243eabb2cb3374939bcee?pvs=21)

[[New InputSystem] 크로스 플랫폼 대응](https://www.notion.so/New-InputSystem-9f413502828e4c6db9e1233256be7b43?pvs=21)

[[IMGUI] 빠른 개발과 테스트를 위한 Cheater](https://www.notion.so/IMGUI-Cheater-3c35f323e76c43b4a104d2e34760a5b2?pvs=21)

[[ObserverPattern] 다양한 데이터를 저장하는 DataManager](https://www.notion.so/ObserverPattern-DataManager-dfcb69929bc544d28c74ef50aeb80009?pvs=21)

[[인터페이스] 다형성을 활용한 주방 기구 설계](https://www.notion.so/5a2fce0c88c14807862271b1dc49868c?pvs=21)

[[Firebase Analytics] 게임 플레이 분석을 위한 외부 모듈 사용](https://www.notion.so/Firebase-Analytics-0e00c5d6acae4bb1a26bf6beb77e0e8e?pvs=21)

[[해상도 대응] 다양한 기기에서 적절하게 보여주기 위한 방법](https://www.notion.so/cd689e07997546839af124154df6b003?pvs=21)

### [📌 목차로 돌아가기 📌](about:blank#%EB%AA%A9%EC%B0%A8)

---

## 기술적 고민 & 트러블 슈팅

**[기술적 고민]**

[인벤토리 탭 UI의 구현 방식](https://www.notion.so/UI-c51e361c674c4854bfce60bffdc3bb0e?pvs=21)

[요리 과정을 검증하는 방식](https://www.notion.so/f42615ae786d4e2c8fd2b90c0578a9ee?pvs=21)

[UIManager를 통해 Popup들을 관리하는 방식 ](https://www.notion.so/UIManager-Popup-872eb52b52ef4f9991b8e2cb62b62ef3?pvs=21)

**[트러블 슈팅]**

[마우스 좌클릭의 동작 ](https://www.notion.so/d9e07e65af2846ec85010b7245576f16?pvs=21)

[손님 AI의 Trigger 충돌 중복 ](https://www.notion.so/AI-Trigger-db6a20a01d874bd0b6a63f2ff3b2a0bb?pvs=21)

### [📌 목차로 돌아가기 📌](about:blank#%EB%AA%A9%EC%B0%A8)

---

## 만든 사람들

[🍡Team. 십이갅zl🐇]

| 이름 | 태그 | 담당 | Github 주소 | 블로그 주소 |
| --- | --- | --- | --- | --- |
| 노동균 | 팀장 | 손님AI, 뉴스, 프롤로그 씬, 게임 엔딩 씬, 데이터, Cheater, 애널리틱스  | https://github.com/shehdrbs123 | https://blog.naver.com/shehdrbs123 |
| 김정민 | 부팀장 | 인벤토리, 장식품 상점, 튜토리얼, 게임 오버 씬, 연습모드, 오브젝트 풀링, UIManager, UI디자인 | https://github.com/j-miiin | https://velog.io/@lazypotato |
| 박희원 | 팀원 | 재료주문, 하루결산, 게임설정, 해상도대응, 파산씬, Sound | https://github.com/phw97123 | https://hwon-note.tistory.com/ |
| 이현지        | 플레이어, 입력시스템, 주방기구, 뉴스, 연습모드, 쿡북, 닷트윈, 애니메이션 디자인, UI디자인 | https://github.com/szlovelee | https://szloveleesz.tistory.com/ |

### [📌 목차로 돌아가기 📌](about:blank#%EB%AA%A9%EC%B0%A8)
