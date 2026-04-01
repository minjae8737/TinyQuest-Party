# TinyQuest Party

**TinyQuest Party**는 Unity 기반의 **방치형 성장 게임** 프로젝트입니다.
MVC 구조를 중심으로 전투, 성장, 편성 루프를 구현하여 결합도를 낮추고 유지보수성을 확보했습니다.

## 🛠 개발 환경 및 기술 스택
* **작업 기간**: `2026/01/07 ~ 진행 중`
* **OS**: `Mac OS 26.4`
* **Engine**: `Unity 6000.0.69f1` 
* **IDE**: `Rider`
* **Version Control**: `Git (GitHub, Sourcetree)` 
* **Language**: `C#` 

---

## 🎮 플레이 영상
<img src="https://github.com/user-attachments/assets/4ec99e71-0a18-48a7-ac4e-77d0e8b41c88" height="450"/>


---
## ✨ 핵심 구현 사항
- **MVC 패턴 적용 구조**
    - Controller: 입력/전투 진행/이벤트 연결 담당.
    - Model: 수치 연산(피해/회복/스탯)과 상태 변경 담당.
    - View: 애니메이션/HP바/피격 텍스트 등 표현 담당.
    - 결합도를 낮춰 기능 변경 시 영향 범위를 줄이고 테스트·확장에 유리한 구조.
<details>
<summary>코드</summary>
      
```csharp
// UnitController.cs
[SerializeField] private Unit model;
[SerializeField] private UnitView view;

private void OnEnable()
{
    model.OnHpChanged += OnHpChanged;
    view.OnDeathFinished += HandleDeathFinished;
    OnDamage += view.HandleDamage;
}

private void OnDisable()
{
    model.OnHpChanged -= OnHpChanged;
    view.OnDeathFinished -= HandleDeathFinished;
    OnDamage -= view.HandleDamage;
}
```
        
```csharp
// Unit.cs (Model)
public float TakeDamage(int damage)
{
    if (damage - Stat.Def < 0) return 0;
    damage -= Stat.Def;
    Status.TakeDamage(damage);
    return damage;
}

public event Action<int, int> OnHpChanged
{
    add => Status.OnHpChanged += value;
    remove => Status.OnHpChanged -= value;
}
```
        
```csharp
 // UnitView.cs (View)
 public void RefreshHp(int maxHp, int hp)
 {
     hpBar.SetHp(maxHp, hp);
 }
 
 public void HandleDamage(float damage)
 {
     damageTextSpawner.Spawn(transform.position, damage);
```
</details>
  
- **자동 전투 AI**
    - `AutoCombat`가 주기적으로 스킬 사용 가능 여부를 판단하고, 
    공격이 불가능한 상황에서는 다음 위치로 이동을 시도하여 전투 흐름이 끊기지 않도록 구성.
    - 규칙 기반 의사결정(쿨타임 기반 우선순위).
    - 공격 실패 시 이동으로 fallback 처리.
    - 전투 로직을 `UnitController`와 분리해 유지보수성 확보.

<details>
<summary>코드</summary>
   
```csharp
private void Update()
{
    if (!isEnabled) return;

    if (Time.time - lastTime >= delay)
    {
        int nextSkill = DecideNextSkill();
        bool attacked = false;

        if (nextSkill != -1)
            attacked = controller.DoAttack(nextSkill, transform.position, Time.time);

        if (!attacked && controller.TryGetNextPos(nextSkill, out Vector2 nextPos))
            controller.SetNextPos(nextPos);

        lastTime = Time.time;
    }
}

private int DecideNextSkill()
{
    if (controller.CanUseSkill((int)SkillSlot.Skill3, Time.time)) return (int)SkillSlot.Skill3;
    if (controller.CanUseSkill((int)SkillSlot.Skill2, Time.time)) return (int)SkillSlot.Skill2;
    if (controller.CanUseSkill((int)SkillSlot.Skill1, Time.time)) return (int)SkillSlot.Skill1;
    if (controller.CanUseSkill((int)SkillSlot.Normal, Time.time)) return (int)SkillSlot.Normal;
    return -1;
}
```
</details>
  
