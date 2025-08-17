# Runner-3D 🎮 | Unity Endless Runner Game Template

**Runner-3D** is a complete **3D endless runner game template built with Unity**.  
It’s designed for developers who want to **build hyper-casual games** quickly—featuring swipe controls, dynamic terrain generation, and mobile-ready performance.  

This project can be used as a **starter kit** for Android, iOS, or PC endless runner games.

---

## 🔥 Key Features

- 🏃 **Smooth 3D runner mechanics** (swipe & keyboard supported)  
- 🌍 **Endless terrain generation** with obstacles & tiles  
- 📱 **Mobile-friendly controls** (swipe gestures for Android/iOS)  
- 🎨 **Ready-to-use prefabs** for player, obstacles, UI, and ads  
- 📊 **Score tracking system** with Game Over screen  
- 💰 **AdsManager integration** for monetization (interstitial ads support)  
- ⚡ **Lightweight & modular** – easy to extend for any hyper-casual project  

---

## 🚀 Getting Started

### Requirements
- **Unity Version**: 2021.3 LTS or newer  
- **Platforms**: Android, iOS, PC  
- **Dependencies**: None  

### Installation
1. Clone this repository:
   ```bash
   git clone https://github.com/0xnazmul/Runner-3D.git
# Runner-3D 🎮 | Unity Endless Runner Game Template

**Runner-3D** is a complete **3D endless runner game template built with Unity**.  
It’s designed for developers who want to **build hyper-casual games** quickly—featuring swipe controls, dynamic terrain generation, and mobile-ready performance.  

---

## 🚀 Getting Started

1. Open the project in **Unity Hub**.  
2. Let Unity import all assets.  
3. Open the **SampleScene** under `Assets/Scenes`.  
4. Press ▶ **Play** in Unity Editor or build for mobile.  

---

## ⚡ Quick Usage

### Player Controls
- Swipe Left / Right on **mobile**  
- Use **Arrow Keys** on **PC (Unity Editor)**  

### Score System
- Distance traveled = higher score  
- Score resets after game over  

### Ads Integration
Configure your Ad IDs and show interstitial ads:  
```csharp
AdsManager.ShowInterstitial();

```
 
📂 Project Structure
```Runner-3D/
├── Assets/
│   ├── Scenes/
│   │   └── SampleScene.unity
│   ├── Scripts/
│   │   ├── PlayerController.cs
│   │   ├── ObstacleSpawner.cs
│   │   ├── GameManager.cs
│   │   └── UIManager.cs
│   ├── Prefabs/
│   │   ├── Player.prefab
│   │   ├── Obstacle.prefab
│   │   ├── UICanvas.prefab
│   │   └── AdsManager.prefab
│   └── Materials, Textures, Audio
├── Packages/
├── ProjectSettings/
└── README.md

```

🛠️ How It Works

PlayerController.cs → Handles player movement, swipe detection & collisions

ObstacleSpawner.cs → Spawns obstacles dynamically for infinite gameplay

GameManager.cs → Controls game state, scoring, and restart logic

UIManager.cs → Updates UI elements like score, game over screen

AdsManager.cs → Manages ad integration (monetization ready)

📈 Why Use Runner-3D?

⚡ Save development time with plug-and-play mechanics

📱 Build mobile-ready endless runner games with ease

🏆 Perfect for hyper-casual game prototypes or full releases

🔧 Highly customizable – add your own models, levels, or power-ups

🤝 Contributing

Want to improve Runner-3D?

Fork the repo

Add new features (e.g., power-ups, coins, skins)

Submit a Pull Request

Bug reports and feature requests are welcome in the Issues section.

📜 License

This project is released under the MIT License – free for personal and commercial use.

👨‍💻 Author

Developed by @0xnazmul
⭐ If you like this project, don’t forget to star the repo and share it!
