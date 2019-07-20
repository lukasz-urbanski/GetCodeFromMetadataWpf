using AcmeCorp.Training.Services;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    public class BusinessLogic
    {
        public LegacyObjectMetadataProvider.LatestVersionProvider metadataProvider = new LegacyObjectMetadataProvider.LatestVersionProvider();
        public ObjectCodeValidator validator = new ObjectCodeValidator();

        public string GetCode(string metadata)
        {
            if (!(metadata.StartsWith("<") || metadata.StartsWith("<")))
            {
                string[] arrayToBeChecked = metadata.Split(new char[] { '~', '_' });
                switch (arrayToBeChecked.Length)
                {
                    case (6):
                        return arrayToBeChecked[4];
                    case (7):
                        return arrayToBeChecked[5];
                    case (9):
                        if (arrayToBeChecked[5].Substring(0, 1).Equals("v"))
                        {
                            return arrayToBeChecked[7];
                        }
                        else
                        {
                            return arrayToBeChecked[5];
                        }
                    case (10):
                        return arrayToBeChecked[6];
                }
            }
            if (metadata.StartsWith("<Object>"))
            {
                if (metadata.Contains("<Code>"))
                {
                    //ReturnCodeFromMetadataBasedOnPattern(metadata, @"(.*)(<Code>)(.*)(</Code>)(.*)");
                    string patternCode = @"(.*)(<Code>)(.*)(<\/Code>)(.*)";
                    string patternMarket = @"(.*)(<Market>)(.*)(<\/Market>)(.*)";
                    MatchCollection matchesCode = Regex.Matches(metadata, patternCode);
                    MatchCollection matchesMarket = Regex.Matches(metadata, patternMarket);
                    foreach (Match matchM in matchesMarket)
                    {
                        if (matchM.Groups[3].Value.Equals("PL") || matchM.Groups[3].Value.Equals("BG") || matchM.Groups[3].Value.Equals("EL"))
                        {
                            foreach (Match matchC in matchesCode)
                            {
                                return matchC.Groups[3].Value.Substring(0, matchC.Groups[3].Value.Length - 2);
                            }
                        }
                        else
                        {
                            foreach (Match matchC in matchesCode)
                            {
                                return matchC.Groups[3].Value;
                            }
                        }
                    }
                }
                else
                {
                    //ReturnCodeFromMetadataBasedOnPattern(metadata, @"(.*)(_v\d~)(.*?(?=~))(.*)");
                    string pattern = @"(.*)(_v\d~)(.*?(?=~))(.*)";
                    MatchCollection matches = Regex.Matches(metadata, pattern);
                    foreach (Match match in matches)
                    {
                        return match.Groups[3].Value;
                    }
                }
            }
            else
            {
                //ReturnCodeFromMetadataBasedOnPattern(metadata, @"(.*)(_v\d~)(.*?(?=~))(.*)");
                string pattern = @"(.*)(_v\d~)(.*?(?=~))(.*)";
                MatchCollection matches = Regex.Matches(metadata, pattern);
                foreach (Match match in matches)
                {
                    return match.Groups[3].Value;
                }
            }
            return "ERROR";
        }
        
        //Have tried to implement method below to follow DRY rule but it didn't work...
        private static string ReturnCodeFromMetadataBasedOnPattern(string metadata, string pattern)
        {
            MatchCollection matches = Regex.Matches(metadata, pattern);
            foreach (Match match in matches)
            {
                return match.Groups[3].Value;
            }
            return "";
        }
    }
}