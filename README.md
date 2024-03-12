# FastStart

This mod provides 2 ways to make your start faster and skip manual digging/crafting stage.  
And there are 3 different packages (few, enough, huge) that you should choose in config (differences below). Every package has coils, circuits and gears, enough to research electromagnetism, assembly, research, smelting and logistics.

## Default mode:

At the time new game starts you instantly receive a bunch of items, based on amount chosen in config. Called once, nothing more, super boring, super stable. *I'm already thinking about checking tech rewards and giving items onTechUnlocking without changing game data*

***Note that you can build things only from inventory until you'll research corresponding techs***. 

## Research mode: 

Alternates space capsule loot and early tech rewards, so you will receive same amount of items, but that way it looks less cheating and there are some additional options. This mode changes game data (techprotos) so it will fail abnormality check (here is a mod...) and probably lock uploading ur save to milky way (i'm not sure how it works but be care of that). Options:

+ **Speed up some techs:** lowers amount of hashes needed by techs that use items as inputs (i mean not cubes), amount of items remains the same. I don't like the idea that it has no way to be accelerated. It still will take some time, but not like 5 mins for every mecha upgrade.

+ **Free ILS:** completing *Interplanetary Logistics* research will reward you with 2 ILS and 6 vessels so you won't fly back and forth a couple times to setup first ILS.

And if tier 0 drones piss you off with that super speed there is an option **Give blue matrix** for both modes that will give you 300 blue cubes for upgrading drones tier 1 speed+count (don't you dare to use them for something else🧐).

**Research mode** is not as clean as **Default** *because of my shitcode* however i tested first 5 mins of the game a few times, unlocked techs with cheats and without, no errors received so far and it works as intended. 

If anything will be wrong or if you have an idea: Issues on github or spiritfarer707 in discord.

### Screenshots with things gained for Research mode (for Default mode its almost same, no mk2 inserters)

#### 'few' option

Gives a little more than default rewards.

![few](https://raw.githubusercontent.com/Trol1face/DSP_FastStart/main/images/few.png)

#### 'enough' option

Gives a lot more, enough to setup blues. Also 60 motors for first movespeed research.

![enough](https://raw.githubusercontent.com/Trol1face/DSP_FastStart/main/images/enough.png)

#### 'huge' option

Lots of everything

![huge](https://raw.githubusercontent.com/Trol1face/DSP_FastStart/main/images/huge.png)

#### Space capsule drop (default in Default)

![space capsule drop](https://raw.githubusercontent.com/Trol1face/DSP_FastStart/main/images/capsuledrop.png)

## Recent changes

#### 1.1.1

**If updated to this version remove config file and create new one (launch game once)**

- Old way to gain items is back and set as **Default**, new way is called **Research mode** and is should be enabled in config if you like its features.
- Also there was a bug with mk2 logistics, not anymore.
- Speedup techs option for Research mode has been added, now even faster.

#### 1.1.0

Changed whole mod, no more cheaters inventory that you can't properly use right after landing, things are gained as awards for basic tech researches and space capsule drops you research components. Also added one more option.

#### 1.0.3

- Changed circuits to 40, gears to 20, to be able also research logistics.  
- Increased **few** wind turbines from 6 to 8.
- Added 60 motors to **huge** variant for fast research of tier 1 movespeed.
- Added an option in config to give blue matrixes for fast drones speed+count research