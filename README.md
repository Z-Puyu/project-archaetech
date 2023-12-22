# Project: Archaetech

## IMPORTANT: Do NOT push to `main`. ALL updates must be done in a separate branch. Do a PULL REQUEST before merging to `main`.

## Motivation

We all love to play 4X strategy games. Most of the currently available 4X games, whether historical or fictional, tend to focus on advancing your civilisation **forwards** from a primitive era to a futuristic one. However, we would like to do some **reverse thinking** here: what if the nation you play as in a 4X game progresses not by **discovering the unknown of the future**, but by **digging out the lost legacies of a mysterious past**?

## Background Story

Intelligent life forms thrived on a planet. Unfortunately, however, the planet is harsh to its residents and annihilates civilisations at random intervals. As a result, the planet is full of archaelogical sites containing the relics and knowlegde of the civilisations in the ancient times.

The player will role-play as a nation whose population has just awaken in a sanctuary built in order to shelter a selected minority from destructive catastrophes. Oblivious to the situation, the people decide to self-organise and make their way to explore the unknown world.

## Features (Subject to change)

### Phase I: Basic Gameplay

#### Pops and Resources

Population in this game is measured in **Pops**. Like Paradox games, a **Pop** is an abstract unit of population with a particular set of traits, which may include:

- Occupation

- Gender

- Age Group

- Other Traits

**Pops** may convert into one another under certain conditions and they may interact with other mechanisms of the game. Pops will grow based on the *Logistic Curve* to simulate realistic population changes.

**Resources** are another basic metric present in every 4X game to model the player's nation quantitatively. In this game, we think of adding the following categories:

##### Raw Materials

- Food: Food constitutes the living necessities of the Pops and affects population growth rate (note that it is an important factor influencing the *environmental capacity*). Pops might suffer from lower productivity or even die off due to insufficient food.

- Minerals: Minerals are an abstraction for the variety of chemical substances used in manufacturing. *We might further categorise these into* **common** *and* **rare** *minerals*.

##### Secondary Resoruces

- Energy: Energy is essential in keeping the various facilities and buildings functioning. In the early stage of the game, the player may not have many options but to use minerals for power generation. However, in later stage of the game when more technologies are unlocked, there can be more interesting (or obscure) ways to generate power.

- Consumer Goods: Consumer goods are a general abstraction for most of commodities. These are tied to the **quality of life** of the Pops, and certain high-end occupations and buildings may need them to produce output.

- Industrial Goods: Industrial goods are a general abstraction for things like alloys, construction materials and organic chemical compounds. These can be directly consumed for construction or maintenance, or be used for industries at a higher end.

##### Tertiary Resources

- Research Points: Research points are meant to capture the academic output from scientists, researchers and literati. They affect the speed of technological advancement.

- High-end Industrial Goods: These are things like high-precision machinary, military supplies, computers and digital devices.

- Luxury Goods: Consumer goods but at a higher level. Rich Pops might buy.

These categories are just a rough classification, specific resources will be added into each of them.

Resources may be **produced** in **buildings** by **jobs**, but certain resources may also be present in the wild and players can assign Pops to collect them and transport back to the sanctuary.

#### Production Systems

The core mechanism of the game is **sanctuary production systems design**. The sanctuary is the only safe place for the Pops to reside in due to the aftermath of the apocalypse in the outer world. Therefore, all production is processed inside the various buildings and units in the sanctuary.

At the start, the player's sanctuary only has a storage unit and several living quarters. All goods and resources produced must be **directed to a storage unit via a transport link**, else they will be **wasted**. The sanctuary has a centralised assistant super computer which by default coordinates the transport of resources in the following manner:

- Any unit $U$ has one or more *input channels* and *output channels*. An input channel can receive resoruces transported from other units, and an output channel export the resources produced in $U$ elsewhere. A channel is said to be *activated* if at least one passage way is connected to it.

- Suppose $x$ units of resource $T$ is produced in $U$ and $y$ output channels are activated, then $\frac{x}{y}$ units of $T$ will be exported via each output channel.

- Suppose $x$ units of resource $T$ is being transported via a passage way $P$ and $P$ forks into $y$ other passage ways at some node, then $\frac{x}{y}$ units of $T$ will be distributed into each of the branches.

- Once some units of resource $T$ reaches another unit $V$ which is a dead end consuming it, all of them will be consumed even if it exceeds the maximum units needed. However, $V$ will have a limited storage space to temporarily store the unused resources. Anything beyond that limit will be immediately wasted.

