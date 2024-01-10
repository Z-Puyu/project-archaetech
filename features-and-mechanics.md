---
description: All about making the game fun
---

# Features and Mechanics

## The Economy

Economy management is the core of Project: Archaetech. After all, the most anticipated part in any post-apocalyptic story is how the survivors rebuild an environment in which they can restore the life of the past. We want the player to spend a considerable proportion of their game time on thinking about ways to optimise the institutions of their economy according to varying needs, so that the economy system of Project: Archaetech can really be dynamic instead of a snowballing game.

### Pops

If there has to be a single thing that forms the very core of economic simulation in games, that is **population**. Indeed, we cannot emphasise more on how instrumental population is when it comes to building a foundation for economic processes and systems in video games. Population is the bridge from game objects like buildings to resources and they would basically determine how assets created by the player at various costs can transform into future profits, and how a poorly planned gameplay would results in huge deficits in resources.

We take a design approach which is similar to that of Stellaris in Project: Archaetech. We use the concept of a **Pop** to represent an abstract unit of population with a particular set of traits. Pops consume resources on a monthly basis. To make this a little more interesting than merely a trigger that reduces the player's food regularly and does absolutely nothing else, we decided to make some reference from Victoria 3's production system where each type of goods has various substitutes which can be consumed instead of the good itself. In Project: Archaetech, we define several types of resources which can be consumed as food. As a metric, 1 unit of the basic food resource represents the demand for food by 1 Pop per month at default conditions, and the various other types of consumable resources can satisfy various degrees of food demand measured in terms of **how much basic food 1 unit of that resource is equivalent to**.

These resources are ranked in terms of their priority score as "food", representing the **tendency of Pops to choose the resource as food in times of shortage of a better food choice**. For example, we have defined a Processed Food resource which has a higher priority than Food (meaning lower-quality food). Therefore, whenever the Pops consume food, they would choose to consume Processed Food (if there is any) first, and only after Processed Food has been all consumed would they start to consume Food to satisfy whatever demand that has not been met yet.

To add more variations, we also implemented something known as a **secondary food demand**. This represents the food consumption by Pops _after their basic living needs have been fulfilled_. Having sufficient amount of food to fulfill these additional demand will make the Pops extra happy.

<details>

<summary>Potential Enhancement</summary>

This mechanic to deal with food consumption can be quite predictable and not so fun when the player has understood the priorities of different types of food. Suppose we have three different food resources, namely A, B and C, it does not make much sense to say that A has to be completely exhausted before B can begin to get consumed and the same goes for C.

Another pitfall of the current design is that once the player has built up the productive capacity and can produce sufficient A for the Pops, then B and C practically become **obsolete** as food resources, which is not what we would wish to see. Instead, we would want all types of food to stay relevant throughout the course of the game. To do so, we propose two possible enhancements:

1. Instead of a sequential selection, the agregate food consumed should be a **random composition of different types of foods based on weighted probabilities**. This weighted probability can be determined collectively by the priority of the food itself, the abundance of the type of food and the traits of the Pops (for example, certain Pops are more inclined to eating a particular type of food than others). Therefore, the player would need to make sure that the production of different foods stays balanced,
2. Pops' demand for food should change dynamically. For example, we can associate each Pop with a wealth indicator, which will affect the type and amount of food this Pop consumes. This will cause the demand for food to change constantly, so that food security is not that straightforward to achieve.

</details>

The abundance of food in Project: Archaetech is tied to another important mechanic about Pops - population growth. We take food abundance as an important factor affecting the _environmental capacity_, so it will definitely impact how fast Pops grow. In the future when we start optimising and balancing game data, we will be using a variant of the Logistic growth model which takes into account the food abundance (or even shortage) factors.

### Production

#### Resources

An indispensable mechanic in economic simulation is the transformation between different types of resources. In Project: Archaetech, resources are categorised into different classes to capture their roles in the production chain.

On game start, the player will have some storage of **Basic Resources** - **Food, Minerals and Wood**. Food is essential to feed the player's population while Minerals and Wood represent the most basic resources used in construction and as raw materials in production. These resources will get consumed fairly quickly, but luckily they can be easily produced from **Farms, Quarries and Logging Camps** respectively, which can be built anywhere as long as the terrain matches their requirement.

