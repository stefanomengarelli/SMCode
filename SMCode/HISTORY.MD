SMCode - HISTORY
================

2.0.274 dated 18-06-2025
------------------------
- fix SQLUpdate() functions with bytes array field management;
- implemented Empty() method for GUID;

2.0.271 dated 07-06-2025
------------------------
- fix Set() method in SMDictionary from another instance with types;
- added further On...Path() methods to SMCode class;

2.0.270 dated 04-06-2025
------------------------
- ToClassCode() from dataset improvement with _PrimaryKey method;
- SqlUpdate() method implemented;

2.0.252 dated 14-05-2025
------------------------
- added Type property to dictionary item;
- implemented tables fields customization;
- fixed MimeType() function;
- SMDictionary class improvement to get and set values from objects;
- swapped parameters alias and SQL statement in SqlExec() method;
- reorganized parameters of SqlStoredProcedure() method;
- method SqlQuery<T>() implemented;

2.0.251 dated 25-04-2025
------------------------
- separated error log to console e to debug;
- removed Output() function;

2.0.250 dated 22-04-2025
------------------------
- error management improvement;
- log management improvement;

2.0.247 dated 17-04-2025
------------------------
- console log output optimization;
- SMTemplates case sensitive implementation;
- fixed hexadecimal conversion;
- OpenAtLeast with readonly option;

2.0.241 dated 15-04-2025
------------------------
- set ErrorLog to true by default;
- Output() function optimization;
- error management ThrowException property implemented;
- LogToConsolePrefix property implemented;

2.0.236 dated 13-04-2025
------------------------
- implemented internal error log history;
- user extend table improvement;
- initialization optimization;

2.0.235 dated 10-04-2025
------------------------
- start 2.0.235 release;
- declarations fix;
- new SMCode constructor;

2.0.233 dated 10-04-2025
------------------------
- start 2.0.233 release;
- virtual IniDefaults, IniSettings, WipeTemporaryFiles properties implementation;

2.0.232 dated 07-04-2025
------------------------
- start 2.0.232 release;
- Dataset exclusive database property renamed exclusive connection;
- SMCode properties ClientMode, Demo, InternalPassword, MainAlias, OEM, SessionID and Test declared as virtual;

2.0.231 dated 02-04-2025
------------------------
- start 2.0.231 release;
- FileRetries, FileRetriesDelay and MaxLoadFileSize declared as virtual properties;
- snupkg package creation;
- InitializeCustom() virtual method implemented;
- ConfigFolderName, DataFolderName, LogsFolderName, TempFolderName static properties implementation;

2.0.230 dated 01-04-2025
------------------------
- SMField implementation;
- smcode.js library improvement;
- implemented file retries parameter in ForcePath() method;
- logger virtual properties implementation;
- auto create path implementation;
- Congif defauts for SMIni class;

2.0.220 dated 13-03-2025
------------------------
- start SMMiniMax class implementation (not completed);
- function FileLoad() renamed LoadFile();
- function SaveFile() implementation;

2.0.216 dated 01-03-2025
------------------------
- SMEmail Send() function optimization;
- dataset Each() function implementation;
- dictionary Set() with all values from another dictionary implementation;
- JSON Get() function defaults improvement and Save() function implementation;

2.0.212 dated 20-02-2025
------------------------
- recursive database log call fixed;

2.0.211 dated 20-02-2025
------------------------
- added DatabaseLog property to SMCode class for database open and execute logging;

2.0.210 dated 13-02-2025
------------------------
- added execute-scalar option to Exec() function of SMDatabase and SMDataset;
- SM.Mid() length parameter set to optional;
- added RootPath property to SM class;
- path Merge() function improvement;

2.0.202 dated 01-02-2025
------------------------
- added include separator option to function ExtractArgument();

2.0.201 dated 30-01-2025
------------------------
- fix dataset exclusive database alias assignment, SMCache and SMDictionary improvement;

2.0.200 dated 29-01-2025
------------------------
- SM static instance optimization;

2.0.130 dated 25-01-2025
------------------------
- SMUser and SMOrganizations improvement;

2.0.126 dated 19-01-2025
------------------------
- SMOnLogin delegate declaration switch bool to void;

2.0.125 dated 15-01-2025
------------------------
- SMTemplate path improvements;

2.0.124 dated 08-01-2025
------------------------
- FTP function implementation;
- EMail function implementation;

2.0.123 dated 03-01-2025
------------------------
- SMCache management optimization;

2.0.122 dated 02-01-2025
------------------------
- fix database close improvement;

2.0.121 dated 01-01-2025
------------------------
- database close and timeout improvement;

2.0.120 dated 23-12-2024
------------------------
- string replace ignore-case implementation;
- macros dictionary implementation;

2.0.116 dated 22-12-2024
------------------------
- macro order optimizations (before custom, after system);

2.0.115 dated 21-12-2024
------------------------
- fix template get cache;

2.0.114 dated 18-12-2024
------------------------
- fix mdb database open;

2.0.112 dated 17-12-2024
------------------------
- raise exception improvement;
- JSON get exception management;

