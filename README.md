# unity-login-ui-manager

![Unity](https://img.shields.io/badge/Unity-2021.3+-000000?style=for-the-badge&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![License](https://img.shields.io/badge/License-All%20Rights%20Reserved-red?style=for-the-badge)

Unity login system with seamless UI navigation and async page/popup management

![image](https://github.com/user-attachments/assets/f5ac771a-8c5c-4f03-91b0-9b06ed08d111)

## 🛠 Tech Stack
- Unity 2021.3+
- C#
- UniTask (비동기 처리)
- Singleton Pattern

## ⭐ Key Features
- 비동기 페이지 전환
- 히스토리 기반 뒤로가기
- 팝업 오버레이 관리
- 메모리 효율적인 UI 관리

## 📖 Usage
```csharp
// 페이지 이동
await LoginUIManager.Instance.Next<MailPage>();

// 이전 페이지로
await LoginUIManager.Instance.Previous();
