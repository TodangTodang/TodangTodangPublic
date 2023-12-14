# [해상도 대응] 다양한 기기에서 적절하게 보여주기 위한 방법

## 개요

- 다양한 화면 해상도에 대응
- 폴더블 기기에서도 실시간으로 변경되는 해상도 대응

## 기술 도입 배경

모바일 디바이스는 스마트폰부터 태블릿까지 다양한 해상도를 가지고 있다

해상도에 따라 카메라에서 보이는 영역이 달라 배치된 오브젝트가 보이지 않거나 너무 많은 빈 공간이 보이게 되어 시각적 일관성을 맞추기 위해 도입

## 주요 코드

```
private IEnumerator UpdateCameraSize()
 {
     while (!IsObjectInScreen())
     {
         if (_camera.orthographic)
             _camera.orthographicSize += 1;
         else
             _camera.fieldOfView += 1;

         yield return null;
     }
 }

 private bool IsObjectInScreen()
 {
     Vector3 updatedScreenPoint = _camera.WorldToScreenPoint(_standardResolution.position);
     int screenWidth = Screen.width;
     int screenHeight = Screen.height;

     return (updatedScreenPoint.x >= 0 && updatedScreenPoint.x <= screenWidth &&
             updatedScreenPoint.y >= 0 && updatedScreenPoint.y <= screenHeight);
 }
```

기준이 되는 오브젝트를 배치하고, ScreenPoint를 가져와서 현재 해상도 안에 들어와있지 않다면 카메라 Size를 조절

---

## 트러블 슈팅

### 문제

- 다양한 해상도에 따라서 배치된 오브젝트들이 보이지 않거나 너무 많은 빈 공간이 보이게 되어 게임 플레이에 영향을 끼침

### 시도

- 레터박스를 이용하여 고정된 화면 비율을 나태내는 방법
    
    → Main 카메라 외에도 UI만 그리는 별도의 카메라를 사용해야 레터박스를 벗어나지 않고 잔상이 남지 않게 UI를 그릴 수 있다
    
    → 추가적으로 카메라를 사용하게 되어 GPU가 두 번의 렌더링을 수행하여 메모리가 추가로 소비하게 된다
    
- 가장 극단적인 해상도 비율을 계산하고 현재 해상도와 비교하여 비율의 차이만큼 카메라 Size를 조절하는 방법
    
    → 가장 극단적인 해상도 비율이라는 매직 넘버를 사용하게 되어 더 극단적인 경우가 발생한다면 유동적으로 대응하기 힘들다
    

### 해결

- 기준이 되는 물체를 두고 ScreenPoint를 가져와서 해상도 안에 들어올 때까지 카메라 사이즈를 조절
    
    → 추가적인 카메라를 사용하지 않아 추가적인 메모리를 소모하지 않는다
    
    → 매직 넘버를 사용하지 않고 유동적으로 화면 해상도에 맞게 일관된 디자인을 보여줄 수 있다
    

## 적용 결과

- 2400 x1080 (기본)

![2400x1080.png](/Detail/10.ResolutionHanding/해상도%20대응/2400x1080.png)
- Galaxy Fold 2152x1536 (해결 전)

![2152x1536.png](/Detail/10.ResolutionHanding/해상도%20대응/2152x1536.png)

- Galaxy Fold 2152x1536 (해결 후)

![2152x1536 1.png](/Detail/10.ResolutionHanding/해상도%20대응/2152x1536_1.png)