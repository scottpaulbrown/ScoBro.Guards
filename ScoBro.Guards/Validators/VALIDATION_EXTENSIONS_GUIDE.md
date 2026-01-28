# ScoBro.Guards Validation Extensions - LLM Guide

## Overview

The ScoBro.Guards validation framework provides a fluent API for building type-safe validators with extensive built-in validation rules. The framework supports synchronous and asynchronous validation, custom error messages, and validation chaining.

## Core Concepts

### Validator Creation Patterns

```csharp
// Pattern 1: Inline validator with configuration action
var validator = Validator.For<Person>(v => {
    v.Rule(x => x.Name).IsNotNullOrEmpty().HasMaxLength(100);
    v.Rule(x => x.Age).IsPositive().IsInRange(0, 120);
});

// Pattern 2: Class-based validator (inherit from Validator<T>)
public class PersonValidator : Validator<Person> {
    public PersonValidator() {
        RuleFor(x => x.Name).IsNotNullOrEmpty().HasMaxLength(100);
        RuleFor(x => x.Age).IsPositive().IsInRange(0, 120);
    }
}

// Pattern 3: Single field validation
var result = Validator.Single<string>("Email", v => v.IsNotNullOrEmpty().IsEmail());

// Pattern 4: Immediate validation
var result = Validator.Validate(person, v => {
    v.Rule(x => x.Name).IsNotNullOrEmpty();
});
```

### Validation Execution

```csharp
// Synchronous validation
ValidationResult result = validator.Validate(item);
if (result.IsValid) { /* success */ }
foreach (var error in result.Errors) {
    Console.WriteLine($"{error.FieldName}: {error.ErrorMessage}");
}

// Asynchronous validation (use when any rule is async)
ValidationResult result = await validator.ValidateAsync(item);

// Convert to SimpleResult
SimpleResult<Person> result = validator.ValidateToSimpleResult(person);
SimpleResult<Person> result = await validator.ValidateToSimpleResultAsync(person);
```

### ValidationResult Structure

```csharp
public record class ValidationResult {
    public bool IsValid { get; }
    public IReadOnlyList<ValidatorError> Errors { get; }
}

public record class ValidatorError(string ErrorMessage, string? FieldName = null);
```

## String Validation Extensions

**Target Type:** `ValidationRuleBuilder<T, string>`

| Method | Description | Default Error Message | Optional Support |
|--------|-------------|----------------------|------------------|
| `IsNotNullOrEmpty(stopIfInvalid = true)` | Validates string is not null/empty (trims whitespace) | "{FieldName} cannot be null or empty." | No |
| `IsNotNullOrWhiteSpace(stopIfInvalid = true)` | Validates string is not null/whitespace | "{FieldName} cannot be null or whitespace." | No |
| `HasMaxLength(int maxLength, stopIfInvalid = false)` | Validates max character length | "{FieldName} cannot exceed {maxLength} characters." | Yes |
| `HasMinLength(int minLength, stopIfInvalid = false)` | Validates min character length | "{FieldName} must be at least {minLength} characters." | Yes |
| `MatchesRegex(string pattern, stopIfInvalid = false)` | Validates against regex pattern | "{FieldName} is not in the correct format." | Yes |
| `Contains(string substring, stopIfInvalid = false)` | Must contain substring | "{FieldName} must contain '{substring}'." | Yes |
| `DoesNotContain(string substring, stopIfInvalid = false)` | Must not contain substring | "{FieldName} cannot contain '{substring}'." | N/A |
| `StartsWith(string prefix, stopIfInvalid = false)` | Must start with prefix | "{FieldName} must start with '{prefix}'." | Yes |
| `EndsWith(string suffix, stopIfInvalid = false)` | Must end with suffix | "{FieldName} must end with '{suffix}'." | Yes |
| `IsEmail(stopIfInvalid = false)` | Validates email format | "{FieldName} must be a valid email address." | Yes |
| `IsGuid(stopIfInvalid = false)` | Validates GUID format | "{FieldName} must be a valid GUID." | Yes |

**Example:**
```csharp
v.Rule(x => x.Email)
    .IsNotNullOrEmpty()
    .HasMaxLength(255)
    .IsEmail();

v.Rule(x => x.Description)
    .Optional()  // Makes field optional
    .HasMaxLength(1000);
```

## Integer Validation Extensions

**Target Type:** `ValidationRuleBuilder<T, int>`

