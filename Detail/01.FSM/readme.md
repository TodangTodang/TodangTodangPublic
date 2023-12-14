# [FSM] 플레이어의 상태관리를 위한 상태머신

![https://private-user-images.githubusercontent.com/77392694/289383169-73db63d0-0f09-4538-9e5a-30624a8599c6.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTEiLCJleHAiOjE3MDI0NDgxNDksIm5iZiI6MTcwMjQ0Nzg0OSwicGF0aCI6Ii83NzM5MjY5NC8yODkzODMxNjktNzNkYjYzZDAtMGYwOS00NTM4LTllNWEtMzA2MjRhODU5OWM2LnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFJV05KWUFYNENTVkVINTNBJTJGMjAyMzEyMTMlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjMxMjEzVDA2MTA0OVomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWI1MDZkN2UzNmE5MjBiYTE5OTU2ZWEyOTgwOWI0Yzk0YmZmMmFhN2QzMjcwZTY5ZDE4MDg5MTVhZGI0ZjBjZmUmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.2uCvh4bH85T5pilU8Q5eEejXW9btuuUSMNBetDCdfck](https://private-user-images.githubusercontent.com/77392694/289383169-73db63d0-0f09-4538-9e5a-30624a8599c6.png?jwt=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJnaXRodWIuY29tIiwiYXVkIjoicmF3LmdpdGh1YnVzZXJjb250ZW50LmNvbSIsImtleSI6ImtleTEiLCJleHAiOjE3MDI0NDgxNDksIm5iZiI6MTcwMjQ0Nzg0OSwicGF0aCI6Ii83NzM5MjY5NC8yODkzODMxNjktNzNkYjYzZDAtMGYwOS00NTM4LTllNWEtMzA2MjRhODU5OWM2LnBuZz9YLUFtei1BbGdvcml0aG09QVdTNC1ITUFDLVNIQTI1NiZYLUFtei1DcmVkZW50aWFsPUFLSUFJV05KWUFYNENTVkVINTNBJTJGMjAyMzEyMTMlMkZ1cy1lYXN0LTElMkZzMyUyRmF3czRfcmVxdWVzdCZYLUFtei1EYXRlPTIwMjMxMjEzVDA2MTA0OVomWC1BbXotRXhwaXJlcz0zMDAmWC1BbXotU2lnbmF0dXJlPWI1MDZkN2UzNmE5MjBiYTE5OTU2ZWEyOTgwOWI0Yzk0YmZmMmFhN2QzMjcwZTY5ZDE4MDg5MTVhZGI0ZjBjZmUmWC1BbXotU2lnbmVkSGVhZGVycz1ob3N0JmFjdG9yX2lkPTAma2V5X2lkPTAmcmVwb19pZD0wIn0.2uCvh4bH85T5pilU8Q5eEejXW9btuuUSMNBetDCdfck)

---

## 상태 머신의 활용

⚠️ **기술 도입 배경**

- 플레이어의 경우 손에 음식을 들고 있는 상태, 아닐 때의 상태에서의 행동이 다른데, 이를 하나의 메서드로 통제할 경우, 코드 가독성과 확장성이 매우 떨어질 우려
    - 플레이어와 손님의 경우 상태에 따라 행동이 달라, 이를 수월하게 통제할 요소가 필요

**💡 기술 도입으로 얻은 이점**

- 각 상태 별로 할 수 있는 행동을 제한하거나, 변경될 수 있는 상태를 지정함으로 써 상태에 따른 행동을 통제하기가 수월해짐.
- 또 새로운 행동이 추가되었다고 하더라도, 해당 행동에서 변화할 수 있는 상태나, 기능, 해당 기능에 붙어있는 상태들의 테스트만 진행하면 되었기 때문에, 확장에 유리

## **활용 결과**

---

### 💫 상태만 변경하여 각 상황에서 필요한 동작만을 실행할 수 있도록!

**관련 코드**

- PlayerStayState
    
    ```
    public class PlayerStayState : PlayerBaseState
    {
        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.StayParameterHash);
            stateMachine.IsWalking = false;
            stateMachine.Player.ActivateParticle(false);
        }
    
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.StayParameterHash);
        }
    
        public override void FixedUpdate()
        {
            base.FixedUpdate();
    
        }
    
        protected override void OnPickUp(InputAction.CallbackContext context)
        {
            base.OnPickUp(context);
            if (!stateMachine.IsHolding)
            {
                stateMachine.Player.PickUp();
                stateMachine.ChangeState(stateMachine.PickUpState);
            }
        }
    }
    ```
    
    - 플레이어가 이동하지 않는 상황
    - PickUp Input에 따라 파생 클래스들에서 공통적으로 수행해야하는 기능 정의 (PlayerPickUpState로 전환)
- PlayerPickUpState
    
    ```
    public class PlayerPickUpState : PlayerStayState
    {
        public override void Enter()
        {
            base.Enter();
            stateMachine.IsHolding = true;
            StartAnimation(stateMachine.Player.AnimationData.PickUpParameterHash);
        }
    
        public override void Exit()
        {
            StopAnimation(stateMachine.Player.AnimationData.PickUpParameterHash);
        }
    
        public override void Update()
        {
            base.Update();
    
            float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "PickUp");
            if (normalizedTime >= 0.9f)
            {
                if (stateMachine.Player.Ingredient == null)
                {
                    stateMachine.IsHolding = false;
                    if (stateMachine.Player.Input.PlayerActions.Move.ReadValue<Vector2>() != Vector2.zero)
                    {
                        stateMachine.ChangeState(stateMachine.WalkState);
                    }
                    else
                    {
                        stateMachine.ChangeState(stateMachine.IdleState);
                    }
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.HoldState);
                }
            }
        }
    
        public override void FixedUpdate()
        {
    
        }
    
        protected override void Move()
        {
    
        }
    
        protected override void OnPickUp(InputAction.CallbackContext context)
        {
    
        }
    
    }
    ```
    
    - PickUp애니메이션의 끝 지점에서 플레이어가 실제로 대상을 집어들었는지 확인한 후 결과에 따라 다른 상태로 전환
- PlayerHoldState
    
    ```
    public class PlayerHoldState : PlayerStayState
    {
        public override void Enter()
        {
            base.Enter();
            StartAnimation(stateMachine.Player.AnimationData.HoldParameterHash);
        }
    
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.HoldParameterHash);
        }
    
        public override void Update()
        {
            base.Update();
            if (stateMachine.MovementInput != Vector2.zero)
            {
                OnMove();
                return;
            }
        }
    
        public override void FixedUpdate()
        {
    
        }
    
        protected override void OnPickUp(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.PutDownState);
        }
    
        private void OnMove()
        {
            stateMachine.ChangeState(stateMachine.HoldAndWalkState);
        }
    
        protected override void OnInteraction(InputAction.CallbackContext context)
        {
            if (stateMachine.Player.IsInteractable())
                stateMachine.Player.Interaction();
    
        }
    }
    ```
    
    - PlayerPickUpState에서 Player가 실제로 물건을 집어들었을 경우 PlayerHoldState로 이동됨
    - 여기서 다시 PickUp Input이 들어온다면, 이전의 State들과는 다르게 PutDownState로 이동
    - PlayerHoldState에서 Player가 이동한다면 PlayerHoldAndWalkState로 이동
    - Interaction Input이 들어온다면, Player가 상호작용 가능한 상태일 때 PlayerInteractionState로 이동하도록

### 상세코드 보기

## 돌아가기