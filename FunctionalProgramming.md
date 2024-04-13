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
