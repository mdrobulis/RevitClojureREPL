# RevitClojureRepl
ClojureClr REPL inside Revit

## Disclamer

This is my hobby project, don't expect Emacs Cider nREPL combo to work just now. 
nREPL for CLR is not even ported yet... https://github.com/clojure/clr.tools.nrepl

This project is very fragile, lots of moving parts will be hard to align and lots of limitations will apply.

## Goal

Create ©Autodesk ©Revit plugin with a WPF GUI for interpreting Clojure S-Expressions

Native Socket Repl should be possible




## Tools 

Visual Studio
ClojureClr out of the nuget package.

## Gothas

When loading ClojureClr as a dependency in a dll it expect its files to be in the root of host executable, i.e path of Revit.exe

I've filed a bug in http://dev.clojure.org/jira/browse/CLJCLR-88. In the meanwhile to avoid TypeInitializationException in RT.ctor just copy the ClojureClr dlls next to Revit.exe


## Licence

Copyright © 2017 Martynas Drobulis
Distributed under the Eclipse Public License version 2.0
