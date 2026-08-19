PRKHelper has a lot of moving parts and is not fully tested out. If you run into issues please make a note of them here. The way the bot will "break" is that it will regurgitate your previous script output instead of generating a new one.

<img width="481" height="385" alt="{865EB111-C2C2-4EA5-9D4E-7A524F9A8D4C}" src="https://github.com/user-attachments/assets/174af59b-26a1-412e-bb8a-abad7f391097" />

You can build the solution yourself or download the zipped executable on the right under [Releases](https://github.com/Boxcar87/PRKHelp/releases/latest)

PRKHelper does not inject into AO, it works by reading logfiles. The premise is that you setup a new chat window which logs vicinity messages and then the program reads commands from vicinity that begin with a !.
The commands are sent via /whisper which has a very low range so conflicting messages between users are highly unlikely. The results themselves are echoed so that only you can see them. 

The flagship feature of PRKHelper is being able to post items and PB loot tables directly to chat. Items, symbiants, and pb commands are all currently supported.

Planned features: Symbiant wishlist tracker, "Shop" advertisement script generator.

Step 1: Create a new chat window in Anarchy online and enable Vicinity messages to it (Shift+C)

Step 2: "Log messages" of that window (right click its name)

Step 3: Type a message in vicinity to generate log file

Step 4: Launch PRKHelp and target your new log file

Step 5: Target your scripts folder

Step 6: Hit confirm

Step 7: Use commands as shown below


Timer will not let you know when it expires, sorry :)

You will have to select a new log file for each new character every time you open PRKHelp

Path settings will persist otherwise

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
