C:\Users\User\source\repos\ArchivHefte\TXT2XML\bin\Release\TXT2XML AH_NS.txt
C:\Users\User\source\repos\ArchivHefte\TXT2XML\bin\Release\TXT2XML AH_AeS.txt

C:\Users\User\source\repos\ArchivHefte\XML2LaTeX\bin\Release\XML2LaTeX AH_NS.xml
C:\Users\User\source\repos\ArchivHefte\XML2LaTeX\bin\Release\XML2LaTeX AH_AeS.xml

DEL /Q AH_NS.xml
DEL /Q AH_AeS.xml

MOVE /Y AH_NS.tex \TeX
MOVE /Y AH_AeS.tex \TeX

PAUSE conversion finished
