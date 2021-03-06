﻿using System.Threading.Tasks;

namespace Tron.AdminClient.Infrastructure
{
    public interface IFileDialogService
    {
        string OpenFileDialog();
        Task<string> OpenFileDialogAsync();
    }
}