- **스킬 시스템**
    - 스킬 데이터(`SkillData`)와 타겟 스캐너(`SkillTargetScanner`)를 분리해, 
    단일/원형/원뿔/직선 타겟팅을 조합형으로 확장할 수 있게 설계
    - 타겟 필터 체인(팀/상태/선택 규칙)으로 재사용성 강화.
    - 즉발/투사체 전달 방식 분리로 연출과 판정의 유연성 확보.
<details>
<summary>코드</summary>

```csharp
public bool CanCast(UnitController caster)
{
    scanResult = scanner.Scan(caster, Data.TargetData);
    return scanResult.Targets.Count > 0;
}

public void Use(UnitController caster)
{
    if (Data.EffectClip != null) PlaySkillEffect(caster);
    else Data.Use(caster, scanResult.Targets);

    RefreshLastUseTime();
}
```

```csharp
public override SkillScanResult Scan(UnitController caster, SkillTargetData targetData)
{
    ConeTargetData coneTargetData = (ConeTargetData)targetData;
    Vector2 casterPos = caster.transform.position;
    Vector2 forward = caster.Forward;

    List<UnitController> targets = new(UnitManager.Instance.Units);
    targets.RemoveAll(u => !coneTargetData.IsInRange(casterPos, u.transform.position, forward));

    SelectActiveUnit(targets);
    targets = ApplyTeamFilter(coneTargetData, caster, targets);
    targets = ApplyConditionFilter(coneTargetData, targets);
    targets = ApplySelect(coneTargetData, targets);

    return new SkillScanResult { Targets = targets, PrimaryTarget = caster };
}
```
</details>
        
- **파티 편성**
    - `PartySetupPanel`에서 파티 슬롯/유닛 목록 슬롯을 런타임 생성하고, 
    드래그 가능한 슬롯 추상화(`DragSlotUI`)를 통해 편성 UI를 구성
    - 이벤트(`OnPartyChanged`) 기반 UI 동기화.
<details>
<summary>코드</summary>
 
```csharp
public void Init()
{
    partySlotUis = new();
    for (int i = 0; i < UnitManager.MaxPartySize; i++)
        CreatePartySlot();

    RefreshPartyPanel();

    Dictionary<UnitName, UnitData> unitDatas = UnitManager.Instance.UnitDataDic;
    unitListSlotUis = new();

    foreach (var unitData in unitDatas)
    {
        if (unitData.Value.TeamType == TeamType.Player)
            CreateUnitListItem(unitData.Value);
    }

    UnitManager.Instance.OnPartyChanged += RefreshPartyPanel;
    UnitManager.Instance.OnPartyChanged += RefreshUnitListPanel;
}
```

```csharp
public void OnBeginDrag(PointerEventData eventData)
{
    if (!CanDrag()) return;

    dragItemUI = UIManager.Instance.GetDragItem();
    dragItemUI.SetActive(true);
    dragItemUI.SetSize(GetDragImage().rectTransform.sizeDelta);
    dragItemUI.SetSprite(GetDragImage().sprite);
    SetDragContext();

    dragItemUI.transform.position = eventData.position;
}
```
</details>

---

## 🧩 Assets & Credits
- **Tools**
  - Cinemachine
  - TextMeshPro
  - DOTween

- **Graphics**
  - Character: https://zerie.itch.io/tiny-rpg-character-asset-pack
  - UI: https://etahoshi.itch.io/minimal-fantasy-gui-by-eta
  - Map: https://kevins-moms-house.itch.io/islands

- **Audio**
  - SFX: https://leohpaz.itch.io/rpg-essentials-sfx-free
  - BGM: https://tallbeard.itch.io/music-loop-bundle
    
