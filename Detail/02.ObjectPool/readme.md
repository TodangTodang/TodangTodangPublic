# [ObjectPool] 객체 재활용을 통한 생성 오버헤드를 줄이자

## 🐰 개요

- PoolManager를 통해 오브젝트 풀링을 간편하게 한다.
- 오브젝트 풀링을 통해 오브젝트 생성/파괴 비용을 줄인다.

---

## 🐇 기술 도입 배경

> ## 문제점
> 인벤토리에서 탭을 바꾸는 동작은 매우 빈번하게 일어날 수 있다.
> 
> 이 과정에서 슬롯 오브젝트를 매번 생성/파괴하면 비용이 매우 많이 들어 CPU 성능에 영향을 미칠 수 있다.
> 
> 인벤토리 뿐만 아니라 요리 과정에서도 재료, 중간 결과물, 완성된 음식 Prefab들이 매번 생성/파괴되는 동작이 자주 발생하여 성능 저하가 일어날 수 있다.
> 
> <br>
- 오브젝트의 생성/파괴 비용과 오브젝트 풀 생성에 소모되는 메모리를 고려했을 때, 오브젝트 풀을 활용하는 것이 더 효율적이라고 판단하여 해당 기술을 도입하였다.
- 오브젝트 풀을 생성할 때 Queue의 초기 사이즈를 지정하여 메모리 효율을 높이고자 하였다.

---

## 🍡 주요 메서드

### ResourceManager

| 메서드 | 기능 |
| --- | --- |
| [Instantiate](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/ResourceManager.cs#L30-L65) | Resources 폴더에서 해당 Prefab을 Load한다.해당 오브젝트에 Poolable 컴포넌트가 있다면 PoolManager에게 오브젝트 Pop을 요청한다. |
| [Destroy](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/ResourceManager.cs#L67-L80) | 게임 오브젝트를 Destroy한다.해당 오브젝트가 Poolable 컴포넌트를 가지고 있다면 PoolManager에게 오브젝트 Push를 요청한다. |

### PoolManager

| 메서드 | 기능 |
| --- | --- |
| [CreatePool](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/PoolManager.cs#L9-L18)| 새로운 오브젝트 풀을 생성한다. |
| [Push](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/PoolManager.cs#L20-L33) | 오브젝트 풀 Dictionary에 요청 받은 오브젝트의 풀이 있다면 해당 풀에 오브젝트를 넣는다.풀이 존재하지 않는 오브젝트일 경우 Destroy한다. |
| [Pop](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/PoolManager.cs#L35-L43) | 오브젝트 풀 Dictionary에 요청 받은 오브젝트를 Pop하여 반환한다.풀이 존재하지 않는 오브젝트라면 해당 오브젝트의 풀을 새로 생성한다. |
| [Clear](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/PoolManager.cs#L45-L48) | 현재 관리 하는 오브젝트 풀 Dictionary를 Clear한다.Scene이 변경되었을 때 Destroy 되어 접근할 수 없는 오브젝트들에 대한 참조를 방지하기 위해 사용한다. |

### Pool

| 메서드 | 기능 |
| --- | --- |
| [Init](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/Pool.cs#L12-L22) | 처음 풀이 생성되었을 때 해당 풀에서 관리하는 오브젝트, 풀의 루트 Transform을 설정한다.기본적으로 3개의 오브젝트를 생성한다. |
| [Create](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/Pool.cs#L24-L31) | Init 메서드에서 설정한 해당 풀의 오브젝트를 생성한다. |
| [Push](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/Pool.cs#L33-L43) | 풀에 오브젝트를 넣는다.풀의 루트를 Parent로 설정한 뒤, 해당 오브젝트를 비활성화하고 큐에 넣는다. |
| [Pop](https://github.com/j-miiin/TodangTodangCodes/blob/8743167cc63b3244252e8718c75dec9b9c05d51e/Object%20Pooling/Pool.cs#L45-L64) | 풀에서 오브젝트를 꺼낸다.지정된 Parent가 있다면 해당 Transform을 Parent로 설정한 뒤 오브젝트를 활성화한다.지정된 Parent가 없을 경우 현재 풀의 루트를 Parent로 설정한다.큐에 오브젝트가 있다면 해당 오브젝트를 꺼내고, 없다면 Create 메서드로 생성한다. |

[🌙 목차로 돌아가기]()

---

## 🍵 활용

### 오브젝트 풀링 활용하기

1. 풀링을 할 오브젝트에 Poolable 스크립트를 컴포넌트로 추가한다.
    
    ![Image](Image/pooling.png)
    
2. 해당 오브젝트를 생성/파괴할 때 ResourceManager의 Instantiate와 Destroy 메서드를 사용한다.
    
    ```
    GameObject go = ResourceManager.Instance.Instantiate(Strings.Prefabs.UI_INVENTORY_SLOT, _scrollViewContainer);
    ResourceManager.Instance.Destroy(go);
    ```
    
    - 인벤토리 슬롯은 ScrollView의 Content 하위 오브젝트로 생성되어야 하므로 부모를 설정해주었다.


[🌙 목차로 돌아가기]()

## [돌아가기](/)