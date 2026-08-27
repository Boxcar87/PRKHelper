PRKHelper has a lot of moving parts and is not fully tested out. If you run into issues please make a note of them here. The way the bot will "break" is that it will regurgitate your previous script output instead of generating a new one.

<img width="481" height="385" alt="{865EB111-C2C2-4EA5-9D4E-7A524F9A8D4C}" src="https://github.com/user-attachments/assets/174af59b-26a1-412e-bb8a-abad7f391097" />

You can build the solution yourself or download the zipped executable on the right under [Releases](https://github.com/Boxcar87/PRKHelp/releases/latest)

PRKHelper does not inject into AO, it works by reading logfiles. The premise is that you setup a new chat window which logs vicinity messages and then the program reads commands from vicinity that begin with a !.
The commands are sent via /whisper which has a very low range so conflicting messages between users are highly unlikely. The results themselves are echoed so that only you can see them. 

Some flagship features of PRKHelper include:
- !items (/itemfind)
- !pb (/pb or /pocketboss)
- !symbiant (/symb or /symbiant)
- Extensive weapon DPM calculator (Returns optimal agg def with drag and drop item configuration)
- Persistent shop script generator

Planned features: Symbiant wishlist tracker.

### Setup

Step 1: Create a new chat window in Anarchy online and enable Vicinity messages to it (Shift+C)

Step 2: "Log messages" of that window (right click its name)

Step 3: Type a message in vicinity to generate log file

Step 4: Launch PRKHelper and target your new log file

Step 5: Target your scripts folder

Step 6: Hit confirm

Step 7: Use commands as shown below

## DPM Calculator
Inits and specials are factored into calculation and the output will provide you with optimal agg/def settings for 1/1 as well as DPM metrics for lower agg/def settings.
### /dps (gear||plan||compare)
<img width="438" height="636" alt="{7171747E-5749-4021-87A7-7CFF799ED30E}" src="https://github.com/user-attachments/assets/d86e4ccb-c951-45b7-abb2-9dc1b03ea3f9" />

## Character stats
This is the foundation for a lot of cool features in the pipeline. Through this we can run accurate damage calculations for different loadouts or get more accurate results for other functions.

### /character
<img width="444" height="624" alt="{8ECAB8A5-5BF6-4275-894D-FE721B9EBBCF}" src="https://github.com/user-attachments/assets/45e2c3ed-1fd2-45d5-8094-6ac3904fc679" />

### /character (gear||plan) (mainhand||offhand) (insert item)
<img width="538" height="59" alt="{337B1861-9A70-47B9-9703-B06581DA2293}" src="https://github.com/user-attachments/assets/90b670ab-1c5a-449f-be14-499722eaafc8" />

### /character (gear||plan) ma 123
<img width="303" height="63" alt="{84449783-E333-42F9-A4DE-FF46E723DB42}" src="https://github.com/user-attachments/assets/df4703d6-dd06-45b5-b919-4dfff23fd277" />

### /character stat 123 (currently support stats are "init", "crit", "ar", "dmg", "complit", "burst", "fullauto", "flingshot", "fastattack", "brawl")
<img width="309" height="61" alt="{8F7A2E3B-AF62-4DFC-93F6-B0BE582F0FEB}" src="https://github.com/user-attachments/assets/544c7cdf-69a3-43af-ba35-606f6db6821d" />

### /character class classname (only relevant for dpm calculator if you are going to incorporate martial arts)
<img width="571" height="60" alt="{7B1FDE07-0086-4AFC-8710-F6FC66EB3C88}" src="https://github.com/user-attachments/assets/7fc98529-d699-4370-a797-86868df797ea" />


## Vendor pricing for items
Drag and drop item into chat and get reasonably close pricing. Some items have unknown special price reduction mechanics. Be sure to update your characters complit first (/character complit 123)
### /vendor (drop item)
<img width="804" height="60" alt="{466566B0-14A9-4CB5-B1A0-C2A17CC36651}" src="https://github.com/user-attachments/assets/496aa096-e22b-4b65-a2e2-86989e5b09a9" />


## Shop script generator
You can drag and drop items into your chat bar and create a persistent shop script. Limited to 15 items due to AO/PRK script size limitation. 
### /editshop add (drop item)
<img width="535" height="418" alt="{A391CC16-F704-46E4-ABCF-52A7CFE58C3D}" src="https://github.com/user-attachments/assets/e9d5b24f-a726-466d-b822-458c2317b473" />

### /editshop text (limited to 8 words, AO scripting limitation. You can manually edit your settings file in AppData/Local/PRKHelp to have a longer message)
<img width="387" height="49" alt="{24D59B5C-7FD8-4C34-A1FB-F6B59E669655}" src="https://github.com/user-attachments/assets/1617ba11-c2d7-4528-876b-bc83956daee1" />

### /postshop
<img width="537" height="523" alt="{9650F73B-BFE8-489A-A9D6-2122E8C54E46}" src="https://github.com/user-attachments/assets/64887ccb-b029-4fbc-9b21-d0ec0bdede62" />


## General bot commands
/items and /item are reserved commands it seems, sorry. Went with /itemfind as a hopefully acceptable substitute. QL is optional.
/pb can take a pattern piece as input for search

### /symbiant and /pb 
<img width="456" height="604" alt="{01A62B65-CAB8-4B4A-8281-CF790095CE4A}" src="https://github.com/user-attachments/assets/e240b0bd-18f7-4131-be14-ace05bd878b7" />

### /trickle stat amount stat amount
<img width="481" height="633" alt="{948AA090-3D5B-44D0-8276-BAAD71267098}" src="https://github.com/user-attachments/assets/42b4d6a7-c8df-4762-98f8-9dd2a3f06a1e" />

### /itemfind (ql) item name
<img width="451" height="686" alt="{B12EEEAD-DC00-497A-A843-ECCFE311ABDF}" src="https://github.com/user-attachments/assets/30976286-7571-46ff-8a09-a76faceb6434" />

### /calc formula+no*spaces
<img width="199" height="54" alt="{B0E3934B-530F-4BAE-ABFC-6473C3218D79}" src="https://github.com/user-attachments/assets/4e02baae-c48b-4f86-8f5e-7117cae2d381" />

### /oe amount
<img width="449" height="368" alt="{35E75D43-842B-4CDE-8B53-8CC90F082D61}" src="https://github.com/user-attachments/assets/78d8023b-d9dc-4cf7-a45d-fc3d6d320089" />

### /mafist amount
<img width="450" height="367" alt="{B474A5BC-3DB7-4CC7-A3A8-2A2505B09CB8}" src="https://github.com/user-attachments/assets/d7023059-d7d9-42b5-9fad-1a5daafedd6c" />

### /timer Name 1h78m1045s
<img width="261" height="88" alt="{11E62725-777F-4A7E-970D-662D37BD6B61}" src="https://github.com/user-attachments/assets/56dea518-e072-49d0-9026-60023d613eda" />

### /timers
<img width="452" height="403" alt="{0CA619F7-3C77-4A19-9C04-B8F686BE95EF}" src="https://github.com/user-attachments/assets/6c900886-f427-4d05-9eb1-88862db7861f" />

### /level level
<img width="415" height="544" alt="{CA949F60-2E43-4B02-9899-DCC5DE5E3EC4}" src="https://github.com/user-attachments/assets/5aec9251-d608-40f3-bb2f-330b1ee37122" />

### /mission level
<img width="608" height="62" alt="{BD17DD3F-4E8A-4C61-AA27-08E31BF9AB77}" src="https://github.com/user-attachments/assets/2f639bc5-7894-411f-98da-493f0aee97fe" />

### /stats
<img width="540" height="680" alt="{3A6769AE-D955-45EA-A2A1-B418935715FC}" src="https://github.com/user-attachments/assets/b5225b3e-9f02-4d91-9b95-fe2f21c44669" />

