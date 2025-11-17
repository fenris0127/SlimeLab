# SlimeLab 개발 계획 - TDD 방식

## 개발 지침
- [ ] 표시된 항목을 순서대로 진행
- 각 테스트를 먼저 작성하고 실패를 확인
- 테스트가 통과할 최소한의 코드만 구현
- 테스트 통과 후 리팩토링 진행
- 완료된 항목은 [x]로 표시

## 프로젝트 초기 설정

### 1. 프로젝트 구조 생성
- [N/A] **Test**: 프로젝트 디렉토리 구조 테스트 작성
  - Unity 프로젝트이므로 해당 없음
- [N/A] **Implement**: 디렉토리 구조 생성
- [N/A] **Test**: package.json 파일 존재 및 기본 설정 테스트
- [N/A] **Implement**: package.json 생성 및 의존성 설정

### 2. 테스트 환경 설정
- [N/A] **Test**: Jest 설정 파일 존재 테스트
  - Unity NUnit 사용으로 해당 없음
- [N/A] **Implement**: Jest 설정 및 테스트 헬퍼 생성
- [N/A] **Test**: 첫 번째 통합 테스트 실행 확인
- [N/A] **Implement**: 테스트 스크립트 설정

## Phase 1: 핵심 도메인 모델 (Core Domain Models)

### 3. Slime 엔티티 기본 구현
- [x] **Test**: Slime 객체가 ID를 가져야 함
- [x] **Implement**: Slime 클래스 생성 with ID
- [x] **Test**: Slime이 이름을 가져야 함
- [x] **Implement**: name 속성 추가
- [x] **Test**: Slime이 기본 속성(element)을 가져야 함 (fire, water, electric, neutral)
- [x] **Implement**: element 속성 및 enum 추가
- [x] **Test**: Slime이 레벨을 가져야 함 (기본값 1)
- [x] **Implement**: level 속성 추가
- [x] **Test**: Slime이 경험치를 가져야 함 (기본값 0)
- [x] **Implement**: experience 속성 추가

### 4. SlimeStats 구현
- [x] **Test**: SlimeStats가 HP를 가져야 함
- [x] **Implement**: SlimeStats 클래스 생성 with HP
- [x] **Test**: SlimeStats가 공격력을 가져야 함
- [x] **Implement**: attack 속성 추가
- [x] **Test**: SlimeStats가 방어력을 가져야 함
- [x] **Implement**: defense 속성 추가
- [x] **Test**: SlimeStats가 속도를 가져야 함
- [x] **Implement**: speed 속성 추가
- [x] **Test**: Slime이 SlimeStats를 가져야 함
- [x] **Implement**: Slime에 stats 속성 추가

### 5. Resource 시스템
- [x] **Test**: Resource 타입 enum이 존재해야 함 (food, material, energy, research)
- [x] **Implement**: ResourceType enum 생성
- [x] **Test**: Resource 객체가 타입과 수량을 가져야 함
- [x] **Implement**: Resource 클래스 생성
- [x] **Test**: ResourceInventory가 자원을 추가할 수 있어야 함
- [x] **Implement**: ResourceInventory 클래스 및 add 메서드
- [x] **Test**: ResourceInventory가 자원을 소비할 수 있어야 함
- [x] **Implement**: consume 메서드 구현
- [x] **Test**: ResourceInventory가 자원 부족 시 예외를 던져야 함
- [x] **Implement**: InsufficientResourceError 구현

## Phase 2: 연구소 시스템 (Laboratory System)

### 6. Laboratory 기본 구현
- [x] **Test**: Laboratory가 ID와 이름을 가져야 함
- [x] **Implement**: Laboratory 클래스 생성
- [x] **Test**: Laboratory가 슬라임을 보관할 수 있어야 함
- [x] **Implement**: slimeStorage 컬렉션 추가
- [x] **Test**: Laboratory가 최대 보관 용량을 가져야 함
- [x] **Implement**: maxCapacity 속성 추가
- [x] **Test**: Laboratory가 슬라임을 추가할 수 있어야 함
- [x] **Implement**: addSlime 메서드 구현
- [x] **Test**: 용량 초과 시 예외를 던져야 함
- [x] **Implement**: CapacityExceededError 구현

