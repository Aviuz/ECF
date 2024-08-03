# ECF library
This is base library of ECF project. It contains base classes for wrapping whole program in command / parser / resolver model.

Projected stated with AutoFac library, but it was replaced with native microsoft dependency injection. This led to seperation of project into two branches: ECF.Microsoft.DependencyInjection and ECF.AutoFac.

# Side notes
Due to ease of use some conventions were broken:
1. ECF library is actually `EasyConsoleFramework.Base` (some naming issues and I wanted to reserve `ECF` as a default choice for developers to use inisde their projects).		
1. Some classes have got different namespaces than their folder structure. Those exceptions are made specifically for developers to use library more easliy, but without enforcing concrete folder structure.

