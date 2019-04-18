# Corallium
* #### 장르 : 힐링게임
* #### 역할 : 프로그래머(인게임 구현)
* #### 개발 언어,툴 : C#, Unity3d

## 프로젝트 소개
### 게임 소개
* 농사를 통해 사용가능한 산소의 양을 늘리거나 이동수단 등을 개발하여 활동 범위를 늘리고 여러 지역을 탐험하는 힐링 게임입니다.
+ **조작법**
   + 좌측 조이스틱 : 플레이어 이동
* **개발시 난관**
   * VR과 NonVR을 오가는 것은 기본으로 제공되는 sdk로 구현하기는 어려웠습니다.
      * 핸드폰의 자이로 센서값을 받아와서 직접 스크립트로 화면전환을 하였으며
      * 화면을 담당하는 카메라를 2개로 나누어서 해결하였습니다.
   * 수족관 속 물고기의 움직임은 그저 랜덤 좌표로는 구현하기 어려웠습니다.
      * Flocking AI 알고리즘을 이용하여 물고기가 때로 다닐 수 있도록 인공지능을 구현하였습니다.
   * 해초류의 모델링을 담당하던 모델러가 연락두절이되어 해초류의 모델이 없었습니다.
      * 마인크래프트의 잔디처럼 sprite2개를 X자로 엇갈려놓음으로써 어느정도 자연스러운 해초를 구현하였습니다.
### 게임 화면
* **게임의 메인화면입니다. 인게임에서의 닉네임을 입력할 수 있습니다.**
<img src="https://user-images.githubusercontent.com/40797534/56393843-79277900-6270-11e9-9cd2-be6b325d43e7.png" width="70%"></img>
<img src="https://user-images.githubusercontent.com/40797534/56393844-79c00f80-6270-11e9-9222-6a939253ffe8.png" width="70%"></img>
<img src="ttps://user-images.githubusercontent.com/40797534/56393842-79277900-6270-11e9-8f96-031d7cde08c4.png" width="70%"></img>

### 기술적 특징
* **Flocking AI**
```c#
foreach(GameObject go in gos)
{
   if(go != gameObject) //자신 제외
   {
      dist = Vector3.Distance(go.transform.position, transform.position);
      if(dist <= neighborDist)
      {
         vcentre += go.transform.position;
         groupSize++;

         if(dist < 2.0f)
            vavoid = vavoid + (transform.position - go.transform.position);

         Flock anotherFlock = go.GetComponent<Flock>();
         gSpeed = gSpeed + anotherFlock.speed;
         }
    }
}
if(groupSize > 0)
{
   //중심 좌표
   vcentre = vcentre / groupSize + (goalPos - transform.position);
   //평균 속도 계산
   speed = gSpeed / groupSize;
   if (!isJelly)
      animator.speed = speed;

      Vector3 dir = (vcentre + vavoid) - transform.position;

      if (dir != Vector3.zero)
         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
}
```
