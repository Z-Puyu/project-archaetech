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
