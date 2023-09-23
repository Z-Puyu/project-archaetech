# Project: Archaetech

## Motivation

We all love to play 4X strategy games. Most of the currently available 4X games, whether historical or fictional, tend to focus on advancing your civilisation **forwards** from a primitive era to a futuristic one. However, we would like to do some **reverse thinking** here: what if the nation you play as in a 4X game progresses not by **discovering the unknown of the future**, but by **digging out the lost legacies of a mysterious past**?

## Background Story

X is a planet which is capable of sustaining intelligent life forms. Unfortunately, however, the planet is harsh to its residents and annihilates civilisations at random intervals. As a result, the planet is full of archaelogical sites containing the relics and knowlegde of the civilisations in the ancient times.

The player will role-play as a nation whose population has just awaken in a sanctuary built in order to shelter a selected minority from destructive catastrophes. Oblivious to the situation, the people decide to self-organise and make their way to explore the unknown world.

## Features

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



#### Education & Leaders



#### Ideologies & Policies



#### Events



#### Species Customisation



### Phase III: Quality-of-life Extensions

#### Competitive AI Players



#### Late-Game Challenges



#### Mini-quests & Achievements