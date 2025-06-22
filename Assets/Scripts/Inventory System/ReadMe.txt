My Inventory Has 2 Type Main Item 
- MonoBehavior Item : Item Instantiate In Scene
- Data Item : Item Only Include Data Not Object

Why Should Have 2 Of Type Item It Because Mix With Save System


Sample 
//Data Not Change In Runtime To Save
        public ItemBaseSO StaticData { get;}
//Data Change In Runtime To Save
        public int Quantity { get; set; }    
        ...Many Variable Here
        
Mono Need Get Ref 2 Of this And Inventory Too

Default Item In Inventory Is Data Item

If You Want GameObject Item Go to Sample Demo Scene

KatTDev