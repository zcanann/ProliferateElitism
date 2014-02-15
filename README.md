River
=====

2D Isometric RPG written in C#/XNA featuring 3 unique classes and multitides of abilities and enemies.

- The classes include Warrior, magician, and bandit.
- Features inventory system with a shop keeper, lootable enemies, lootable chests
- Stat/leveling system.
- Swappable skills with 3 skill slots active at any time, but each slot has 3 choices that can be selected.
- Random item quality on drops, different item types can have different special attributes -- ie boots have a 'speed' property, weapons can have a 'burn', 'poison', etc effect.
- Enchantment system that allows an item to be enchanted up to 10 times for a price -- and after each enchantment the probability that the item is destroyed by the enchantment increases.
- Mini map

TODO:

(There is a more detailed TODO list in the 'entry' class)

- Swap out any borrowed art
- Finish implementing skills
- Fix the problems that come with the particle system being used for skills -- they cause frame rate issues
  due to improper use. (May need to implement my own particle system that works better with the isometric view)
- Use the level editor to design more levels. As of right now there are not very many.
- Perhaps improve pathing AI
- Saving (inventories, level -- should be seralizable so its not a huge task)
