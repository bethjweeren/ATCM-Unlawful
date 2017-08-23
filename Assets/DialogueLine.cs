public class DialogueLine {
    Character spokenBy;
    string line;

    public void formatColors()
    {
        string final = "";
        int startIndex = line.IndexOf('[');
        while (startIndex > -1)
        {
            final = final + line.Substring(0, startIndex);
            int endIndex = line.IndexOf(']');
            string name = line.Substring(startIndex + 1, endIndex);
            Character subject = Characters.CharacterLookup(name.ToUpper());
            startIndex = line.IndexOf('[');
        }
    }

	
}
