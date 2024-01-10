---
description: How we have built the foundational frameworks
---

# #1 Setting up the Basics

As compared to many other genres of games, a simulation/strategy game has a particularly heavy load in terms of data and various statistics. Therefore, recognising the importance of data management, we did not rush to creating various game assets, but instead focused on designing a reasonable and well-structured data management system to store and process the various data crucial to our game's functionalities.

## Make the Time Running

First, to create the RTS aspects of our game, we set up a global `Timer` as the game clock. Every emission of the `timeout` signal corresponds to one tick (i.e. one game day). To reduce computational burden, most of the game data will only be updated once every 30 game days, which is a standard practice in many RTS games. We achieve this by using a `GameManager` node which releases a `new_month` signal once every 30 game days have passed.

## Storing and Proceesing Data

One of the core systems in a simulation game is the buildings and its interaction with population, which is what keeps the game economy running dynamically. Therefore, the very first task we embarked on was to build global manager scripts for **population, resources and buildings**.

### Resources

Initially, we thought of keeping a dictionary of resources with the keys being the names and the values being the amount.

However, this brings about many potential problems. First and foremost, **names of resources are mutable**, which compromises the robustness of this design and we could easily break our code if we decide to change the names of resources. The use of names as keys also make it less convenient nor clean when we need to manipulate resource data, in a sense that we will always need to retrieve the correct resource object by its name before we can do anything about it, and when we need to update a resource's data fields we need to use the name as a reference to retrieve it and use that reference again to put it back. These operations are not necessary in the first place.

We then decided to use custom resources to wrap each resource as a stand-alone `ResourceData` resource script. Godot keeps these custom resources as global objects and each resource will contain all the immutable information about that particular resource object. As such, these `ResourceData` scripts will be unique references to the resources, which we can easily use as the keys to store and access them. So now, the resource management uses the following implementation:

{% code title="ResourceManager.gd" lineNumbers="true" %}
```gdscript
var resources: Dictionary

func add(res: ResourceData, amount: float):
    if resources.has(resource_name):
        self._resources[resource_name] += amount
    else:
        self._resources[resource_name] = amount

func use(res: ResourceData, amount: float):
    if self._has_enough(res, amount):
        self._resources[res] -= amount
        
func has_enough(res: ResourceData, amount: float) -> bool:
    if self._resources.has(res):
        return self._resources.get(res) >= amount
    return false
```
{% endcode %}

### Population

The population management has been fairly simple as of the current stage. We first keep track of a total population count and another count for the _unemployed_ population. We connected the `new_month` signal to the `PopManager` so that we can consume food and update population growth every month. There is a base growth rate by which a progress bar will grow per game day, and when the progress bar hits the maximum, we will increment the population count. We did a small check using the `floor` function to carry overflowing population growth progress to the subsequent month.

{% code title="PopManager.gd" lineNumbers="true" %}
```gdscript
@export var growth_rate: float
var _growth_progress: float
@export var pop_count: int

func _ready():
    self._growth_progress = 0
    GameManager.new_month.connect(self._update)
    
func _update():
    self._consume_food()
    self._growth_progress += self.growth_rate
    if self._growth_progress >= 1:
        var nGrow: int = floor(self._growth_progress)
        self.pop_count += nGrow
        self._growth_progress -= nGrow
```
{% endcode %}

### **Buildings**

Buildings form the core of our game's economy and we did spend a lot of time on its design. In the very initial version, we use an array to store all the buildings which have been constructed by the player. We let our `BuildingManager` receive the `new_month` signal from the game clock, such that for every 30 game days, the manager iterates through the list of buildings and produce the resources, like this:

{% code title="BuildingManager.gd" lineNumbers="true" %}
```gdscript
func _on_new_month():
    for building in self._buildings:
        building.work()
```
{% endcode %}

We soon realised that this was far from optimal. If we store all buildings at all times and iterate every single one of them in this manner, it forces every building class to implement a `work` method, which is not flexible. There could be some buildings which do not produce any output, so they do not need to have that method and do not need to be processed every game month.&#x20;

We solved this problem by using custom signals. We moved the logic to execute resource production to each building object itself by creating a custom class `Building` which contains a `data` field as a reference to a custom resource containing all immutable data for a building type.

Then, we defined a `ProductionBuilding` class extending `Building`:

{% code title="ProductionBuilding.gd" lineNumbers="true" %}
```gdscript
class_name ProductionBuilding extends Building

var _jobs: Dictionary

func _ready():
    GameManager.timer.timeout.connect(self._work)

func _work():
    for job in self._jobs,keys():
        var nWorkers: int = self._job.get(job)
        var k: float = 1
        for resource in job.input.keys():
            var demand: float = job.input.get(resource) * nWorkers
            var available: float = ResourceManager.get(resource)
            k = min(available / demand, k)
        for resource in job.input.keys():
            ResourceManager.use(resource, k * job.input.get(resource) * nWorkers)
        for resource in job.output.keys():
            ResourceManager.add(resource, k * job.output.get(resource) * nWorkers)
            
```
{% endcode %}

