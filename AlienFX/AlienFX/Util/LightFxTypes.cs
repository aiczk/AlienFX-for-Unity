namespace AlienFX.Util
{
    /// <summary>
    /// Device types
    /// </summary>
    public enum LfxDeviceType : ushort
    {
        Unknown = 0x00,
        Notebook = 0x01,
        Desktop = 0x02,
        Server = 0x03,
        Display = 0x04,
        Mouse = 0x05,
        Keyboard = 0x06,
        GamePad = 0x07,
        Speaker = 0x08,
        Custom = 0xFE,
        Other = 0xFF
    }
    
    /// <summary>
    /// Return values
    /// </summary>
    public enum LfxResult
    {
        /// <summary>
        /// Sucess
        /// </summary>
        Success,
        
        /// <summary>
        /// Generic failure
        /// </summary>
        Failure,
        
        /// <summary>
        /// System not initialized yet
        /// </summary>
        ErrorNoInit,
        
        /// <summary>
        /// No devices available
        /// </summary>
        ErrorNoDevs,
        
        /// <summary>
        /// No lights available
        /// </summary>
        ErrorNoLights,
        
        /// <summary>
        /// Buffer size too small
        /// </summary>
        ErrorBuffSize,
    }
    
    /// <summary>
    /// Location masks
    /// </summary>
    public enum LfxLocationMask : uint
    {
        //Near Z-plane light definitions
        FrontLowerLeft = 0x00000001,
        FrontLowerCenter = 0x00000002,
        FrontLowerRight = 0x00000004,
        
        FrontMiddleLeft = 0x00000008,
        FrontMiddleCenter = 0x00000010,
        FrontMiddleRight = 0x00000020,
        
        FrontUpperLeft = 0x00000040,
        FrontUpperCenter = 0x00000080,
        FrontUpperRight = 0x00000100,
        
        //Mid Z-plane light definitions
        MiddleLowerLeft = 0x00000200,
        MiddleLowerCenter = 0x00000400,
        MiddleLowerRight = 0x00000800,
        
        MiddleMiddleLeft = 0x00001000,
        MiddleMiddleCenter = 0x00002000,
        MiddleMiddleRight = 0x00004000,
        
        MiddleUpperLeft = 0x00008000,
        MiddleUpperCenter = 0x00010000,
        MiddleUpperRight = 0x00020000,
        
        //Far Z-plane light definitions
        RearLowerLeft = 0x00040000,
        RearLowerCenter = 0x00080000,
        RearLowerRight = 0x00100000,
        
        RearMiddleLeft = 0x00200000,
        RearMiddleCenter = 0x00400000,
        RearMiddleRight = 0x00800000,
        
        RearUpperLeft = 0x01000000,
        LfxRearUpperCenter = 0x02000000,
        LfxRearUpperRight = 0x04000000,
        
        //Combination bit masks
        All = 0x07FFFFFF,
        AllRight = 0x04924924,
        AllLeft = 0x01249249,
        AllUpper = 0x070381C0,
        AllLower = 0x001C0E07,
        AllFront = 0x000001FF,
        AllRear = 0x07FC0000
    }

    /// <summary>
    /// Translation layer color encoding
    /// </summary>
    public enum LfxColorEncode : uint
    {
        Off    = 0x00000000,
        Black  = 0x00000000,
        Red    = 0x00FF0000,
        PuGreen  = 0x0000FF00,
        Blue   = 0x000000FF,
        White  = 0x00FFFFFF,
        Yellow = 0x00FFFF00,
        Orange = 0x00FF8000,
        Pink   = 0x00FF80FF,
        Cyan   = 0x0000FFFF
    }

    /// <summary>
    /// Translation layer brightness encoding
    /// </summary>
    public enum LfxBrightness : uint
    {
        Full = 0xFF000000,
        Half = 0x80000000,
        Min = 0x00000000
    }

    /// <summary>
    /// Predifined kinds of actions
    /// </summary>
    public enum LfxActionType : uint
    {
        Morph = 0x00000001,
        Pulse = 0x00000002,
        Color = 0x00000003,
    }
    
    /// <summary>
    /// Color, encoded into 4 bytes
    /// </summary>
    public struct LfxColor
    {
        public byte red, green, blue, brightness;

        public LfxColor(byte red, byte green, byte blue, byte brightness)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.brightness = brightness;
        }
    }
    
/**************************************************************************************
IMPORTANT NOTE:

The semantics of LightFX position, location mask, and bounds are defined as follows:

BOUNDS are the physical bounds, in centimeters, of any given device/enclosure, 
		relative to an anchor point at the lower, left, rear corner
POSITION is a physical position, in centimeters, of any given light relative to 
		the lower, left, rear corner of the device's bounding box.
LOCATION (or "location mask") is a 32-bit mask that denotes one or more light positions 
		in terms of the device's bounding box. There are 27 bits for each smaller cube 
		within this bounding box, divided evenly. (Imagine a Rubick's cube...)

BOUNDS or POSITION may be encoded into a LFX_POSITION structure, so it is important to 
examine the context of the usage to determine what the data inside the structure refers to.

LOCATION should always be encoded into a 32-bit (or larger) value; see the bit field
declarations above.
***************************************************************************************/
    
    /// <summary>
    /// Position, encoded into a 3-axis value.
    /// Note that these are relative to the lower, left, rear
    /// corner of the device's bounding box.
    /// </summary>
    public struct LfxPosition
    {
        /// <summary>
        /// left to right.
        /// </summary>
        public byte x;
        
        /// <summary>
        /// bottom to top.
        /// </summary>
        public byte y;
        
        /// <summary>
        /// back to front. 
        /// </summary>
        public byte z;

        public LfxPosition(byte x, byte y, byte z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}