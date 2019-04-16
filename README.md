# Corallium
* #### 장르 : 힐링게임
* #### 역할 : 프로그래머(인게임 구현)
* #### 개발 언어,툴 : C#, Unity3d

## 프로젝트 소개
### 게임 소개
* 농사를 통해 사용가능한 산소의 양을 늘리거나 이동수단 등을 개발하여 활동 범위를 늘리고 여러 지역을 탐함하는 힐링 게임입니다.
+ **조작법**
   + 좌측 조이스틱 : 플레이어 이동
### 게임 화면
* **게임의 메인화면입니다. 인게임에서의 닉네임을 입력할 수 있습니다.**
<img width="70%" src=https://user-images.githubusercontent.com/40797534/56102123-ac62c300-5f65-11e9-8b03-c39e0627c82c.png></img>
### 기술적 특징
* **Flocking AI**
'''
foreach(GameObject go in gos)
{
   if(go != gameObject)
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
   vcentre = vcentre / groupSize + (goalPos - transform.position);
   speed = gSpeed / groupSize;
   if (!isJelly)
      animator.speed = speed;

      Vector3 dir = (vcentre + vavoid) - transform.position;

      if (dir != Vector3.zero)
         transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
}
'''
* **카드보드 VR/NonVR**
