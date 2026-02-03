# Character Creation

Firstly, I suggest making a Custom Item before diving into Character Creation.

Character Creation first requires a few patchs to ensure it gets recognized by the game, and is unlocked.

for this i suggest taking a look at [Example.cs](https://github.com/Neptune1190/CursedWords-Modding-Documentation/blob/main/Creating/Example.cs) which is the end result of creating a character (and a item which i will get into later)

First, i will get into what exactly makes up a Character, 
```
{
		this._name = name;
		this._description = description;
		this.CharacterItem = characterItem;
		this._artFileName = artFileName;
		this._uiColorA = uiColorA;
		this._uiColorB = uiColorB;
		this._goodIntroItemTypes = goodIntroItemTypes;
		this._endScreenVictoryOffset = endScreenVictoryOffset;
		this._endScreenDefeatOffset = endScreenDefeatOffset;
		this._isSecret = isSecret;
		this._secretUnlockCondition = secretUnlockCondition;
		this.MyEmotion = myEmotion;
	}
```
Here we see what a full Character is made of, 
1. A name
2. a description
3. the item the character holds (the pin item)
4. the sprite to use for the character (also seems to be used for the beggining of emotions)
5. uiColorA, which is used in the character select screen, as well as the colored background for the inspector in games. 
6. uiColorB, which is used in the UI for your sticker and stamp slots
7. items that are reccomended to appear in first shop to prevent anti synergies or otherwise runs that end at first shop
8. how much to offset the sprite in  the victory screen
9. how much to offset the sprite in the defeat screen
10.boolean for if the character is secret
11. the secret unlock condition  (beating a secret boss as far as i can tell)
12. finally the emotion to use for the character, unsure when this is used to be honest.

in the end, your character should look something like this, 
```
{
	public CustomCharacter()
		: base("CustomCharacter", "this is a custom character test to see how easy it would be to add one", new Vowelizer(), "CatFish", new Color32(180, 111, 74, byte.MaxValue), new Color32(98, 158, 233, byte.MaxValue), Vector3.zero, Vector3.zero, new List<Type>
		{
			typeof(Pear),
			typeof(Egg),
			typeof(Doughnut)
		}, Emotions.DinosaurPowerful, false, "")
	{
	}
}
```
the reason I reccomended creating a custom item before beginning, is that characters themselves dont have any unique effects, their items HOWEVER do.

If you're not sure about how to actually turn your character into a mod, you can always dm me on discord (Neptune6866), ask in the modding channel within the Buried Things discord, or otherwise look at my [Example.cs](https://github.com/Neptune1190/CursedWords-Modding-Documentation/blob/main/Creating/Example.cs) and just overwrite the custom character example within it.
