mkdir .\bin
mkdir .\bin\debug
del .\bin\debug\*.* /Q
csc /target:library /out:./bin\debug\SVGLib.dll /warn:0 /nologo /debug *.cs
mkdir .\bin\release
del .\relase\*.* /Q
csc /target:library /out:.\bin\release\SVGLib.dll /warn:0 /nologo /optimize *.cs
