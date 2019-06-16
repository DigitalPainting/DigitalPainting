---
theme : "white"
transition: "zoom"
highlightTheme: "darkula"
logoImg: "https://raw.githubusercontent.com/evilz/vscode-reveal/master/images/logo-v2.png"
slideNumber: true
---

# Create a new Digital Painting Plugin

---

## Digital Painting

An asset to make Unity more accessible

An asset for rapid prototyping of scenes

---

### A Digital Painting plugin

An additive feature to the core Digital Painting asset

An easy way to integrate third party assets

---

### Learn how to create a plugin

This plugin will be new code, not dependent on a third party asset.

---

## The Plugin

A simple terrain generator

  1. Design time generation
  2. Realtime generation

---

## The Goal

Users can generate a terrain automatically,

so that they can quickly prototype game features.

---

## Demo

Terrain generation code

note:

Lets take a quick look at the procedural code that will generate our terrain.

Switch to [GitHub Project Page](https://github.com/DigitalPainting/Plugin_Terrain_ProceduralTerrain)

"You can get the code from GitHub..."

"This is based on work by Sebastian Lague's excellent [tutorial on YouTube](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3)

Go To Unity

"We are in the procedural landmass generation project..."

Open the "Terrain Generation Demo" scene

Select the Terrain Preview and center the scene view on it.

"Here a preview of the terrain I generated last time I was here."

Double click the `Height Map Settings` and play around - it may be necessary to click `Generate Terrain` in the `Terrain Preview` inspector.

"This is actually a mesh preview of the terrain, the texturing is generated to give us a feel for what it will look like, but won't be part of the final terrain."

Zoom in and take a look around the terrain.

Click `Create Terrain`

Select the Terrain in the Hierarchy and hit f to zoom the scene view on it.

"That's it, but how do we make this available in The Digital Painting asset?"

END

---

## Create The Plugin Project

Convention is to name the project: `Plugin_CATEGORY_DESCRIPTIVENAME`

Our plugin is therefore `Plugin_Terrain_ProceduralTerrain`

note:

We already did this so we need to do a Git clone and a bunch of submodules

cd Assets

git submodule add git@github.com:DigitalPainting/DigitalPainting.git

git submodule add git@github.com:DigitalPainting/Flying-Pathfinding.git

git submodule add git@github.com:rgardler/ScriptableObject-Architecture.git

Open project in Unity

Install Cinemachine

Install Post Processing 2

--

## Prepare the Project
Step 1 of 8

```bash
cd Assets
```

--

## Prepare the Project
Step 2 of 8

```bash
git submodule add git@github.com:DigitalPainting/DigitalPainting.git
```

--

## Prepare the Project
Step 3 of 8

```bash
git submodule add git@github.com:DigitalPainting/Flying-Pathfinding.git
```

--

## Prepare the Project
Step 4 of 8

```bash
git submodule add git@github.com:rgardler/ScriptableObject-Architecture.git
```

--

## Prepare the Project
Step 5 of 8

Open project in Unity

--

## Prepare the Project
Step 6 of 8

Install Cinemachine

--

## Prepare the Project
Step 7 of 8

Install Post Processing 2

--

## Prepare the Project
Step 8 of 8

Create and open a new Scene called "Digitial Painting"

---

## Plugin Manager

Provides integration code between the Digital Painting the plugin code.

Digital Painting provides base class called `CATEGORY_PluginManager`

Often this is all that is needed.

---

## Plugin Profile

Scriptable Object for configuration of the Plugin. 

Users set values here when adopting the plugin.

---

Extends `AbstractPluginProfile`

Naming Convention: `CATEGORY_DESCRIPTIVENAME_Profile`

---

### Create from Asset Menu

```cs
[CreateAssetMenu(
    fileName = "CATEGORY_DESCRIPTIVENAME_Profile",
    menuName = "Wizards Code/CATEGORY/DESCRIPTIVENAME")]
```

---

```cs
using UnityEngine;

namespace wizardscode.plugin.terrain
{
    [CreateAssetMenu(
        fileName = "Terrain_ProceduralTerrain_Profile", 
        menuName = "Wizards Code/Terrain/Procedural Generation")]
    public class ProceduralTerrain_Terrain_Profile 
        : AbstractPluginProfile
    {
    }
}

```

---

Later we will adding some settings parameters.

For now, an empty profile is enough.

---

## Plugin Definition

Script describing the plugin to the Digital Painting core system.

---

Extends `AbstractPluginDefinition`

Naming Convention: `CATEGORY_DESCRIPTIVENAME_PluginDefinition`

---

```cs
namespace wizardscode.plugin.terrain
{
    public class Terrain_ProceduralTerrain_PluginDefinition
        : AbstractPluginDefinition
    {

      // Implement the abstract methods

    }
}
```

--

```cs
public override PluginCategory GetCategory()
{
    return PluginCategory.Terrain;
}
```

note:

This is primarily used to categorize plugins in the UI.

--

```cs
public override Type GetManagerType()
{
    return typeof(Terrain_PluginManager);
}
```

note:

Digital Painting core provides implementations for each of the supported types of plugin. Often this is sufficient.

If not then you should create a new class in your plugin that extends the base class provided by Digital Painting.

--

```cs
public override string GetPluginImplementationClassName()
{
    return "wizardscode.terrain.TerrainGenerator";
}
```

note:

This is used to check whether dependencies for the plugin are available.

If this class can be found then the UI for Digital Painting will allow the plugin to be enabled.

--

```cs
public override string GetProfileTypeName()
{
    return typeof(ProceduralTerrain_Terrain_Profile).ToString();
}
```

note:

This is used to ensure that a valid profile is provided in the UI.

It is also used to decide how to validate the plugins configuraion.

--

```cs
public override string GetReadableName()
{
    return "Procedural Terrain Generator";
}
```

note:

This is simply a human friendly name used in the UI.

--

```cs
public override string GetURL()
{
    return "https://github.com/DigitalPainting/Plugin_Terrain_ProceduralTerrain";
}
```

note:

Used in the UI to provide a link to more information about the plugin.

---

## Validation Tests

Validation of the plugin configuration.

Validation of the scene.

---

Extends `ValidationTest<CATEGORY_[DESCRIPTIVENAME_]PluginManager>`

Naming Convention: `CATEGORY_DESCRIPTIVENAME_PluginDefinition`

---

```cs
using wizardscode.plugin;

namespace wizardscode.validation
{
    public class Terrain_ProceduralTerrain_PluginValidation : ValidationTest<Terrain_PluginManager>
    {

      // Implement the abstract methods

    }
}
```

--

```cs
public override ValidationTest<Terrain_PluginManager> Instance => new Terrain_ProceduralTerrain_PluginValidation();
```

--

```cs
internal override string ProfileType => typeof
  (ProceduralTerrain_Terrain_Profile).ToString();
```

---

The base `ValidationTest<T>` class will validation many settings automatically.

In some cases specific validation tests will be implemented in this class.

---

## Digital Painting Configuration

Provide default configuration for this plugin.

Prevent accidental overwriting of settings for other plugins.

---

Create a directory called `Scenes/Digital Painting Data`

---

Copy the contents of `Digital Painting/Data/Default Collection` into the `Scenes/Digital Painting Data` folder

---

Rename all files in here:

`s/_Default/_ProceduralTerrain_Default`

---

Open `DigitalPaintingManagerProfile_ProceduralTerrain_Default` in this folder

---

Update all settings to reflect the SOs in this folder

---

# Testing the Plugin Skeleton

---

Open the Digital Painting Manager Window

`Window -> Wizards Code -> Digital Painting Manager`

Click `Add Digital Painting Manager`

---

Fix the setup errors that occur using the data in the `Scenes/Digital Painting Data` folder

---

Should have an "Enable Procedural Terrain Generator" button.

Click it!

---

New Game Object called "Terrain Plugin Manager" as a child of "Digital Painting Manager"

This does not have a plugin profile yet.

Error reported in Digital Painting Manager Window

---

Create a Profile `Scenes/Digital Painting Data` create a profile

Use `Assets -> Create -> Wizards Code -> Terrain -> Procedural Generation Plugin Profile`

---

Add this to the Terrain Plugin Manager

There is nothing to edit, yet

Error report has gone

---

# Adding the Functionality

We want to ensure a terrain is in the scene

Need a validation check for a valid terrain

---

In `Terrain_ProceduralTerrain_PluginValidation`

Override `CustomValidations()` 

Check for a terrain object in the scene

--

```cs
internal override void CustomValidations() {
    ValidationResult result;
    if (Terrain.activeTerrain)
    {
        result = GetPassResult("Terrain is required.", 
            "There is a terrain in the scene.", 
            this.GetType().Name);
        ResultCollection.AddOrUpdate(result, this.GetType().Name);
        return;
    }
    else
    {
        result = GetErrorResult("Terrain is required.", 
            "There is no terrain in the scene.", 
            this.GetType().Name);
        ResultCollection.AddOrUpdate(result, this.GetType().Name);
        return;
    }
}
```

---

Now we see an error "There is no terrain in the scene"

This is good, now we can automatically fix it.

Create a placeholder method to call in order to fix the error

--

```cs
private void GenerateTerrain()
{
    Debug.Log("Generate a terrain");
}
```

--

Add a callback to the error in `CustomerValidations`

--

```cs
else
{
    result = GetErrorResult("Terrain is required.", 
        "There is no terrain in the scene.", 
        this.GetType().Name);
    ProfileCallback cb = new ProfileCallback(GenerateTerrain);
    result.AddCallback(new ResolutionCallback(cb));
    ResultCollection.AddOrUpdate(result, this.GetType().Name);
    return;
}
```   
---

A button labelled "Generate Terrain" will be provided in the UI

Clicking it will display a message in the console

---

We now need to have the terrain generator in the scene

We will do this by creating a Setting Scriptable Object to  define the Terrain Generator.


---

In the `Scenes/Digital Painting Data` folder create an instance of this Scriptable Object called `TerrainGenerator_PrefabSettingSO`

Create -> Wizards Code -> Validation -> Prefab`

---

Setting name: "Terrain Generator"
Nullable: False
Suggested Value: (drag in `Prefabs/Terrain Preview`)
Add to Scene: False
Spawn Pos: -1500, 0, -1500 

---

Add a field for this Setting to the `ProceduralTerrain_Terrain_Profile`

```cs
[Tooltip("The prefab that provides the necessary components to procedurally generate a terrain.")]
[Expandable(isRequired: true)]
public PrefabSettingSO terrainGenerator;
```

---

In the Inspector for Terrain Plugin Manager you will now have a slot for the Terrain Generato Prefab

Drag you Scriptable Object into it

---

Nearly there...

Need to wire up the terrain generation in the `GenerateTerrain` method:

```cs
private void GenerateTerrain()
{
  ...
}
```

--

First, get the `TerrainGeneratorPreview` Game Object:

```cs

bool destroyPreview = false;
string terrainName = "Generated Terrain";

TerrainGeneratorPreview terrainPreview = 
    GameObject.FindObjectOfType<TerrainGeneratorPreview>();
```

--

If there is no preview object, create one temporarily:

```cs
if (terrainPreview == null)
{
    destroyPreview = true;
    Terrain_PluginManager manager = GameObject.FindObjectOfType<Terrain_PluginManager>();
    Terrain_ProceduralTerrain_Profile profile = (Terrain_ProceduralTerrain_Profile)manager.Profile;
    profile.terrainGenerator.InstantiatePrefab();
    terrainPreview = profile.terrainGenerator.Instance.GetComponent< TerrainGeneratorPreview>();
}
```

--

Generate the terrain:

```cs
terrainPreview.DrawMapInEditor();

TerrainData data = MeshToTerrain.CreateTerrainData(terrainName, 512, Vector3.zero, 0);
GameObject terrainObject = Terrain.CreateTerrainGameObject(data);
terrainObject.name = terrainName;
```

--

If we temporarily created the preview then destroy it:

```cs
if (destroyPreview)
{
    GameObject.DestroyImmediate(terrainPreview.gameObject);
}
```

---

Now you can click "Generate Terrain" and voila - a terrain!

But... it will always look the same, so let's add some customization.

---

Two approaches:

  1. Create different TerrainPreview prefabs with different settings
  2. Create a custom PrefabSettingSO that exposes customizations

---

There are no coding changes needed if you provide custom prefabs.

Each prefab can set all settings in the TerrainGenerator.

No code changes are needed for this approach.

However, newcomers may be overwhelmed by the settings exposed.

---

Providing a custom SettingsSO enables us to expose specific features.

This allows variety to be added, but keeps the learning curve low.

Later users can graduate to using custom prefabs.

---

Create a new folder called `Settings`

Within it create a class `TerrainGenerator_PrefabSettingSO`

This class extends `PrefabSettingSO`

---

We will add an ability to set a custom seed, or generate a random seed.

Add fields for this customization to the `TerrainGenerator_PrefabSettingSO`

Override `InstantiatePrefab` to configure the Terrain Generator.

--

```cs
[Header("Height Map Settings")]
[Tooltip("Generate a random terrain. Note that if you check this then the `heightMapSeed` will be updated with the new seed.")]
public bool randomTerrain = false;

[Tooltip("The seed to use when generating the terrains height map. This allows us to have a predictable terrain. Note that if `Random Terrain` is set to true, this value will be overwritten with the seed generated. If set to 0 then whatever is set in the Terrain Generator prefab will be used.")]
public int heightMapSeed;
```

--

```cs
internal override void InstantiatePrefab()
{
    base.InstantiatePrefab();
    TerrainGeneratorPreview generator = Instance.GetComponent<TerrainGeneratorPreview>();
    if (randomTerrain)
    {
        heightMapSeed = Random.Range(int.MinValue, int.MaxValue);
        generator.heightMapSettings.noiseSettings.seed = heightMapSeed;
    } else
    {
        if (heightMapSeed != 0)
        {
            generator.heightMapSettings.noiseSettings.seed = heightMapSeed;
        }
    }
}
```

---

Change the `terrainGenerator` field in `ProceduralTerrain_Terrain_Profile` to use this new SettingSo.

--

```cs
[Tooltip("The prefab that provides the necessary components to procedurally generate a terrain.")]
[Expandable(isRequired: true)]
public TerrainGenerator_PrefabSettingSO terrainGenerator;
```

---

In the `Scenes/Digital Painting Data` folder 

Delete the old `TerrainGenerator_PrefabSettingSO`.

Create an instance of the new `TerrainGenerator_PrefabSettingSO`

Use `Create -> Wizards Code -> Validation -> Terrain -> Procedural Generation Prefab``

--

Setting name: "Terrain Generator"
Nullable: False
Suggested Value: (drag in `Prefabs/Terrain Preview`)
Add to Scene: False
Spawn Pos: -1500, 0, -1500 

Random Terrain : true

---

Now, each time you can generate the terrain you create a random one.

Delete the terrain to make the `Generate Terrain` button reappear.

---

# End

