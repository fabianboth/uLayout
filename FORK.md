# This fork

A fork of [pokeblokdude/uLayout](https://github.com/pokeblokdude/uLayout) that carries fixes until upstream has them.

## Branches

- `main` follows upstream.
- Each fix is one branch with one commit on upstream's `main`. It holds only the change, so it can be offered upstream as it is.
- `integration` merges every fix and adds a play-mode test assembly (`Tests/`) with a failing-first test for each fix. Projects pin `integration` by commit hash. The tests are this fork's own and are not offered upstream.

| Branch | Fixes | Upstream |
|---|---|---|
| `fix/text-layout-dirty` | A changed text, font size, style or font asset is laid out in the frame of the change | not offered yet |
| `perf/event-driven-layout` | Layout runs only on a change, as uGUI's `LayoutGroup` does, instead of polling every frame (builds on `fix/text-layout-dirty`) | to be raised as an issue first |
| `fix/layout-element-values` | `ILayoutElement` reports the sizes the element lays itself out to | not offered yet |
| `fix/log-allocations` | A layout pass allocates nothing while logging is off | not offered yet |
| `feat/size-setters` | Min and max sizes and the sizing mode are settable from code | not offered yet |
| `fix/centre-in-padding` | Centring respects the padding on every axis | not offered yet |
| `fix/tracker-claims` | An item claims only the `RectTransform` values something sets, so saving a prefab no longer zeroes them | not offered yet |
| `fix/wrap` | Wrapped lines are broken once the given size and the children's heights are known | not offered yet |

## Working on it

1. Write the failing test on `integration`, in its own commit (`Tests: ...`).
2. Branch the fix from upstream's `main`, one commit whose message names the fault, how to reproduce it and the fix.
3. Merge the fix into `integration` and check that every test passes.

To run the tests, add the package as a local file dependency and list it in `testables` in the consuming project's `Packages/manifest.json`.
