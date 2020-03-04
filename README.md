LabelSpooler
============

This program polls RAW data from a SQL Server database table and sends data to a printer.
It is useful to send RAW ZPL to Zebra printers, for example.  

Requirements
------------

- .NET Framework 4.0
- Working SQL Server Database

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

If you need commercial support you can send a mail to: r.bicelli@gmail.com.