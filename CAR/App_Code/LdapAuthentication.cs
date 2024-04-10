using System;
using System.DirectoryServices;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CAR.App_Code
{
    public class LdapAuthentication
    {
        //private string ldapPath;
        private string filterAttribute;

        //public LdapAuthentication(string path)
        //{
        //    ldapPath = path;
        //}


        public string whitelist = @"^[a-zA-Z\-\.']$";
        /// <summary>
        /// Characters that must be escaped in an LDAP filter path
        /// WARNING: Always keep '\\' at the very beginning to avoid recursive replacements
        /// </summary>
        private static char[] ldapFilterEscapeSequence = new char[] { '\\', '*', '(', ')', '\0', '/' };

        /// <summary>
        /// Mapping strings of the LDAP filter escape sequence characters
        /// </summary>
        private static string[] ldapFilterEscapeSequenceCharacter = new string[] { "\\5c", "\\2a", "\\28", "\\29", "\\00", "\\2f" };

        /// <summary>
        /// Characters that must be escaped in an LDAP DN path
        /// </summary>
        private static char[] ldapDnEscapeSequence = new char[] { '\\', ',', '+', '"', '<', '>', ';' };


        /// <summary>
        /// Canonicalize a ldap filter string by inserting LDAP escape sequences.
        /// </summary>
        /// <param name="userInput">User input string to canonicalize</param>
        /// <returns>Canonicalized user input so it can be used in LDAP filter</returns>
        public static string CanonicalizeStringForLdapFilter(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                return userInput;
            }
            string name = (string)userInput.Clone();

            for (int charIndex = 0; charIndex < ldapFilterEscapeSequence.Length; ++charIndex)
            {
                int index = name.IndexOf(ldapFilterEscapeSequence[charIndex]);
                if (index != -1)
                {
                    name = name.Replace(new string(ldapFilterEscapeSequence[charIndex], 1), ldapFilterEscapeSequenceCharacter[charIndex]);
                }
            }

            return name;
        }

        /// <summary>
        /// Ensure that a user provided string can be plugged into an LDAP search filter 
        /// such that there is no risk of an LDAP injection attack.
        /// </summary>
        /// <param name="userInput">String value to check.</param>
        /// <returns>True if value is valid or null, false otherwise.</returns>
        public static bool IsUserGivenStringPluggableIntoLdapSearchFilter(string userInput)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                return true;
            }
            if (userInput.IndexOfAny(ldapDnEscapeSequence) != -1)
            {
                return false;
            }
            return true;
        }

        public bool IsAuthenticated(string domain, string username, string pwd)
        {
            string domainAndUsername = domain + @"\" + username;
            Regex pattern = new Regex(whitelist);
            string safeUserID = "(SAMAccountName=" + Microsoft.Security.Application.Encoder.LdapFilterEncode(username) + ")";
            DirectoryEntry entry = null;
            try
            {
                entry = new DirectoryEntry("LDAP://ldap.corp.halliburton.com",
                                           domainAndUsername,
                                           Decrypt(pwd));
                DirectorySearcher search = null;
                try
                {

                    // Bind to the native AdsObject to force authentication.
                    //Object obj = entry.NativeObject;

                    search = new DirectorySearcher(entry)
                    {
                        Filter = safeUserID
                    };
                    search.PropertiesToLoad.Add(value: "cn");

                    var result = search.FindOne();
                    if (null == result)
                    {
                        return false;
                    }
                    // Update the new path to the user in the directory
                    //ldapPath = result.Path;
                    filterAttribute = (string)result.Properties["cn"][0];

                    search.Dispose();

                    entry.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error authenticating user. " + ex.Message);
                }
                finally
                {
                    search.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }
            finally
            {
                if (entry != null)
                {
                    entry.Close();
                }
            }

            return true;
        }

        public static string Decrypt(string textToBeDecrypted)
        {
            RijndaelManaged rijndaelCipher;
            rijndaelCipher = new RijndaelManaged();

            string keyStore = "YOUDUMMY";

            string decryptedData;
            try
            {

                byte[] encryptedData = Convert.FromBase64String(textToBeDecrypted);
                byte[] salt = Encoding.ASCII.GetBytes(keyStore.Length.ToString());

                //Making of the key for decryption

                PasswordDeriveBytes secretKey;
                secretKey = new PasswordDeriveBytes(keyStore, salt, "SHA256", 100000);

                //Creates a symmetric Rijndael decryptor object.
                ICryptoTransform decryptor;
                decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(cb: 32), secretKey.GetBytes(cb: 16));

                MemoryStream memoryStream;
                memoryStream = new MemoryStream(encryptedData);

                //Defines the cryptographics stream for decryption.THe stream contains decrpted data
                CryptoStream cryptoStream;
                cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                byte[] plainText;
                plainText = new byte[encryptedData.Length];

                int decryptedCount;
                decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);

                memoryStream.Close();
                //cryptoStream.Close();

                //Converting to string
                decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
            }
            catch (Exception ex)
            {
                decryptedData = textToBeDecrypted;
                throw new Exception("Error Authentication:  " + ex.Message);
            }
            finally
            {
                if (rijndaelCipher != null)
                {
                    rijndaelCipher.Clear();
                }
            }
            return decryptedData;
        }

    }
}