﻿using Microsoft.Win32;

namespace License.Desktop.Helpers
{
    public class ConstantHelper
    {
        public static RegistryKey REGISTRY_KEY = Microsoft.Win32.Registry.CurrentUser;
        public static string REGISTRY_SUBKEY = "License";
        public static string REGISTRY_LOGIN = "Login";
    }
}