As the player explores the map, more **Collectible Resources** will get discovered. These resources are scattered across the wild based on a weighted spawning chance which is related to the resources' rarity and terrains. However, most of these resources will not directly show up on the map and require the player to conduct **Resource Investigations** to locate them in a map cell before they become visible. Some of them can be directly collected manually by assigning Pops to them, but manual collection is very inefficient. Therefore, the best way to collect these natural resources is to build **Buildings** to extract them in bulk.

Buildings do more than just collect natural resources. As the player unlocks more technologies and building types, some buildings will be able to consume these collected raw materials and produce **Consumer Goods and Industrial Goods**. Consumer Goods are meant to be consumed by Pops and used to maintain some high-end **Jobs**, while Industrial Goods can be used to build higher-level facilities or consumed as input materials for production of high-end products.

At a higher level, the player can also produce **Luxury Goods and High-end Products** to further boost the life quality of Pops and productive efficiency, but the cost of production of these resources will be very expensive.

#### Buildings

We have talked some aspects of buildings in the previous section. Buildings are the core of resource production in Project: Archaetech, but they do not automatically generate resources on their own. Each building will first offer a series of **Jobs** and ultimately, it is these Jobs that produce output. Whenever there are unemployed Pops and available Jobs, the building will start recruiting Pops to work there. Only when a building has employed Pops will it start to actually generate output from these Jobs. Each Job will come with some maintenance cost representing the raw materials needed for it to produce the corresponding output. Thus, all of these boil down to population management again, since stacking buildings without enough Pops to work at them is not going to help the player grow the economy.

Each building can switch between several **Production Methods** which determine the types and numbers of Jobs that will be provided by the building. More Production Methods get unlocked as the player's technology progresses. More advanced Production Methods may produce higher output, but their high maintenance cost may eventually result in an overall deficit instead of profit for the player, so the player will need to delicately choose the most appropriate Production Method to run the production system.

#### Transport System

A core difference between the production system in Project: Archaetech and some other similar games is that _resources do not immediately enter the player's storage once they get produced._ Instead, they stay inside the _local warehouse_ of the buildings which have produced them first. The player will not be able to use these resources until they get transported into the player headquarter. Similarly, when a building produces resources, it does not consume raw materials from the player's storage directly, but instead uses those stored in its local warehouse. Therefore, it is crucial to keep the buildings' warehouses filled with sufficient resources so that the production cycle does not get disrupted.

Each building can only support a very limited number of **Transport Route** between itself and other buildings. For example, by default, the player's headquarter can only be connected to at most 6 other buildings to supply raw materials to them, and all basic productive buildings can only have 1 route to move out the resources produced and 3 incoming routes for receiving raw materials. This is definitely not enough to maintain a growing productive capacity, and so the player needs to plan the transport system, using buildings as intermediate logistic centres to agregate resources produced at different locations before eventually moving them to the headquarter. In this sense, buildings and Transport Route together form a _directed graph_ in which resources move around.

The player can choose what types of resources to be transported between two buildings. By default, transportation is conducted _manually_, meaning that each Transport Route also needs the manpower to maintain. Manual transportation is also inefficient and can only transport 30 to 75 percent of what a building has produced. This transportation capacity increases with _diminishing marginal benifit_ as more manpower is assigned to the route, but will be capped at 75%. To increase this capacity further, the player will need to use **Automated Transport Route**.

An automated route is a transport link which is maintained by AI and robots and will only become available at a later stage of the game. These routes will always operate at 100% efficiency as long as their maintenance is fully supplied (i.e. no attrition in transportation). However, the cost for this is that the player can no longer designate the resources to be transported between buildings as now a centralised super computer takes the control over and it will plan the transportation as follows:

* Suppose $$x$$ units of resource of type $$T$$ is produced in a building and the buildings has $$n$$ outward transport routes to other buildings, then $$\frac{x}{n}$$ units of $$T$$ will be exported via each output route.
* If $$T$$ is transported to a building which does not consume it, all of it will be transported out of the building via all of its outward routes, if any, under the same rule. If the building is a dead end, it will then store all of $$T$$ it receives in its local warehouse
* A building can have a mix of manual and automated routes, where manual routes will take the priority in transportation and whatever left-over resources will be processed by the automated routes.

