# Changelog

All notable changes to iRacing Teammate are documented here.

The project uses semantic versioning. GitHub release tags use the `vX.Y.Z` format.

## [1.2.1] - 2026-08-19

### Added

- Automatic detection and lifecycle support for CONSPIT Launcher and SimConnect Manager.

### Fixed

- Track replacement and child processes started by self-updating launchers, so
  Electron/Squirrel applications such as irDashies close with the iRacing session.

## [1.2.0] - 2026-08-19

### Added

- Start with Windows toggle for the current user.
- Auto Mode tied to the real iRacing simulator session lifecycle.
- Automatic companion-app cleanup after a three-second session-end confirmation.
- GitHub Releases update checker.
- Automatic repository embedding during GitHub Actions builds.
- GitHub build and release workflows.
- Support for irDashies, GO Fast, iRSidekick, VRS Telemetry Logger, Kapps,
  Joel Real Timing, OpenKneeboard, and Marvin's AIRA.
- Hide/show controls for individual application cards.

### Changed

- iRacing detection now reads its installed location and scans fixed-drive game folders.
- Garage61 detection supports its current roaming installation directory.
- Snails Motorsport mascot is embedded in the executable.

## [1.0.0] - 2026-08-19

- Initial Windows launcher with sequential race-stack launch, safe tracked-process
  shutdown, application discovery, persistent configuration, and livery-inspired UI.
