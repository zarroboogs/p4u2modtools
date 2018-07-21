# Persona 4 Arena Ultimax Mod Tools

## What's Included

``` txt
bddata\  - bddata.bin extraction tools
p4u2mod\ - custom game update creation tool
patch\   - patch files for use with rpcs3
```

## Glossary

* `P4U` and `P4U2` - Internal codenames for "Persona 4 Arena" and "Persona 4 Arena Ultimax".
* `BLJM61209` - The Japanese version of P4U2.
* `BLUS31469` - The North American version of P4U2.
* `bddata.bin` - File containing most of the game data files, can be found on disc.
* `UpdateFileList.bin` - File containing data regarding changed game files for game updates.

## General Notes

* These tools were tested on `BLUS31469` and `BLJM61209`, but might work with other versions of P4U2, P4U or other games that support the `bddata.bin` or `UpdateFileList.bin` format.
* `BLUS31469` and `BLJM61209` are save compatible.
* `BLUS31469` patch 1.01 == `BLJM61209` patch 1.03.

## Modding the Game

### Extracting Game Data

Game data can be found in:

``` txt
(1) dev_hdd0\game\<title_id>\USRDIR\update\ver_*\data\*
(2) dev_hdd0\disc\<title_id>\PS3_GAME\USRDIR\data\*
(3) dev_hdd0\disc\<title_id>\PS3_GAME\USRDIR\bddata.bin
```

While (1) and (2) contain raw data files, (3) is a container file that needs to be unpacked to access files stored within.

#### Unpacking Game Data

1. Extract and inflate `*.segs` from `bddata.bin` using [asmodean's `exsegs.exe`](http://asmodean.reverse.net/pages/exah3pac.html) utility:

    ``` powershell
    cd extract\
    exsegs.exe bddata.bin
    ```

2. Extract the hardcoded table of contents from the **original, unpatched** disc version of the **decrypted** eboot file:

    ``` powershell
    cd extract\
    eboot-extract-toc.ps1 boot.elf bddata-toc.txt
    ```

3. Rename the extracted file according to the table of contents:

    ``` powershell
    cd extract\
    bddata-organize-toc.ps1 bddata-toc.txt
    ```

### Custom Game Updates

The game uses an `UpdateFileList.bin` file to manage versioning and to indicate which files have been updated.

Note that the file table is hardcoded in the eboot, so adding files isn't possible without eboot editing or patching, but replacing files is possible.

The format of `UpdateFileList.bin` is described in the `UpdateFileList.bt` template file, compatible with 010 Editor.

The `p4u2mod.exe` tool can generate a custom `UpdateFileList.bin` for user generated patches to the game.

#### Creating an Update

1. Simply put your modified files under a new version folder in the game's update folder, for example: `dev_hdd0\game\<title_id>\USRDIR\update\ver_0104_undub`.

    * Make sure that your folder appears last in the folder list when sorted by name, otherwise files in previous updates might override your new files.
    * Make sure to keep the same directory structure as in the original extracted `bddata.bin`.

2. Increment the `VERSION` variable in the `catalog_env.info` file.

3. Put the `p4u2mod.exe` tool in the game's update folder and run it - it should generate a new `UpdateFileList.bin` that contains your modifications.

### EBOOT Patching

You can also use rpcs3's built in patching system to patch the eboot file.

Put the appropriate `patch.yml` file under `<rpcs3_dir>\data\<title_id>\`.

## Example - `BLUS31469` Undub

1. Extract `bddata.bin` from `BLJM61209` using instructions specified above.
2. Create an `ver_0104_undub` folder under `<rpcs3_vfs>\dev_hdd0\game\BLUS31469\USRDIR\update\`.
3. Copy the extracted Japanese voice files like so:

   ``` powershell
   copy-item `
       <extract_dir>\data\story\voice\ `
       <rpcs3_vfs>\dev_hdd0\disc\PS3_GAME\USRDIR\data\update\ver_0104_undub\data\story\voice_eng\ `
       -force -recurse
   copy-item `
       <extract_dir>\data\sound\voice\ `
       <rpcs3_vfs>\dev_hdd0\disc\PS3_GAME\USRDIR\data\update\ver_0104_undub\data\sound\voice_eng\ `
       -force -recurse
   ```

4. Look for updated voice files under the `BLJM61209` update folder `<rpcs3_vfs>\dev_hdd0\game\BLUS61209\USRDIR\update\`. Copy them as needed, overwriting old files.
5. Backup `UpdateFileList.bin` and `catalog_env.info` under  `<rpcs3_vfs>\dev_hdd0\game\BLUS31469\USRDIR\update\`.
6. Increment the version number in `catalog_env.info` using a text editor.
7. Copy `p4u2mod.exe` to `<rpcs3_vfs>\dev_hdd0\game\BLUS31469\USRDIR\update\` and run the program. This should generate a new `UpdateFileList.bin` file containing your changes.
8. Use the provided `patch.yml` file to enable Japanese cutscenes and movies.
9. Launch the game.
