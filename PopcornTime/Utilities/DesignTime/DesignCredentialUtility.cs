using PopcornTime.Utilities.Interfaces;

namespace PopcornTime.Utilities.DesignTime
{
    public class DesignCredentialUtility : ICredentialUtility
    {
        public AppCredential GetCredentials(string resource)
        {
            return null;
        }

        public void SaveCredentials(string resource, string username, string password)
        {
        }

        public void DeleteCredentials(string resource)
        {
        }
    }
}