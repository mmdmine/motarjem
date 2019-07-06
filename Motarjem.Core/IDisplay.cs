namespace Motarjem.Core
{
    public enum FontColor
    {
        /// <summary>
        /// Used for Error messages
        /// </summary>
        Red,
        /// <summary>
        /// For highlighting Nouns
        /// </summary>
        Black,
        /// <summary>
        /// For highlighting Pronouns
        /// </summary>
        Blue,
        /// <summary>
        /// For highlighting Verbs
        /// </summary>
        Green,
        /// <summary>
        /// For highlighting Adjectives
        /// </summary>
        LightRed,
        /// <summary>
        /// For highlighting Prepositions
        /// </summary>
        LightGreen,
        /// <summary>
        /// For highlighting Determiners
        /// </summary>
        LightBlue,
        /// <summary>
        /// For highlighting Conjunctions
        /// </summary>
        Gray,
    }

    public enum FontStyle
    {
        Default,
        Bold,
        Italic
    }

    /// <summary>
    /// Interface for printing sentences
    /// </summary>
    public interface IDisplay
    {
        /// <summary>
        /// Printing by giving word and highlighting options
        /// </summary>
        /// <param name="text">Text to be printed</param>
        /// <param name="color">Color to be used</param>
        /// <param name="style">Style to be used</param>
        void Print(string text, FontColor color = FontColor.Black, FontStyle style = FontStyle.Default);
        
        /// <summary>
        /// Printing an space
        /// </summary>
        void PrintSpace();
    }
}