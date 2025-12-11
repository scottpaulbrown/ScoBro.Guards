# ScoBro.Guards

A comprehensive .NET validation and guard library that provides both fluent guard clauses and advanced validation capabilities for .NET applications.

## Overview

ScoBro.Guards offers two main approaches to data validation:

1. **Guard Clauses** - Simple, fluent validation for immediate parameter checking
2. **Advanced Validators** - Comprehensive validation framework with rule builders and async support

## Features

- ✅ **Guard Clauses** - Fluent API for parameter validation
- ✅ **Advanced Validators** - Rule-based validation with builders
- ✅ **Type-Specific Guards** - Specialized guards for strings, integers, decimals, etc.
- ✅ **Async Support** - Asynchronous validation rules
- ✅ **Dependency Injection** - Built-in DI registration extensions
- ✅ **Validation Constraints** - Metadata for UI validation
- ✅ **SimpleResult Integration** - Seamless integration with ScoBro.Foundation

## Installation

```bash
dotnet add package ScoBro.Guards
```

## Quick Start

### Guard Clauses

Guard clauses provide immediate validation with fluent syntax:

```csharp
using ScoBro.Guards;

public class UserService
{
    public void CreateUser(string name, int age, string email)
    {
        // Guard clauses - throws exceptions on validation failure
        Guard.For(name).IsNotNullOrEmpty();
        Guard.For(age).IsGreaterThan(0);
        Guard.For(email).IsNotNullOrEmpty().MatchesRegEx(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        
        // Your business logic here...
    }
}
```

### Advanced Validators

For more complex validation scenarios, use the validator framework:

```csharp
using ScoBro.Guards;

public class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name)
            .IsNotNullOrEmpty()
            .HasMinLength(2)
            .HasMaxLength(50);

        RuleFor(x => x.Email)
            .IsNotNullOrEmpty()
            .IsEmail();

        RuleFor(x => x.Age)
            .IsGreaterThan(0)
            .IsLessThan(120);
    }
}

// Usage
var validator = new UserValidator();
var result = validator.Validate(user);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.FieldName}: {error.ErrorMessage}");
    }
}
```

## Guard Types

### String Guards
```csharp
Guard.For(value)
    .IsNotNullOrEmpty()
    .HasMaxLength(100)
    .MatchesRegEx(@"^[A-Za-z]+$");
```

### Numeric Guards
```csharp
Guard.For(value)
    .IsGreaterThan(0)
    .IsLessThan(100)
    .IsInRange(1, 10);
```

### General Guards
```csharp
Guard.For(value)
    .IsNotNull()
    .IsNotTrue(x => x.SomeCondition, "Custom error message");
```

## Validation Extensions

### String Validation
```csharp
RuleFor(x => x.Property)
    .IsNotNullOrEmpty()
    .HasMinLength(5)
    .HasMaxLength(50)
    .IsEmail()
    .MatchesRegex(@"^[A-Za-z0-9]+$")
    .StartsWith("prefix")
    .EndsWith("suffix");
```

### Numeric Validation
```csharp
RuleFor(x => x.Age)
    .IsGreaterThan(0)
    .IsLessThan(120)
    .IsInRange(18, 65)
    .IsPositive()
    .IsNegative();
```

### Date Validation
```csharp
RuleFor(x => x.BirthDate)
    .IsInPast()
    .IsInFuture()
    .IsAfter(DateTime.MinValue)
    .IsBefore(DateTime.MaxValue);
```

## Dependency Injection

Register validators automatically:

```csharp
services.AddValidatorsFromAssemblyContaining<UserValidator>();
```

## Async Validation

Support for asynchronous validation rules:

```csharp
public class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Email)
            .IsNotNullOrEmpty()
            .IsEmail()
            .MustAsync(async (email, cancellationToken) => 
            {
                // Check if email exists in database
                return !await _userRepository.EmailExistsAsync(email, cancellationToken);
            });
    }
}

// Usage
var result = await validator.ValidateAsync(user);
```

## Inline Validation

For ad-hoc validation without creating a validator class:

```csharp
var result = Validator.Validate(user, builder =>
{
    builder.Rule(x => x.Name).IsNotNullOrEmpty();
    builder.Rule(x => x.Email).IsEmail();
});
```

## Single Field Validation

Validate a single field inline:

```csharp
var result = Validator.Single<User>("Email", rule =>
{
    rule.IsNotNullOrEmpty().IsEmail();
});
```

## Integration with ScoBro.Foundation

Convert validation results to SimpleResult:

```csharp
var result = validator.ValidateToSimpleResult(user);
if (result.IsSuccess)
{
    // Handle success
}
else
{
    // Handle validation errors
    var errors = result.Errors;
}
```

## Error Handling

Validation errors provide detailed information:

```csharp
public class ValidatorError
{
    public string ErrorMessage { get; }
    public string FieldName { get; }
    public string MemberName { get; }
}
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Author

**Scott Brown** - ScoBro Software

## Repository

- GitHub: [https://github.com/scottpaulbrown/ScoBro.Guards](https://github.com/scottpaulbrown/ScoBro.Guards)
- NuGet: [ScoBro.Guards](https://www.nuget.org/packages/ScoBro.Guards/)

## Dependencies

- .NET 8.0
- Microsoft.Extensions.DependencyInjection
- ScoBro.Foundation