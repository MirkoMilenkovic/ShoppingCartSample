
----Solution overview----

--Library--
Library contains:
- Shopping cart model
- Shopping cart interface and implementation. 
- Intefaces of needed dependencies
- Default implementation of TotalDiscountCalculator, that is probably sufficient for most projects. 
Shopping cart implementation is as "dumb" as possible. It handles it's own state. It delegates calculation of discounts to ITotalDiscountCalculator, 
which, in turn, depends on list of ISingleDiscountCalculator that comes from the Host. 
Therefore, every project can define it's own discounts and way of applying them.

--DiscountPlugin--
Contains implementations of ISingleDiscountCalculator that are specific to this project. Implementations are discovered at runtime by Host (in Host.Program.ConfigureServices),
and passed to ITotalDiscountCalculator.

--Host--
"The project" that is using IShoppingCart.
Contains:
- In memory, object DB where product catalog is defined, and shopping cart saves it's state.
- DAL classes that implement repositories required by shopping cart
- Discount plugin loader
- Output preparation logic, for both console apps and HTML
- Sample runner that tests simmulates user actions. Sample runner has configurable number of threads, to simulate multi user environment.

----How to read the code----
Start from Host.Program.ConfigureServices. All "services" are injected into DI container there.
Take note of namespaces. That will tell you where interfaces and their implemntations are defined, as well as their lifecycle.