### 7. Containment Unit (슬라임 격납 시설)
- [x] **Test**: ContainmentUnit이 하나의 슬라임을 보관할 수 있어야 함
- [x] **Implement**: ContainmentUnit 클래스 생성
- [x] **Test**: ContainmentUnit이 환경 타입을 가져야 함
- [x] **Implement**: environmentType 속성 추가
- [x] **Test**: 슬라임 속성과 환경이 맞지 않으면 효율이 떨어져야 함
- [x] **Implement**: getEfficiency 메서드 구현
- [x] **Test**: Laboratory가 여러 ContainmentUnit을 가져야 함
- [x] **Implement**: containmentUnits 컬렉션 추가

### 8. 슬라임 먹이 시스템
- [x] **Test**: Slime이 배고픔 수치를 가져야 함
- [x] **Implement**: hunger 속성 추가
- [x] **Test**: 시간이 지나면 배고픔이 증가해야 함
- [x] **Implement**: increaseHunger 메서드 구현
- [x] **Test**: Feed 액션으로 슬라임에게 먹이를 줄 수 있어야 함
- [x] **Implement**: FeedAction 클래스 구현 (Feed 메서드로 직접 구현)
- [x] **Test**: 먹이를 주면 배고픔이 감소하고 경험치가 증가해야 함
- [x] **Implement**: feed 메서드 구현
- [x] **Test**: 배고픔이 최대치를 넘으면 슬라임 상태가 unhappy가 되어야 함
- [x] **Implement**: SlimeMood enum 및 상태 관리 구현

## Phase 3: 유전자 조합 시스템 (Genetics System)

### 9. Gene 기본 구현
- [x] **Test**: Gene이 고유 ID와 이름을 가져야 함
- [x] **Implement**: Gene 클래스 생성
- [x] **Test**: Gene이 dominant/recessive 타입을 가져야 함
- [x] **Implement**: GeneType enum 및 속성 추가
- [x] **Test**: Slime이 Gene 컬렉션을 가져야 함
- [x] **Implement**: Slime에 genes 속성 추가

### 10. 교배 시스템 (Breeding)
- [x] **Test**: BreedingChamber가 두 슬라임을 받을 수 있어야 함
- [x] **Implement**: BreedingChamber 클래스 생성
- [x] **Test**: 호환되지 않는 슬라임 교배 시 실패해야 함
- [x] **Implement**: checkCompatibility 메서드 구현
- [x] **Test**: 교배 시작 시 자원을 소비해야 함
- [x] **Implement**: startBreeding 메서드 구현
- [x] **Test**: 교배 완료까지 시간이 필요해야 함
- [x] **Implement**: breedingDuration 및 타이머 구현
- [x] **Test**: 교배 결과로 새로운 슬라임이 생성되어야 함
- [x] **Implement**: completeBreeding 메서드 구현

### 11. 유전자 조합 규칙
- [x] **Test**: 부모의 dominant gene이 자식에게 우선 전달되어야 함
- [x] **Implement**: inheritGenes 메서드 구현
- [x] **Test**: 특정 gene 조합이 특별한 결과를 만들어야 함
- [x] **Implement**: GeneComboRegistry 구현
- [x] **Test**: 돌연변이가 낮은 확률로 발생해야 함
- [x] **Implement**: mutation 시스템 구현

## Phase 4: 진화 시스템 (Evolution System)

### 12. Evolution 기본 구현
- [x] **Test**: Slime이 특정 레벨에 도달하면 진화 가능해야 함
- [x] **Implement**: canEvolve 메서드 구현
- [x] **Test**: 진화에 특수 아이템이 필요해야 함
- [x] **Implement**: EvolutionItem 클래스 생성
- [x] **Test**: 진화 시 새로운 형태로 변해야 함
- [x] **Implement**: evolve 메서드 구현
- [x] **Test**: 진화 트리가 존재해야 함
- [x] **Implement**: EvolutionTree 데이터 구조 구현

### 13. 특수 진화 조건
- [x] **Test**: 환경 조건에 따른 진화가 가능해야 함
- [x] **Implement**: EnvironmentEvolution 구현
- [x] **Test**: 특정 시간대에만 가능한 진화가 있어야 함
- [x] **Implement**: TimeBasedEvolution 구현
- [x] **Test**: 친밀도 기반 진화가 가능해야 함
- [x] **Implement**: AffinityEvolution 구현

