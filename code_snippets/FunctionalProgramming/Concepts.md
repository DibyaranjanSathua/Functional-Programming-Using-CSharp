# What is Functional Programming ?
Functional Programming is programming with matthematical functions.

## Mathematical function should meets the following retquirements.
- Same input produces same result. Doesn't affect or refer to the global state.
- Information about possible inputs and outcomes in function signature

## Method Signature Honesty
- A method signature should indicate
    - All possible inputs
    - All possible outcomes
- Dishonest signature

  ```C#
  public static int Divide(int x, int y)
  {
      return x / y;
  }
  ```
  The is expected to return integer but for y = 0, it will raise a `DivideByZeroException`.

- Honest signature

  ```C#
  public static int Divide(int x, NonZeroInteger y)
  {
      return x / y.Value;
  }
  ```
  `NonZeroInteger` is custom type which contains any integer except zero.

  ```C#
  public static int? DIvide(int x, int y)
  {
      if (y == 0)
          return null;
      return x / y;
  }
  ```

# Why Functional Programming ?
- Reduces code complexity
- Composable
- Easy to reason about
- Easier to unit test

# Immutability
## Why Does Immutability Matter ?
- Mutable operations make the function dishonest. Method signature does not tell about the side effect.
- Increased readability
- Thread Safty

## How to Deal with Side Effects ?
- Command-Query separation principle

# Exceptions

## Refactoring Away from Exceptions
- Methods with exceptions are not mathematical functions because exceptions hide the actual outcome of the function.
- Exceptions for flow control is often similar to goto statements.
- Exceptions are not part of the method signature. They make the function dishonest.

## Use Cases for Exceptions
- Always prefer to return a value instead of throwing an exception.
- Exceptions should be used for exceptional cases.
- Exceptions should be used for unrecoverable errors.
- Don't use exceptions for flow control.
- Don't use exceptions for expected errors.
- Don't use exceptions for validation. Validations are not exceptional cases. Validation logic by definition expect the
  incoming data to be invalid.
- If the validation logic is not complete and there are some incorrect data sink in to the system, then throw an exception.
- Fail Fast Principle: If there is a bug in the code, then throw an exception.
