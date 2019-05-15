The Digital Painting editor is configured via a Scriptable Object defined by `EditorConfigScriptableObject`. One will be created when the Digital Painting Manager window is first opened. This object is versioned to ensure that any updates to Digital Painting are automatic.

You can view which version you are currently using on the `More...` tab of the Digital Painting window.

# Adding a Parameter to the Configuration Object

  1. Bump the value of `LatestVersion` to trigger an update when reloaded
  2. Add a public parameter for the property
  3. Ensure a good default is provided either in the parameter definition line, the `Init()` method or the defauilt configiration object provided in the asset
  4. By default the upgrade behaviour is to simply sopy the old value to the new. To use the default method add `Upgrade(oldConfig, "PROPERTY_NAME")` to the end of the `Upgrade(oldConfig)` method. If a different action is needed add an `UpgradePROPERTY_NAME(oldConfig)` method and implement your upgrade code.
  5. 