## Phase 5: 탐험 시스템 (Exploration System)

### 14. Zone 기본 구현
- [x] **Test**: Zone이 ID, 이름, 난이도를 가져야 함
- [x] **Implement**: Zone 클래스 생성
- [x] **Test**: Zone이 그리드 기반 맵을 가져야 함
- [x] **Implement**: GridMap 클래스 구현
- [x] **Test**: Zone에 입장 조건이 있어야 함
- [x] **Implement**: ZoneRequirement 구현

### 15. Expedition (원정대)
- [x] **Test**: Expedition이 슬라임 팀을 구성할 수 있어야 함
- [x] **Implement**: Expedition 클래스 생성
- [x] **Test**: 최대 팀 크기 제한이 있어야 함
- [x] **Implement**: maxTeamSize 검증
- [x] **Test**: 원정 시작 시 슬라임이 연구소에서 제거되어야 함
- [x] **Implement**: startExpedition 메서드 구현

### 16. 전투 시스템
- [x] **Test**: Battle이 턴 기반으로 진행되어야 함
- [x] **Implement**: Battle 클래스 및 턴 시스템
- [x] **Test**: 슬라임 속도에 따라 행동 순서가 결정되어야 함
- [x] **Implement**: TurnOrder 계산 로직
- [x] **Test**: 기본 공격이 가능해야 함
- [x] **Implement**: AttackAction 구현
- [x] **Test**: 속성 상성이 데미지에 영향을 줘야 함
- [x] **Implement**: ElementalAdvantage 시스템

### 17. 자원 수집
- [x] **Test**: Zone에서 자원을 발견할 수 있어야 함
- [x] **Implement**: ResourceNode 클래스
- [x] **Test**: 슬라임 능력에 따라 수집 효율이 달라야 함
- [x] **Implement**: gatherResource 메서드
- [x] **Test**: 원정 완료 시 수집한 자원을 획득해야 함
- [x] **Implement**: completeExpedition 메서드

## Phase 6: 자동화 시스템 (Automation System)

### 18. Feeder (자동 먹이 공급기)
- [x] **Test**: Feeder가 일정 간격으로 먹이를 공급해야 함
- [x] **Implement**: AutoFeeder 클래스 생성
- [x] **Test**: Feeder가 자원을 소비해야 함
- [x] **Implement**: consumeResource 로직
- [x] **Test**: Feeder를 ContainmentUnit에 연결할 수 있어야 함
- [x] **Implement**: attachFeeder 메서드

### 19. Collector (자원 수집기)
- [x] **Test**: Collector가 슬라임이 생성하는 자원을 수집해야 함
- [x] **Implement**: ResourceCollector 클래스
- [x] **Test**: 수집 효율이 업그레이드 가능해야 함
- [x] **Implement**: upgrade 시스템

### 20. Sorter (슬라임 분류기)
- [x] **Test**: Sorter가 슬라임을 속성별로 분류해야 함
- [x] **Implement**: SlimeSorter 클래스
- [x] **Test**: 커스텀 분류 규칙을 설정할 수 있어야 함
- [x] **Implement**: SortingRule 시스템

## Phase 7: 연구 시스템 (Research System)

### 21. Tech Tree
- [x] **Test**: TechNode가 연구 가능 상태를 가져야 함
- [x] **Implement**: TechNode 클래스
- [x] **Test**: 선행 연구가 완료되어야 연구 가능해야 함
- [x] **Implement**: prerequisite 시스템
- [x] **Test**: 연구에 자원과 시간이 필요해야 함
- [x] **Implement**: startResearch 메서드
- [x] **Test**: 연구 완료 시 새로운 기능이 해금되어야 함
- [x] **Implement**: unlockFeature 메서드

### 22. 연구 보너스
- [x] **Test**: 연구 완료 시 영구 보너스를 제공해야 함
- [x] **Implement**: PermanentBonus 시스템
- [x] **Test**: 보너스가 스택 가능해야 함
- [x] **Implement**: BonusStack 구현

## Phase 8: 이벤트 시스템 (Event System)

