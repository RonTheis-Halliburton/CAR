using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text.RegularExpressions;

namespace CAR.App_Code
{
    public class Ldap
    {
        public string UserID { set; get; }
        public string whitelist = @"^[a-zA-Z\-\.']$";
        public string rootPath = "LDAP://ldap.corp.halliburton.com";

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

        public static DirectoryEntry GetDirectoryEntry()
        {
            DirectoryEntry de;
            de = new DirectoryEntry
            {
                Path = Encoder.LdapDistinguishedNameEncode("LDAP://ldap.corp.halliburton.com"),
                AuthenticationType = AuthenticationTypes.Secure
            };

            return de;
        }

        public bool IsUserExists()
        {
            Regex pattern = new Regex(whitelist);
            string safeUserID = "(anr=" + Encoder.LdapFilterEncode(UserID) + ")";
            if (IsUserGivenStringPluggableIntoLdapSearchFilter(safeUserID) && !pattern.IsMatch(safeUserID))
            {
                using (DirectoryEntry de = new DirectoryEntry(Encoder.LdapDistinguishedNameEncode("LDAP://ldap.corp.halliburton.com")))
                {
                    int results = 0;

                    DirectorySearcher deSearch;
                    deSearch = new DirectorySearcher(safeUserID)
                    {
                        SearchRoot = de,
                        SearchScope = SearchScope.Subtree
                    };

                    foreach (SearchResult res in deSearch.FindAll())
                    {
                        results++;
                    }

                    deSearch.Dispose();

                    de.Close();

                    if (results > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        //public List<HesUsers> GetMgr()
        //{
        //    List<HesUsers> lstADUsers;
        //    lstADUsers = new List<HesUsers>();

        //    try
        //    {
        //        Regex pattern = new Regex(whitelist);
        //        string safeUserID = ("(anr=" + Encoder.LdapFilterEncode(UserID) + ")");

        //        if (IsUserGivenStringPluggableIntoLdapSearchFilter(safeUserID) && !pattern.IsMatch(safeUserID))
        //        {
        //            using (DirectoryEntry searchRoot = GetDirectoryEntry())
        //            {
        //                DirectorySearcher search;
        //                search = new DirectorySearcher(searchRoot);
        //                //search.Filter = "(&(objectClass=user)(objectCategory=person))";
        //                search.Filter = ("(anr=" + (UserID + ")"));
        //                search.PropertiesToLoad.Add(value: "samaccountname");
        //                search.PropertiesToLoad.Add("Manager");
        //                search.SearchScope = SearchScope.Subtree;

        //                SearchResult result;
        //                SearchResultCollection resultCol = search.FindAll();

        //                if (resultCol != null)
        //                {
        //                    for (int counter = 0; counter < resultCol.Count; counter++)
        //                    {
        //                        result = resultCol[counter];

        //                        DirectoryEntry Userde = result.GetDirectoryEntry();

        //                        if (result.Properties.Contains("samaccountname"))
        //                        {
        //                            if (result.Properties["Manager"] != null)
        //                            {
        //                                DirectorySearcher searchMgr = new DirectorySearcher(result.GetDirectoryEntry())
        //                                {
        //                                    Filter = string.Format("(distinguishedName={0})", result.Properties["manager"])
        //                                };

        //                                searchMgr.PropertiesToLoad.Add("displayName");
        //                                searchMgr.PropertiesToLoad.Add("mail");
        //                                searchMgr.PropertiesToLoad.Add("manager");
        //                                DirectoryEntry mgrAcc = searchMgr.FindOne().GetDirectoryEntry();

        //                                if (searchMgr.FindOne().GetDirectoryEntry().Properties["manager"].Value != null)
        //                                {
        //                                    string mgrDN = mgrAcc.Properties["manager"][0].ToString();
        //                                    // Get the manager UserPrincipal via the DN 
        //                                    return UserPrincipal.FindByIdentity(ctx, mgrDN);
        //                                }

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error Processing LDAP:  " + ex.Message);
        //    }

        //}



        public List<HesUsers> GetADUsers()
        {
            List<HesUsers> lstADUsers;
            lstADUsers = new List<HesUsers>();

            try
            {
                Regex pattern = new Regex(whitelist);
                string safeUserID = "(anr=" + Encoder.LdapFilterEncode(UserID) + ")";

                if (IsUserGivenStringPluggableIntoLdapSearchFilter(safeUserID) && !pattern.IsMatch(safeUserID))
                {
                    using (DirectoryEntry searchRoot = GetDirectoryEntry())
                    {
                        DirectorySearcher search;
                        search = new DirectorySearcher(searchRoot)
                        {
                            //search.Filter = "(&(objectClass=user)(objectCategory=person))";
                            Filter = "(anr=" + UserID + ")"
                        };
                        search.PropertiesToLoad.Add(value: "samaccountname");
                        search.PropertiesToLoad.Add(value: "mail");
                        search.PropertiesToLoad.Add(value: "displayname");
                        search.PropertiesToLoad.Add(value: "telephonenumber");
                        //search.PropertiesToLoad.Add("streetAddress");
                        search.PropertiesToLoad.Add(value: "l");
                        //search.PropertiesToLoad.Add("st");
                        //search.PropertiesToLoad.Add("postalCode");
                        search.PropertiesToLoad.Add(value: "c");
                        search.PropertiesToLoad.Add("Manager");
                        //search.PropertiesToLoad.Add("employeeID");
                        //search.PropertiesToLoad.Add("department");
                        search.PropertiesToLoad.Add(value: "givenName");
                        search.PropertiesToLoad.Add(value: "sn");
                        //search.PropertiesToLoad.Add("homeDirectory");
                        //search.PropertiesToLoad.Add("Title");
                        //search.PropertiesToLoad.Add("name");
                        //search.PropertiesToLoad.Add("middleName");
                        SearchResult result;
                        SearchResultCollection resultCol = search.FindAll();

                        if (resultCol != null)
                        {
                            for (int counter = 0; counter < resultCol.Count; counter++)
                            {
                                result = resultCol[counter];
                                if (result.Properties.Contains(propertyName: "samaccountname") &&
                                         result.Properties.Contains(propertyName: "mail") &&
                                    result.Properties.Contains(propertyName: "displayname"))
                                {
                                    HesUsers objHalUsers;
                                    objHalUsers = new HesUsers();

                                    if (result.Properties["mail"].Count > 0)
                                    {
                                        objHalUsers.Email = (string)result.Properties["mail"][0];
                                    }

                                    if (result.Properties["samaccountname"].Count > 0)
                                    {
                                        objHalUsers.UserName = (string)result.Properties["samaccountname"][0];
                                    }

                                    if (result.Properties["displayname"].Count > 0)
                                    {
                                        objHalUsers.DisplayName = (string)result.Properties["displayname"][0];
                                    }

                                    //objHalUsers.StreetAddress = (String)result.Properties["streetAddress"][0];

                                    if (result.Properties["l"].Count > 0)
                                    {
                                        objHalUsers.City = (string)result.Properties["l"][0];
                                    }

                                    //objHalUsers.State = (String)result.Properties["st"][0];
                                    //objHalUsers.ZipCode = (String)result.Properties["postalCode"][0];

                                    if (result.Properties["c"].Count > 0)
                                    {
                                        objHalUsers.Country = (string)result.Properties["c"][0];
                                    }

                                    if (result.Properties["Manager"].Count > 0)
                                    {
                                        DirectorySearcher searchMgr = new DirectorySearcher(result.GetDirectoryEntry());

                                        searchMgr.AttributeScopeQuery = "manager";

                                        searchMgr.PropertiesToLoad.Add("sAMAccountName");
                                        searchMgr.PropertiesToLoad.Add("mail");
                                        searchMgr.PropertiesToLoad.Add("displayName");

                                        DirectoryEntry mgrAcc = searchMgr.FindOne().GetDirectoryEntry();

                                        if (searchMgr.FindOne().GetDirectoryEntry().Properties["sAMAccountName"].Value != null)
                                        {
                                            objHalUsers.ManagerID = (string)mgrAcc.Properties["sAMAccountName"][0];
                                        }

                                        if (searchMgr.FindOne().GetDirectoryEntry().Properties["displayName"].Value != null)
                                        {
                                            objHalUsers.Manager = (string)mgrAcc.Properties["displayName"][0];
                                        }

                                        if (searchMgr.FindOne().GetDirectoryEntry().Properties["mail"].Value != null)
                                        {
                                            objHalUsers.ManagerEmail = (string)mgrAcc.Properties["mail"][0];
                                        }

                                    }


                                    //objHalUsers.EmployeeID = (String)result.Properties["employeeID"][0];
                                    //objHalUsers.Department = (String)result.Properties["Department"][0];

                                    if (result.Properties["GivenName"].Count > 0)
                                    {
                                        objHalUsers.GivenName = (string)result.Properties["GivenName"][0];
                                    }

                                    if (result.Properties["sn"].Count > 0)
                                    {
                                        objHalUsers.SN = (string)result.Properties["sn"][0];
                                    }

                                    if (result.Properties["telephonenumber"].Count > 0)
                                    {
                                        objHalUsers.Phone = (string)result.Properties["telephonenumber"][0];
                                    }

                                    //objHalUsers.HomeDirectory = (String)result.Properties["HomeDirectory"][0];
                                    //objHalUsers.Title = (String)result.Properties["Title"][0];
                                    //objHalUsers.Name = (String)result.Properties["name"][0];
                                    //objHalUsers.MiddleName = (String)result.Properties["MiddleName"][0];

                                    lstADUsers.Add(objHalUsers);
                                }
                            }
                        }
                        search.Dispose(); ;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error Processing LDAP:  " + ex.Message);
            }
            return lstADUsers;
        }




    }
}