# Moby-Dick NPC Interaction Examples in Unity

This branch contains multiple examples of how to interact with NPCs using different Unity components, such as **keyboard inputs, UI dialogs, triggers, and animations**. The goal is to create a well-structured system that demonstrates various ways to make NPC interactions engaging and dynamic.

---

## 📂 **Scenes**
Each scene has its own dedicated folder to keep scene-specific assets together. This makes it easier to manage large projects with multiple levels or gameplay sections.

### 🔹 **Example Scene Folder: `Scene_NPC_Interaction`**
- **Scene_NPC_Interaction.unity** → The Unity scene file.
- **Materials/** → Contains materials specific to this scene.
- **Models/** → 3D models used exclusively in this scene.
- **Prefabs/** → Prefabs specific to this scene, such as NPCs or interactive objects.
- **Scripts/** → Scene-specific scripts that control behaviors and interactions.
- **Audio/** → Sound effects and background music related to this scene.
- **UI/** → UI elements such as dialog boxes or HUDs used in this scene.
- **Textures/** → Textures used for models and environments in this scene.

---

## 📂 **SharedAssets**
This folder contains assets that are **reused across multiple scenes**, reducing duplication and keeping the project clean.

- **Materials/** → Common materials shared across multiple scenes.
- **Models/** → Reusable 3D models (e.g., the player character, global NPCs).
- **Prefabs/** → Prefabs that can be used in multiple scenes (e.g., UI elements, global objects).
- **Scripts/** → Global scripts (e.g., game manager, common utilities).
- **Audio/** → Sounds and music used in more than one scene.
- **UI/** → Reusable UI elements such as buttons, menus, or HUD elements.

📌 **Why Have a `SharedAssets` Folder?**
- Prevents asset duplication across scenes.
- Keeps project size optimized.
- Makes asset management easier for team collaboration.

---

## 📂 **Resources**
The `Resources` folder is a special Unity folder that allows assets to be **loaded dynamically at runtime** using `Resources.Load()`. 

📌 **Use Cases for `Resources/`**
- Loading assets dynamically (e.g., character skins, level data).
- Storing configuration files or scriptable objects.
- Keeping assets that cannot be assigned in the Unity Editor beforehand.

⚠ **Important:**  
Avoid overusing this folder, as it can **increase build size** by forcing Unity to include all assets, even if they are unused.

---

## 📂 **TextMeshPro**
This folder contains TextMesh Pro assets, including fonts, settings, and custom text styles.

📌 **Why?**
- TextMesh Pro provides **better text rendering** than Unity’s default UI Text.
- Ensures consistency across all text elements in the game.

---

## 📂 **Plugins**
The `Plugins` folder is used for **third-party assets, Unity packages, and external tools**.

📌 **Why?**
- Keeps all external dependencies in one place.
- Ensures that Unity loads third-party scripts correctly.
- Useful for integrating tools like physics engines, database managers, or input libraries.

---

## **🎯 Best Practices for Scene Organization**
✔ **Keep each scene’s assets inside its respective folder** → This prevents clutter and makes it easy to work with multiple levels.  
✔ **Use `SharedAssets/` for reusable assets** → Reduces redundancy and optimizes memory usage.  
✔ **Limit `Resources/` usage** → Only store assets that need to be dynamically loaded.  
✔ **Use `Plugins/` for third-party tools** → Keeps dependencies organized and easy to manage.  
✔ **Keep UI elements in their own folder per scene** → Ensures UI assets don’t get mixed with game objects.  

---

## 🚀 **Next Steps**
- [ ] Add more scenes following this structure.
- [ ] Convert frequently used objects into **Prefabs** to ensure consistency.
- [ ] Optimize the `Resources/` folder to avoid unnecessary assets in the build.
- [ ] Document game-specific scripts (e.g., `DialogManager`, `NPCInteraction`).


---