### 23. Random Events
- [x] **Test**: RandomEvent가 확률적으로 발생해야 함
- [x] **Implement**: EventScheduler 클래스
- [x] **Test**: 이벤트가 선택지를 제공해야 함
- [x] **Implement**: EventChoice 시스템
- [x] **Test**: 선택에 따라 다른 결과가 발생해야 함
- [x] **Implement**: EventOutcome 처리

### 24. Story Events
- [x] **Test**: 스토리 진행도에 따라 이벤트가 트리거되어야 함
- [x] **Implement**: StoryProgress 트래킹
- [x] **Test**: 스토리 이벤트가 새로운 콘텐츠를 해금해야 함
- [x] **Implement**: ContentUnlock 시스템

## Phase 9: 저장/불러오기 시스템 (Save System)

### 25. Save Game
- [x] **Test**: 게임 상태를 JSON으로 직렬화할 수 있어야 함
- [x] **Implement**: GameState serialization
- [x] **Test**: 저장 파일이 로컬 스토리지에 저장되어야 함
- [x] **Implement**: SaveManager 구현
- [x] **Test**: 자동 저장이 일정 간격으로 실행되어야 함
- [x] **Implement**: AutoSave 시스템

### 26. Load Game
- [x] **Test**: 저장된 게임을 불러올 수 있어야 함
- [x] **Implement**: LoadManager 구현
- [x] **Test**: 버전 호환성을 체크해야 함
- [x] **Implement**: VersionMigration 시스템

## Phase 10: UI 시스템 (UI System)

### 27. Main UI Components
- [x] **Test**: SlimeCard 컴포넌트가 슬라임 정보를 표시해야 함
- [x] **Implement**: SlimeCard 컴포넌트
- [x] **Test**: ResourceBar가 자원 현황을 표시해야 함
- [x] **Implement**: ResourceBar 컴포넌트
- [x] **Test**: LabView가 연구소 상태를 시각화해야 함
- [x] **Implement**: LabView 컴포넌트

### 28. Interaction System
- [x] **Test**: 드래그 앤 드롭으로 슬라임을 이동할 수 있어야 함
- [x] **Implement**: DragDrop 시스템
- [x] **Test**: 우클릭 메뉴가 컨텍스트 액션을 제공해야 함
- [x] **Implement**: ContextMenu 시스템

## Phase 11: 사운드 시스템 (Sound System)

### 29. Audio Manager
- [x] **Test**: AudioManager가 배경음악을 재생할 수 있어야 함
- [x] **Implement**: AudioManager 클래스
- [x] **Test**: 상황에 따라 BGM이 전환되어야 함
- [x] **Implement**: MusicTransition 시스템
- [x] **Test**: 효과음이 이벤트에 따라 재생되어야 함
- [x] **Implement**: SoundEffect 시스템

## Phase 12: 최적화 및 마무리

### 30. Performance
- [x] **Test**: 100개 이상의 슬라임 처리 시 60fps 유지해야 함
- [x] **Implement**: 최적화 (object pooling, culling)
- [x] **Test**: 메모리 누수가 없어야 함
- [x] **Implement**: 메모리 관리 개선

### 31. Polish
- [x] **Test**: 모든 사용자 액션에 피드백이 있어야 함
- [x] **Implement**: Visual/Audio 피드백 추가
- [x] **Test**: 튜토리얼이 핵심 기능을 설명해야 함
- [x] **Implement**: Tutorial 시스템

## 완료 기준
- ✅ 모든 테스트가 작성됨 (29 test files)
- ✅ 모든 기능이 구현됨 (77 production files)
- ✅ TDD 방식으로 개발 완료 (Red-Green-Refactor)
- ✅ 31개 섹션 모두 완료

## 프로젝트 통계
- **생성 파일 수**: 106개 (77개 구현 + 29개 테스트)
- **커밋 수**: 18개 (세션 14-31)
- **구현된 시스템**: 11개 주요 시스템 (Core, Laboratory, Genetics, Evolution, Exploration, Automation, Research, Events, Save/Load, UI, Audio, Performance, Polish)

## 다음 단계
"go" 명령어를 입력하면 다음 미완료 테스트를 찾아 구현합니다.
각 단계는:
1. 테스트 작성 (RED)
2. 최소 구현 (GREEN)
3. 리팩토링 (REFACTOR)
순서로 진행됩니다.