| Method | Description | Default Error Message |
|--------|-------------|----------------------|
| `IsNotNegative(stopIfInvalid = false)` | Value >= 0 | "{FieldName} cannot be negative." |
| `IsPositive(stopIfInvalid = false)` | Value > 0 | "{FieldName} must be positive." |
| `IsInRange(int min, int max, stopIfInvalid = false)` | Value in range [min, max] | "{FieldName} must be between {min} and {max}." |
| `IsGreaterThan(int min, stopIfInvalid = false)` | Value > min | "{FieldName} must be greater than {min}." |
| `IsLessThan(int max, stopIfInvalid = false)` | Value < max | "{FieldName} must be less than {max}." |
| `IsEven(stopIfInvalid = false)` | Value is even | "{FieldName} must be even." |
| `IsOdd(stopIfInvalid = false)` | Value is odd | "{FieldName} must be odd." |
| `IsOneOf(params int[] validValues)` | Value in specified set | "{FieldName} must be one of: {values}." |

**Example:**
```csharp
v.Rule(x => x.Age).IsPositive().IsInRange(0, 150);
v.Rule(x => x.Status).IsOneOf(1, 2, 3, 5);
```

## Long Validation Extensions

**Target Type:** `ValidationRuleBuilder<T, long>`

Identical to Integer extensions, accepts `long` parameters.

| Method | Parameters |
|--------|-----------|
| `IsNotNegative(stopIfInvalid = false)` | |
| `IsPositive(stopIfInvalid = false)` | |
| `IsInRange(long min, long max, stopIfInvalid = false)` | |
| `IsGreaterThan(long min, stopIfInvalid = false)` | |
| `IsLessThan(long max, stopIfInvalid = false)` | |
| `IsEven(stopIfInvalid = false)` | |
| `IsOdd(stopIfInvalid = false)` | |
| `IsOneOf(params long[] validValues)` | |

## Short Validation Extensions

**Target Type:** `ValidationRuleBuilder<T, short>`

Identical to Integer extensions, accepts `short` parameters.

## Double Validation Extensions

**Target Type:** `ValidationRuleBuilder<T, double>`

Identical to Integer extensions, accepts `double` parameters.

| Method | Parameters |
|--------|-----------|
| `IsNotNegative(stopIfInvalid = false)` | |
| `IsPositive(stopIfInvalid = false)` | |
| `IsInRange(double min, double max, stopIfInvalid = false)` | |
| `IsGreaterThan(double min, stopIfInvalid = false)` | |
| `IsLessThan(double max, stopIfInvalid = false)` | |
| `IsEven(stopIfInvalid = false)` | |
| `IsOdd(stopIfInvalid = false)` | |
| `IsOneOf(params double[] validValues)` | |

**Example:**
```csharp
v.Rule(x => x.Score).IsPositive().IsInRange(0.01, 100.0);
v.Rule(x => x.Ratio).IsNotNegative().IsLessThan(1.0);
```

## Decimal Validation Extensions

**Target Type:** `ValidationRuleBuilder<T, decimal>`

Identical to Integer extensions, accepts `decimal` parameters. Ideal for financial calculations and monetary values where precision is critical.

| Method | Parameters |
|--------|-----------|
| `IsNotNegative(stopIfInvalid = false)` | |
| `IsPositive(stopIfInvalid = false)` | |
| `IsInRange(decimal min, decimal max, stopIfInvalid = false)` | |
| `IsGreaterThan(decimal min, stopIfInvalid = false)` | |
| `IsLessThan(decimal max, stopIfInvalid = false)` | |
| `IsEven(stopIfInvalid = false)` | |
| `IsOdd(stopIfInvalid = false)` | |
| `IsOneOf(params decimal[] validValues)` | |

**Example:**
```csharp
v.Rule(x => x.Price).IsPositive().IsInRange(0.01m, 999999.99m);
v.Rule(x => x.Discount).IsNotNegative().IsLessThan(1.0m);
v.Rule(x => x.Amount).IsPositive().IsGreaterThan(0m);
```

## DateTime Validation Extensions

**Target Type:** `ValidationRuleBuilder<T, DateTime>` and `ValidationRuleBuilder<T, DateTime?>`

| Method | Description | Default Error Message |
|--------|-------------|----------------------|
| `IsNotDefault(stopIfInvalid = false)` | Not default(DateTime) | "{FieldName} cannot be the default date." |
| `IsNotInFuture(stopIfInvalid = false)` | Date <= DateTime.UtcNow | "{FieldName} cannot be in the future." |
| `IsNotInPast(stopIfInvalid = false)` | Date >= DateTime.UtcNow | "{FieldName} cannot be in the past." |
| `IsAfter(DateTime minDate, stopIfInvalid = false)` | Date > minDate | "{FieldName} must be after {minDate:O}." |
| `IsBefore(DateTime maxDate, stopIfInvalid = false)` | Date < maxDate | "{FieldName} must be before {maxDate:O}." |
| `IsBetween(DateTime min, DateTime max, stopIfInvalid = false)` | Date in range [min, max] | "{FieldName} must be between {min:O} and {max:O}." |
| `IsNotNullOrDefault(stopIfInvalid = false)` | For DateTime?, not null and not default | "{FieldName} cannot be null or default." |

