namespace AlienFX.Util
{
    public static class LightFxTypeEx
    {
        public static LfxColorEncode Brightness(this LfxColorEncode colorEncode, LfxBrightness brightness) => colorEncode + (uint) brightness;
    }
}