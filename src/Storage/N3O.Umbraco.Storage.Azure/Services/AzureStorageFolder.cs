﻿using Azure.Storage.Blobs;
using HeyRed.Mime;
using Humanizer.Bytes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Storage.Services;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Azure.Services {
    public class AzureStorageFolder : IStorageFolder {
        private readonly BlobContainerClient _container;
        private readonly Action _onContainerDeleted;

        public AzureStorageFolder(BlobContainerClient container, Action onContainerDeleted) {
            _container = container;
            _onContainerDeleted = onContainerDeleted;
        }
        
        public async Task<Blob> AddFileAsync(string name, Stream stream) {
            await _container.UploadBlobAsync(name, stream);

            stream.Rewind();

            return new Blob(name, GetContentType(name), ByteSize.FromBytes(stream.Length), stream);
        }

        public async Task<Blob> AddFileAsync(string name, byte[] contents) {
            using (var stream = new MemoryStream(contents)) {
                return await AddFileAsync(name, stream);
            }
        }

        public async Task DeleteAllFilesAsync() {
            await _container.DeleteAsync();

            _onContainerDeleted();
        }

        public async Task DeleteFileAsync(string name) {
            var blobClient = await GetBlobClientAsync(name);

            await blobClient.DeleteAsync();
        }

        public async Task<Blob> GetFileAsync(string name, CancellationToken cancellationToken = default) {
            var blobClient = await GetBlobClientAsync(name);
            var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

            return new Blob(blobClient.Name,
                            GetContentType(name),
                            ByteSize.FromBytes(properties.Value.ContentLength),
                            await blobClient.OpenReadAsync(cancellationToken: cancellationToken));
        }

        private async Task<BlobClient> GetBlobClientAsync(string name) {
            var blobClient = _container.GetBlobClient(name);
            var exists = await blobClient.ExistsAsync();

            if (!exists) {
                throw new FileNotFoundException($"File {name.Quote()} does not exist in folder {_container.Name.Quote()}");
            }

            return blobClient;
        }
        
        private string GetContentType(string filename) {
            try {
                return MimeTypesMap.GetMimeType(filename);
            } catch {
                return "application/octet-stream";
            }
        }
    }
}