Now, on each `new_month` signal emission, each `ProductionBuilding` calls its own `work()` method, while other buildings without this method will be ignored.

## Spawning System

With the data management set up, the next task is to build the spawning system to add the buildings and other objects to our game's scene tree. We wanted to make this system as generic as possible from the very beginning, so that it can be used to spawn not only buildings, but other similar objects such as units (which can be built in a similar manner).

The first thing we noticed is that Godot does not have a built-in linked list structure, so this was when we decided to build this system entirely with C#. A side benefit of using C# here is that C# provides good support for generics which allows the system to be safely usable by any spawnable objects which we may define later.&#x20;

In the initial version, when the player build a new building on a map cell, we would instantiate a building object at that cell but make it invisible. Then, we would augment the building with its waiting time to build as a pair and enqueue the pair into a queue. At each game day, we would retrieve the head of the queue and decrement the waiting time by one day. If the waiting time has reduced to 0, we will pop from the queue and make the building node visible.

{% code title="ConstructionQueue.cs" lineNumbers="true" %}
```csharp
private LinkedList<KeyValuePair<T, int>> q;

private progress() {
    KeyValuePair<T, int> task = this.q.First;
    this.q.RemoveFirst();
    if (task.Value > 1) {
        this.q.AddFirst(new KeyValuePair<T, int>(task.Key, task.Value - 1));
    } else {
        task.Key.IsVisible = true;
    }
}
```
{% endcode %}

The pitfall of this implementation is obvious: it only supports constructing one building at a time. To construct multiple tasks concurrently, we have tried to use an array of queues. However, there was another problem: suppose we have $$n$$ parallel construction queues but the $$n$$-th task completes first. The inituitive thing here is to move the $$(n + 1)$$-th enqueued task to the head of the $$n$$-th queue, but **there is no way to know which queue and which position this task is currently at!** Nevertheless, an array of queues faces degenerating performance issues so we decided to discard this design.

To improve our design, we decided to analyse the functionalities of the ideal construction queue:

* The queue needs to be able to pop out any task which has completed, regardless of its location, while maintaining compactness: this means we need a linked list, but at the same time we also need to hash all the values inside this list's nodes.
* The queue needs to update multiple tasks at once while knowing how many tasks can be carried on concurrently at maximum, so that other tasks will wait for the completion of earlier ones: how about **dividing the queue into two parts**, `active` and `inactive`?
* We should not be iterating the queue to update all active tasks, and each task must be able to update itself!

We immediately realised that the C# `LinkedList<T>` class in the standard library is not powerful enough to satisfy our need. Luckily, we have found the C5 library which provides a `HashedLinkedList<T>` class which internally uses a doubly linked list with a hash table to support removal and insertion at any location in $$\mathcal{O}(1)$$. Subsequently, our new `ConstructionQueue` has been built upon two self-balancing hashed linked lists.

{% code title="ConstructionQueue.cs" lineNumbers="true" %}
```csharp
public partial class ConstructionQueue<T> : Node where T : Node {
    private readonly HashedLinkedList<ConstructibleTask<T>> active;
    private readonly HashedLinkedList<ConstructibleTask<T>> inactive;
    private int maxActiveSize;
    private readonly HashDictionary<Cell, ConstructibleTask<T>> constructionSites;
    
    private void OnTaskCompleted(object sender, EventArgs e) {
	ConstructibleTask<T> task = (ConstructibleTask<T>) sender;
	if (this.constructionSites.Contains(task.Location)) {
	    this.Remove(task);
	}
    }

    public void Enqueue(ConstructibleTask<T> task) {
	if (this.active.Count < this.MaxActiveSize) {
	    this.active.InsertFirst(task);
	    task.Start();
	} else {
	    this.inactive.InsertFirst(task);
	}
	this.constructionSites.Add(task.Location, task);
	this.AddChild(task);
    }

    public void Remove(ConstructibleTask<T> task) {
	if (this.active.Contains(task)) {
	   this.active.Remove(task);
	} else {
	   this.inactive.Remove(task);
	}
	Global.Grave.Enqueue(task);
	this.constructionSites.Remove(task.Location);
	this.Refresh(); // This will re-balance the two queues
    }

    public bool RemoveAt(Cell location, out ConstructibleTask<T> task) {
	return this.constructionSites.Remove(location, out task);
    }
}
```
{% endcode %}

Here, the `ConstructibleTask<T>` class is a wrapper encapsulating a node which is in the process of being spawned. At each game day's passing, the task well update its delay time until it reaches 0 at which point the task will tell `ConstructionQueue` to remove it from the queue and self-destruct. By using generics, we can essentially use this utility class to manage the spawning of any node with a delay time while ensuring type safety.
