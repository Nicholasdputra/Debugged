# 🪰 About
Debugged is a pixel tower defense game which takes place in the far future. In this future, everything is super futuristic and high energy, yet in the pursuit of making this future, they've killed off one big subclass of life, which are insects. So, they eventually chose to make a world which is more sustainable, converting their main source of energy to something more renewable, like solar energy, they've also chosen to put most of said panels on insect like robots, as a way to both maximize sun exposure by having the panels be mobile, and specifically in the form of an insect as a reminder to prioritize the ecosystem in future technological advancements. All was well until one day, the A.I. in said robots began to go rogue and turn against humanity. Now, you as the commander of humanity's last line of defense must manage what little energy you have left on hand to fend off the rogue mecha.

# 📜 Scripts

| Script Name | Script Description |
|---|---|
| 🎧 AudioManager.cs | Responsible for the game's sfx and bgm |
| 🛡️ BarrierScript.cs | Responsible for the barrier near the 'base / hub' that instakills the enemy when they reach it but only has three uses |
| 🏹 BasicProjectileScript.cs | A basic projectile that takes the form of a 'laser bullet'|
| 🚫 DeleteProjectileScript.cs | Responsible for deleting projectiles that go out of the map so they don't make the game run slower |
| 🎯 Enemy.cs | An abstract class that functions as a blueprint for what functions and attributes the enemies in general will have |
| 🎌&nbsp;EnemyCompositionDataSO.cs | A scriptable object that holds the list of enemy composition data for all levels |
| 🚩 EnemyCompositionSO.cs | A scriptable object that holds the list of waves that are in a level and the time between the waves|
| 👆 LevelSelectScript.cs | A script for level selection |
| 👎 LossCheck.cs | A script attached to the base / hub that detects if the enemy has breached the defenses to trigger a loss |
| 🏠 MainMenu.cs | A script used for handling the main menu of a game |
| 🗻 PassiveSounds.cs | A script used for handling passive sounds that would sound weird or too loud if they stacked |
| 🫳 PlaceableItemScript.cs | A script used for placing down a specific tower |
| ⭕ PlaceableItemSlotScript.cs | A script used for marking placeable tiles |
| 🐜 T1BasicEnemy.cs | This script is assigns a basic bug enemy its attributes and handles its movement |
| 🐞 T2BasicEnemy.cs | This script is assigns a tier two (tankier) enemy its attributes and handles its movement |
| 📡 TeslaFieldScript.cs | A script used for making the tesla tower (AOE Tower that damages the enemies around it) function |
| 🔫 TurretScript.cs | A script used for making the base turret tower function |
| 🏰 Tower.cs | An abstract class used as a blueprint for towers |
| 🌊 WaveSO.cs | A scriptable object that holds the enemy details of each individual wave |
