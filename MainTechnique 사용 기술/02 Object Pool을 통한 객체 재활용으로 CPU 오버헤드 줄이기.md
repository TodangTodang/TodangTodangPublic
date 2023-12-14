![header](https://capsule-render.vercel.app/api?type=cylinder&color=A1B6FF&height=150&section=header&text=Object%20Pooling&fontSize=60&fontColor=ECFBFF&animation=fadeIn)

<br>


## :crescent_moon: 목차

| [🐰 개요 🐰](#rabbit-개요) |
| :---: |
| [🐇 기술 도입 배경 🐇](#rabbit2-기술-도입-배경) |
| [🍡 주요 메서드 🍡](#dango-주요-메서드) |
| [🍵 활용 🍵](#tea-활용) |

<br>

* * *

<br>

## :rabbit: 개요  
- PoolManager를 통해 오브젝트 풀링을 간편하게 한다.
- 오브젝트 풀링을 통해 오브젝트 생성/파괴 비용은 물론 가비지 컬렉션을 줄인다.

<br>

* * *

<br>

## :rabbit2: 기술 도입 배경

> 문제점<br>
> 인벤토리에서 탭을 바꾸는 동작은 매우 빈번하게 일어날 수 있다.<br>
> 이 과정에서 슬롯 오브젝트를 매번 생성/파괴하면 비용이 많이 발생하게 되며, 메모리의 생성과 삭제로 인한 가비지가 발생할 수 있다.<br>
> 또한 인벤토리 뿐만 아니라 요리 과정에서도 재료, 중간 결과물, 완성된 음식 오브젝트의 생성 및 파괴가 빈번하게 발생하여 가비지로 인해 CPU 성능이 저하될 수 있다.<br>
- 오브젝트의 생성 및 파괴로 발생하는 가비지로 인해 GC Spike가 발생하여 유저 경험이 저하되는 것은 바람직하지 않다고 판단해 해당 기술을 도입하였다.
- 오브젝트 풀을 생성할 때 Queue의 초기 사이즈를 지정하여 메모리 효율을 높이고자 하였다.

<br>

* * *

<br>

## :dango: 주요 메서드

### ResourceManager

|메서드|기능|
|:---:|:---:|
|[Instantiate](https://github.com/TodangTodang/TodangTodangPublic/blob/67cd97076bd3a93284b40d230c277ccbe69850f4/Scripts/Manager/ResourceManager.cs#L30-L63)|Resources 폴더에서 해당 Prefab을 Load한다.<br>해당 오브젝트에 Poolable 컴포넌트가 있다면 PoolManager에게 오브젝트 Pop을 요청한다.|
|[Destroy](https://github.com/TodangTodang/TodangTodangPublic/blob/67cd97076bd3a93284b40d230c277ccbe69850f4/Scripts/Manager/ResourceManager.cs#L65-L78)|게임 오브젝트를 Destroy한다.<br>해당 오브젝트가 Poolable 컴포넌트를 가지고 있다면 PoolManager에게 오브젝트 Push를 요청한다.|

<br>

### PoolManager

|메서드|기능|
|:---:|:---:|
|[CreatePool](https://github.com/TodangTodang/TodangTodangPublic/blob/2b5e16ffc5b48cefbdae98689b16329ac1593c9a/Scripts/Manager/PoolManager.cs#L9-L18)|새로운 오브젝트 풀을 생성한다.|
|[Push](https://github.com/TodangTodang/TodangTodangPublic/blob/2b5e16ffc5b48cefbdae98689b16329ac1593c9a/Scripts/Manager/PoolManager.cs#L20-L33)|오브젝트 풀 Dictionary에 요청 받은 오브젝트의 풀이 있다면 해당 풀에 오브젝트를 넣는다.<br>풀이 존재하지 않는 오브젝트일 경우 Destroy한다.|
|[Pop](https://github.com/TodangTodang/TodangTodangPublic/blob/2b5e16ffc5b48cefbdae98689b16329ac1593c9a/Scripts/Manager/PoolManager.cs#L35-L43)|오브젝트 풀 Dictionary에 요청 받은 오브젝트를 Pop하여 반환한다.<br>풀이 존재하지 않는 오브젝트라면 해당 오브젝트의 풀을 새로 생성한다.|
|[Clear](https://github.com/TodangTodang/TodangTodangPublic/blob/2b5e16ffc5b48cefbdae98689b16329ac1593c9a/Scripts/Manager/PoolManager.cs#L45-L48)|현재 관리 하는 오브젝트 풀 Dictionary를 Clear한다.<br>Scene이 변경되었을 때 Destroy 되어 접근할 수 없는 오브젝트들에 대한 참조를 방지하기 위해 사용한다.|

<br>

### Pool

|메서드|기능|
|:---:|:---:|
|[Init](https://github.com/TodangTodang/TodangTodangPublic/blob/2b5e16ffc5b48cefbdae98689b16329ac1593c9a/Scripts/Manager/ObjectPooling/Pool.cs#L12-L22)|처음 풀이 생성되었을 때 해당 풀에서 관리하는 오브젝트, 풀의 루트 Transform을 설정한다.<br>기본적으로 3개의 오브젝트를 생성한다.|
|[Create](https://github.com/TodangTodang/TodangTodangPublic/blob/2b5e16ffc5b48cefbdae98689b16329ac1593c9a/Scripts/Manager/ObjectPooling/Pool.cs#L24-L31)|Init 메서드에서 설정한 해당 풀의 오브젝트를 생성한다.|
|[Push](https://github.com/TodangTodang/TodangTodangPublic/blob/2b5e16ffc5b48cefbdae98689b16329ac1593c9a/Scripts/Manager/ObjectPooling/Pool.cs#L33-L43)|풀에 오브젝트를 넣는다.<br>풀의 루트를 Parent로 설정한 뒤, 해당 오브젝트를 비활성화하고 큐에 넣는다.|
|[Pop](https://github.com/TodangTodang/TodangTodangPublic/blob/2b5e16ffc5b48cefbdae98689b16329ac1593c9a/Scripts/Manager/ObjectPooling/Pool.cs#L45-L64)|풀에서 오브젝트를 꺼낸다.<br>지정된 Parent가 있다면 해당 Transform을 Parent로 설정한 뒤 오브젝트를 활성화한다.<br>지정된 Parent가 없을 경우 현재 풀의 루트를 Parent로 설정한다.<br>큐에 오브젝트가 있다면 해당 오브젝트를 꺼내고, 없다면 Create 메서드로 생성한다.|

<br>

[🌙 목차로 돌아가기](#crescent_moon-목차)

<br>

* * *

<br>

## :tea: 활용 

### 오브젝트 풀링 활용하기

1. 풀링을 할 오브젝트에 Poolable 스크립트를 컴포넌트로 추가한다.
![image](https://github.com/j-miiin/TodangTodangCodes/assets/62470991/5bea7661-4b63-47fe-aabb-36191912e548)

2. 해당 오브젝트를 생성/파괴할 때 ResourceManager의 Instantiate와 Destroy 메서드를 사용한다.
    ```cs
    GameObject go = ResourceManager.Instance.Instantiate(Strings.Prefabs.UI_INVENTORY_SLOT, _scrollViewContainer);
    ResourceManager.Instance.Destroy(go);
    ```
    - 인벤토리 슬롯은 ScrollView의 Content 하위 오브젝트로 생성되어야 하므로 부모를 설정해주었다.
    
<br>

[🌙 목차로 돌아가기](#crescent_moon-목차)

<br><br>


#### [🐰 상세코드 보기 🐰]()

#### [🌙 Main README로 돌아가기 🌙](/README.md)
