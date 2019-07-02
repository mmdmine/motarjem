namespace Motarjem.Core
{
    public enum FontColor
    {
        Red, // Error
        Black, // Noun
        Blue, // Pronoun
        Green, // Verb
        LightRed, // Adjective
        LightGreen, // Preposition
        LightBlue, // Determiner
        Gray // Conjunction
    }

    public enum FontStyle
    {
        Default,
        Bold,
        Italic
    }

    public interface IDisplay
    {
        void Print(string text, FontColor color = FontColor.Black, FontStyle style = FontStyle.Default);
        void PrintSpace();
    }
}