## Binary-Space-Partitioning   
   
<hr>   
<h3>Ex</h3>   
<image src="./image/bsp_result.PNG"/>   
<hr>
<h3> UML</h3>   
<image src="./image/BSP_UML.png"/>
<hr>   
---  

### 개발과정
- 로그라이크 맵을 제작하는 과정에서 매 스테이지 랜덤한 방의 크기 및 위치를 플레이어 제공하기 위해서 개발하게 되었습니다.
- 플레이어가 전투 씬에 입장하면 동적으로 랜덤하게 맵을 생성하도록 설계하였습니다.
   - BSP(Binary Space Partitioning) 알고리즘을 사용하였습니다


### 간단한 클래스 설명

${\textsf{\color\{red}Ctrl + 클릭을 통해 새창열기로 쉽게 코드를 확인하실 수 있습니다.}}$


- [**MapCreator.cs**](https://github.com/shji0318/Binary-Space-Partitioning/blob/main/Assets/BSP/Script/MapCreator.cs)
  - BSP 알고리즘들을 종합하여 실행하기 위한 최상위 클래스입니다. **노드 생성, 다리 생성, 타일 생성 등, 순서를 제어하기 위해 설계**했습니다. 
  - Room 분할을 진행하며 완전 이진 트리를 생성 후, Room 정보 저장 및 메쉬 생성 (InitRoom() 부분)
  - 생성된 Room 정보를 토대로 형제 노드를 이어주는 Bridge 정보 저장 및 메쉬 생성 (InitBridge() 부분)
  - Room 위치 정보와 Bridge 위치 정보를 기준으로 바닥에 타일을 생성하며, 방을 연결하는 부분을 제외하고 외곽 부분에 벽 생성 (InitWallTile() 부분)


 - [**TreeNode.cs**](https://github.com/shji0318/Binary-Space-Partitioning/blob/main/Assets/BSP/Script/TreeNode.cs)
    - 이진 트리의 단위로 사용하기 위해 설계한 클래스
    - 노드 클래스는 **자신의 방 정보와 방의 정보를 통해 방을 나누거나, 다리를 연결하는 함수들을 내포**하고 있습니다.
    - 랜덤하게 방을 나눌 비율과 좌우 혹은 상하로 나눌지를 정하며, 정해진 조건에 따라 방을 2분할 하는 함수 (DividingNode() 부분)
    - 리프 노드의 방을 연결하는 함수 (MakeLeafNodeBridge() 부분)
    - 상위 부모 노드들을 연결하는 함수 (MakeParentNodeBridge() 부분) 


- [**Square.cs**](https://github.com/shji0318/Binary-Space-Partitioning/blob/main/Assets/BSP/Script/Square.cs)
    - 방의 좌표 및 길이 등, **동일한 데이터 구조를 갖는 Room과 Bridge에 상속하여 사용**하기 위한 클래스 


- [**Room.cs**](https://github.com/shji0318/Binary-Space-Partitioning/blob/main/Assets/BSP/Script/Room.cs)
    - 방의 대한 정보를 담고 있는 클래스 (Square 상속)
    - 2분할 한 방을 자연스럽게 배치하기 위해서 범위를 축소시키는 함수 (ReduceRoom() 부분)
 
  
- [**Bridge.cs**](https://github.com/shji0318/Binary-Space-Partitioning/blob/main/Assets/BSP/Script/Bridge.cs)
    - 생성자를 통해 두 개의 방 정보를 토대로 연결 시키기 적합한 위치에 좌표를 계산하여 담고 있는 클래스 (Square 상속)
 

- [**OutLine.cs**](https://github.com/shji0318/Binary-Space-Partitioning/blob/main/Assets/BSP/Script/OutLine.cs)
    - 외곽 부분 좌표 정보를 담고 있는 클래스입니다
    - 방과 방을 연결시키는 부분에 방을 이동하기 위한 입구를 만들기 위한 함수 (DeleteBridgeWall() 부분)    
