## 📄 License
This project is public for portfolio/demo purposes only. **No license is granted** to use, modify, or distribute the code.

# 📦 CPR Simulator VR

A full interactive CPR simulator for Meta Quest, developed in Unity using:
- **Hurricane VR (HVR)** for VR interaction
- **State Machine Architecture** for game flow control
- **ScriptableObjects** for modular, in-game interaction systems

## 🧪 Features
- Realistic CPR training experience
- VR interactivity with HVR
- Modular state-driven gameplay
- ScriptableObject-based system for flexible gameplay logic

- ## 🧠 Architecture

### 🗂 Scene Management
- `SceneStateMachine.cs`: Manages the overall scene transitions and game flow.

### 🧱 State Machine
- `.../CPR/CPRBaseState.cs`: Abstract base class for all CPR states. All individual states inherit from this class.
- `.../Runtime/Helpers/`: Contains all the specific CPR states used in the simulation .
