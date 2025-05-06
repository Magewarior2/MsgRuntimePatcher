# MsgRuntimePatcher

**MsgRuntimePatcher** is a memory patching tool that dynamically replaces in-game messages (often in Chinese) with English translations in real-time. This tool is primarily designed for Eudemons Online private servers or similar legacy clients using GBK-encoded strings.

## ✨ Features

- 🔍 AOB (Array of Bytes) scan support
- 🧠 Runtime memory patching via `WriteProcessMemory`
- 💬 Patch translated strings *without modifying* the original game files
- ✅ Translations byte-matched (ensures English strings ≤ original byte length)
- 🧵 Multi-thread-safe for runtime injection
- 🔐 No permanent changes to EXE or resources



## 🚀 Usage

1. Launch the target game client (e.g., `Game.exe`)
2. Run `MsgRuntimePatcher.exe` as Administrator
3. The tool will scan for AOB patterns and patch strings in memory
4. In-game messages will now appear in English instead of Chinese

> ✅ All translated messages are pre-encoded to ensure they do not exceed original byte lengths.

## 🛠 Example Patch

```csharp
// 您的钱袋已满!
// C4FAB5C4C7AEB4FCD2D1C2FA21
PatchString(handle, proc, new byte[]
{
    0xC4, 0xFA, 0xB5, 0xC4, 0xC7, 0xAE, 0xB4, 0xFC, 0xD2, 0xD1, 0xC2, 0xFA, 0x21
}, "Gold is full!", "msg_moneybag_full");
```

## 🧱 Requirements

- .NET 6 or later
- Admin privileges to patch process memory
