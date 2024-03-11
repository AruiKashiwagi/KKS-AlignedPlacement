namespace AlignedPlacement
{
    public static class Constants
    {
#if KK
        public const string Prefix = "KK";
        public const string GameName = "Koikatsu";
        public const string MainGameProcessName = "Koikatu";
        public const string StudioProcessName = "CharaStudio";
        public const string VRProcessName = "KoikatuVR";
#elif KKS
        public const string Prefix = "KKS";
        public const string GameName = "Koikatsu Sunshine";
        public const string MainGameProcessName = "KoikatsuSunshine";
        public const string StudioProcessName = "CharaStudio";
        public const string VRProcessName = "KoikatsuSunshine_VR";
#else
#error "No game selected"
#endif

        public const string PluginGUID = "com.kashiwagi.arui.alignedplacement";
        public const string PluginName = "Aligned Placement Helper";
        public const string PluginNameInternal = Constants.Prefix + "_AlignedPlacement";
        public const string PluginVersion = "1.0";
    }
}
