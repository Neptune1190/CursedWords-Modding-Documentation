# Item Creation

Here we will get into the creation of Items, which is the most important thing of modding this game (in my opinion), as it introduces all new functionality to the game and runs.

First we will talk about what makes up a Item, and then move on to an example

An Item is made up of 

1.a Name
2.a sprite (an icon)
3.a Rarity (either Common, Rare, Legendary, Weird, or Unique
4.a list of Upgradeable components, either consisting of One (for most raritys) or Two (for unique items with 2 upgrade paths) different upgradeablecomponents
5.a item tag which determines what kind of tile build this item naturally belongs to
6.an item function tag which tells what this item does in terms of score
7.a cost if you want the item to show up in shops (and is a eligible rarity to show up in the shops)

otherwise there are a long list of other methods a item needs to function, including a description, and the items functionality, some examples of these methods are.

```
ApplyItemToScore
ApplyStartOfGridEffect
ApplyTileBonus
ApplyWordBonus
OnAcquire
```
etc, unfortunately for the sake of time, i wont cover what all these do in this documentation, when i eventually do, someone remind me to remove this.

Now that we know what an item actually is, we can create one, such as this example 
```
public class Vowelizer : Item
{
    public int MostRecentUpgrade;

    public Vowelizer()
    {
        Name = "Vowelizer";
        SpriteData.Add(new ItemSpriteData(ItemSpriteUsage.Default, "Vowelizer"));

        Sprite sprite = Core.LoadEmbeddedSprite("CorePatchs.Resources.Vowelizer.png");
        if (sprite != null)
        {
            foreach (var s in SpriteData)
                s.ItemSprite = sprite;
            MelonLogger.Msg("[Core Character Patchs] Vowelizer sprite loaded successfully!");
        }

        Rarity = ItemRarity.Unique;
        UpgradeableComponents = new List<UpgradeableComponent>
        {
            new UpgradeableComponent(1, 1, 1),
            new UpgradeableComponent(1, 8, 8)
        };
        Tags = new List<ItemTag> { ItemTag.CurseBuild };
        PinColors = new List<Color>
        {
            new Color32(byte.MaxValue, 183, 77, byte.MaxValue),
            new Color32(byte.MaxValue, 87, 34, byte.MaxValue)
        };
        ItemFunctionTags = new List<ItemFunctionTag>
        {
            ItemFunctionTag.Scatterer,
            ItemFunctionTag.SpecificAdditive
        };
    }

    public override string GetDescription()
    {
        string text = "START OF GRID: Turn most tiles into vowels.";
        if (MostRecentUpgrade == 0) text += " +1 Vowel becomes a wildcard (?)";
        if (MostRecentUpgrade == 1) text += " +8 score per word length";
        return text;
    }

    public override void Upgrade(int componentIndex)
    {
        base.Upgrade(componentIndex);
        MostRecentUpgrade = componentIndex;
    }

    public override GridData ApplyStartOfGridEffect(
        GridData gridData,
        int gridNumber,
        int numberOfGrids,
        List<HistoricWord> previousWords,
        List<BoardGenVizInfo> vizSteps,
        bool isReroll)
    {
        var affected = new List<Tile>();
        var all = new List<Tile>();
        string[] vowels = { "A", "E", "I", "O", "U", "Y" };
        int attempts = gridData.Width * gridData.Height * 3;

        for (int i = 0; i < attempts; i++)
        {
            Tile t = GridUtility.Singleton.GetTileForItemScatter(
                gridData,
                TileType.Normal,
                false,
                null,
                false
            );

            if (t != null && t.GetGlyphType() == GlyphType.Letter)
            {
                t.SetLetter(vowels[UnityEngine.Random.Range(0, vowels.Length)]);
                affected.Add(t);
                all.Add(t);
            }
        }

        if (MostRecentUpgrade >= 0 && all.Count > 0)
            all[UnityEngine.Random.Range(0, all.Count)]
                .SetGlyphType(GlyphType.Blank);

        if (affected.Count > 0)
            vizSteps.Add(new BoardGenVizInfo(gridData, this, affected, false, false, false, false, false));

        return gridData;
    }
}
```
here we can notice that this item has extra data, 
```
Sprite sprite = Core.LoadEmbeddedSprite("CorePatchs.Resources.Vowelizer.png");
        if (sprite != null)
        {
            foreach (var s in SpriteData)
                s.ItemSprite = sprite;
            MelonLogger.Msg("[Core Character Patchs] Vowelizer sprite loaded successfully!");
        }
```
this is a patch required to get custom icons working with the items, custom item sprites will have to be a embedded resource in visual studio

This concludes all the information i can offer on Items at this point, please look at the [Example.cs](https://github.com/Neptune1190/CursedWords-Modding-Documentation/blob/main/Creating/Example.cs) if you wish to know how this actually looks in a mod.


