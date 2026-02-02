# Creating Mods
So, you've fallen down the rabbit hole of wanting to create mods for this game, as I will walk you through everything there is to know about the basics of creating mods for this games, specifically creating new Characters, and Items.

Your first step is setting up a Modloader of your choice, these documents will assume you're using MelonLoader, so i suggest going with that, but otherwise just follow along using the documentation for your specific ModLoader.

With that out of the way, I will be explaining the basics of how to set up your modding environment in this document, and then linking you towards other documentation for whatever specific type of mod you wish to make (character,item,etc).

# Prerequisites

This guide will assume you know C#, as that is what this ( and most Unity games) are coded in, however dont be afraid if you dont, as i will try and simplify it for you.

This guide will walk you through everything else, from installing neccessary tools, to setting up a dev environment.

First of all, the tools you will need are [Melonloader](https://melonwiki.xyz/#/?id=requirements) as well as [Visual Studio](https://visualstudio.microsoft.com/).

Next I reccomend also installing the [MelonLoader templates for Visual Studio](https://melonwiki.xyz/#/modders/quickstart?id=visual-studio-template), as generally it will save you alot of time manually setting up the project.

With that out of the way we can officially begin.

First of all. open Visual Studio, create a new Project, and select the MelonLoader Mod template (or if youre a advanced user, you can use the "MelonLoader Mod (Manual)" option), then hit next.

Here you will provide the Project Info, you may call it whatever you like, so long as you remember what you call it, for the remainder of this documentation, I will be using the name "CursedMod".

Hitting next again will prompt you to select the game executable, for this goto Steam, and right click the game, and select "Browse Local Files", for the Demo this will bring you to "C:\Program Files (x86)\Steam\steamapps\common\Cursed Words Demo".

after that, we are officially set up in terms of our Modding Environment.

From here this is where the documentation ends, and I will likely edit this once I've Setup the remainder of the documentation
