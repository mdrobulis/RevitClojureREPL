
(in-ns 'user)

(use 'clojure.reflect 'clojure.pprint 'clojure.repl)


(defn get-doc []
	(. Autodesk.Navisworks.Api.Application ActiveDocument)
)


(defn get-selected [] (. (. (get-doc) CurrentSelection ) SelectedItems))


(defn get-items [] (map (fn [x] x) (. (. (first (. (get-doc) get_Models)) get_RootItem) get_Descendants)))



;(count (distinct (mapv #(. % get_InstanceGuid )(get-items))))

(defn get-tabs [model-item]
  (. model-item get-PropertyCategories ) 
  )

(defn get-props [item]
  (->> item
       get-tabs
     ;  (map (fn [x] x) )
       (mapcat #(. % get_Properties) )
;      
      )
  )

(use 'clojure.pprint )


(->> (mapcat get-props (get-selected))
     (map #(hash-map :name (. % get_DisplayName) :value (. % get_Value  )  :type (-> % (. get_Value) (. DataType) ))  )
     print-table      
     )






(clojure.pprint/pprint (. (first (. (first (get-selected)) get_PropertyCategories)) get_Properties))


 (defn reflect-type [t]
  (pprint (sort (map #(:name %) (:members (reflect t))))
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

;(start-server 5555)

