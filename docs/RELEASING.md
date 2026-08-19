# Release procedure

## One-time repository setup

1. Create the GitHub repository and push the `main` branch.
2. In **Settings → Actions → General**, allow GitHub Actions to read and write
   repository contents so the release workflow can publish assets.
3. In **Settings → Code security**, enable private vulnerability reporting.
4. Protect `main` and require the **Build** workflow before merging changes.
5. Decide whether the current all-rights-reserved license should remain or be
   replaced with an approved open-source license.

## Publish a version

1. Update `version.txt` using semantic versioning.
2. Move user-facing changes into a new section in `CHANGELOG.md`.
3. Run `.\build.ps1` locally and smoke-test the executable.
4. Commit and push the release change.
5. Create a tag that exactly matches the version:

   ```powershell
   git tag -a v1.2.0 -m "iRacing Teammate 1.2.0"
   git push origin v1.2.0
   ```

6. The release workflow validates the tag, builds the launcher, creates the ZIP
   and checksum, and publishes the GitHub Release.
7. Download the published ZIP, verify its SHA-256 checksum, and perform a final
   launch test from outside the source directory.

Do not manually replace an existing release asset. Publish a new patch version so
clients can compare versions reliably.
