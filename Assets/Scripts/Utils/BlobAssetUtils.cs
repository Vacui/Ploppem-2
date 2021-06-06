﻿// source: https://neilmarkcorre.wordpress.com/2020/12/01/working-with-scriptable-objects-and-blob-assets-and-creating-a-utility-class/

using Unity.Collections;
using Unity.Entities;

public static class BlobAssetUtils {

    // We expose this to the clients to allow them to create BlobArray using BlobBuilderArray
    public static BlobBuilder BlobBuilder { get; private set; }

    // We allow the client to pass an action containing their blob creation logic
    public delegate void ActionRef<TBlobAssetType, in TDataType>(ref TBlobAssetType blobAsset, TDataType data);

    public static BlobAssetReference<TBlobAssetType> BuildBlobAsset<TBlobAssetType, TDataType> (TDataType data, ActionRef<TBlobAssetType, TDataType> action) where TBlobAssetType : struct {
        BlobBuilder = new BlobBuilder(Allocator.Temp);

        // Take note of the "ref" keywords. Unity will throw an error without them, since we're working with structs.
        ref TBlobAssetType blobAsset = ref BlobBuilder.ConstructRoot<TBlobAssetType>();

        // Invoke the client's blob asset creation logic
        action.Invoke(ref blobAsset, data);

        // Store the created reference to the memory location of the blob asset, before disposing the builder
        BlobAssetReference<TBlobAssetType> blobAssetReference = BlobBuilder.CreateBlobAssetReference<TBlobAssetType>(Allocator.Persistent);

        // We're not in a Using block, so we manually dispose the builder
        BlobBuilder.Dispose();

        // Return the created reference
        return blobAssetReference;
    }

    // source: https://coffeebraingames.wordpress.com/2020/11/29/getting-started-with-blob-asset/
    public static BlobAssetReference<T> CreateReference<T>(T value, Allocator allocator) where T : struct {
        BlobBuilder builder = new BlobBuilder(Allocator.TempJob);
        ref T data = ref builder.ConstructRoot<T>();
        data = value;
        BlobAssetReference<T> reference = builder.CreateBlobAssetReference<T>(allocator);
        builder.Dispose();

        return reference;
    }

}