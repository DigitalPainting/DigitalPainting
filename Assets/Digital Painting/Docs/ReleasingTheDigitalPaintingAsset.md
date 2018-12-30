# Releasing The Digital Painting Asset

We don't currently do formal releases, but we are preparing to do so. This document describes the process as it is currently defined as we refine it.

## Building the Package

When you have configured your developer machine to work with The Digital Painting asset you will have imported a number of assets we do not have the authority to redistribute. It is therefore very important to ensure the package does not include any of these resources. Following these steps, precisely should produce a releasable asset package.

  1. Open the Demo scene 
  2. Check the `Renderer` for the `Drone` is disabled
  3. Check the ClearShot camera does not have debug turned on
  2. Right Click on the "Demo" scene and select `Export Package...`
  3. Verify that there are no assets to be packaged that are not in the DigitalPaintings folder
  4. Click Export and select your preferred save location

## Validating the Package

We should never release a package without verifying that it works. These steps are the minimum required to validate.

  1. Open a new copy of Unity
  2. Create a new project (called something that will remind you to delete it, e.g. "Validation_DELETE_ME")
  3. Install the Cinemachine package (Windows -> Package Manager -> All -> Cinemachine -> Install)
  3. Import The Digital Painting asset bundle
  4. Check there were no errors during import
  4. Play the Demo scene
  7. Ensure there are no errors during the run
 