2.0.110 dated 15-12-2024
------------------------
- upgrade dotnetzip to prodotnetzip libraries;

2.0.95 dated 05-12-2024
-----------------------
- added keys list as string function in SMDictionary;
- added keys list as string function in SMOrganizations;
- added keys list as string function in SMRules;

2.0.92 dated 03-12-2024
-----------------------
- limited nidificate log errors implementation;
- error write log property implementation;

2.0.91 dated 02-12-2024
-----------------------
- load template fix;

2.0.90 dated 01-12-2024
-----------------------
- add static defaults class;
- default table names added;

2.0.85 dated 30-11-2024
-----------------------
- added TableName property to SMCache class;
- macros new functions implementation;
- templates class management implementation;

2.0.82 dated 27-11-2024
-----------------------
- db cache class implemented;

2.0.72 dated 20-11-2024
-----------------------
- fixed user sex assignment;
- macro quote property implementation;
- SMRules Has() function implementation;

2.0.62 dated 07-11-2024
-----------------------
- optimizations;

2.0.60 dated 31-10-2024
-----------------------
- Gender() and Ending() functions implementation;
- SMJson class serialization and deserialization improvement;
- User text property if empty return firstname + lastname;
- Log information improvement;
- Test/Demo indicator functions;

2.0.56 dated 26-10-2024
-----------------------
- Title() method fix;
- SMJson class implemented for read;

2.0.55 dated 24-10-2024
-----------------------
- restored ToString() method on LogItem;

2.0.54 dated 24-10-2024
-----------------------
- implemented organizations;
- optimized logs, users and rules classes;
- implemented About() function;
- JSON conversion and object tag implementation in users, 
  rules and organizations classes;

2.0.50 dated 05-09-2024
-----------------------
- implemented more log type;

2.0.48 dated 05-09-2024
-----------------------
- net8 e net48 support;
- language initialization implementation;
- user pin implementation;
- database timeout initialization;
- fix dictionary JSON read;
- last log and event on log implementation;

2.0.44 dated 19-08-2024
-----------------------
- implemented recursive search on filelist function;

2.0.43 dated 17-08-2024
-----------------------
- language en and it detection;
- implemented cardinal duplicate history file name detection;

2.0.42 dated 08-08-2024
-----------------------
- Platform property changed to Environment and SMPlatform 
  enumeration changed to SMEnvironment;
- SMCode instance inheriting optimization;
- Application path custom initialization;
- executable date property implementation;

2.0.38 dated 25-07-2024
-----------------------
- users and rules relation implementation;
- login implementation;

2.0.37 dated 18-07-2024
-----------------------
- dictionary class optimization;

2.0.36 dated 18-07-2024
-----------------------
- fix dictionary search cache;

2.0.32 dated 04-07-2024
-----------------------
- GUID and record modification fields on SMDataset;

2.0.30 dated 27-06-2024
-----------------------
- SMUser class implementation;
- SMUsers collection class implementation;
- SMRule class implementation;
- SMRules collection class implementation;
- macros implementations;

2.0.25 dated 03-06-2024
-----------------------
- dataset rows and columns properties implementation;
- application session UID implementation;

2.0.24 dated 25-05-2024
-----------------------
- email component implemented;

2.0.23 dated 24-05-2024
-----------------------
- fix conversion exceptions;

- 2.0.22 dated 23-05-2024
-----------------------
- dictionary exceptions implementation;
- new icons;

2.0.21 dated 16-05-2024
-----------------------
- readme update;
- cache function implementation;
- web function implementation;

2.0.20 dated 16-05-2024
-----------------------
- dictionary constructors improvement;

2.0.19 dated 16-05-2024
-----------------------
- NewDatabaseOnKeep database mode implementation;
- Googlemaps functions;
- Cat() method for integer array implementation;

2.0.16 dated 02-05-2024
-----------------------
- node item implementation;

2.0.15 dated 25-04-2024
-----------------------
- simulated annealing implementation;

2.0.14 dated 20-04-2024
-----------------------
- geo point class implementation;

2.0.12 dated 18-04-2024
-----------------------
- remove javascript library folder, github SMCodeJS project created;

2.0.11 dated 18-04-2024
-----------------------
- restore javascript library folder;
- fix toHtml() method;

2.0.10 dated 18-04-2024
-----------------------
- renamed javascript library folder to smcode;

2.0.9 dated 18-04-2024
----------------------
- added javascript library;
- fix javascript mid() and toVal() functions;

2.0.8 dated 17-04-2024
----------------------
- fix dictionary for from arguments function;

2.0.7 dated 16-04-2024
----------------------
- package documentation included;
- guid data type implementation;
- automatic system date format improvement;

2.0.6 dated 13-04-2024
----------------------
- error functions improvement;
- format functions improvement;
- database functions improvement;

2.0.5 dated 11-04-2024
----------------------
- access wdb implementation;

2.0.3 dated 06-04-2024
----------------------
- multi target platform;

2.0.2 dated 25-03-2024
----------------------
- dictionary improvement;

2.0.1 dated 24-03-2024
----------------------
- minor fixes;

2.0.0 dated 23-03-2024
----------------------
- start of code library development;


