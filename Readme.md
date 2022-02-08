# Wix# Samples

* [WixSharp.WPF](https://github.com/level120/WixSharp-Samples/blob/main/WixSharp.WPF/Readme.md) - Wix# sample for WPF with custom dialogs
* [WixSharp.WinForm](https://github.com/level120/WixSharp-Samples/blob/main/WixSharp.WinForm/Readme.md) - Wix# sample for WinForm with custom dialogs

### Tasks

- [ ] Install files settings with json model
- [ ] Create pre-install and post-install step(From [official sample](https://github.com/oleg-shilo/wixsharp/blob/master/Source/src/WixSharp.Samples/Wix%23%20Samples/Managed%20Setup/SetupEvents/setup.cs))
- [ ] Apply locals(From [official sample](https://github.com/oleg-shilo/wixsharp/blob/master/Source/src/WixSharp.Samples/Wix%23%20Samples/Managed%20Setup/MultiLanguageUI/setup.cs))
- [ ] Apply that "Run as Administrator"(From [official sample](https://github.com/oleg-shilo/wixsharp/blob/master/Source/src/WixSharp.Samples/Wix%23%20Samples/RestartElevated(UI)/setup.cs))

- [ ] Language selection dialog
- [ ] Requirement utilities checking dialog

### Json Model

* [string] Version
* [string] GUID
* [string] Manufacturer
* [string] MSI file name(i.e `MSIFileName`)
* [string] Background image path
* [string] Banner image path
* [boolean] Use running administrator
* [dictionary] Support locals file and license.rtf path(i.e `LocalizationFile`, `Language`, `LicenseFile`)
* [dictionary] Mapping directories and files
* [dictionary] Mapping registries