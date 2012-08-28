Run this from the Visual Studio Package Manager Console:

Install-Package Simple.Validation

Simple.Validation is composed of 3 basic parts. [[Validator]] is a static class containing the Validate() method which will give you [validation results](ValidationResult) for your object and the specified rules sets. Rules set names are arbitrary for your application. [[Validator]] will reach into the configured [ValidatorProvider](DefaultValidatorProvider) and retrieve validators applicable to your object. It will then filter for the list of [validators](IValidator) that AppliesTo one of the requested rules sets.

You _can_ write your own ValidatorProvider. You _must_ write your own [implementations of IValidator](IValidator).

You can call Validator.Validate(yourObject) to get a list of validation results. Of course, this won't yet do anything. You need to first [create an implementation of IValidator&lt;T&gt;](IValidator) and [register it](DefaultValidator).

There are a variety of [Built-in validators](Validators) to help you get started.