- However, if in the above case, $V$ is connected to the side of a passage way (i.e. $V$ does not completely block off the flow of resources), it will only consume as much as it needs.

- Any resource imported into a unit $V$ which does not need it will be exported intact via the output channels of $V$.

- The player may choose to override the above logic for any unit, but at an expense of lowering the unit's productivity.

Therefore, we would like to encourage the players to design their transport networks such that minimal wastage is achieved.

#### Standard of Living (SOL)

We wish to quantify how successful the player is at running his sanctuary, so we introduce the design concept of **SOL**. This may be based on several indicators:

- Amount of food: Excess food storage will bring an increase in SOL while insufficient food supplies drastically decrease SOL.

- Consumer and luxury goods: Shortage in consumer goods decreases SOL, and surplus in luxury goods increases SOL **provided that consumer goods do not have a shortage**.

- Available space: **Living quarters** provide a certain amount of residential space. A surplus in residential space will slightly increase SOL but a crowded environment will significantly lower it.

#### Map Navigation & Exploration

Players can form **scout teams** with Pops to chart the map. Due to the adverse environment outside amid the aftermath of an apocalypse, you cannot anyhow move your scout teams anywhere like in Civilisation games. Now each scout team has a **health** value which will diminish at a variable speed based on terrain and climate conditions. To regenerate the health, the scount team needs to find a habitable hexagonal tile to rest in a camp.

There might be dangerous wildlife attacking the scount teams, so it might be a good idea to equip the Pops with some weapons when forming the team. The player will start the game with a limited number of weapons inside the initial storage.

*Explored regions do not stay un-fogged forever!* This is another unrealistic feature in other 4X games where a pre-industrial country can scan the whole world and somehow remember the locations for centuries. In our game, the sanctuary has something known as the **remembrace radius**. Explored regions outside of it will be **forgotten** and **re-fogged** after some time. To prevent this, the player needs to do at least one of the following:

- Expand the sanctuary so that the region falls within the remembrance radius again.

- Assign Pops to collect resources from that region regularly.

- Build an **outpost** and assign Pops to work there. An outpost has its own remembrance radius and everything inside it will be remembered.

- Climb the technological tree to discover more relevant technologies.

#### Research & Technologies

This is another core mechanism of the game. At game start, the main storage unit of the player's sanctuary contains a collection of technological documents. The player can assign Pops to learn from them and unlock some fundamental technologies. However, these only cover a very small part of the technological tree. For the rest, there are two main ways to unlock them:

1. Build a **research quarter** and assign Pops to work as **researchers** to generate *research points*. These research points by default get randomly distributed to all available technologies. Once a technology has accumulated enough research points it will be unlocked. The player can manually set a **focus area** to one technology, which will force at least 50% of all research points to be distributed to it. Nevertheless, this way of research point generation will be very slow at the early stage of the game.

2. As scount teams explore the world, they will encounter **precursor archeaological sites** at random chances. The player can assign Pops to work as **archeaologists** and dig out the secrets of the ancient. At each stage of archeaological work, the player can choose one technology from a set of potentially discoverable ones, and a large number of research points will be generated as the archeaological work progresses, which will then be all directed to that chosen technology. In the early stage of the game, this is the main channel to get research points.

The technologies will be divided into three categories with consideration for future extensions:

- **Natural Sciences**: These includes subjects like Mathematics, Physics, Chemistry and Biology. Natural Sciences technologies usually deliver buffs related to productivity, research point generation and serve as crucial pre-requisites to other technologies.

- **Engineering**: These includes subjects like Computer Sciences, Electrical Engineering, Material Science, Urban Planning, Architecture, etc. and usually unlock new building types.

- **Humanities**: These includes subjects like Art, Music, Literature, Liguistics, Geography, History, Politics, Philosophy, etc. and will provide soft improvements like higher SOL and faster archaeology (so history is indeed crucial to advance technologies in our game).

### Phase II: Advanced Gameplay Mechanisms

#### Production Methods

We plan to include two types of production methods for each productive building, namely **automated** and **manpowered**. 

In the automated mode, the building is run by a centralised AI computer and the maintenance of the building consumes more energy.  The player also cannot manually override the output level, i.e., the building always functions at 100% capacity (this means at a time of resource shortage these buildings will continuously cause deficits). To increase the output of automated building, the only way is through upgrading technologies.

