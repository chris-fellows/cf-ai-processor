namespace CFAIProcessor.Interfaces
{
    public interface IPasswordService
    { 
        /// <summary>
        /// Whether password meets rules for new passwords
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        bool IsAllowed(string password);

        /// <summary>
        /// Encrypts password
        /// </summary>
        /// <param name="password"></param>        
        /// <returns>Encrypted password + Salt</returns>
        string[] Encrypt(string password);

        /// <summary>
        /// Whether input password matches encrypted password
        /// </summary>
        /// <param name="encryptedPassword"></param>
        /// <param name="inputPassword"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        bool IsValid(string encryptedPassword, string inputPassword, string salt);
    }
}
