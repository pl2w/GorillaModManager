# Gorilla Mod Manager

![image](https://github.com/pl2w/GorillaModManager/assets/137610832/91871e46-49da-4380-8050-49725d8e498c)

# How to make your mod compatible with Gorilla Mod Manager 
## Upload it to GameBanana
1. Make sure your mod is correctly zipped like this. \
   Mod.Zip \
     -> Mod.dll
2. Make sure the dependencies are correctly listed in gamebanana. \
   It must have a valid zip file link as an url, a github latest download link or a gamebanana mod link \
     Latest: "https://github.com/legoandmars/Utilla/releases/latest" \
     Zip: "https://github.com/legoandmars/Utilla/releases/download/v1.6.13/Utilla.zip" \
     GameBanana: "https://gamebanana.com/mods/507053"
   
Failure to comply with those two rules will result in your GameBanana profile/mod being blacklisted from Gorilla Mod Manager.

# Building
1. Clone GorillaModManager
### If GameBananaAPI.dll does not exist next to the .sln file.
2. Compile GameBananaAPI and move the dll next to the '.sln' file.
![image](https://github.com/pl2w/GorillaModManager/assets/137610832/8c79f2a3-8ac0-492d-a822-9720c0ef09b8)

4. Run 'build-release.bat' next to the '.csproj' file. (may take a couple of attempts for some reason.)