In the manpowered mode, the building requires a certain number of Pops to be employed by the **jobs** it offers to fully function. Employed Pops require some amount of food and consumer goods to generate output, but the building itself requires less energy. The player can also adjust the output level between 50% to 125%. Pops have a probability of transforming into **experts**, which will bring an additional productivity bonus.

#### Education & Leaders

The player can build **educational institutions** to train regular Pops into experts in a certain industry at a higher conversion rate. Expert researchers can only appear via educational institutions.

If an industry has at least one expert Pop employed, then there will be a chance of a **leader** spawning from the industry. The player can assign the leader to the respective building to further boost its productivity provided that the building is manpowered. The more experts an industry has, the higher the chance of a leader spawning.

#### Ideologies & Policies

As the nation grows, **ideologies** may emerge among the Pops. Ideologies are sets of value systems a Pop believes in. This may influence the behaviours of the Pops. For example, an environmentalist Pop may demand less consumer goods and refuse to work at heavy industries.

The player will be prompted to choose a few among all ideologies present as the **mainstream** ideologies, which will give buffs and debuffs to the whole nation and increase the chances of these ideologies appearing among Pops.

**Policies** are closely tied to ideologies and are unlocked mainly via researching humanities technologies. Policies define the rules in the sanctuary and may produce interesting effects in interaction with ideologies.

#### Events

**Events** are an integral element to create immersion. We plan to script a large number of flavour events which will randomly get triggered during the game process. These events help establish the storyline of the gameplay in a 4X game like ours. 

The player will need to resolve each event by choosing one of the available options. Sometimes the options will bring about predictable outcomes, whereas other times the outcomes can be vague and unclear.

#### Species Customisation

The player may customise the species to play as before the game starts. This includes set the basic traits, custom race names and so on.

PS. Most likely we don't need a very fancy system for this.

### Phase III: Quality-of-life Extensions

#### Competitive AI Players

Being the only player can be boring sometimes and some competition may make the game more dynamic and interactive. Therefore, we plan to implement AI sanctuary dwellers. They may come from different pre-apocalypse civilisations and have totally different traits.

The human player should be able to do diplomacy with the AI players. Basic features of diplomacy will include:

- Trade resources

- Share technologies

- Exchange chartered maps

- Spy & sabotage

We are still considering whether a war mechanic is needed.

#### Late-Game Challenges

Experienced players can get over-powered pretty quickly in 4X games like ours. To prevent late-game from becoming too boring, late-game challenges might be useful.

#### Mini-quests & Achievements

Based on the current condition of the player's sanctuary, we can dynamically provide a list of mini-tasks for the player to accomplish, so that the player will have a stronger sense of purpose while playing the game.

## Class Diagram

