## ReadMe for romano branch

This is just a short explanation of some choices I have made in this challenge.


### Repositories

I've decided for repository pattern, suits well with this challenge and makes unit testing easier.
Because of only two repositories in solution implementation of unit of work would be overkill.

BaseRepository is a bit dry, but it handles common functionalities (IDisposable);


### Services

They are small, with base functionality extracted to base class.
For APIs with methods who can have more than one outcome I prefer to use some kind of Result object to contain value, outcome, user messages and similar.
I have decided to make simple Result class, because I don't like most available as nuget package.


### Authentication

Authentication and authorization should be implemented by all public API, even though this is a simple challenge I have approached it as if it would end on production.
Basic authentication is used because it simple, standard and I believe I have saved me some time.


### Unit tests

I like working with Moq, it really simplifies many tasks.
I prefer XUnit over other testing frameworks.
Code coverage isn't complete because I was in a hurry, and tests available are good example of dev practices.
 

### AutoMapper

I have didn't want to use AutoMapper via dependency injection on purpose, for scenarios with many profiles and mapping in many classes that would be logical choice,
but for this small project every nuget is potentially cause for bloating.


### Swagger

Swagger is great for testing and debugging so I have included it. Personally for simpler APIs I prefer it over Postaman.