**Example:**
```csharp
v.Rule(x => x.BirthDate).IsNotDefault().IsNotInFuture();
v.Rule(x => x.StartDate).IsNotInPast();
v.Rule(x => x.EventDate).IsBetween(minDate, maxDate);
v.Rule(x => x.OptionalDate).IsNotNullOrDefault(); // For DateTime?
```

## General Validation Extensions

**Universal extensions that work with any type.**

### IsNotNull
```csharp
v.Rule(x => x.SomeObject)
    .IsNotNull(errorMessage: "Custom error", stopIfInvalid: true, stopAllIfInvalid: false);
```

### IsNotEmpty (Guid)
```csharp
v.Rule(x => x.Id).IsNotEmpty(); // Validates Guid != Guid.Empty
```

### Optional
Marks a field as optional - subsequent validations skip if value is null/empty.
```csharp
v.Rule(x => x.MiddleName)
    .Optional()  // If null/empty, skip remaining validations
    .HasMaxLength(50);
```

### Must (Custom Predicate)
```csharp
v.Rule(x => x.Value)
    .Must(val => val % 2 == 0, "Value must be even")
    .Must(val => val < 100, "Value must be less than 100");
```

### MustAsync (Async Custom Predicate)
```csharp
v.Rule(x => x.Username)
    .MustAsync(async username => await IsUsernameAvailable(username), 
               "Username is already taken");
```

### DomainEnum (For ScoBro.Foundation EnumBase)
```csharp
v.Rule(x => x.Status)
    .DomainEnum<MyEntity, MyEnumType>(
        errorMessage: "Invalid status value",
        stopIfInvalid: true);
```

### UseValidator (Nested Validation)
```csharp
// Use existing validator instance
v.Rule(x => x.Address).UseValidator(new AddressValidator(), stopIfInvalid: false);

// Use validator factory
v.Rule(x => x.Address).UseValidator(() => new AddressValidator());

// Async nested validation
v.Rule(x => x.Address).UseAsyncValidator(() => new AddressValidator());
```

### CreateValidationRule (Custom Rule Builder)
Low-level method for creating custom validation rules. Most other extensions use this internally.
```csharp
builder.CreateValidationRule(
    validateValue: val => val.Length > 0,
    errorMessage: "Cannot be empty",
    stopIfInvalid: false,
    stopAllIfInvalid: false,
    validationConstraint: null);
```

## Control Flow Parameters

### stopIfInvalid
- **Type:** `bool`
- **Default:** Varies by method (typically `false`, except null/empty checks which default to `true`)
- **Effect:** If validation fails, stops validating remaining rules **for this field only**

```csharp
v.Rule(x => x.Name)
    .IsNotNullOrEmpty(stopIfInvalid: true)  // If fails, skip remaining rules for Name
    .HasMaxLength(100);
```

### stopAllIfInvalid
- **Type:** `bool`
- **Default:** `false`
- **Effect:** If validation fails, stops **all validation** across all fields

```csharp
v.Rule(x => x.Id)
    .IsNotNull(stopAllIfInvalid: true);  // If fails, don't validate anything else

v.Rule(x => x.Name).IsNotNullOrEmpty();  // Won't run if Id is null
```

## Pre-built Common Validators

```csharp
// Required string with max length
var validator = new RequiredMaxLengthStringValidator(maxLength: 100, fieldName: "Username");
var result = validator.Validate(username);

// Optional string with max length
var validator = new OptionalMaxLengthStringValidator(maxLength: 500, fieldName: "Description");

// Domain enum validator
var validator = new RequiredDomainEnumValidator<MyEnumType>(fieldName: "Status");
```

## Advanced Usage Patterns

### Multiple Rules on Same Field
```csharp
v.Rule(x => x.Email)
    .IsNotNullOrEmpty(stopIfInvalid: true)
    .HasMaxLength(255)
    .IsEmail()
    .Must(email => !email.Contains("+"), "Cannot contain plus sign");
```

### Multiple Fields
```csharp
Validator.For<User>(v => {
    v.Rule(x => x.Username).IsNotNullOrEmpty().HasMaxLength(50);
    v.Rule(x => x.Email).IsNotNullOrEmpty().IsEmail();
    v.Rule(x => x.Age).IsPositive().IsInRange(13, 120);
    v.Rule(x => x.Id).IsNotEmpty();
});
```

### Field Name Customization
```csharp
// Default: uses property name
v.Rule(x => x.FirstName).IsNotNullOrEmpty(); 
// Error: "FirstName cannot be null or empty."

// Custom field name
v.Rule(x => x.FirstName, "First Name").IsNotNullOrEmpty();
// Error: "First Name cannot be null or empty."
```

