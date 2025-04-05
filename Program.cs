﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace MsgRuntimePatcher {
    class Program {
        const int PROCESS_VM_OPERATION = 0x0008;
        const int PROCESS_VM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_QUERY_INFORMATION = 0x0400;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);


        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, out int bytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, out int bytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        static async Task Main(string[] args) {

            Console.WriteLine("MsgRuntimePatcher v1.0");
            Console.WriteLine("waiting 10 minutes before Patching LoadMsg_1.exe...");
            await DelayAsync(10 * 60 * 1000); // 10 minutes = 600,000 milliseconds
            Console.WriteLine("Patching LoadMsg_1.exe...");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Process[] processes = Process.GetProcessesByName("LoadMsg_1");
            if (processes.Length == 0) {
                Console.WriteLine("Process not found.");
                Console.ReadKey();
                return;
            }

            Process proc = processes[0];
            IntPtr handle = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION | PROCESS_QUERY_INFORMATION, false, proc.Id);

            if (handle == IntPtr.Zero) {
                Console.WriteLine("Failed to open process.");
                Console.ReadKey();
                return;
            }

            PatchString(handle, proc, new byte[]
            {
    0x5B, 0xC4, 0xFA, 0xB5, 0xC4, 0xC4, 0xA7, 0xCA, 0xAF, 0xD4, 0xF6,
    0xBC, 0xD3, 0xC1, 0xCB, 0xA1, 0xA3, 0x5D
            }, "[+MAGIC STONE!]", "msg_magicstone");

            PatchString(handle, proc, new byte[]
            {
    0x5B, 0xC4, 0xFA, 0xB5, 0xC4, 0xC9, 0xF1, 0xCA, 0xAF, 0xD4, 0xF6,
    0xBC, 0xD3, 0xC1, 0xCB, 0xA1, 0xA3, 0x5D
            }, "[+GOD MONEY!]", "msg_godmoney");

            PatchString(handle, proc, new byte[]
            {
    0x5B, 0xC4, 0xFA, 0xB5, 0xC4, 0xC7, 0xAE, 0xD4, 0xF6, 0xBC, 0xD3,
    0xC1, 0xCB, 0xA1, 0xA3, 0x5D
            }, "[+MONEY]", "msg_money");

            PatchString(handle, proc, new byte[]
            {
    0xB5, 0xD8, 0xCD, 0xBC, 0x3A, 0x25, 0x75, 0x2C, 0x20, 0xB9, 0xD6,
    0xCE, 0xEF, 0x3A, 0x20, 0x25, 0x64, 0x2C, 0x20, 0xB6, 0xAF, 0xCC,
    0xAC, 0x4E, 0x50, 0x43, 0x3A, 0x20, 0x25, 0x64, 0x2C, 0x20, 0xCD,
    0xE6, 0xBC, 0xD2, 0x3A, 0x20, 0x25, 0x64, 0x2C, 0x20, 0xCF, 0xDD,
    0xDA, 0xE5, 0x3A, 0x20, 0x25, 0x64
            }, "[MAP:%u, ROLE:%d, HP:%d, MP:%d, ATK:%d, DEF:%d]", "msg_status");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xE3, 0xD2, 0xD1, 0xBE, 0xAD, 0xBB, 0xA8, 0xB7, 0xD1, 0xC1,
    0xCB, 0x25, 0x64, 0xB8, 0xF6, 0xC4, 0xA7, 0xCA, 0xAF, 0xB9, 0xBA,
    0xC2, 0xF2, 0xC1, 0xCB, 0x25, 0x64, 0xB8, 0xF6, 0x25, 0x73, 0xA1, 0xA3
            }, "Spent %d Pts to buy %d %s.", "msg_buy");

            PatchString(handle, proc, new byte[]
            {
    0xB9, 0xBA, 0xC2, 0xF2, 0xB3, 0xC9, 0xB9, 0xA6, 0x2E
            }, "Bought OK", "msg_buysuccess");

            PatchString(handle, proc, new byte[]
            {
    0xC5, 0xFA, 0xC1, 0xBF, 0xB7, 0xF5, 0xBB, 0xAF, 0xCA, 0xB1, 0xA3,
    0xAC, 0x5B, 0x25, 0x64, 0x5D, 0xC0, 0xF1, 0xB0, 0xFC, 0xB5, 0xC4,
    0x6D, 0x6F, 0x6E, 0x73, 0x74, 0x65, 0x72, 0x74, 0x79, 0x70, 0x65,
    0xD6, 0xB5, 0xCE, 0xDE, 0xB7, 0xA8, 0xD5, 0xD2, 0xB5, 0xBD, 0xA3, 0xA1
            }, "Invalid monstertype [%d], failed!", "msg_monstertypefail");


            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xFA, 0xB6, 0xAA, 0xC6, 0xFA, 0xC1, 0xCB, 0x25, 0x73, 0x20,
    0xA3, 0xA1
            }, "Dropped %s!", "msg_itemdrop");

            PatchString(handle, proc, new byte[]
            {
    0xCE, 0xDE, 0xB7, 0xA8, 0xCA, 0xB9, 0xD3, 0xC3, 0xCE, 0xEF, 0xC6,
    0xB7, 0x21
            }, "Bad item!", "msg_invaliditem");

            PatchString(handle, proc, new byte[]
            {
    0xCC, 0xE1, 0xCA, 0xBE, 0x3A, 0xB4, 0xA5, 0xB7, 0xA2, 0x5B, 0xCC,
    0xF4, 0xD5, 0xBD, 0x42, 0x4F, 0x53, 0x53, 0x5D, 0xA3, 0xAC, 0xC8,
    0xCE, 0xCE, 0xF1, 0x49, 0x44, 0x3A, 0x5B, 0x25, 0x64, 0x5D, 0x20,
    0xBD, 0xC5, 0xB1, 0xBE, 0x3A, 0x5B, 0x25, 0x64, 0x5D
            }, "Triggered [BOSS], QID:[%d] Script:[%d]", "msg_bosstrigger");

            PatchString(handle, proc, new byte[]
            {
    0xD7, 0xA2, 0xD2, 0xE2, 0xA3, 0xBA, 0xB4, 0xCB, 0xC3, 0xFC, 0xC1,
    0xEE, 0xD6, 0xBB, 0xD3, 0xC3, 0xD3, 0xDA, 0xB1, 0xBE, 0xB5, 0xD8,
    0xB2, 0xE2, 0xCA, 0xD4, 0xA3, 0xAC, 0xB7, 0xF1, 0xD4, 0xF2, 0xBF,
    0xC9, 0xC4, 0xDC, 0xB5, 0xBC, 0xD6, 0xC2, 0xB7, 0xFE, 0xCE, 0xF1,
    0xC6, 0xF7, 0xB1, 0xC0, 0xC0, 0xA3, 0x21
            }, "Test map: consigned items can't be used!", "msg_consignmentwarn");

            PatchString(handle, proc, new byte[]
            {
    0xC7, 0xA7, 0xC0, 0xEF, 0xB4, 0xAB, 0xD2, 0xF4
            }, "Timeout", "msg_timeout");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xBF, 0xB1, 0xEA, 0xB5, 0xE3, 0xB2, 0xBB, 0xC4, 0xDC, 0xC2,
    0xE4, 0xBD, 0xC5, 0x21
            }, "Action denied!", "msg_actionblocked");

            PatchString(handle, proc, new byte[]
            {
    0xD7, 0xEE, 0xB4, 0xF3, 0xD4, 0xCA, 0xD0, 0xED, 0xB5, 0xC7, 0xC2,
    0xBC, 0xC8, 0xCB, 0xCA, 0xFD, 0xB8, 0xC4, 0xCE, 0xAA, 0x3A, 0x20,
    0x25, 0x75
            }, "Duplicate logins: %u", "msg_duplogins");

            PatchString(handle, proc, new byte[]
            {
    0xB5, 0xB1, 0xC7, 0xB0, 0xD4, 0xDA, 0xCF, 0xDF, 0x3A, 0x25, 0x75,
    0x2C, 0x20, 0xD7, 0xEE, 0xB4, 0xF3, 0xD4, 0xDA, 0xCF, 0xDF, 0x3A,
    0x25, 0x75
            }, "Logins: %u, Dup: %u", "msg_loginstats");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xFA, 0xB5, 0xC4, 0xB1, 0xB3, 0xB0, 0xFC, 0xC4, 0xBF, 0xC7,
    0xB0, 0xD3, 0xD0, 0x5B, 0x25, 0x64, 0x5D, 0xBC, 0xFE, 0xCE, 0xEF,
    0xC6, 0xB7, 0x2E
            }, "Bag [%d] locked, low level.", "msg_baglevel");

            PatchString(handle, proc, new byte[]
            {
    0xB2, 0xBB, 0xCF, 0xD4, 0xCA, 0xBE, 0x41, 0x43, 0x54, 0x49, 0x4F,
    0x4E, 0xC1, 0xCB, 0xA1, 0xA3
            }, "Invalid ACTION", "msg_actionunsupported");

            PatchString(handle, proc, new byte[]
            {
    0xBF, 0xC9, 0xD2, 0xD4, 0xCF, 0xD4, 0xCA, 0xBE, 0x41, 0x43, 0x54,
    0x49, 0x4F, 0x4E, 0xC1, 0xCB, 0xA1, 0xA3
            }, "ACTION failed!", "msg_actionfail");

            PatchString(handle, proc, new byte[]
            {
    0xB5, 0xD8, 0xCD, 0xBC, 0x3A, 0x25, 0x75, 0x2C, 0x20, 0xB9, 0xD6,
    0xCE, 0xEF, 0x3A, 0x20, 0x25, 0x64, 0x2C, 0x20, 0xB6, 0xAF, 0xCC,
    0xAC, 0x4E, 0x50, 0x43, 0x3A, 0x20, 0x25, 0x64, 0x2C, 0x20, 0xCD,
    0xE6, 0xBC, 0xD2, 0x3A, 0x20, 0x25, 0x64, 0x2C, 0x20, 0xCF, 0xDD,
    0xDA, 0xE5, 0x3A, 0x20, 0x25, 0x64
            }, "Map:%u, Monster:%d, DynNPC:%d, Player:%d, Trap:%d", "msg_entitycounts");

            PatchString(handle, proc, new byte[]
            {
    0xBF, 0xAA, 0xCA, 0xBC, 0xB4, 0xA6, 0xC0, 0xED, 0x6B, 0x69, 0x63,
    0x6B, 0x6F, 0x75, 0x74, 0x61, 0x6C, 0x6C, 0xA3, 0xAC, 0xB7, 0xFE,
    0xCE, 0xF1, 0xC6, 0xF7, 0xBD, 0xFB, 0xD6, 0xB9, 0xB5, 0xC7, 0xC2,
    0xBC, 0xA3, 0xAC, 0xCB, 0xF9, 0xD3, 0xD0, 0xCD, 0xE6, 0xBC, 0xD2,
    0xCD, 0xCB, 0xB3, 0xF6, 0xD6, 0xD0, 0x20, 0x2E, 0x20, 0x2E, 0x20,
    0x2E
            }, "kickoutall enabled, may affect server stability...", "msg_kickwarn");

            PatchString(handle, proc, new byte[]
            {
    0x5B, 0xC4, 0xFA, 0xB5, 0xC4, 0xCE, 0xEF, 0xC6, 0xB7, 0xD4, 0xF6,
    0xBC, 0xD3, 0xC1, 0xCB, 0xA1, 0xA3, 0x5D
            }, "[+ITEM RECEIVED]", "msg_itemgain");

            PatchString(handle, proc, new byte[]
            {
    0x5B, 0xC4, 0xFA, 0xB5, 0xC4, 0xC7, 0xAE, 0xD4, 0xF6, 0xBC, 0xD3,
    0xC1, 0xCB, 0xA1, 0xA3, 0x5D
            }, "[+GOLD RECEIVED]", "msg_goldgain");
            PatchString(handle, proc, new byte[]
            {
    0xB5, 0xB1, 0xC7, 0xB0, 0xCD, 0xC5, 0xD5, 0xBD, 0xBD, 0xE1, 0xCA,
    0xF8, 0xCA, 0xB1, 0xBC, 0xE4, 0xCE, 0xAA, 0x3A, 0x25, 0x64, 0x2D,
    0x25, 0x64, 0x2D, 0x25, 0x64, 0x20, 0x25, 0x64, 0x3A, 0x25, 0x64,
    0x3A, 0x25, 0x64
            }, "Passive Skill: %d-%d-%d %d:%d:%d", "msg_passiveskill");

            PatchString(handle, proc, new byte[]
            {
    0xB5, 0xB1, 0xC7, 0xB0, 0xCD, 0xC5, 0xD5, 0xBD, 0xCA, 0xB1, 0xBC,
    0xE4, 0xCE, 0xAA, 0x3A, 0x25, 0x64, 0x2D, 0x25, 0x64, 0x2D, 0x25,
    0x64, 0x20, 0x25, 0x64, 0x3A, 0x25, 0x64, 0x3A, 0x25, 0x64
            }, "Active Skill: %d-%d-%d %d:%d:%d", "msg_activeskill");

            PatchString(handle, proc, new byte[]
            {
    0xB2, 0xE9, 0xBF, 0xB4, 0xBE, 0xFC, 0xCD, 0xC5, 0xCA, 0xB1, 0xBC,
    0xE4
            }, "Check Legion Time", "msg_checklegiontime");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xE3, 0xD2, 0xD1, 0xB0, 0xD1, 0xBE, 0xFC, 0xCD, 0xC5, 0xD5,
    0xBD, 0xCA, 0xB1, 0xBC, 0xE4, 0xC9, 0xE8, 0xD6, 0xC3, 0xCE, 0xAA,
    0x3A, 0x25, 0x64, 0x2D, 0x25, 0x64, 0x2D, 0x25, 0x64, 0x20, 0x25,
    0x64, 0x3A, 0x25, 0x64, 0x3A, 0x25, 0x64, 0x20, 0xCA, 0xB1, 0xB3,
    0xA4, 0x3A, 0x25, 0x64, 0xB7, 0xD6, 0xD6, 0xD3
            }, "Legion war set: %d-%d-%d %d:%d:%d (Duration: %d min)", "msg_legionsettime");

            PatchString(handle, proc, new byte[]
            {
    0xD7, 0xD4, 0xC9, 0xB1
            }, "Die", "msg_suicide");

            PatchString(handle, proc, new byte[]
            {
    0xCC, 0xD8, 0xD0, 0xA7
            }, "Fx", "msg_effect");

            PatchString(handle, proc, new byte[]
            {
    0xD1, 0xE6, 0xBB, 0xF0, 0x32
            }, "FW2", "msg_firework2");

            PatchString(handle, proc, new byte[]
            {
    0xD1, 0xE6, 0xBB, 0xF0
            }, "FW", "msg_firework");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xFA, 0xCF, 0xD6, 0xD4, 0xDA, 0xBB, 0xB9, 0xB2, 0xBB, 0xC4,
    0xDC, 0xC1, 0xC4, 0xCC, 0xEC, 0xA3, 0xAC, 0xC9, 0xCF, 0xCF, 0xDF,
    0x25, 0x64, 0xB7, 0xD6, 0xD6, 0xD3, 0xD2, 0xD4, 0xBA, 0xF3, 0xB2,
    0xC5, 0xBF, 0xC9, 0xD2, 0xD4, 0xC1, 0xC4, 0xCC, 0xEC, 0x2E
            }, "Chat available in %d min.", "msg_nochat");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xFA, 0xCF, 0xD6, 0xD4, 0xDA, 0xBB, 0xB9, 0xB2, 0xBB, 0xC4,
    0xDC, 0xC1, 0xC4, 0xCC, 0xEC, 0xA3, 0xAC, 0xB5, 0xC8, 0xBC, 0xB6,
    0xB2, 0xBB, 0xD7, 0xE3, 0x25, 0x64, 0xBC, 0xB6, 0x2E
            }, "Chat locked. Need level %d.", "msg_chatlvlreq");

            PatchString(handle, proc, new byte[]
            {
    0xB8, 0xF7, 0xD6, 0xD6, 0xB9, 0xAB, 0xC4, 0xDC
            }, "funcs", "msg_functions");

            PatchString(handle, proc, new byte[]
            {
    0xCC, 0xAB, 0xB9, 0xC5, 0xC1, 0xE9, 0xB5, 0xA4
            }, "elixir", "msg_ancientelixir");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xFA, 0xB1, 0xBB, 0xB5, 0xE3, 0xD1, 0xA8, 0xC1, 0xCB, 0xA3,
    0xAC, 0xCE, 0xDE, 0xB7, 0xA8, 0xCB, 0xB5, 0xBB, 0xB0, 0x21
            }, "silenced: no chat!", "msg_silenced");

            PatchString(handle, proc, new byte[]
            {
    0xCD, 0xE6, 0xBC, 0xD2, 0x5B, 0x25, 0x73, 0x5D, 0xC6, 0xF3, 0xCD,
    0xBC, 0xC3, 0xB0, 0xD3, 0xC3, 0xCB, 0xFB, 0xC8, 0xCB, 0xC3, 0xFB,
    0xD7, 0xD6, 0x5B, 0x25, 0x73, 0x5D, 0xB7, 0xA2, 0xB2, 0xBC, 0x54,
    0x61, 0x6C, 0x6B, 0xCF, 0xFB, 0xCF, 0xA2, 0xA3, 0xAC, 0xC7, 0xEB,
    0xD3, 0xE8, 0xB7, 0xE2, 0xBA, 0xC5, 0xA1, 0xA3
            }, "[%s] impersonated [%s] - ban flagged.", "msg_impersonation");

            PatchString(handle, proc, new byte[]
            {
    0xB1, 0xBE, 0xB5, 0xD8, 0xCD, 0xBC, 0xBD, 0xFB, 0xD6, 0xB9, 0xB7,
    0xC9, 0xD0, 0xD0, 0x2E
            }, "can't fly here.", "msg_noflymap");

            PatchString(handle, proc, new byte[]
            {
    0xC6, 0xEF, 0xCA, 0xBF, 0xCD, 0xC5, 0xBB, 0xF9, 0xBD, 0xF0, 0xB5,
    0xD6, 0xCF, 0xFB, 0xBE, 0xAD, 0xD1, 0xE9, 0xCB, 0xF0, 0xCA, 0xA7,
    0x25, 0x75, 0xA1, 0xA3
            }, "knight exp loss: %u", "msg_expoffset");

            PatchString(handle, proc, new byte[]
            {
    0xC8, 0xCE, 0xCE, 0xF1, 0xCD, 0xEA, 0xB3, 0xC9, 0xA1, 0xA3
            }, "quest ok.", "msg_questcomplete");

            PatchString(handle, proc, new byte[]
            {
    0x50, 0x4B, 0xD6, 0xB5, 0xBC, 0xCC, 0xD0, 0xF8, 0xD4, 0xF6, 0xBC,
    0xD3, 0xA3, 0xAC, 0xBD, 0xAB, 0xBB, 0xE1, 0xD1, 0xD3, 0xB3, 0xA4,
    0xBA, 0xDA, 0xC3, 0xFB, 0xB3, 0xCD, 0xB7, 0xA3, 0xB5, 0xC4, 0xCA,
    0xB1, 0xBC, 0xE4, 0xA1, 0xA3
            }, "PK warning: blackname", "msg_pkpenalty");

            PatchString(handle, proc, new byte[]
            {
    0x50, 0x4B, 0xD6, 0xB5, 0xBC, 0xCC, 0xD0, 0xF8, 0xD4, 0xF6, 0xBC,
    0xD3, 0xA3, 0xAC, 0xBD, 0xAB, 0xBB, 0xE1, 0xCA, 0xDC, 0xB5, 0xBD,
    0xBA, 0xDA, 0xC3, 0xFB, 0xB5, 0xC4, 0xB3, 0xCD, 0xB7, 0xA3, 0xA1,
    0xA3
            }, "PK warning — black penalty", "msg_pkpunish");

            PatchString(handle, proc, new byte[]
            {
    0x50, 0x4B, 0xD6, 0xB5, 0xBC, 0xCC, 0xD0, 0xF8, 0xD4, 0xF6, 0xBC,
    0xD3, 0xA3, 0xAC, 0xBD, 0xAB, 0xBB, 0xE1, 0xCA, 0xDC, 0xB5, 0xBD,
    0xBA, 0xEC, 0xC3, 0xFB, 0xB5, 0xC4, 0xB3, 0xCD, 0xB7, 0xA3, 0xA1,
    0xA3
            }, "PK warning — red penalty", "msg_pkredpunish");

            PatchString(handle, proc, new byte[]
            {
    0xB1, 0xBE, 0xBE, 0xFC, 0xCD, 0xC5, 0xB5, 0xC4, 0x20, 0x25, 0x73,
    0x20, 0x25, 0x73, 0xD2, 0xBB, 0xCA, 0xB1, 0xB2, 0xBB, 0xC9, 0xF7,
    0xA3, 0xAC, 0xD4, 0xDA, 0x20, 0x25, 0x73, 0xB1, 0xBB, 0xB5, 0xD0,
    0xB6, 0xD4, 0xBE, 0xFC, 0xCD, 0xC5, 0x20, 0x25, 0x73, 0x20, 0xD6,
    0xD0, 0xB5, 0xC4, 0x20, 0x25, 0x73, 0x20, 0x25, 0x73, 0xC9, 0xB1,
    0xCB, 0xC0, 0xC1, 0xCB, 0xA3, 0xAC, 0xD5, 0xE2, 0xD5, 0xE6, 0xCA,
    0xC7, 0xB6, 0xD4, 0xB1, 0xBE, 0xBE, 0xFC, 0xCD, 0xC5, 0xCB, 0xF9,
    0xD3, 0xD0, 0xB3, 0xC9, 0xD4, 0xB1, 0xB5, 0xC4, 0xCC, 0xF4, 0xD0,
    0xC6, 0xA1, 0xA3
            }, "%s %s was killed in %s by %s's %s %s — a clear provocation to our legion!", "msg_guildkill");

            PatchString(handle, proc, new byte[]
            {
    0xB1, 0xBE, 0xBE, 0xFC, 0xCD, 0xC5, 0xB5, 0xC4, 0x20, 0x25, 0x73,
    0x20, 0x25, 0x73, 0xD3, 0xA2, 0xD3, 0xC2, 0xCE, 0xDE, 0xCB, 0xAB,
    0xA3, 0xAC, 0xD4, 0xDA, 0x20, 0x25, 0x73, 0xBB, 0xF7, 0xC9, 0xB1,
    0xC1, 0xCB, 0xB5, 0xD0, 0xB6, 0xD4, 0xBE, 0xFC, 0xCD, 0xC5, 0x20,
    0x25, 0x73, 0x20, 0xD6, 0xD0, 0xB5, 0xC4, 0x20, 0x25, 0x73, 0x20,
    0x25, 0x73, 0xA3, 0xAC, 0xCE, 0xAA, 0xBE, 0xFC, 0xCD, 0xC5, 0xD1,
    0xEF, 0xCD, 0xFE, 0xA1, 0xA3
            }, "%s %s killed %s's %s %s in %s — glory to our legion!", "msg_guildvictory");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xFA, 0xB5, 0xC4, 0xD7, 0xF8, 0xC6, 0xEF, 0xB5, 0xC4, 0xC7,
    0xD7, 0xC3, 0xDC, 0xB6, 0xC8, 0xCF, 0xC2, 0xBD, 0xB5, 0xC1, 0xCB,
    0xA1, 0xA3
            }, "Mount loyalty too low!", "msg_mountsummonfail");

            PatchString(handle, proc, new byte[]
            {
    0x25, 0x73, 0xC9, 0xB1, 0xCB, 0xC0, 0xC1, 0xCB, 0xD5, 0xFD, 0xD4,
    0xDA, 0xC5, 0xDC, 0xC9, 0xCC, 0xB5, 0xC4, 0xCD, 0xE6, 0xBC, 0xD2,
    0xA3, 0xAC, 0xBB, 0xF1, 0xB5, 0xC3, 0xC1, 0xCB, 0x5B, 0x25, 0x75,
    0x5D, 0xBD, 0xF0, 0xB1, 0xD2, 0x2E
            }, "%s killed a trader and got [%u] gold.", "msg_traderkill");

            PatchString(handle, proc, new byte[]
            {
    0xC4, 0xFA, 0xC9, 0xB1, 0xCB, 0xC0, 0xC1, 0xCB, 0xD5, 0xFD, 0xD4,
    0xDA, 0xC5, 0xDC, 0xC9, 0xCC, 0xB5, 0xC4, 0xCD, 0xE6, 0xBC, 0xD2,
    0xA3, 0xAC, 0xBB, 0xF1, 0xB5, 0xC3, 0xC1, 0xCB, 0x5B, 0x25, 0x75,
    0x5D, 0xBD, 0xF0, 0xB1, 0xD2, 0x2E
            }, "you killed a trader and got [%u] gold.", "msg_youkilltrader");

            PatchString(handle, proc, new byte[]
            {
    0xBE, 0xFC, 0xCD, 0xC5, 0xCB, 0xF9, 0xD3, 0xD0, 0xB3, 0xC9, 0xD4,
    0xB1, 0xB5, 0xC4, 0xCC, 0xF4, 0xD0, 0xC6, 0xA1, 0xA3
            }, "Disgraced the legion!", "msg_legionshame");

            PatchString(handle, proc, new byte[]
            {
    0xD7, 0xD4, 0xB6, 0xAF, 0xCA, 0xB9, 0xD3, 0xC3, 0x20, 0x25, 0x73,
    0x20, 0xB1, 0xA6, 0xCA, 0xAF, 0xA3, 0xAC, 0xC3, 0xE2, 0xCA, 0xDC,
    0xB3, 0xCD, 0xB7, 0xA3, 0xA1, 0xA3
            }, "%s lost the duel!", "msg_duelloss");

            PatchString(handle, proc, new byte[]
            {
    0xD7, 0xD4, 0xB6, 0xAF, 0xCA, 0xB9, 0xD3, 0xC3, 0x20, 0x25, 0x73,
    0x20, 0xB1, 0xA6, 0xCA, 0xAF, 0xA3, 0xAC, 0xC3, 0xE2, 0xCB, 0xC0,
    0xD2, 0xBB, 0xB4, 0xCE, 0xA1, 0xA3
            }, "%s was crushed!", "msg_duelloss2");

            CloseHandle(handle);
            Console.WriteLine("Done. Closing");
        }

        static async Task DelayAsync(int delay) {
            await Task.Delay(delay);
        }

        static void PatchString(IntPtr handle, Process proc, byte[] aobPattern, string newText, string label) {
            long startAddress = proc.MainModule.BaseAddress.ToInt64();
            byte[] buffer = new byte[proc.MainModule.ModuleMemorySize];

            if (!ReadProcessMemory(handle, (IntPtr)startAddress, buffer, buffer.Length, out _)) {
                Console.WriteLine($"Failed to read memory for {label}.");
                return;
            }

            int index = FindPattern(buffer, aobPattern);
            if (index == -1) {
                Console.WriteLine($"Pattern {label} not found.");
                return;
            }

            long patchAddress = startAddress + index;
            byte[] patchData = Encoding.GetEncoding(936).GetBytes(newText);
            Array.Resize(ref patchData, aobPattern.Length);

            const uint PAGE_EXECUTE_READWRITE = 0x40;
            uint oldProtect;

            if (!VirtualProtectEx(handle, (IntPtr)patchAddress, (UIntPtr)patchData.Length, PAGE_EXECUTE_READWRITE, out oldProtect)) {
                Console.WriteLine($"Failed to change memory protection for {label}.");
                return;
            }

            if (!WriteProcessMemory(handle, (IntPtr)patchAddress, patchData, patchData.Length, out _)) {
                Console.WriteLine($"Failed to write memory for {label}.");
                return;
            }

            Console.WriteLine($"Patched {label} at 0x{patchAddress:X}");
        }


        static int FindPattern(byte[] buffer, byte[] pattern) {
            for (int i = 0; i <= buffer.Length - pattern.Length; i++) {
                bool match = true;
                for (int j = 0; j < pattern.Length; j++) {
                    if (buffer[i + j] != pattern[j]) {
                        match = false;
                        break;
                    }
                }
                if (match)
                    return i;
            }
            return -1;
        }
    }
}
