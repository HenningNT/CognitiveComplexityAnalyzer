# RULEID: Friendly rule name

## Cause

There are too many nested section, or too many logical conditions.

## Rule description



## How to fix violations

To reduce the complexity of the method there's not many options. Either split long methods up into smaller ones, or use early return to reduce nesting.
If the logic conditions use `!` to negate a result, try reverting the logic so it doesn't use `!`.

## When to suppress warnings
Suppress this rule if you have no intention of rewriting the method.

## Example of a violation

### Description

The following code has been taken from https://github.com/dotnet-state-machine/stateless/blob/dev/src/Stateless/StateRepresentation.Async.cs
It has high cognitive complexity due four levels of nesting, method calls in the logic condition and a negated logic condition.

### Code

```
public Transition Exit(Transition transition)
{
    if (transition.IsReentry)
    {
        ExecuteExitActions(transition);
    }
    else if (!Includes(transition.Destination))
    {
        ExecuteExitActions(transition);

        // Must check if there is a superstate, and if we are leaving that superstate
        if (_superstate != null)
        {
            // Check if destination is within the state list
            if (IsIncludedIn(transition.Destination))
            {
                // Destination state is within the list, exit first superstate only if it is NOT the the first
                if (!_superstate.UnderlyingState.Equals(transition.Destination))
                {
                    return _superstate.Exit(transition);
                }
            }
            else
            {
                // Exit the superstate as well
                return _superstate.Exit(transition);
            }
        }
    }
    return transition;
}
```
## Example of how to fix

- Refactor
- Reduce nesting

### Description

### Code

```
// Refactored
public Transition Exit(Transition transition)
            {
                if (transition.IsReentry)
                {
                    ExecuteExitActions(transition);
                }
                else if (!Includes(transition.Destination))
                {
                    ExecuteExitActions(transition);

                    // Take care of exiting superstate(if any)
                    HandleSuperState(transition)
                }
                return transition;
            }
```

```
// Reduced nesting
public Transition Exit(Transition transition)
{
    if (transition.IsReentry)
    {
        ExecuteExitActions(transition);
        return transition;
    }

    if (Includes(transition.Destination))
        return transition;
                
    ExecuteExitActions(transition);

    // Must check if there is a superstate, and if we are leaving that superstate
    if (_superstate == null) return transition;
                    
    // Check if destination is within the state list
    if (IsIncludedIn(transition.Destination) && !_superstate.UnderlyingState.Equals(transition.Destination))
    {
        // Destination state is within the list, exit first superstate only if it is NOT the the first
            return _superstate.Exit(transition);
    }
    else
    {
        // Exit the superstate as well
        return _superstate.Exit(transition);
    }
                
    return transition;
}
```
