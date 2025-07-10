# SutakFanGame (In Progress 진행중)

![screenshot](https://github.com/user-attachments/assets/9674deee-d645-4812-a7c9-b79ee3365bac)

## About

이 프로젝트는 인벤토리, 장비 착용, 턴제 전투, 대화 시스템, 능력치 분배, 저장 기능 등을 포괄하는 Unity 기반 RPG 게임 시스템 템플릿입니다.
플레이어는 마우스 클릭을 통해 캐릭터를 조작하고, 아이템을 획득·소비·조합하거나, 적과 전투를 벌이고, NPC와의 대화를 통해 퀘스트를 진행하게 됩니다.

이 시스템은 개인 개발자 또는 소규모 팀이 기초부터 RPG 게임을 구현하고 확장할 수 있도록 구조화되어 있으며, 실제 OS처럼 작동하는 UI 창 구조와 직관적인 인벤토리 조작 방식으로 게임 내 상호작용성을 높입니다.

24년도에 시작 후 중단되었던 프로젝트로, 현재 취업 및 자격증 취득을 기점으로 리팩토링이 쉬운 구조화된 코드로 개선하기 위해서 다시 재개되었습니다.

<br>

This project is a Unity-based RPG game system template that includes a comprehensive range of systems such as inventory management, equipment handling, turn-based combat, dialogue interactions, stat allocation, and save/load features.

Players control a character through mouse input to collect, consume, and craft items, engage in battles with enemies, and progress through quests by interacting with NPCs.

Designed for solo developers or small teams, the system is modular and structured to serve as a foundation for building and expanding full-featured RPG games. It also features desktop-like UI windows and intuitive inventory handling for enhanced in-game interaction.

Originally started in 2024 and paused, the project was resumed after completing job-related milestones and certifications, with a focus on refactoring and improving code structure for scalability and maintainability.

<br>

## Information
- **Engine**: Unity(C#)
- **Platform**: WebGL
- **Tooling**: NavMesh, UI Toolkit
- **Team size**: 1
- **Time Frame**: In Progress

<br>

## Features

시스템 항목 | 설명
:--- | :---
📦 인벤토리 시스템 <br><br> Inventory System | 드래그 앤 드롭, 우클릭 메뉴, 홉 설정, 쌓기/분리 기능 지원 <br><br> Drag-and-drop items, stack/split items, hover tooltips, right-click actions
🧥 장비 착용 시스템 <br><br> Equipment System | 망토, 부츠, 검, 활 등 부위별 슬롯 제공 및 장착 해제 기능 <br><br> 	Equip/unequip capes, boots, swords, bows with dedicated slots
⚗️ 아이템 조합 시스템 <br><br> Crafting System | 여러 재료 아이템을 조합해 상위 아이템 제작 가능 <br><br> Combine multiple ingredient items to create a new item
💬 대화 시스템 <br><br> Dialogue System | 클릭 시 대화 시작, 시나리오에 따라 단계적으로 진행 <br><br> 	Trigger dialogues by clicking NPCs; supports scenario-based progress
🗡️ 턴제 전투 시스템 <br><br> Turn-based Combat System | 주도권 기반 순서, 이동, 아웃라인, 카드 UI 등 포함 <br><br> 	Initiative-based turn order, unit cards, outline indicators, move/attack phases
🖱 사용자 상호작용 <br><br> Mouse Interaction | 마우스 호버/클릭/우클릭에 따른 UI 반응 구현 <br><br> 	Hover, click, right-click detection with interactive feedback
🧠 능력치 분배 시스템 <br><br> Stat Allocation System | 주도권, 힘, 회피 등 능력치를 포인트로 직접 조정 가능 <br><br> Increase/decrease stats using available points; UI synced
💾 저장 및 불러오기 <br><br> Save/Load Support | 플레이어 상태, 인벤토리, 장비, 지도 상태 등 JSON 기반 저장 <br><br> Save/load player stats, inventory, equipment, map status via JSON
🧭 카메라 전환 <br><br> Camera Switching | 키 입력으로 가상 카메라 전환 (Cinemachine 활용) <br><br> 	Switch between multiple virtual cameras using keyboard input
🧍‍♂️ 유닛 인터페이스 통합 <br><br> Unified Unit Interface | IUnitData를 통해 플레이어/적 공통 구조화 <br><br>	Shared interface (```IUnitData```) for player and enemy logic
🖼️ 유닛 카드 UI <br><br> Unit Card UI | 전투 순서를 카드 UI로 시각화하여 가독성 향상 <br><br> Visually display turn order through scalable portrait cards
🪟 UI 창 시스템 <br><br> UI Window System | 드래그, ESC로 닫기, 창 우선순위 등 실제 데스크탑 구조 구현 <br><br> Window dragging, stacking, ESC-close, and right-click menus for OS-like UX

<br>

## Key Points

- **상태 기반 구조  Centralized Game State(GameManager)** <br>
게임의 전체 상태를 GameManager가 전역으로 관리 (싱글톤 구조) <br> Managed globally via GameManager singleton, including time, email count, and health.

- **인벤토리 + 장비 + 조합의 통합적 연결  Unified Inventory, Equipment, Crafting Flow** <br>
각 슬롯에 개별 ID와 타입, 장비 여부를 할당하여 동작 통일 <br> Each slot handles IDs, types, and state consistently for seamless interaction

- **대화 시스템 단계별 분기 처리  Scenario-based Dialogue Flow** <br>
첫 대화, 퀘스트 수락, 보상 수령 등 시나리오별 분기 가능 <br> Branching conversation logic with flags for first-time talk, quest acceptance, and rewards.

- **턴제 전투의 가시화 구현  Combat Visual Feedback** <br>
카드 크기, 아웃라인, 카메라 전환 등으로 전투의 몰입도 향상 <br> LineRenderer-based path preview, character highlighting, and camera focus via double-click.

## Structure

```
📁 Scripts/
├── Core
│   ├── GameManager.cs                  # 전역 상태 관리 (싱글톤)
│                                         Global state controller (singleton)
│   ├── DataManager.cs                  # 저장/불러오기 처리
│                                         Save/load handler
│   ├── UIManager.cs                    # UI 열기/닫기 및 ESC 처리
│                                         UI stack and input handling
│
├── InventorySystem
│   ├── InventoryManager.cs             # 인벤토리, 장비, 조합 슬롯 및 UI 관리
│   │                                     Inventory, equipment, and crafting slot logic
│   ├── InventoryItem.cs                # 아이템 데이터 (ScriptableObject)
│   │                                     Item data (ScriptableObject)
│   ├── InventorySlot.cs                # 슬롯 동작 처리
│   │                                     Slot behavior and hover/click logic
│   ├── EquipSlot.cs                    # 장비 전용 슬롯
│   │                                     Equipment slot UI and state
│   ├── ItemDragDrop.cs                 # 드래그 & 드롭 처리
│   │                                     Drag and drop functionality
│   ├── RightClickWindow.cs             # 우클릭 메뉴 UI
│   │                                     Right-click UI menu
│
├── FileSystem
│   ├── InventoryData.cs                # 인벤토리 저장 데이터
│   │                                     Inventory save data
│   ├── EquipData.cs                    # 장비 저장 데이터
│   │                                     Equipment save data
│   ├── ConvoData.cs                    # 대사 스크립트 구조 (JSON)
│   │                                     Player and map data
│   ├── PlayerData.cs / MapData.cs      # 플레이어, 지도 저장 구조
│                                         Dialogue script structure (JSON)
│
├── BattleSystem
│   ├── BattleManager.cs               # 턴 관리, 유닛 리스트, UI 표시
│   │                                    Turn control, unit management, action UI
│   ├── Player.cs / Enemy.cs           # 전투 유닛 구현 (IUnitData 인터페이스)
│   │                                    Battle characters (IUnitData) 
│   ├── PortraitCard.cs                # 카드 UI 및 더블 클릭 처리
│   │                                    Turn-order card UI 
│   ├── PlaceCards.cs                  # 카드 배치 및 정렬 처리
│   │                                    Card generation and sorting 
│   ├── Enemies.cs                     # 적 데이터 (ScriptableObject)
│   │                                    Enemy data (ScriptableObject) 
│   ├── BattleRange.cs                 # 전투 범위 감지 트리거
│                                        Trigger zone to enter combat 
│
├── DialogueSystem
│   ├── ConvoManager.cs                # 대화 시작, 대사 전환, 시나리오 분기
│   │                                    Scenario-based dialogue controller
│   ├── Amtak.cs                       # NPC 클릭 시 대화 시작 처리
│                                        Triggers conversation on click
│
├── StatSystem
│   ├── StatManager.cs                 # 능력치 증가/감소 및 UI 연동
│                                        Stat UI and logic (initiative, strength, etc.) 
│
├── UI
│   ├── DragWindow.cs                  # 창 드래그 및 클릭 시 투명도 조절
│   │                                    UI window drag and layering   
│   ├── SwithCamera.cs                 # 카메라 전환 기능
│                                        Virtual camera switching via key input   
```