## Map Navigation & Exploration

The map in Project: Archaetech is based on a hexagonal grid with fog-of-war (FOG) mechanics. Players can form **Scout Teams** with Pops to chart the map. Due to the adverse environment outside amid the aftermath of an apocalypse, each scout team has a **health** value which will diminish at a variable speed based on terrain and climate conditions. To regenerate the health, the scount team needs to find a habitable hexagonal cell to rest in a camp. Additionally, different terrains also impact how fast a unit can travel through it.

_Explored regions do not stay un-fogged forever!_ This is another unrealistic feature in many other 4X games where a pre-industrial country can scan the whole world and somehow remember the locations for centuries. In our game, the sanctuary has something known as the **remembrace radius**. Explored regions outside of it will be **forgotten** and **re-fogged** after some time. To prevent this, the player needs to do at least one of the following:

* Expand the sanctuary so that the region falls within the remembrance radius again.
* Assign Pops to collect resources from that region regularly.
* Build an **Frontier Outpost** and assign Pops to work there. An outpost has its own remembrance radius and everything inside it will be remembered.
* Climb the technological tree to discover technologies that will expand the remembrance radius.

## Research & Technologies

The fun part about Project: Archaetech's technology system is that the population _learns their ancestors technologies_ rather than discover their own ones. At game start, the sanctuary contains a collection of ancient technological documents. The player can assign Pops to learn from them and unlock some fundamental technologies. However, these documents will soon be exhausted.&#x20;

To advance the rest of the technological tree, the player can build a **Research Quarter** and assign Pops to work as **Researchers** to generate **Research Points**. Research Points are a special kind of resource which will be credited directly to the player's main storage no matter where they are produced. At the end of each game month, the points collected over the past 30 game days will be randomly distributed to all available technologies and increase their progress. Once a technology has reached 100 progress, it will be unlocked. The player can manually set a **Focus** to one technology, which will force at least 33% of all research points to be distributed to it. Nevertheless, Research Quarters generally generate very few Research Points for the most part of the game and is not sufficient alone to build up the player's technologies.

Therefore, we introduce another mechanic to gain Research Points and technological progress, which is more reflective of the idea of "learning from the pre-cursors". There are **Pre-cursor Archeaological Sites** ramdomly located in the world and can be discovered by Scount Teams as they explore the world. The player can assign Pops to work as **Archeaologists** and dig out the secrets of the ancient. At each stage of archeaological work, events will fire which allows the player to gain a large number of Research Points or directly increase the progress of some selected technologies.&#x20;

Either way, we wish the technology mechanics in Project: Archaetech to epitomise the concept of uncertainty and exploration. To achieve that goal, we decide to _hide_ the technological tree from the player. The **Tech Tree** which the player can view will only display the technologies which have already been unlocked. To add more flavours, we also wish to make the relative positions of different technologies to be somewhat randomly placed instead of complying to a fixed structure, so that each game the player has started will show a completely different path of technological advancement.

However, we do realise that a completely invisible technological tree can have its disadvantages in terms of gameplay as it makes the player feels like losing control over the course of the game. Therefore, we have planned another special mechanic - **Eureka!** which will be randomly triggered at the player's Research Quarters based on some conditions that modifies its weighted probability. At each **Eureka!** moment, the player will be granted a chance to reveal the potential paths of technological advancement from a technology which has been unlocked. Alternatively, the player can also give up the chance of unveiling the Tech Tree in exchange for extra research progress to existing technologies which are researchable.

## Events

**Events** are an integral element to create immersion. They play an important role as notifications for major game progression and achievements. In addition, events should allow the player to feel involved in the story happening in the game world and add crucial flavours to the plot.&#x20;

Events have their effects confined within their respective scopes and avoid unnecessary interactions with game objects beyond what will be affected by them. This is due to performance considerations where we wish to reduce computations by blocking irrelevant game objects from getting involved in Events. In Project: Archaetech, we have implemented the following types of Events:

