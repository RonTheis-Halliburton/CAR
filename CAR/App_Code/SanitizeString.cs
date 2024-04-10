
public class SanitizeString
{
    public string SanitizeTextString(string rawString)
    {
        string results = rawString;

        results = results.Replace(oldValue: "%", newValue: string.Empty);
        results = results.Replace(oldValue: "<", newValue: string.Empty);
        results = results.Replace(oldValue: ">", newValue: string.Empty);
        results = results.Replace(oldValue: "#", newValue: string.Empty);
        results = results.Replace(oldValue: "{", newValue: string.Empty);
        results = results.Replace(oldValue: "}", newValue: string.Empty);
        results = results.Replace(oldValue: "|", newValue: string.Empty);
        results = results.Replace(oldValue: "\\", newValue: string.Empty);
        results = results.Replace(oldValue: "^", newValue: string.Empty);
        results = results.Replace(oldValue: "~", newValue: string.Empty);
        results = results.Replace(oldValue: "[", newValue: string.Empty);
        results = results.Replace(oldValue: "]", newValue: string.Empty);
        results = results.Replace(oldValue: "`", newValue: string.Empty);
        results = results.Replace(oldValue: ";", newValue: string.Empty);
        results = results.Replace(oldValue: "/", newValue: string.Empty);
        results = results.Replace(oldValue: "?", newValue: string.Empty);
        results = results.Replace(oldValue: ":", newValue: string.Empty);
        results = results.Replace(oldValue: "@", newValue: string.Empty);
        results = results.Replace(oldValue: "=", newValue: string.Empty);
        results = results.Replace(oldValue: "&", newValue: string.Empty);
        results = results.Replace(oldValue: "$", newValue: string.Empty);
        results = results.Replace(oldValue: "\"", newValue: string.Empty); //--Double quotes
        results = results.Replace(oldValue: "'", newValue: string.Empty); //--Single quote
        results = results.Replace(oldValue: "&nbsp;", newValue: string.Empty);

        return results;
    }

    public string SanitizeUrlString(string rawString)
    {
        string results = rawString;

        results = results.Replace(oldValue: "%", newValue: "%25");
        results = results.Replace(oldValue: "<", newValue: "%3C");
        results = results.Replace(oldValue: ">", newValue: "%3E");
        results = results.Replace(oldValue: "#", newValue: "%23");
        results = results.Replace(oldValue: "{", newValue: "%7B");
        results = results.Replace(oldValue: "}", newValue: "%7D");
        results = results.Replace(oldValue: "|", newValue: "%7C");
        results = results.Replace(oldValue: "\\", newValue: "%5C");
        results = results.Replace(oldValue: "^", newValue: "%5E");
        results = results.Replace(oldValue: "~", newValue: "%7E");
        results = results.Replace(oldValue: "[", newValue: "%5B");
        results = results.Replace(oldValue: "]", newValue: "%5D");
        results = results.Replace(oldValue: "`", newValue: "%60");
        results = results.Replace(oldValue: ";", newValue: "%3B");
        results = results.Replace(oldValue: "/", newValue: "%2F");
        results = results.Replace(oldValue: "?", newValue: "%3F");
        results = results.Replace(oldValue: ":", newValue: "%3A");
        results = results.Replace(oldValue: "@", newValue: "%40");
        results = results.Replace(oldValue: "=", newValue: "%3D");
        results = results.Replace(oldValue: "&", newValue: "%26");
        results = results.Replace(oldValue: "$", newValue: "%24");
        results = results.Replace(oldValue: "!", newValue: "%21");
        results = results.Replace(oldValue: "*", newValue: "%2A");
        results = results.Replace(oldValue: "(", newValue: "%28");
        results = results.Replace(oldValue: ")", newValue: "%29");
        results = results.Replace(oldValue: "\"", newValue: "%22"); //--Double quotes
        results = results.Replace(oldValue: "'", newValue: "%27"); //--Single quote
        return results;
    }


    public string SanitizeQuoteString(string rawString)
    {
        string results = rawString;

        results = results.Replace(oldValue: "\"", newValue: "``"); //--Double quotes
        results = results.Replace(oldValue: "'", newValue: "`"); //--Single quote

        return results;
    }

    public string SanitizeBlank(string rawString)
    {
        string results = rawString;

        results = results.Replace(oldValue: "&nbsp;", newValue: ""); //--blank space
        results = results.Replace(oldValue: "#", newValue: "");
        results = results.Replace(oldValue: "&", newValue: " ");

        return results;
    }

    public string SanitizeField(string rawString)
    {
        string results = rawString;


        if (string.IsNullOrEmpty(rawString.Trim()))
        {
            results = "  ";
        }

        return results;
    }

    public string SanitizeDbBlank(string rawString)
    {
        string results = rawString;

        results = results.Replace(oldValue: "&nbsp;", newValue: ""); //--blank space

        return results;
    }
}