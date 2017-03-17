


(use 'clojure.reflect 'clojure.pprint 'clojure.repl)


(defn get-doc []
	(. Autodesk.Navisworks.Api.Application ActiveDocument)
)


(defn get-selected [] (. (. (get-doc) CurrentSelection ) SelectedItems))


;(defn get-items [] (into [] (. (. (first (. (get-doc) get_Models)) get_RootItem) get_Descendants)))



;(count (distinct (mapv #(. % get_InstanceGuid )(get-items))))


 (defn reflect-type [t]
  (pprint (sort (map #(:name %) (:members (clojure.reflect/reflect t))))
 )
 )

 (defn reflect-type-str [t]
 (with-out-str (pprint (sort (map #(:name %) (:members (clojure.reflect/reflect t))))
 )
 )
 )
 

(defn start-server [port] 
	(let [config {:port port :name "repl" :accept 'clojure.core.server/repl } ]
	 (clojure.core.server/start-server config)
	)
)

(start-server 9999)