* **Global Event**: Events which are fired at a global level. This usually involves changes in the game world's high-level parameters, such as a global modifier that affects all game objects' properties, or a change in a global flag that marks the stage progression of the game's story.
* **Local Event**: Events which target the player and affect the player's game statistics. For example, modifiers affecting resource production or Pop growth, technology progression, etc.
* **Cell Event**: Events which occur at a particular cell. For example, the discovery of new resources or natural disaster events.
* **Object Event**: Events which only affect some particular game object, such as a building or a unit.

In the spirit of uncertainty, no all Events are visible to the player when they fire. Some of them fire and resolve secretly, whose effects will only be noticed by the player much later.

There are some generic events designed to add uncertainties to the gameplay, increase special flavours to story-telling, etc., and there are also some events which mark completion of important actions or major game stage progression. Therefore, these events should be triggered differently. We plan to design the following event triggers:

* **Random Event**: These are mainly generic events which are fired on random intervals based on their _rarity_, or _mean-time-to-happen_ (MTTH, Paradox players would be well-acquainted with this term). We will probably be designing a polling system based on an event pool, so that we can schedule the occurrence of these events as a queue to conveniently spawn them to the game world.
* **Conditional Event**: These are events which will fire whenever a condition is met. The key difference between these and random events (which will also be likely to have custom triggering conditions) is that the context of conditional events will be closely linked to how the player acts in the game and represent the consequences of the player's decisions. They also tend to be a bit complex in terms of effects, so we will be likely to check the respective conditions once every month in the game instead to save computational cost.
* **On-Action Event**: These are events which will _immediately fire_ once a certain action is executed. A classic example is unlocking new technologies. We can easily use the global event bus to handle the triggering of these events.

The player will need to resolve each event by choosing one of the available options. Some of these decisions will bring about immediate effects which are more or less predictable. However, some Events will set up an **Event Chain** if they are resolved in a certain way, which will not be made clear to the player again to increase the uncertainty and "fun" in strategising.

## Education & Leaders

{% hint style="warning" %}
This section has not been completed.
{% endhint %}

The Education mechanics in Project: Archaetech aim to bring the involvement of Pops in gameplay to a further level. To start off, we categorise Pops working at a certain Job in to three stages: **Novice, Regular and Expert** which reflect the competency of the Pops at that Job. The competency will naturally grow as the Pop works longer in the industry, but the player can build **Educational Institutions** to train regular Pops towards higher competency levels. _In particular, Educational Institutions are the only way to convert Pops working in Research Quarters into Experts._

If a Job has at least one Expert employed, then there will be a chance of a **Leader** spawning from the Job type. The player can assign the Leader to buildings with this Job to further boost its productivity. The more Experts there are at a Job, the higher the chance of a Leader spawning for that Job will be.

## Ideologies & Policies

{% hint style="warning" %}
This section has not been completed.
{% endhint %}

As the game progresses, **Ideologies** may emerge among the Pops. Ideologies are sets of value systems a Pop believes in. This may influence the behaviours of the Pops. For example, an environmentalist Pop may demand less consumer goods and refuse to work at heavy industries.

The player will be prompted to choose a few among all ideologies present as the **mainstream** ideologies, which will give buffs and debuffs to the whole nation and increase the chances of these ideologies appearing among Pops.

**Policies** are closely tied to ideologies and are unlocked mainly via researching humanities technologies. Policies define the rules in the sanctuary and may produce interesting effects in interaction with ideologies.

## Standard of Living (SOL)

{% hint style="warning" %}
This section has not been completed.
{% endhint %}

We wish to quantify how successful the player is at running his sanctuary, so we introduce the design concept of **SOL**. This may be based on several indicators:

* Amount of food: Excess food storage will bring an increase in SOL while insufficient food supplies drastically decrease SOL.
* Consumer and luxury goods: Shortage in consumer goods decreases SOL, and surplus in luxury goods increases SOL **provided that consumer goods do not have a shortage**.
* Available space: **Living quarters** provide a certain amount of residential space. A surplus in residential space will slightly increase SOL but a crowded environment will significantly lower it.
