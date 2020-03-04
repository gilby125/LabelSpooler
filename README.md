LabelSpooler
============

This program polls RAW data from a SQL Server database table and sends data to a printer.
It is useful to send RAW ZPL to Zebra printers, for example.  

Installation
------------

- Grab the latest release file
- Extract the zip to a folder of your choice (for example c:\LabelSpooler\ ) 
- Create the LabelSpooler table in the database with file CreateTables.sql
- Create the file config.ini by copying the config-example.ini
- Install service from an elevated command prompt:
```
c:\LabelSpooler\LabelSpooler.exe --install
```

Uninstallation
--------------

- Uninstall service from an elevated command prompt:

```
c:\LabelSpooler\LabelSpooler.exe --uninstall
```

Need Commercial Support?
------------------------

Send me a mail: r.bicelli@gmail.com