# RevitClojureRepl
ClojureClr REPL inside Revit

# Disclamer

This is my hobby project, don't expect Emacs Cider nREPL combo to work just now. 
nREPL for CLR is not even ported yet... https://github.com/clojure/clr.tools.nrepl

This project is very fragile, lots of moving parts will be hard to align and lots of limitations will apply.

# Installers

Im not there yet.

TODO: 
Make an installer that also moves all the clojure dependencies to the root dir of host application

# Current state

## Supported host Environments 

Autodesk Revit 2016 
Autodesk Navisworks 2017

TODO: Autodesk Revit 2017

## Console window

You have a place to write code, an a place to see some of results.

Evaluation is always bound to 'user namespace and you can't change it. 
Console window is modless. All code is evaluated inside Revit Transaction.
Its ment to protect you from crashing your Revit Instance with AccessViolation Exception. 
Revit API is not ment for multithreaded access, so plesse avoid document reads with pmap

limitations

## Socket REPL 

Using the console provided, you can start Socket with a wrapper function 

(start-server 5555) 

This will start a Socket REPL on port 5555.

You can connect to it using telnet

telnet localhost 5555

This repl bu

!!! WARNING !!! Socket REPL does not provide revit transaction context.

TODO: Add a macro that will force all reads and writes to queue and run on Main Revit thread inside transaction.

Navisworks API is thread safe, so no extra precautions are needed, nock your self out with (thread) and pmap....

## Clojure wrapper around Revit API

Its a thin layer around interop calls like

(. Document GetElement <element id>) -> (get-element doc id)

Its in early alpha, so dont expect much



# Referemces

TODO: add links to Clojure, Documentation, CLR Interop, repositories, ClojureTV talks.

# Gothas

When loading ClojureClr as a dependency in a dll it expect its files to be in the root of host executable, i.e path of Revit.exe

I've filed a bug in http://dev.clojure.org/jira/browse/CLJCLR-88. In the meanwhile to avoid TypeInitializationException in RT.ctor just copy the ClojureClr dlls next to Revit.exe


# Contributors

At the moment there is only one


## Licence

Copyright © 2017 Martynas Drobulis
Distributed under the Eclipse Public License version 2.0
