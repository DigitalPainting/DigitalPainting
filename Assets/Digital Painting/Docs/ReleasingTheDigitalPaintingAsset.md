# Releasing The Digital Painting Asset

This document describes the release process for The Digital Painting asset.

## Building the Main Package

  1. `Digital Painting -> Build -> Build [Package Name] Package` - builds the package selected in the menu. Note that only options for currently imported or open plugins will appear

## Validating the Package

We should never release a package without verifying that it works. These steps are the minimum required to validate.

  1. Open a new copy of Unity
  2. Create a new project (called something that will remind you to delete it, e.g. "Validation_DELETE_ME")
  3. Install the Cinemachine package (Windows -> Package Manager -> All -> Cinemachine -> Install)
  3. Import The Digital Painting asset bundle
  4. Check there were no errors during import
  4. Play the Demo scene
  7. Ensure there are no errors during the run
 

