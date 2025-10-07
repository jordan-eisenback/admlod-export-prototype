namespace AdmLodPrototype.Interfaces
{
    public interface IFtpUploader
    {
        void Upload(string localFilePath, string remoteFilePath);
        Task UploadFileAsync(string localFilePath, string remoteFilePath);
    }
}