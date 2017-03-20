# RevitClojureRepl
ClojureClr REPL inside Revit

# Disclamer

This is my hobby project, don't expect Emacs Cider nREPL combo to work just now. 
nREPL for CLR is not even ported yet... https://github.com/clojure/clr.tools.nrepl

# Installers

Im not there yet.

TODO: 
Make an installer that also moves all the clojure dependencies to the root dir of host application

## Supported host Environments 

Autodesk Revit 2016 
Autodesk Navisworks 2017

TODO: Autodesk Revit 2017

## Console window

Simple modless (does not block user user interface) console window . Runs REPL on a separate thread.
Write code, Get results.

Basicly the same CLR Repl you get when you call 

	CMD> Clojure.Main.exe

All code is evaluated outside Revit Transaction. 

Repl has sync provider, that takes a function with one argument as current Active Document and returns a promise.

	(def p (. sync Run (fn [document] document )))
	#object[core$promise$reify__22531__22535 0x1580df5 {:status :pending, :val nil}]

	@p   ;; will block your repl till your fn is executed, if your code breaks
	#object[core$promise$reify__22531__22535 0x34f9981 {:status :ready, :val #object[Document 0x1d8e2c9 "Document (Untitled)"]}]

	(deref p 1000 "still working")   ; wait for 1s if no result, return message.
	"still working"

will return a promise, once the fn was executed. promise will be delivered with the return value of your fn

In Revit, sync works based on Idling Event.
In Navisworks, you dont need sync, but its there, runs on the same thread.

once your func is executed, promise is delivered and you can access the value.
more on Clojure promises  https://clojuredocs.org/clojure.core/promise


Executing conurrent read's or especialy wrtites will crash your Revit Instance with AccessViolation Exception. 
Revit API is not ment for multithreaded access.

limitations:
When defining paths, use the regular forward slash "/" as oposed to default backslash "\" used in windows paths.


Future, I may want to steal Console UI from https://github.com/architecture-building-systems/revitpythonshell/tree/master/PythonConsoleControl


## Socket REPL 

Using the console provided, you can start Socket with a wrapper function 

	(start-server 5555) 

This will start a Socket REPL on port 5555.

You can connect to it using telnet or PuTTY clients

	CMD >telnet localhost 5555

In telnet terminal backspace doesn't work as you would expect, so editing/fixing your command is not-a-feature

PuTTY works much better. 
Download PuTTY here http://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html

localhost = localhost 
port = 5555  // or what ever you specified when you started the server
Connection type -> RAW 

Its also a good idea to turn on "Implicit CR in every LF",
it makes (doc <sym>) output look nice;

Backspace in PuTTY works perfectly, as oposed to telnet terminal.

Only drawback, arrow keys don't work, so good luck deleting half your expression when fixing a typo.


!!! WARNING !!! Socket REPL does not provide revit transaction context.

TODO: Add a macro that will force all reads and writes to queue and run on Main Revit thread inside transaction.

Navisworks API is thread safe, so no extra precautions are needed, nock your self out with (thread) and pmap....

## Clojure wrapper around Revit API

Its a thin layer around interop calls like

(. Document GetElement <element id>) -> (get-element doc id)

Its in early alpha, so dont expect much


## Emacs 

If youre that much of a geek that you want emacs... 
Here are the hoops to jump over.

### Cider package

Cider works with nRepl only. Clr version of that thing is on David Millers shelf. https://github.com/clojure/clr.tools.nrepl


* Fork it
* finish his work porting to Clr host, 
* compile it
* upload to NUGET repo
* send me a link 
* I'll buy you a beer

### Inf-clojure package

This is a much simpler package, that supports Socket REPL connection.
Great, all you need is 
* start a Socket server
* start Emacs
* Telnet to localhost [some port] 
* Be enraged by the fact that emacs and telnet ar not friends.

* download putty.exe and his console friend plink.exe and pray they work together.
* and... 
* well you're on your own.

Let me know, if you've found a solution.

## Documentation about host environments

Revit		http://usa.autodesk.com/adsk/servlet/index?siteID=123112&id=2484975
Navisworks	http://usa.autodesk.com/adsk/servlet/index?id=15024694&siteID=123112



## Referemces

Clojure main site 
http://www.clojure.org/

Clojure docs
https://clojuredocs.org

Clojure Clr repo - if you feel the need to see under the skirt
https://github.com/clojure/clojure-clr/

Clojure Clr wiki
https://github.com/clojure/clojure-clr/wiki


## ClojureClr interop

If you're looking at these, you're probably doing sth worty of adding the the wrapper.clj. 

Clojure Interop basics https://github.com/clojure/clojure-clr/wiki/Basic-CLR-interop

Advanced Interop 
https://github.com/clojure/clojure-clr/wiki/Specifying-types  
https://github.com/clojure/clojure-clr/wiki/Defining-types



Learning clojure. If you dont know where to start. 



TODO: add links to Clojure, Documentation, CLR Interop, repositories, ClojureTV talks.

# Gothas

When loading ClojureClr as a dependency in a dll it expect its files to be in the root of host executable, i.e path of Revit.exe

I've filed a bug in http://dev.clojure.org/jira/browse/CLJCLR-88. In the meanwhile to avoid TypeInitializationException in RT.ctor just copy the ClojureClr dlls next to Revit.exe


# Contributors

At the moment there is only one


## Licence

Copyright © 2017 Martynas Drobulis
Distributed under the Eclipse Public License version 2.0
