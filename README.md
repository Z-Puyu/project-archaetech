# Project: Archaetech

## What Is This?

Project: Archaetech is a hexagonal-tile-based 4X strategy game with a strong focus in colony simulation and economy. Some of its game elements are inspired by other strategy games such as Stellaris, Civilisation VII, Victoria 3 etc.

This project is public and currently developed by a group of students from the National University of Singapore who are interested in game development.

The detailed project information (including a break-down of its features, background story and development logs) has been transferred to [this Gitbook page](https://puyu.gitbook.io/project-archaetech/).

## You Probably Want to Read These before Blowing Things up :O

- [Recommended coding practices](https://github.com/Z-Puyu/project-archaetech/discussions/25)
  
- [How to manage events and signals](https://github.com/Z-Puyu/project-archaetech/issues/16)

## Workflow

- The `main` branch is **protected**. Only code which has been comprehensively tested and debugged will be pushed there via a *pull request*.

- The `production-and-test` branch is used to merge all newly implemented feature branches *after each branch has been tested individually*. Its purpose is to test and debug a latest version of the game. All other branches should be merged to this branch once a new feature has been implemented.

- All other branches serve to classify and modularise the development of different features. Preferrably, use **kebab-case** to name all branches.

- We use [**Issues**](https://github.com/Z-Puyu/project-archaetech/issues) to document important **todo** items and potential enhancement to the game's features and mechanics. Please use the following template when submitting new Issues:
  ```markdown
  ## Description
  
  // Put an overview of your Issue here.
  
  ## Features and Mechanics to Include
  
  // If the Issue is about new features and/or modifications on current implementation, please specify the changes suggested.
  
  ## Bug Description

  // If the Issue is about a bug, please describe how the code is expected to behave and what actually happened.

  ## Steps to Reproduce the Bug

  // If the Issue is about a bug, please list down the steps to reproduce the bug.

  ## Others

  // Any other information which might be relevant goes here.
  ```

- [**Discussions**](https://github.com/Z-Puyu/project-archaetech/discussions) are used to make important announcement, discuss general/minor issues and other random topics.

- **Github Projects** is used to track the work progress of this project's development.
  
## Class Diagram

[PlantUML](//www.plantuml.com/plantuml/png/hLTBR-Cs4BxhLn0vsSNwiaSXCBRT00iKkqNGz74e5W8jJcokAP8bXycwHVvxIOgYQ4a6TAWFWJsFRsQ-UKZvO94QT9sex9bBJGtjYE0IyAA1Q2KkuSUIJXaygKhJzaoFGEwUa29loIUX3bIGfzPmZVQE_5iH_sa5jPPyeXryeTARco1FdwENDgrGxRacl_4EJDPO3S0Q0IyWQi8bPaNhZ6aOHQZ9ZyRGorzU-JLh9RmsdP9uscnhyiIFaeeh7QEN1Oq5R324RsBOpHXsMtX_nm4w2j-AwdvoN30kZ_CA3qdJ1DviJK2wRgRqyWVU8bUIcilz7z2YCk_vkmoxpnxqXN913mAQPLykyHrvz7q1zjI2XFqrRrRJLncoKOTlDVb5CNZ_a2qyDzOp_epNXF_jm7coFtFfGJzpY_UztmWTKND2c-7B463EDsLiDDVAbp3M77YQKCz00C8DZqc7GeFd_NbKUjuQhNJJAYSdzHwDUiLJO-A8FGj5PofjrD60jLEraz1feIx0Cc8E55mN4liLh7AcNLWDapJbuCoKryqVm1ueqdTzRlkIymrzVeOMWJNh_KTwhTDQN3RVrA4cFwj3R-IJQbwLEOEnawQTg6r0AdSyROPeDRcES03PdZfgped8BTdjoK4fCQFfTybnJf7pqZazIN5ZfzBH281VbzOgkFv_AnML4NCJ_WpwvTiKB9PdEw_072bHzLRrHFsfZ629eexgS5M57ouT1LmsyU0wNfEFIg9H8Zjwc-HsD9pTuEuZ215Qjw_6ya74aHgJ7Y2pTS9Dp1RyHt6-5ldQHJ62j-DoNAJb8VB1wC6YeIrEJd6AohpH21rTWhXVr0uaNfMQvvhBj3fXk3VRqUTw0RQvs3922YqzrxxHQ8QQBFy72fvYIrebVPFWEpe3FQlAuksj71CBpwi6LNyLHvgqKSn5twO3F2cMAN_CcYzHqTDFlUG5cfbRJQRsGwapnQrovxKPQBc6supd44cw7fbBxN3TCLGErzZcN4hb-eJoOsm-eM7so-cCWZoPd6Nab47Mhuz-TKMg_3wLxkpgENkh-dlomsuN3qJjlIHIBcqLVWRnNKVbvIxlUc6IVRDHNit2ACkyNJQO8fNo4d0sYIjyoZ9B-ndMw9F7yLRUjysyQ6q2cK9lkyOe0uRnA6t2xw2FYYd0z4rrjzkNVOaFB4keP65PbM4zNBRkF_i8lMnUl6hT5oqbTR0mZgCbwOMX6efSjWyvMzT1X4zt3UpuwbEL_rBifdN4nwijxdoFafiSldeGEyCPOGPPJPnc5C76ZD0aWFXUN7q0oLmd_WC0)

## Development Timeline

#### Separation of Duties:

- Luo Zhiyang: UI design

- Ma Yirui: Gameplay programming

- Zhang Puyu: Game design, gameplay programming

#### 12.8 - 12.15

- [x] Map: smooth camera movement and zooming, tile selection.

- [x] Unit movement: unit selection

- [x] Pops and resources: build database, basic pop growth

#### 12.16 - 12.22

- [x] Map: terrain, resource & building placement

- [ ] Unit movement: path-finding, terrain effects, resource collection

- [x] Pops and resources: buildings, jobs, employment

- [x] UI: main menu, in-game pause menu, in-game data display (e.g. hovering windows, selection menus)

#### 12.23 - 12.29

- [ ] Map: random tilemap generation

- [ ] Unit: formation and dismissal, health point

- [x] Production: resource transportation system, storage system

- [x] Technology: tech-tree, research mechanism

- [ ] UI: textures, unit movement paths, highlighted texts

#### 12.30 - 1.5

- [ ] Event: event triggers, event effects

- [ ] Map: fog-of-war

- [x] Pops and resources: resource consumption

- [ ] UI: event windows