[PlantUML](//www.plantuml.com/plantuml/png/hLTBR-Cs4BxhLn0vsSNwiaSXCBRT00iKkqNGz74e5W8jJcokAP8bXycwHVvxIOgYQ4a6TAWFWJsFRsQ-UKZvO94QT9sex9bBJGtjYE0IyAA1Q2KkuSUIJXaygKhJzaoFGEwUa29loIUX3bIGfzPmZVQE_5iH_sa5jPPyeXryeTARco1FdwENDgrGxRacl_4EJDPO3S0Q0IyWQi8bPaNhZ6aOHQZ9ZyRGorzU-JLh9RmsdP9uscnhyiIFaeeh7QEN1Oq5R324RsBOpHXsMtX_nm4w2j-AwdvoN30kZ_CA3qdJ1DviJK2wRgRqyWVU8bUIcilz7z2YCk_vkmoxpnxqXN913mAQPLykyHrvz7q1zjI2XFqrRrRJLncoKOTlDVb5CNZ_a2qyDzOp_epNXF_jm7coFtFfGJzpY_UztmWTKND2c-7B463EDsLiDDVAbp3M77YQKCz00C8DZqc7GeFd_NbKUjuQhNJJAYSdzHwDUiLJO-A8FGj5PofjrD60jLEraz1feIx0Cc8E55mN4liLh7AcNLWDapJbuCoKryqVm1ueqdTzRlkIymrzVeOMWJNh_KTwhTDQN3RVrA4cFwj3R-IJQbwLEOEnawQTg6r0AdSyROPeDRcES03PdZfgped8BTdjoK4fCQFfTybnJf7pqZazIN5ZfzBH281VbzOgkFv_AnML4NCJ_WpwvTiKB9PdEw_072bHzLRrHFsfZ629eexgS5M57ouT1LmsyU0wNfEFIg9H8Zjwc-HsD9pTuEuZ215Qjw_6ya74aHgJ7Y2pTS9Dp1RyHt6-5ldQHJ62j-DoNAJb8VB1wC6YeIrEJd6AohpH21rTWhXVr0uaNfMQvvhBj3fXk3VRqUTw0RQvs3922YqzrxxHQ8QQBFy72fvYIrebVPFWEpe3FQlAuksj71CBpwi6LNyLHvgqKSn5twO3F2cMAN_CcYzHqTDFlUG5cfbRJQRsGwapnQrovxKPQBc6supd44cw7fbBxN3TCLGErzZcN4hb-eJoOsm-eM7so-cCWZoPd6Nab47Mhuz-TKMg_3wLxkpgENkh-dlomsuN3qJjlIHIBcqLVWRnNKVbvIxlUc6IVRDHNit2ACkyNJQO8fNo4d0sYIjyoZ9B-ndMw9F7yLRUjysyQ6q2cK9lkyOe0uRnA6t2xw2FYYd0z4rrjzkNVOaFB4keP65PbM4zNBRkF_i8lMnUl6hT5oqbTR0mZgCbwOMX6efSjWyvMzT1X4zt3UpuwbEL_rBifdN4nwijxdoFafiSldeGEyCPOGPPJPnc5C76ZD0aWFXUN7q0oLmd_WC0)

<img src="file:///C:/Users/Zhang%20Puyu/Downloads/hLTBR-Cs4BxhLn0vsSNwiaSXCBRT00iKkqNGz74e5W8jJcokAP8bXycwHVvxIOgYQ4a6TAWFWJsFRsQ-UKZvO94QT9sex9bBJGtjYE0IyAA1Q2KkuSUIJXaygKhJzaoFGEwUa29loIUX3bIGfzPmZVQE_5iH_sa5jPPyeXryeTARco1FdwENDgrGxRacl_4EJDPO3S0Q0IyWQ.png" title="" alt="hLTBR-Cs4BxhLn0vsSNwiaSXCBRT00iKkqNGz74e5W8jJcokAP8bXycwHVvxIOgYQ4a6TAWFWJsFRsQ-UKZvO94QT9sex9bBJGtjYE0IyAA1Q2KkuSUIJXaygKhJzaoFGEwUa29loIUX3bIGfzPmZVQE_5iH_sa5jPPyeXryeTARco1FdwENDgrGxRacl_4EJDPO3S0Q0IyWQ.png" data-align="center">



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

- [ ] Map: terrain, resource & building placement

- [ ] Unit movement: path-finding, terrain effects, resource collection

- [x] Pops and resources: buildings, jobs, employment

- [ ] UI: main menu, in-game pause menu, in-game data display (e.g. hovering windows, selection menus)

#### 12.23 - 12.29

- [ ] Map: random tilemap generation

- [ ] Unit: formation and dismissal, health point

- [ ] Production: resource transportation system, storage system

- [ ] Technology: tech-tree, research mechanism

- [ ] UI: textures, unit movement paths, highlighted texts

#### 12.30 - 1.5

- [ ] Event: event triggers, event effects

- [ ] Map: fog-of-war

- [ ] Pops and resources: resource consumption

- [ ] UI: event windows

## Coding Conventions

- All variables and functions have their names spelt in **snake_case**.

- All classes and game files have their names spelt in **PascalCase**.

- Folders should be named using **kebab-case**.

- **Private** methods should have their names preceeded by an **underscore**.

- Always use the **self** keyword in class methods when referencing class attributes.

- Always use **type notations** for variable declaration and (non-void) return type of functions.

- Semi-colons are **not** used.

- Each line should contain only a single statement. 

- Use **and**, **or**, **not** rather than **&&**, **||**, **!**.

- No statement should be longer than **100 characters**.

- Names of variables and functions should be **descriptive** and **no custom abbreviations** are allowed.

- `match` statements should always have a default `_` branch.

- `while (true)` is **banned**.

- When declaring new classes, write `class A extends B` in a **single line**.

- Custom scripts and resources are placed under **common** folder while packed scenes (assets) are placed under **assets** folder.

- To avoid confusion in syntaxes, retrieving from dictionaries will use **Dictionary::get**  method instead of accessing by index.

- `to_string` and `_to_string` methods are called implicitly.

- Use `Signal::connect`  and `Signal::emit` to manage signals (this is the new Godot 4 syntax).
