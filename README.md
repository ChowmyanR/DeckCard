README
Multiplayer Card Game Prototype

This repository contains the source code for a turn-based multiplayer card game built with Unity and Unity Netcode for GameObjects (NGO). The project features an automated hosting/joining system and a core loop centered around elemental power management.

---
Core Features

Automated Networking: Implements an `AutoConnect` logic that attempts to join a session as a client and automatically promotes the user to host if no server is found.
Turn-Based Logic: A server-authoritative `GameManager` handles turn cycles, round resolutions, and score tracking.
Card System: Uses `ScriptableObjects` for modular card creation, including stats for Attack, Defense, and custom Sprite imagery.
Dynamic UI: A flexible `CardUI` system that handles player selection visuals and real-time stat updates.
Custom Messaging: Uses a JSON-based messaging system to communicate player actions (like ending turns) across the network.

---

Project Structure

| File | Description |
| --- | --- |
| AutoConnect.cs | Handles the initial connection logic and host/client transitions. |
| GameManager.cs | Manages the match lifecycle, turn order, and combat resolution. |
| CardData.cs | The data template for elemental cards (Attack, Defense, Balanced). |
| PlayerState.cs | Tracks individual player data like Client ID, Score, and active cards. |
| Deck.cs | Manages card pooling, shuffling, and drawing mechanics. |
| NetworkGameManager.cs | The communication hub for sending and receiving custom network messages. |

---

How to Play

1. Build and Run: Create a build of the project or run it in the Unity Editor.
2. Connection: The first instance will automatically become the Host; subsequent instances will join as Clients.
3. Gameplay:
 The game begins once two players are connected.
 Players draw 3 cards initially and one card at the start of each new round.
 Select cards in your hand to play them to the field.
 Click Play to commit your cards and End Turn to pass to your opponent.


4. Winning: The game lasts for 6 rounds. The player with the most round wins based on ATK vs DEF calculations is declared the winner. 

---

Development Details

Engine: Unity 6
Networking: Unity Netcode for GameObjects
UI System: TextMeshPro & Unity UI