### Conditional Validation
```csharp
v.Rule(x => x.ShippingAddress)
    .Must(addr => CurrentOrder.RequiresShipping ? addr != null : true,
          "Shipping address required for physical items");
```

### Async Validation
```csharp
public class UserValidator : Validator<User> {
    private readonly IUserService _userService;
    
    public UserValidator(IUserService userService) {
        _userService = userService;
        
        RuleFor(x => x.Email)
            .IsNotNullOrEmpty()
            .IsEmail()
            .MustAsync(async email => await _userService.IsEmailAvailable(email),
                      "Email is already registered");
    }
}

// Usage
var result = await validator.ValidateAsync(user);
```

### Nested Object Validation
```csharp
public class OrderValidator : Validator<Order> {
    public OrderValidator() {
        RuleFor(x => x.CustomerId).IsNotEmpty();
        RuleFor(x => x.ShippingAddress).UseValidator(new AddressValidator());
        RuleFor(x => x.Items)
            .Must(items => items?.Count > 0, "Order must have at least one item");
    }
}

public class AddressValidator : Validator<Address> {
    public AddressValidator() {
        RuleFor(x => x.Street).IsNotNullOrEmpty().HasMaxLength(200);
        RuleFor(x => x.City).IsNotNullOrEmpty().HasMaxLength(100);
        RuleFor(x => x.PostalCode).IsNotNullOrEmpty().MatchesRegex(@"^\d{5}(-\d{4})?$");
    }
}
```

## Validation Constraints

Some validation methods generate `ValidationConstraint` objects that describe the validation rules applied. These can be retrieved and used for other purposes (e.g., UI generation).

```csharp
var validator = new PersonValidator();
List<ValidationConstraint> constraints = validator.GetValidationConstraints();

// Currently supported:
// - StringMaxLengthConstraint (from HasMaxLength)
```

## Best Practices

1. **Use `stopIfInvalid: true` for prerequisite checks:**
   ```csharp
   v.Rule(x => x.Email)
       .IsNotNullOrEmpty(stopIfInvalid: true)  // Stop if null
       .IsEmail();  // Only check format if not null
   ```

2. **Mark optional fields explicitly:**
   ```csharp
   v.Rule(x => x.MiddleName)
       .Optional()
       .HasMaxLength(50);
   ```

3. **Order rules from most to least restrictive:**
   ```csharp
   v.Rule(x => x.Username)
       .IsNotNullOrEmpty()      // Most restrictive
       .HasMinLength(3)
       .HasMaxLength(20)
       .MatchesRegex("[a-zA-Z0-9_]+");  // Least restrictive
   ```

4. **Use class-based validators for reusable validation logic:**
   ```csharp
   public class CreateUserValidator : Validator<CreateUserCommand> {
       public CreateUserValidator() {
           RuleFor(x => x.Username).IsNotNullOrEmpty().HasMaxLength(50);
           RuleFor(x => x.Email).IsNotNullOrEmpty().IsEmail();
       }
   }
   ```

5. **Use inline validators for one-off validations:**
   ```csharp
   var result = Validator.Validate(input, v => {
       v.Rule(x => x.Value).IsPositive();
   });
   ```

6. **Always await ValidateAsync when using async rules:**
   ```csharp
   if (_ruleSets.Any(x => x.Rules.Any(a => a.IsAsync))) {
       return await validator.ValidateAsync(item);  // Required for async
   } else {
       return validator.Validate(item);  // Faster for sync-only
   }
   ```

## Common Patterns Summary

```csharp
// ✅ Required string with constraints
v.Rule(x => x.Name).IsNotNullOrEmpty().HasMaxLength(100);

// ✅ Optional string with constraints
v.Rule(x => x.Description).Optional().HasMaxLength(500);

// ✅ Required number in range
v.Rule(x => x.Age).IsPositive().IsInRange(18, 120);

// ✅ Required Guid
v.Rule(x => x.Id).IsNotEmpty();

// ✅ Required email
v.Rule(x => x.Email).IsNotNullOrEmpty().IsEmail();

// ✅ Optional DateTime
v.Rule(x => x.BirthDate).IsNotNullOrDefault();  // For DateTime?

// ✅ Required DateTime
v.Rule(x => x.CreatedAt).IsNotDefault().IsNotInFuture();

// ✅ Custom validation
v.Rule(x => x.Password)
    .IsNotNullOrEmpty()
    .HasMinLength(8)
    .Must(pwd => pwd.Any(char.IsUpper), "Must contain uppercase")
    .Must(pwd => pwd.Any(char.IsDigit), "Must contain digit");

// ✅ Async validation
v.Rule(x => x.Username)
    .IsNotNullOrEmpty()
    .MustAsync(async u => await IsUnique(u), "Already exists");

// ✅ Nested validation
v.Rule(x => x.Address).UseValidator(new AddressValidator());
```

