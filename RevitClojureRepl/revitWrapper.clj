; document => ExternalCommandData
;


(import '(Autodesk.Revit.DB Element ElementId Wall Area  ))
(use 'clojure.repl 'clojure.pprint 'clojure.reflect )


(def App RevitClojureRepl.GuiReplPlugin/app)
(def CommandData RevitClojureRepl.GuiCommand/CommandData)


(def ui-doc 
	(-> CommandData
		(. Application)
		(. ActiveUIDocument)
	)
)

(def document (. ui-doc Document)  ) 


(defn get-element [doc id]
"gets Element provided ElementId or guid"
(. doc GetElement id)
)

(defn UniqueId 
"gets the UniqueId of the element"
 [element] (. element get_UniqueId) )



(defn get-selected-elements
"returns selected element vector"
[uidoc]
  (let
      [doc (. uidoc Document)
       selection (. uidoc Selection)
        ids (. selection GetElementIds)]
    ids
))

(defn get-elements-of-type 
"returns a lazy-seq of elements if some type"
[doc type]
	(let [ collector (new Autodesk.Revit.DB.FilteredElementCollector doc)
		   filter (. collector OfClass type)
	]
	(map (fn [x] x) collector )
	)
)



(defn get-element-ids-of-type
"returns a list of ElementId's "
 [doc type]
	(let [ collector (new Autodesk.Revit.DB.FilteredElementCollector doc)
		   filter (. collector OfClass type)
	]
	(. collector ToElementIds)
	)
)

;(def params (mapv  #(get-parameters (get-element document %)) (get-selected-elements ui-doc)))



(defn get-filtered-elements [doc filter] 
(let [collector (doto (new Autodesk.Revit.DB.FilteredElementCollector doc) 
					(. WherePasses filter))]
	(map (fn [x] x) collector ))
)
(defn get-filtered-element-ids [doc filter] 
(let [collector (doto (new Autodesk.Revit.DB.FilteredElementCollector doc) 
					(. WherePasses filter))]
	(map (fn [x] x) collector ))
)


(def ElementIdColl |System.Collections.Generic.ICollection`1[Autodesk.Revit.DB.ElementId]|)

(defn valid-elementid-coll [coll]
	(try (do (cast ElementIdColl coll)
		true)
		(catch InvalidCastException ex false)
	)
)






(def ElementIdList |System.Collections.Generic.List`1[Autodesk.Revit.DB.ElementId]|)

(defn select! 
  "selects elements in the document"
  [uidoc elements]
  (if (valid-elementid-coll elements) 
	(-> uidoc
      (. Selection)
      (. SetElementIds elements )
    )
	
	(let [array (into-array ElementId elements)]
		  (-> uidoc
      (. Selection)
      (. SetElementIds array )))))
 

(defn get-parameters [element] 
	(let [set (. element get_Parameters)]
	(into [] set)
	)
 )

 (defn reflect-type [t]
  (pprint (sort (map #(:name %) (:members (clojure.reflect/reflect t))))
 )
 )

 (defn reflect-type-str [t]
 (with-out-str (pprint (sort (map #(:name %) (:members (clojure.reflect/reflect t))))
 )
 ))



 (defn all-filter [] 
  (let [type (new Autodesk.Revit.DB.ElementIsElementTypeFilter true ) 
		inst (new Autodesk.Revit.DB.ElementIsElementTypeFilter false )
		logic (new Autodesk.Revit.DB.LogicalOrFilter type inst )
  ] logic)
  )

  (defn class-filter [t] (new Autodesk.Revit.DB.ElementClassFilter t) )
  
  (defn category-filter 
  " Get category filter based on enum or ElementId of the category "
  ([c] (new Autodesk.Revit.DB.ElementCategoryFilter c))
  ([c invert] (new Autodesk.Revit.DB.ElementCategoryFilter c invert) )
  )

  (defn list-category-ids [] 
   "Lists all category Ids"
   (. System.Enum GetValues Autodesk.Revit.DB.BuiltInCategory)
   )

   (defn list-category-names [] 
   "lists all category names"
   (. System.Enum GetNames Autodesk.Revit.DB.BuiltInCategory)
   )


   (defn all-elements [doc]
	(get-filtered-elements doc (all-filter))
   )


(defn start-server [port] 
	(let [config {:port port :name "repl" :accept 'clojure.core.server/repl } ]
	 (clojure.core.server/start-server config)
	)
)