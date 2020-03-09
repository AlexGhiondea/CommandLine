namespace CommandLine
{
    public class HelpRequestedException : ParserException
    {
        public HelpRequestedException() : base("Help was requested.", null)
        {
        }
    }
}
